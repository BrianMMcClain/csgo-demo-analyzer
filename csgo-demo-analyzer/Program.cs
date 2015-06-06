using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Results results = worker.ParseDemo("C:\\demos\\test_demo.dem");

            Console.WriteLine(String.Format("Most Headshots: {0} ({1})", results.MostHeadshots.Name, results.MostHeadshots.HeadshotCount));
            Console.WriteLine(String.Format("Most Kills: {0} ({1})", results.MostKills.Name, results.MostKills.Kills.Count));
            Console.WriteLine(String.Format("Least Kills: {0} ({1})", results.LeastKills.Name, results.LeastKills.Kills.Count));
            Console.WriteLine(String.Format("Most Deaths: {0} ({1})", results.MostDeaths.Name, results.MostDeaths.Deaths.Count));
            Console.WriteLine(String.Format("Least Deaths: {0} ({1})", results.LeastDeaths.Name, results.LeastDeaths.Deaths.Count));
            Console.WriteLine(String.Format("Most TKs: {0} ({1})", results.MostTKs.Name, results.MostTKs.Teamkills.Count));
            Console.WriteLine(String.Format("Least TKs: {0} ({1})", results.LeastTKs.Name, results.LeastTKs.Teamkills.Count));
            Console.WriteLine(String.Format("Highest Frag/Death Ratio: {0} ({1})", results.HighestFragDeathRatio.Name, ((float)results.HighestFragDeathRatio.Kills.Count / (float)results.HighestFragDeathRatio.Deaths.Count)));
            Console.WriteLine(String.Format("Most Cowardly: {0}", results.MostCowardly.Name));
            Console.WriteLine(String.Format("Most Plants: {0} ({1})", results.MostBombPlants.Name, results.MostBombPlants.BombPlants.Count));
            Console.WriteLine(String.Format("Most Defuses: {0} ({1})", results.MostBombDefuses.Name, results.MostBombDefuses.BombDefuses.Count));

            Console.ReadLine();
        }
    }
}
