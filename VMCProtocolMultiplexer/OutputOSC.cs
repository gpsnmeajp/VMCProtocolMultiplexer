/*
MIT License

Copyright (c) 2020 gpsnmeajp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
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
        public long PacketCounter = 0;
        public string Address;
        public int Port;

        public OutputOSC(string adr,int port)
        {
            this.Address = adr;
            this.Port = port;

            IPAddress ip = IPAddress.Parse(this.Address);
            oscSender = new OscSender(ip, 0,this.Port);
            oscSender.Connect();
            //例外は上位に打ち上げる
        }

        //受信待受停止
        public void Send(OscPacket packet)
        {
            PacketCounter++;
            oscSender.Send(packet);
        }
        public void Dispose()
        {
            oscSender.Close();
        }
    }
}
