using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    class Player
    {
        public string Name;
        public long SteamID;
        public List<Kill> Kills;
        public List<Kill> Assists;

        public Player()
        {
            this.Name = null;
            this.SteamID = 0;
            this.Kills = new List<Kill>();
            this.Assists = new List<Kill>();
        }

        public Player(string Name, long SteamID)
        {
            this.Name = Name;
            this.SteamID = SteamID;
            this.Kills = new List<Kill>();
            this.Assists = new List<Kill>();
        }
    }
}
