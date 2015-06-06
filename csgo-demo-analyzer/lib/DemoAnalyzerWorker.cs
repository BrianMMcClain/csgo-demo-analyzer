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

		public void ParseDemo(string demoPath)
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
			parser.BombPlanted += Parser_BombPlanted;
			parser.BombDefused += Parser_BombDefused;

			parser.ParseToEnd();

			// Record the final score and MVPs
			foreach (DemoInfo.Player p in parser.PlayingParticipants)
			{
				this.results.Players[p.SteamID].Score = p.AdditionaInformations.Score;
				this.results.Players[p.SteamID].MVPs = p.AdditionaInformations.MVPs;
			}

			Debug.WriteLine("");
			Debug.WriteLine(String.Format("Most Headshots: {0} ({1})", results.MostHeadshots.Name, results.MostHeadshots.HeadshotCount));
            Debug.WriteLine(String.Format("Most Kills: {0} ({1})", results.MostKills.Name, results.MostKills.Kills.Count));
            Debug.WriteLine(String.Format("Least Kills: {0} ({1})", results.LeastKills.Name, results.LeastKills.Kills.Count));
            Debug.WriteLine(String.Format("Most Deaths: {0} ({1})", results.MostDeaths.Name, results.MostDeaths.Deaths.Count));
            Debug.WriteLine(String.Format("Least Deaths: {0} ({1})", results.LeastDeaths.Name, results.LeastDeaths.Deaths.Count));
            Debug.WriteLine(String.Format("Most TKs: {0} ({1})", results.MostTKs.Name, results.MostTKs.Teamkills.Count));
            Debug.WriteLine(String.Format("Least TKs: {0} ({1})", results.LeastTKs.Name, results.LeastTKs.Teamkills.Count));
            Debug.WriteLine(String.Format("Highest Frag/Death Ratio: {0} ({1})", results.HighestFragDeathRatio.Name, ((float)results.HighestFragDeathRatio.Kills.Count / (float)results.HighestFragDeathRatio.Deaths.Count)));
            Debug.WriteLine(String.Format("Most Cowardly: {0}", results.MostCowardly.Name));
            Debug.WriteLine(String.Format("Most Plants: {0} ({1})", results.MostBombPlants.Name, results.MostBombPlants.BombPlants.Count));
            Debug.WriteLine(String.Format("Most Defuses: {0} ({1})", results.MostBombDefuses.Name, results.MostBombDefuses.BombDefuses.Count));
        }

        private void Parser_BombDefused(object sender, BombEventArgs e)
        {
            results.Players[e.Player.SteamID].BombDefuses.Add(this.currentRound);
            this.currentRound.bombDefuser = results.Players[e.Player.SteamID];
        }

        private void Parser_BombPlanted(object sender, BombEventArgs e)
        {
            results.Players[e.Player.SteamID].BombPlants.Add(this.currentRound);
            this.currentRound.bombPlanter = results.Players[e.Player.SteamID];
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
            this.currentRound.Winner = winningTeam;

            // Reset in-progres stats tracking
            lastTScore = parser.TScore;
            lastCTScore = parser.CTScore;

            // Record the current round
            this.results.Rounds.Add(this.currentRoundNumber, this.currentRound);

            Debug.WriteLine("End round " + this.currentRound.RoundNumber + ". Winner: " + this.currentRound.Winner);
            Debug.WriteLine(String.Format("Round {0}: T {1} | CT {2} - Winner: {3}.", this.currentRound.RoundNumber, parser.TScore, parser.CTScore, this.currentRound.Winner));
            if (this.currentRound.bombPlanter != null)
                Debug.WriteLine(String.Format("Bomb planted by {0}", this.currentRound.bombPlanter.Name));
            if (this.currentRound.bombDefuser != null)
                Debug.WriteLine(String.Format("Bomb defused by {0}", this.currentRound.bombDefuser.Name));
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
            if (matchStarted && e.Killer.SteamID != 0 && e.DeathPerson.SteamID != 0)
            {
                // Record each kill
                Kill kill = new Kill(this.results.Players[e.Killer.SteamID], this.results.Players[e.DeathPerson.SteamID], e.Headshot, e.Weapon.Weapon.ToString());
                if (e.Assister != null)
                {
                    kill.Assister = this.results.Players[e.Assister.SteamID];
                    kill.Assister.Assists.Add(kill);
                }
                kill.PenetratedObjects = e.PenetratedObjects;
                this.results.Players[e.Killer.SteamID].Kills.Add(kill);

                // Record the corresponding death for the killed player
                this.results.Players[e.DeathPerson.SteamID].Deaths.Add(kill);

                // Record if this is a teamkill
                if (e.Killer.Team == e.DeathPerson.Team)
                {
                    results.Players[e.Killer.SteamID].Teamkills.Add(kill);
                }

                Debug.WriteLine(kill.ToString());
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
