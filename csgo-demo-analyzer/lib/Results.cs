using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    public class Results
    {
        public string Hash = string.Empty;
        public string Map;
        public Dictionary<long, Player> Players = new Dictionary<long, Player>();
        public Dictionary<int, Round> Rounds = new Dictionary<int, Round>();

        public Player MostHeadshots
        {
            get
			{
                return this.Players.Aggregate((a, b) => a.Value.HeadshotCount > b.Value.HeadshotCount ? a : b).Value;
            }
        }

        // ESEA: Most Deadly
        public Player MostKills
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.Kills.Count > b.Value.Kills.Count ? a : b).Value;
            }
        }

        // ESEA: Mostly Harmless
        public Player LeastKills
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.Kills.Count < b.Value.Kills.Count ? a : b).Value;
            }
        }

        // ESEA: Most Professional
        public Player HighestFragDeathRatio
        {
            get
            {
                return this.Players.Aggregate((a, b) => ((float)a.Value.Kills.Count / (float)a.Value.Deaths.Count) > ((float)b.Value.Kills.Count / (float)b.Value.Deaths.Count) ? a : b).Value;
            }
        }

        // ESEA: Longest Innings
        public Player LeastDeaths
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.Deaths.Count < b.Value.Deaths.Count ? a : b).Value;
            }
        }

        // ESEA: Shortest Innings
        public Player MostDeaths
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.Deaths.Count > b.Value.Deaths.Count ? a : b).Value;
            }
        }

        // ESEA: Most Honorable
        public Player LeastTKs
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.Teamkills.Count < b.Value.Teamkills.Count ? a : b).Value;
            }
        }

        // ESEA: Most Dishonorable
        public Player MostTKs
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.Teamkills.Count > b.Value.Teamkills.Count ? a : b).Value;
            }
        }

        // ESEA: Most Cowardly
        public Player MostCowardly
        {
            get
            {
                return this.Players.Aggregate((a, b) => (a.Value.Kills.Count + a.Value.Deaths.Count) < (b.Value.Kills.Count + b.Value.Deaths.Count) ? a : b).Value;
            }
        }

        public Player MostBombPlants
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.BombPlants.Count > b.Value.BombPlants.Count ? a : b).Value;
            }
        }

        public Player MostBombDefuses
        {
            get
            {
                return this.Players.Aggregate((a, b) => a.Value.BombDefuses.Count > b.Value.BombDefuses.Count ? a : b).Value;
            }
        }


        public Results()
        {
        }

        public Results(String hash)
        {
            this.Hash = hash;
        }
    }
}
