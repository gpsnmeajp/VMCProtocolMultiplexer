using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Rug.Osc;

namespace VMCProtocolMultiplexer
{
    class OutputOSC : IDisposable
    {
        OscSender oscSender;        
        public OutputOSC(string adr,int port)
        {
            IPAddress ip = IPAddress.Parse(adr);
            oscSender = new OscSender(ip, port);
            oscSender.Connect();
            //例外は上位に打ち上げる
        }

        //受信待受停止
        public void Send(OscPacket packet)
        {
            oscSender.Send(packet);
        }
        public void Dispose()
        {
            oscSender.Close();
        }
    }
}
