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
        public long SteamID;
        public List<Kill> Kills;
        public List<Kill> Deaths;
        public List<Kill> Assists;
        public List<Round> BombPlants;
        public List<Round> BombExplosions;
        public List<Round> BombDefuses;
        public int Score = 0;
        public int MVPs = 0;

        public Player()
        {
            this.Name = null;
            this.SteamID = 0;
            this.Kills = new List<Kill>();
            this.Deaths = new List<Kill>();
            this.Assists = new List<Kill>();
            this.BombPlants = new List<Round>();
            this.BombExplosions = new List<Round>();
            this.BombDefuses = new List<Round>();
        }

        public Player(string Name, long SteamID)
        {
            this.Name = Name;
            this.SteamID = SteamID;
            this.Kills = new List<Kill>();
            this.Deaths = new List<Kill>();
            this.Assists = new List<Kill>();
            this.BombPlants = new List<Round>();
            this.BombExplosions = new List<Round>();
            this.BombDefuses = new List<Round>();
        }

        public int HeadshotCount
        {
            get { return _HeadshotCount(); }
        }

        private int _HeadshotCount()
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
