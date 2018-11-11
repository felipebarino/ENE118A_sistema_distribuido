using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace conexao
{
    [Serializable]
    public class conexao
    {
        public bool clientSet { get; set; }
        public bool serverSet { get; set; }
        public bool stateValve { get; set; }
        public bool stateValveGUI { get; set; }
        public bool clientGetLevel { get; set; }
        public bool updateGUI { get; set; }
        public double level { get; set; }

        public conexao() { }
        public conexao(bool clientset, bool serverset, bool statevalve, bool statevalvegui, bool clientgetlevel, bool updategui, double lev)
        {
            this.clientSet = clientset;
            this.serverSet = serverset;
            this.stateValve = statevalve;
            this.stateValveGUI = statevalvegui;
            this.clientGetLevel = clientgetlevel;
            this.updateGUI = updategui;
            this.level = lev;
        }
    }
}