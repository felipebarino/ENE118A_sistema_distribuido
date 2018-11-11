using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using conexao;
using System.Threading;


namespace cliente
{
    public partial class Form1 : Form
    {
        static TcpClient cliente;                      //Cria uma nova instância do TcpClient
        static private NetworkStream stream;
        static bool valveGUI = false;
        static double levelGUI = 0.0;

        static private bool waitingServer = true;

        Thread attachServer;

        public Form1()
        {
            InitializeComponent();
            med_lb.Text = " ";
            lb_valveState.Text = " ";
            pictureBox_valve.Image = Image.FromFile("../../imgs/Closed Valve GR6.png");
        }


        public static void ServerAttach()
        {
            BinaryFormatter bfrx = new BinaryFormatter();
            //MemoryStream -> o objeto instanciado será utilizado para armazenar o resultado da serialização
            MemoryStream msrx = new MemoryStream();

            byte[] bufferrx;
            while (true)
            {
                while (cliente.Connected)
                {
                    try
                    {
                        if (cliente.Available > 0)                          //Aguarda até que o cliente envie a mensagem (número de bytes para leitura > 0)
                        {
                            bufferrx = new byte[cliente.Available];         //Aloca dinamicamente o buffer que receberá os dados
                            stream.Read(bufferrx, 0, cliente.Available);    //Realiza a leitura do buffer

                            msrx = new MemoryStream(bufferrx);              // Instancia um objeto MemoryStream que será utilizado para a deserialização
                            conexao.conexao conexrx = (conexao.conexao)bfrx.Deserialize(msrx);  //Deserializa o 

                            if (conexrx.updateGUI)
                            {
                                levelGUI = conexrx.level;
                                valveGUI = conexrx.stateValveGUI;
                                waitingServer = false;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public void desconectar()
        {
            bt_connect.Text = "CONECTAR";
            lb_status.BackColor = Color.Red;
            stream.Close();
            attachServer.Abort();
        }

        public void sendData(bool clientSet, bool serverSet, bool stateValve, bool stateValveGUI, bool clientGetLevel, bool updategui, double level)
        {
            try
            {
                //Classe definida em MinhaBiblioteca.dll (namespace MinhaBiblioteca)
                conexao.conexao conex = new conexao.conexao(clientSet, serverSet, stateValve, stateValveGUI, clientGetLevel, updategui, level);

                //Binary Formatter -> classe utilizada para serializar (converter para um array de bytes) o objeto
                BinaryFormatter bf = new BinaryFormatter();

                //MemoryStream -> o objeto instanciado será utilizado para armazenar o resultado da serialização
                MemoryStream ms = new MemoryStream();

                //Serialização
                bf.Serialize(ms, conex);
                //MessageBox.Show("serializou");
                //Envio do objeto pela rede
                stream.Write(ms.ToArray(), 0, ms.ToArray().Length);
            }
            catch
            {
                MessageBox.Show("Erro ao comunicar com o servidor");
                this.desconectar();
            }
        }

        private void bt_connect_Click(object sender, EventArgs e)
        {
            if (bt_connect.Text == "CONECTAR")
            {
                try
                {
                    cliente = new TcpClient();
                    configConex fCC = new configConex(cliente);
                    fCC.ShowDialog();
                    stream = cliente.GetStream();
                    bt_connect.Text = "DESCONECTAR";                          //Troca o texto do botão conectar
                    lb_status.BackColor = Color.Lime;                         //Troca a cor do label de status
                    attachServer = new Thread(ServerAttach);
                    attachServer.Start();
                }
                catch
                {
                    MessageBox.Show("Falha na conexão com servidor\n");       //Caso não consiga se conectar mostra MessageBox        
                }
            }
            else
            {
                this.desconectar();                                            // Caso o botão esteja na condição desconectar, invoca o método desconectar           
            }
        }

        private void pictureBox_valve_Click(object sender, EventArgs e)
        {
            bool clientSet = true;
            bool serverSet = false;
            bool stateValve = !valveGUI;
            bool clientGetLevel = true;
            bool updategui = false;
            double level = 0.0;
            sendData(clientSet, serverSet, !valveGUI, valveGUI, clientGetLevel, updategui, level);

            lb_valveState.Text = "Atuando ...";
            if (valveGUI)
                pictureBox_valve.Image = Image.FromFile("../../imgs/Open Valve GR6.png");
            else
                pictureBox_valve.Image = Image.FromFile("../../imgs/Closed Valve GR6.png");

            waitingServer = true;
        }

        private void timer1_Tick(object sender, EventArgs e)                // Atualiza GUI
        {
            if (!waitingServer)
            {
                if (valveGUI)
                {
                    pictureBox_valve.Image = Image.FromFile("../../imgs/Open Valve G1.png");
                    lb_valveState.Text = "Aberta";
                }
                else
                {
                    pictureBox_valve.Image = Image.FromFile("../../imgs/Closed Valve R6.png");
                    lb_valveState.Text = "Fechada";
                }

                med_lb.Text = levelGUI.ToString() + "L";

                waitingServer = true;
            }

            if (levelGUI > 12000 && levelGUI < 15000)
            {
                pictureBox_tank.Image = Image.FromFile("../../imgs/Water Tower Y4.png");
            }
            if (levelGUI >= 15000)
            {
                pictureBox_tank.Image = Image.FromFile("../../imgs/Water Tower R4.png");
            }
            if (levelGUI <= 12000)
            {
                pictureBox_tank.Image = Image.FromFile("../../imgs/Water Tower B4.png");
            }

        }


    }
}
