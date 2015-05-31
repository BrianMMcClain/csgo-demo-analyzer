using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    class Round
    {
        public DemoInfo.Team Winner;

        public Round()
        {
            this.Winner = DemoInfo.Team.Spectate;

        }
    }
}
