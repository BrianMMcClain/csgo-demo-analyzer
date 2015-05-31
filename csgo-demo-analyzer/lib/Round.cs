using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    public class Round
    {
        public DemoInfo.Team Winner;
        public int RoundNumber;

        public Round()
        {
            this.Winner = DemoInfo.Team.Spectate;
            this.RoundNumber = -1;

        }
    }
}
