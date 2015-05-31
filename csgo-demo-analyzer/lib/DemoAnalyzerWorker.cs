using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using DemoInfo;

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
            this.currentRound = new Round();
            Debug.WriteLine(String.Format("Map: {0}", this.results.Map));

            parser.MatchStarted += Parser_MatchStarted;
            parser.RoundStart += Parser_RoundStart;
            parser.PlayerKilled += Parser_PlayerKilled;
            parser.RoundEnd += Parser_RoundEnd;
            parser.TickDone += Parser_TickDone;

            parser.ParseToEnd();
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
            // Determine the winner of the round
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
            this.currentRound.Winner = winningTeam;

            // Record the current round
            this.results.Rounds.Add(this.currentRoundNumber, this.currentRound);

            Debug.WriteLine("End round " + this.currentRound.RoundNumber);
            Debug.WriteLine(String.Format("Round {0}: T {1} | CT {2} - Winner: {3}", this.currentRound.RoundNumber, parser.TScore, parser.CTScore, this.currentRound.Winner));
        }

        private void Parser_RoundStart(object sender, RoundStartedEventArgs e)
        {
            // Record the start of a new round
            if (matchStarted)
            {
                this.currentRound = new Round();
                this.currentRoundNumber++;
                this.currentRound.RoundNumber = this.currentRoundNumber;

                Debug.WriteLine("Start round " + this.currentRound.RoundNumber);
            }
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
                // Record each kill
                Kill kill = new Kill(this.results.Players[e.Killer.SteamID], this.results.Players[e.DeathPerson.SteamID], e.Headshot, e.Weapon.Weapon.ToString());
                if (e.Assister != null)
                {
                    kill.HasAssistance = true;
                    kill.Assister = this.results.Players[e.Assister.SteamID];
                    kill.Assister.Assists.Add(kill);
                }
                kill.PenetratedObjects = e.PenetratedObjects;
                this.results.Players[e.Killer.SteamID].Kills.Add(kill);
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

            // Record the start of the first round
            // This is a special case because timings and stuff
            this.currentRound = new Round();
            this.currentRound.RoundNumber = this.currentRoundNumber;

            Debug.WriteLine("Start round " + this.currentRound.RoundNumber);
        }
    }
}
