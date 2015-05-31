using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgo_demo_analyzer
{
    class Results
    {
        public string Hash;
        public string Map;
        public Dictionary<long, Player> Players;
        public Dictionary<int, Round> Rounds;

        public Results(String hash)
        {
            this.Hash = hash;
            this.Players = new Dictionary<long, Player>();
            this.Rounds = new Dictionary<int, Round>();
        }

    }
}
