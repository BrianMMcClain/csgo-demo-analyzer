using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DemoInfo;
using System.Security.Cryptography;

namespace csgo_demo_analyzer
{
    class DemoAnalyzerWorker
    {
        private Results results;

        private DemoParser parser;
        private bool matchStarted;
        private Round currentRound;
        private int currentRoundNumber = 1;
        private int roundEndedCount = 0;

        private int lastTScore = 0;
        private int lastCTScore = 0;

        public DemoAnalyzerWorker(String demoPath)
        {
            this.matchStarted = false;

            // Verify hash of demo
            SHA256Managed sha = new SHA256Managed();
            byte[] bhash = sha.ComputeHash(File.OpenRead(demoPath));
            string hash = BitConverter.ToString(bhash).Replace("-", String.Empty);
            Debug.WriteLine(String.Format("Demo hash: {0}", hash));

            this.results = new Results(hash);

            parser = new DemoParser(File.OpenRead(demoPath));
            parser.ParseHeader();

            // Record the map
            this.results.Map = parser.Map;
            Debug.WriteLine(String.Format("Map: {0}", this.results.Map));

            parser.MatchStarted += Parser_MatchStarted;
            parser.RoundStart += Parser_RoundStart;
            parser.PlayerKilled += Parser_PlayerKilled;
            parser.RoundEnd += Parser_RoundEnd;
            parser.TickDone += Parser_TickDone;

            parser.ParseToEnd();

            foreach (long sid in results.Players.Keys)
            {
                Debug.WriteLine(String.Format("{0} - {1}", results.Players[sid].Name, results.Players[sid].Kills.Count));
            }
        }

        private void Parser_TickDone(object sender, TickDoneEventArgs e)
        {
            if (roundEndedCount == 0)
            {
                return;
            }

            roundEndedCount++;
            
            // Wait 2 seconds after round ends to properly record winner of the round
            if (roundEndedCount < parser.TickRate * 2)
            {
                return;
            }

            roundEndedCount = 0;
            RoundEnd();
        }

        private void RoundEnd()
        {
            DemoInfo.Team winningTeam = Team.Spectate;
            if (lastTScore != parser.TScore)
            {
                winningTeam = Team.Terrorist;
            }
            else if (lastCTScore != parser.CTScore)
            {
                winningTeam = Team.CounterTerrorist;
            }

            lastTScore = parser.TScore;
            lastCTScore = parser.CTScore;

            Debug.WriteLine("End round " + currentRoundNumber);

            Debug.WriteLine(String.Format("T {0} | CT {1} - Winner: {2}", parser.TScore, parser.CTScore, winningTeam));
        }

        private void Parser_RoundEnd(object sender, RoundEndedEventArgs e)
        {
            if (matchStarted)
            {
                roundEndedCount = 1;
            }
        }

        private void Parser_PlayerKilled(object sender, PlayerKilledEventArgs e)
        {
            if (matchStarted)
            {
                Kill kill = new Kill(this.results.Players[e.Killer.SteamID], this.results.Players[e.DeathPerson.SteamID], e.Headshot, e.Weapon.Weapon.ToString());
                this.results.Players[e.Killer.SteamID].Kills.Add(kill);
                Debug.Write(String.Format("{0} <{1}> {2} ", kill.Killer.Name, kill.Weapon, kill.Killed.Name));
                if (kill.Headshot)
                {
                    Debug.WriteLine("(Headshot)");
                }
                else
                {
                    Debug.WriteLine("");
                }
            }
        }

        private void Parser_RoundStart(object sender, RoundStartedEventArgs e)
        {
            if (matchStarted)
            {
                this.currentRound = new Round();
                this.currentRoundNumber++;
                Debug.WriteLine("Start round " + currentRoundNumber);
            }
        }

        private void Parser_MatchStarted(object sender, MatchStartedEventArgs e)
        {
            this.matchStarted = true;
            
            // Record each player 
            foreach (DemoInfo.Player p in parser.PlayingParticipants)
            {
                Player pl = new Player(p.Name, p.SteamID);
                results.Players.Add(p.SteamID, pl);
            }

            Debug.WriteLine("Start round " + currentRoundNumber);
        }
    }
}
