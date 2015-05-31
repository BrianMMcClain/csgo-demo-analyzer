using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DemoInfo;

namespace csgo_demo_analyzer
{
    class DemoAnalyzerWorker
    {
        public DemoAnalyzerWorker(String demoPath)
        {
            DemoParser parser = new DemoParser(File.OpenRead(demoPath));
            parser.ParseHeader();

            Debug.WriteLine("Map: " + parser.Map);
        }
    }
}
