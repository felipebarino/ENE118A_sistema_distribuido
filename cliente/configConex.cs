using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace cliente
{
    public partial class configConex : Form
    {
        private TcpClient clienteFCC;

        public configConex(TcpClient cliente)
        {
            InitializeComponent();
            clienteFCC = cliente;
        }

        private void bt_connect_Click(object sender, EventArgs e)
        {
            try
            {
                clienteFCC.Connect(IPAddress.Parse(tb_ip.Text), Convert.ToInt16(tb_porta.Text));
                MessageBox.Show("Conexão realizada com sucesso");
                this.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro na tentativa de conexão com o servidor\r\nErro: " + erro.Message);
            }
        }
    }
}
