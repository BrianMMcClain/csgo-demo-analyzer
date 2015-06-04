using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    public class Kill
    {
        public Player Killer;
        public Player Killed;
        public Player Assister = null;
        public bool Headshot = false;
        public string Weapon = null;
        public int PenetratedObjects = 0;

		public bool HasAssistance
		{
			get { return Assister != null; }
		}

		public Kill()
        {
        }

        public Kill(Player killer, Player killed, bool headshot = false, string weapon = null)
        {
            this.Killer = killer;
            this.Killed = killed;
            this.Headshot = headshot;
            this.Weapon = weapon;
        }

        public override string ToString()
        {
            string outString = this.Killer.Name;

			if (this.HasAssistance)
			{
				outString += String.Format(" (+{0})", this.Assister.Name);
			}

            outString += String.Format(" killed {0}", this.Killed.Name);
            outString += String.Format(" ({0})", this.Weapon);

            if (this.PenetratedObjects > 0)
            {
                outString += String.Format(" (Penetrated {0} object(s))", this.PenetratedObjects);
            }

            return outString; 
        }
    }
}
