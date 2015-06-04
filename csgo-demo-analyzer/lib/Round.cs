using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    public class Round
    {
        public DemoInfo.Team Winner = DemoInfo.Team.Spectate;
        public int RoundNumber = -1;

        public Round()
        {
        }
    }
}
