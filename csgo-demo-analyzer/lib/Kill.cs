using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    class Kill
    {
        public Player Killer;
        public Player Killed;
        public Player Assister = null;
        public bool HasAssistance = false;
        public bool Headshot = false;
        public string Weapon = null;
        public int PenetratedObjects = 0;

        public Kill()
        {
            this.Killer = null;
            this.Killed = null;
        }

        public Kill(Player killer, Player killed)
        {
            this.Killer = killer;
            this.Killed = killed;
        }

        public Kill(Player killer, Player killed, bool headshot)
        {
            this.Killer = killer;
            this.Killed = killed;
            this.Headshot = headshot;
        }

        public Kill(Player killer, Player killed, bool headshot, string weapon)
        {
            this.Killer = killer;
            this.Killed = killed;
            this.Headshot = headshot;
            this.Weapon = weapon;
        }
    }
}
