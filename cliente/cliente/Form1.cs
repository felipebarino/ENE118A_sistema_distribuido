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
        public Form1()
        {
            InitializeComponent();
        }

        static TcpClient cliente = new TcpClient();                      //Cria uma nova instância do TcpClient
        static private NetworkStream stream;
        static private NetworkStream streamrx;
        private bool valve = false;

        Thread attachServer = new Thread(ServerAttach);
        public static string lb_lastServerMSGText = "";

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
                        if (cliente.Available > 0)  //Aguarda até que o cliente envie a mensagem (número de bytes para leitura > 0)
                        {
                            bufferrx = new byte[cliente.Available]; //Aloca dinamicamente o buffer que receberá os dados
                            stream.Read(bufferrx, 0, cliente.Available); //Realiza a leitura do buffer

                            msrx = new MemoryStream(bufferrx); // Instancia um objeto MemoryStream que será utilizado para a deserialização
                            conexao.conexao conexrx = (conexao.conexao)bfrx.Deserialize(msrx);  //Deserializa o objeto
                            lb_lastServerMSGText = "Servidor: " + cliente.Client.RemoteEndPoint.ToString() + "\r\n--> Level: " + conexrx.level.ToString("0.00") + "\r\n--> Valve: " + Convert.ToString(conexrx.stateValveGUI);
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
                    stream = cliente.GetStream();                             //Obtém o stream do socket
                    bt_connect.Text = "DESCONECTAR";                          //Troca o texto do botão conectar
                    lb_status.BackColor = Color.Lime;                         //Troca a cor do label de status
                }
                catch
                {
                    MessageBox.Show("Falha na conexão com servidor");       //Caso não consiga se conectar mostra MessageBox        
                }
            }
            else
            {
                this.desconectar();                                            // Caso o botão esteja na condição desconectar, invoca o método desconectar           
            }
        }

        private void conexãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configConex fCC = new configConex(cliente);
            fCC.Show();
        }

        private void pictureBox_valve_Click(object sender, EventArgs e)
        {
            if(valve)
            {
                MessageBox.Show("Fechando Válvula");
                pictureBox_valve.Image = Image.FromFile("../../imgs/Closed Valve R6.png");
                lb_valveState.Text = "Fechada";
                valve = false;
            }
            else
            {
                MessageBox.Show("Abrindo Válvula");
                pictureBox_valve.Image = Image.FromFile("../../imgs/Open Valve G1.png");
                lb_valveState.Text = "Aberta";
                valve = true;
            }
    
            //sendData( clientSet, bool serverSet, bool stateValve, bool stateValveGUI, bool clientGetLevel, bool updategui, double level);
        }
    }
}
