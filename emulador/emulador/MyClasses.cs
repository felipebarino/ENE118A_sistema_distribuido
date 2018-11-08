using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClasses
{
    public class Vazao
    {
        public int contador { get; set; }
        public double a { get; set; }
        public double b { get; set; }
        public double vazao { get; set; }
        public Vazao(double v0)
        {
            vazao = v0;
        }
        public void calculaVazao()
        {
            if (this.contador == 0)
            {
                Random rnd1 = new Random();
                a = rnd1.Next(-10, 10) + (double)0.1 * (rnd1.Next(1, 10));
                b = this.vazao;
            }
            this.vazao = ((a * contador + b) > 0) ? (a * contador + b) : -(a * contador + b);
        }
    }
}