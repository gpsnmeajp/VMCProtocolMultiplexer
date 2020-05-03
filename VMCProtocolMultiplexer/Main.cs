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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rug.Osc;
using Newtonsoft.Json;
/*
 * InputJson : 入力ポートと入力名
 * OutputJson : 出力ポートと入力名
 * Filter Json: 振り分け法則(入力名-フィルタ-出力名)
 */

namespace VMCProtocolMultiplexer
{
    class Filter {
        public string Output { get; set; }
    }

    class Main
    {
        Dictionary<string, InputOSC> inputOSC = new Dictionary<string, InputOSC>(); //名前-入力ソケット
        Dictionary<string, OutputOSC> outputOSC = new Dictionary<string, OutputOSC>(); //名前-出力ソケット
        Dictionary<string, List<string>> routing = new Dictionary<string, List<string>>();
        //入力名 - フィルタ法則>[]

        public void Process()
        {
            Console.WriteLine("### VMCProtocolMultiplexer v0.00");
            //---------サーバー開始------------
            try
            {
                StartServer();

                Console.WriteLine("Press ENTER key to stop server");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("# MainThread : " + e);
            }

            //---------サーバー停止------------
            try
            {
                StopServer();
            }
            catch (Exception e)
            {
                Console.WriteLine("# MainThread : " + e);
            }

            Console.WriteLine("Press ENTER key to close window");
            Console.ReadLine();
        }

        private void StartServer()
        {
            List<InputJson> input = JsonConvert.DeserializeObject<List<InputJson>>(File.ReadAllText("input.json", new UTF8Encoding(false)));
            List<OutputJson> output = JsonConvert.DeserializeObject<List<OutputJson>>(File.ReadAllText("output.json", new UTF8Encoding(false)));
            List<FilterJson> filter = JsonConvert.DeserializeObject<List<FilterJson>>(File.ReadAllText("filter.json", new UTF8Encoding(false)));

            Console.WriteLine("# Setting loaded");
            //入力を準備
            Console.WriteLine("");
            Console.WriteLine("### Input Table");
            foreach (var i in input)
            {
                inputOSC[i.Name] = new InputOSC(i.Name, i.Port, OnBundle, OnMessage);
                routing[i.Name] = new List<string>(); //ルーティングテーブル
                Console.WriteLine("INPUT: " + "Port " + i.Port + " -> " + i.Name);
            }
            
            //出力を準備
            Console.WriteLine("");
            Console.WriteLine("### Output Table");
            foreach (var o in output)
            {
                outputOSC[o.Name] = new OutputOSC(o.Address, o.Port);
                Console.WriteLine("OUTPUT: " + o.Name + " -> " + o.Address + ":" + o.Port);
            }

            //ルーティングテーブルを準備
            Console.WriteLine("");
            Console.WriteLine("### Routing Table");
            foreach (var f in filter)
            {
                routing[f.InputName].Add(f.OutputName);
                Console.WriteLine("ROUTE: " + f.InputName + " -> " + f.OutputName);
            }
            Console.WriteLine("");
        }
        private void StopServer()
        {
            foreach (var i in inputOSC)
            {
                i.Value.Dispose();
                Console.WriteLine("INPUT: " + i.Key + " CLOSED");
            }
            foreach (var o in outputOSC)
            {
                o.Value.Dispose();
                Console.WriteLine("OUTPUT: " + o.Key + " CLOSED");
            }
        }

        private void OnMessage(string name, OscMessage message) {
            //ルーティングテーブルに従って処理
            foreach (string o in routing[name])
            {
                outputOSC[o].Send(message);
            }
        }
        private void OnBundle(string name, OscBundle bundle) {
            //ルーティングテーブルに従って処理
            foreach (string o in routing[name])
            {
                outputOSC[o].Send(bundle);
            }
        }
    }
}
