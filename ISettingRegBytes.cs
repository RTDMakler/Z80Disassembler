using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80Dissassembler
{
    internal interface ISettingRegBytes
    {
        void SetL(byte lowByte);

        void SetH(byte highByte);

        void SetHL(byte highByte,byte lowByte);
    }
}
