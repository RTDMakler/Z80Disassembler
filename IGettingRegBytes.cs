using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80Dissassembler
{
    internal interface IGettingRegBytes
    {
        void GetL(out byte lowByte);

        void GetH(out byte highByte);

        void GetHL(out byte highByte,out byte lowByte);
    }
}
