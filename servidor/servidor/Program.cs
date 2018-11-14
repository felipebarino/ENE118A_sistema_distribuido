using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using conexao;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace ConsoleApplication1
{
    class Program
    {
        private static bool ServidorAtivo = false;
        static ParameterizedThreadStart delSerial1 = new ParameterizedThreadStart(reqSensor);// DELEGATE para enviar requisição
        static Thread Threadreq;// thread que fará requisições sincronas para objeto serial
        static ParameterizedThreadStart delTemp = new ParameterizedThreadStart(timerTick);
        static Thread temporizer;
        //flags para controle da válvula
        static bool timerFlag = false;// fla que habilita o envio de estado da válvula para os cientes atualizarem suas telas
        static bool intention_setValve = false;// flag que sinaliza a intenção do cliente em alterar o valor da valvula
        static int flag_anti_burrice = 0;// flag que impede mudanças de estados rápidas, que poderiam danificar a válvula
        static int fab_max_cont = 3;//tempo morto da válvula, que impede mudança de estado

        static StreamWriter sw;
        static bool stateValve;
        static double nivel;

        private static void timerTick(object objport)
        {
            SerialPort sp = (SerialPort)objport;

            while (true)
            {
                timerFlag = true;
                Thread.Sleep(50);
                timerFlag = false;
                Thread.Sleep(150);
                flag_anti_burrice++;
                // para que o contador flag_anti_burrice não estoure
                if (flag_anti_burrice > 100000)
                    flag_anti_burrice = 100000;

                //se  há intenção e o contador já contou tempo suficiente, então confere a variável static stateValve e envia novo estado para o atuador
                if (intention_setValve && (flag_anti_burrice > fab_max_cont))
                {
                    try
                    {
                        if (stateValve)
                            sp.WriteLine("ON");
                        else
                            sp.WriteLine("OFF");
                    }
                    catch
                    {
                        Console.WriteLine("Falha ao comunicar com o sensor");
                    }
                    intention_setValve = false; //desabilita flag de intenção
                    flag_anti_burrice = 0; // para novamente impedir sucessivas mudanças de estado

                }


            }

        }

        private static void reqSensor(object obj)
        {

            SerialPort sp = (SerialPort)obj;
            while (true)
            {
                if (sp.IsOpen)
                {
                    sp.WriteLine("REQ");
                    // Console.WriteLine("TO NA THREAD"); //
                }
                Thread.Sleep(1000);
            }
        }

        public static bool flag_send = true;
        private static void TratamentodeDados(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            sp.DiscardInBuffer();

            try
            {

                if (indata == "EOT")
                {
                    Console.WriteLine("Sensor finalizou a transmissão de dados");
                }
                else
                {
                    nivel = Convert.ToDouble(indata);
                    indata = indata.Replace(",", ".");
                    Console.WriteLine(" " + DateTime.Now + " " + indata + "L");

                    using (StreamWriter w = File.AppendText("dadosout.txt"))
                    {
                        w.WriteLine(DateTime.Now + "," + indata);
                        w.Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Dados perdidos");
            }
        }

        public static void PrintCabecalho()
        {
            Console.WriteLine("---------- Programa de leitura dos dados de um sensor de vazão---------- \n");


            Console.WriteLine(" Dia Horário Valor");
        }



        //-------------------------------------------------NETWORK-------------------------------------------------------------

        public static void trataClientes(object Objcliente)
        {
            TcpClient cliente = (TcpClient)Objcliente;
            NetworkStream stream;
            conexao.conexao conex = new conexao.conexao(false, false, false, false, false, false, 0);          //Instância da classe conexao, recebe os dados dos clientes e alterações são realizadas simutalneamente
            BinaryFormatter bf = new BinaryFormatter(); //Classe para deserializar
            byte[] buffer;     //Array de bytes, que será utilizado para receber o stream proveniente da comunicação TCP IP

            bool flag_level = false;
            Console.WriteLine(System.DateTime.Now.ToString() + " - Cliente " + cliente.Client.RemoteEndPoint.ToString() + " Conectado - Atendendo requisições");

            stream = cliente.GetStream();  //Obtém o network stream

            while (cliente.Connected)
            {
                try
                {       //A propriedade Available informa o número de bytes disponíveis para leitura de um determinado cliente
                    if (cliente.Available > 0)  //Aguarda até que o cliente envie a mensagem (número de bytes para leitura > 0)
                    {
                        buffer = new byte[cliente.Available]; //Aloca dinamicamente o buffer que receberá os dados
                        stream.Read(buffer, 0, cliente.Available); //Realiza a leitura do buffer

                        MemoryStream ms = new MemoryStream(buffer); // Instancia um objeto MemoryStream que será utilizado para a deserialização
                        conex = (conexao.conexao)bf.Deserialize(ms);  //Deserializa o objeto

                        if (conex.clientSet)
                        {
                            stateValve = conex.stateValve;
                            intention_setValve = true;
                        }
                        if (conex.clientGetLevel)
                        {
                            flag_level = true;
                        }

                        Console.WriteLine("Cliente: " + cliente.Client.RemoteEndPoint.ToString() + "--> clientSet: " + conex.clientSet.ToString() + " stateValve: " + conex.stateValve + "clientGetLevel: " + conex.clientGetLevel);

                    }

                    if (flag_level)
                    {
                        conex.serverSet = true;
                        conex.level = nivel;
                        conex.updateGUI = true;
                        conex.stateValveGUI = stateValve;

                        //Binary Formatter -> classe utilizada para serializar (converter para um array de bytes) o objeto
                        BinaryFormatter bf2 = new BinaryFormatter();
                        //MemoryStream -> o objeto instanciado será utilizado para armazenar o resultado da serialização
                        MemoryStream ms2 = new MemoryStream();
                        //Serialização
                        bf2.Serialize(ms2, conex);
                        //Envio do objeto pela rede
                        stream.Write(ms2.ToArray(), 0, ms2.ToArray().Length);
                        flag_level = false;
                    }

                    if (timerFlag) // não é a melhor abordagem, mas funciona legal
                    {
                        conex.updateGUI = true;
                        conex.stateValveGUI = stateValve;
                        conex.clientGetLevel = false;
                        conex.level = nivel;
                        //Binary Formatter -> classe utilizada para serializar (converter para um array de bytes) o objeto
                        BinaryFormatter bf2 = new BinaryFormatter();
                        //MemoryStream -> o objeto instanciado será utilizado para armazenar o resultado da serialização
                        MemoryStream ms2 = new MemoryStream();
                        //Serialização
                        bf2.Serialize(ms2, conex);
                        //Envio do objeto pela rede
                        stream.Write(ms2.ToArray(), 0, ms2.ToArray().Length);
                        flag_level = false;
                    }

                }
                catch (SystemException ex)
                {
                    if (cliente.Connected)
                        Console.WriteLine("Erro: " + ex.Message);

                    stream.Close();
                    cliente.Close();

                }
            }
            cliente.Close();
            stream.Close();
            Console.WriteLine(System.DateTime.Now.ToString() + " - Cliente Desconectado");
        }

        private static bool IsDead(Thread T)
        {
            return !T.IsAlive;
        }

        //Função de configuração do serviço e obtenção dos clientes
        public static void ServerHandler()
        {
            TcpListener servidor = new TcpListener(IPAddress.Any, 5000);
            TcpClient cliente;
            servidor.Start();
            //Criar ma lista e Threads que irão tratar os clientes
            List<Thread> ListThread = new List<Thread>();

            ParameterizedThreadStart ThreadTrataCliente = new ParameterizedThreadStart(trataClientes);
            Console.WriteLine("SERVIDOR ATIVO - AGUARDANDO CONEXÕES");
            while (ServidorAtivo)
            {
                if (servidor.Pending())
                {
                    //Aceita um novo cliente
                    Console.WriteLine(System.DateTime.Now.ToString() + " - Aguardando Cliente....");
                    cliente = servidor.AcceptTcpClient();
                    ListThread.RemoveAll(IsDead);
                    ListThread.Add(new Thread(ThreadTrataCliente));//cria thread e põe na lista
                    ListThread[ListThread.Count - 1].Name = ListThread[ListThread.Count - 1].ManagedThreadId.ToString();//dá nome para thread
                    ListThread[ListThread.Count - 1].Start(cliente);

                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------


        static void Main(string[] args)
        {
            //sw = (File.AppendText("dadosout.txt")); 
            Program.PrintCabecalho();
            SerialPort S1 = new SerialPort("COM11", 9600, Parity.None, 8);
            SerialPort S2 = new SerialPort("COM13", 9600, Parity.None, 8);

            S1.ReadTimeout = 500;
            S1.DataReceived += new SerialDataReceivedEventHandler(TratamentodeDados);
            S2.ReadTimeout = 500;
            S2.DataReceived += new SerialDataReceivedEventHandler(TratamentodeDados);

            Threadreq = new Thread(delSerial1);
            Threadreq.Start(S1);
            Thread ServidorT = new Thread(ServerHandler);

            temporizer = new Thread(delTemp);// inicializa thread temporizada para fazer requisições ao sensor serial
            temporizer.Start(S2);// thread que atua na valvula e é temporizada

            try
            {
                S1.Open();
                S1.DiscardInBuffer();
                S2.Open();
                S2.DiscardInBuffer();

            }
            catch
            {
                Console.WriteLine("Porta serial inválida");
            }
            if (S2.IsOpen)
            {

                if (flag_send)
                {
                    S2.WriteLine("OFF");
                    flag_send = false;
                }
            }
            ServidorAtivo = true;
            ServidorT.Start();
            while (!Console.KeyAvailable) { }
            S1.Close();
            S2.Close();
            ServidorAtivo = false;

            Environment.Exit(0);
        }
    }
}
