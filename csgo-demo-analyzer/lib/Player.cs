using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    public class Player
    {
        public string Name;
        public long SteamID = 0;
        public List<Kill> Kills = new List<Kill>();
        public List<Kill> Deaths = new List<Kill>();
        public List<Kill> Assists = new List<Kill>();
        public List<Kill> Teamkills = new List<Kill>();
		public List<Round> BombPlants = new List<Round>();
		public List<Round> BombExplosions = new List<Round>();
		public List<Round> BombDefuses = new List<Round>();
        public int Score = 0;
        public int MVPs = 0;

        public Player()
        {
        }

        public Player(string Name, long SteamID) : base()
        {
            this.Name = Name;
            this.SteamID = SteamID;
        }

        public int HeadshotCount
        {
            get
			{
				int hsCount = 0;
				foreach (Kill k in this.Kills)
				{
					if (k.Headshot)
						hsCount++;
				}
				return hsCount;
			}
        }
    }
}
