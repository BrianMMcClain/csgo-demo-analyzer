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
        public bool Headshot;
        public string Weapon;

        public Kill()
        {
            this.Killer = null;
            this.Killed = null;
            this.Headshot = false;
            this.Weapon = null;
        }

        public Kill(Player killer, Player killed)
        {
            this.Killer = killer;
            this.Killed = killed;
            this.Headshot = false;
            this.Weapon = null;
        }

        public Kill(Player killer, Player killed, bool headshot)
        {
            this.Killer = killer;
            this.Killed = killed;
            this.Headshot = headshot;
            this.Weapon = null;
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
