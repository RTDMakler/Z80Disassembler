using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80Dissassembler
{
    internal class Regfisters
    {
        public static ushort AF { get; set; }
        public ushort BC { get; set; }
        public ushort DE { get; set; }
        public ushort HL { get; set; }


        public ushort SP { get; set; }


        public ushort PC { get; set; }

        public ushort Shadow_AF { get; set; }
        public ushort Shadow_BC { get; set; }
        public ushort Shadow_DE { get; set; }
        public ushort Shadow_HL { get; set; }

        public ushort Shadow_SP { get; set; }
        public ushort Shadow_PC { get; set; }

        public bool CarryFlag { get; set; }
        public bool ZeroFlag { get; set; }
        public bool SignFlag { get; set; }
    }
}
