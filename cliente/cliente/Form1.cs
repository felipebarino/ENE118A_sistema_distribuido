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

namespace cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private NetworkStream stream;
        private TcpClient cliente;
        private bool valve = false;

        public void desconectar()
        {
            bt_connect.Text = "CONECTAR";
            lb_status.BackColor = Color.Red;
            stream.Close();
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
            cliente = new TcpClient();
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
            
        }
    }
}
