using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
namespace ConsoleApplication1
{
    class Program
    {
        public static int cont = 0;

        public static bool flag_send = true;
        private static void TratamentodeDados(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            sp.DiscardInBuffer();
            Program.cont = Program.cont + 1;
            if (Program.cont == 21)
            {
                Console.Clear();
                Program.PrintCabecalho();
                Program.cont = 0;
            }
            try
            {
                if (indata == "EOT")
                {
                    Console.WriteLine("Sensor finalizou a transmissão de dados");
                }
                else
                    Console.WriteLine(" " + System.DateTime.Now + " " + indata + " L/s");
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
        static void Main(string[] args)
        {
            Program.PrintCabecalho();
            SerialPort S1 = new SerialPort("COM11", 9600, Parity.None, 8);
            S1.ReadTimeout = 500;
            S1.DataReceived += new SerialDataReceivedEventHandler(TratamentodeDados);
            try
            {
                S1.Open();
                S1.DiscardInBuffer();
            }
            catch
            {
                Console.WriteLine("Porta serial inválida");
            }
            if (S1.IsOpen)
            {   
                while (!Console.KeyAvailable)
                {
                    if(flag_send)
                    {
                        S1.WriteLine("REQ");
                        flag_send = false;
                    }
                }
                S1.Close();
            }
        }
    }
}
