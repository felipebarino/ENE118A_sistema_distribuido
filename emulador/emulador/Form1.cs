using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace emulador
{
    public partial class Emulador : Form
    {
        ParameterizedThreadStart delSerial1 = new ParameterizedThreadStart(serialSensor);
        ParameterizedThreadStart delSerial2 = new ParameterizedThreadStart(serialAtuador);
        Thread ThreadSerial1;
        Thread ThreadSerial2;
        public static bool sendData1;
        public static bool sendData2;
        public static bool valve = false;
        public static bool close1;
        public static bool close2;

        public static PictureBox valvePic;

        public static double level = 9000;

        public void enable(object ObjSerial)
        {
            SerialPort SP = (SerialPort)ObjSerial;
            try
            {
                SP.Open();
            }
            catch
            {
                MessageBox.Show("Erro ao iniciar a conexão");
                return;
            }
            this.bt_iniciar.Text = "PARAR";
        }

        public void disable(object ObjSerial)
        {
            SerialPort SP = (SerialPort)ObjSerial;
            try
            {
                SP.WriteLine("EOT");
                SP.Close();
                this.bt_iniciar.Text = "INICIAR";
                this.med_lb.Text = "";
            }
            catch
            {
                MessageBox.Show("Falha ao desligar o sensor");
            }
        }

        public Emulador()
        {
            InitializeComponent();
            valvePic = pictureBox_valve;
        }

        private static void serialSensor(object obj)
        {
            SerialPort sp = (SerialPort)obj;

            while (sp.IsOpen)
            {
                if (sendData1)
                {
                    Thread.Sleep(2);
                    sp.WriteLine(level.ToString());
                    sendData1 = false;
                }

                if (close1)
                {
                    sp.Close();
                    close1 = false;
                }
            }
        }

        private static void serialAtuador(object obj)
        {
            SerialPort sp = (SerialPort)obj;

            while (sp.IsOpen)
            {
                if (close2)
                {
                    sp.Close();
                    close2 = false;
                }
                if (sendData2)
                {
                    if (valve)
                        sp.WriteLine("VALVE IS ON");
                    else
                        sp.WriteLine("VALVE IS OFF");

                    sendData2 = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.bt_iniciar.Text == "INICIAR")
            {
                try
                {
                    serialPort1.DataReceived += new SerialDataReceivedEventHandler(rxAck1);

                    this.enable(serialPort1);
                    ThreadSerial1 = new Thread(delSerial1);
                    ThreadSerial1.Start(this.serialPort1);
                }
                catch
                {
                    MessageBox.Show("Falha ao conectar o Sensor");
                }

                try
                {
                    serialPort2.DataReceived += new SerialDataReceivedEventHandler(rxAck2);

                    this.enable(serialPort2);
                    ThreadSerial2 = new Thread(delSerial2);
                    ThreadSerial2.Start(this.serialPort2);
                }
                catch
                {
                    MessageBox.Show("Falha ao conectar o Atuador");
                }
            }
            else
            {
                try
                {
                    this.disable(serialPort1);
                    ThreadSerial1.Abort();
                }
                catch
                {
                    MessageBox.Show("Falha ao desconectar o Sensor");
                }

                try
                {
                    this.disable(serialPort2);
                    ThreadSerial2.Abort();
                }
                catch
                {
                    MessageBox.Show("Falha ao desconectar o Atuador");
                }
            }
        }

        private void conexãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigConex fCC = new FormConfigConex(this.serialPort1, "sensor");
            fCC.Show();
        }

        private void conexãoAtuadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigConex fCC = new FormConfigConex(this.serialPort2, "atuador");
            fCC.Show();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static void rxAck1(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            sp.DiscardInBuffer();

            if (indata == "EOT")
            {
                Emulador.close1 = true;
                MessageBox.Show("Sensor teve conexão terminada.");
            }

            if (indata == "REQ")
            {
                Emulador.sendData1 = true;
            }
        }

        private static void rxAck2(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            sp.DiscardInBuffer();

            if (indata == "EOT")
            {
                Emulador.close2 = true;
                MessageBox.Show("Atuador teve conexão terminada.");
            }

            if (indata == "REQ")
            {
                Emulador.sendData2 = true;
            }

            if (indata == "ON")
            {
                Emulador.valve = true;
                valvePic.Image = Image.FromFile("../../imgs/Open Valve G1.png");
            }

            if (indata == "OFF")
            {
                Emulador.valve = false;
                valvePic.Image = Image.FromFile("../../imgs/Closed Valve R6.png");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (valve)
            {
                if(level > 0)
                    level = level - 3.14;
            }
            else
            {
                if (level < 15000)
                    level = level + 2.71;
            }

            if (level < 0)
                level = 0;
            if (level > 15000)
                level = 15000;

            med_lb.Text = level.ToString();

            if (level > 12000 && level < 15000)
            {
                pictureBox_tank.Image = Image.FromFile("../../imgs/Water Tower Y4.png");
            }
            if (level >= 15000)
            {
                pictureBox_tank.Image = Image.FromFile("../../imgs/Water Tower R4.png");
            }
            if (level <= 12000)
            {
                pictureBox_tank.Image = Image.FromFile("../../imgs/Water Tower B4.png");
            }
        }

        private void med_lb_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox_tank_Click(object sender, EventArgs e)
        {

        }
    }
}
