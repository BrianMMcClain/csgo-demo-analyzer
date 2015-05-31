using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    class Results
    {
        public string Hash;
        public string Map;
        public Dictionary<long, Player> Players;
        public Dictionary<int, Round> Rounds;

        public Player MostHeadshots
        {
            get { return _MostHeadshots(); }
        }

        public Results(String hash)
        {
            this.Hash = hash;
            this.Players = new Dictionary<long, Player>();
            this.Rounds = new Dictionary<int, Round>();
        }

        private Player _MostHeadshots()
        {
            Player mostHS = null;
            int mostHSCount = -1;
            foreach (Player p in this.Players.Values)
            {
                if (p.HeadshotCount > mostHSCount)
                {
                    mostHS = p;
                    mostHSCount = p.HeadshotCount;
                }
            }

            return mostHS;
        }
    }
}
