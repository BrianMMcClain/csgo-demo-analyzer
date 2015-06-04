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

        public Results()
        {
        }

        public Results(String hash)
        {
            this.Hash = hash;
        }
    }
}
