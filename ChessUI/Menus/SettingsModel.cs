using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessUI
{
    public class SettingsModel
    {
        public int Volume { get; set; } = 50;
        public bool HumanFirst { get; set; } = true;
        public bool IsTimeLimit { get; set; } = true;
        public int TimeLimit { get; set; } = 10;
    }
}
