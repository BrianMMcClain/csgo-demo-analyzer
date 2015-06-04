using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoAnalyzerWorker worker = new DemoAnalyzerWorker();
			worker.ParseDemo("D:\\demos\\test_demo.dem");
        }
    }
}
