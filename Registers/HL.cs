using System;

namespace Z80Dissassembler.Registers
{
    internal class HL : ISettingRegBytes, IGettingRegBytes
    {
        private ushort hl;

        public void SetHL(byte highByte, byte lowByte)
        {
            hl = (ushort)((highByte << 8) | lowByte);
        }

        public void SetH(byte highByte)
        {
            hl = (ushort)((hl & 0x00FF) | (highByte << 8));
        }

        public void SetL(byte lowByte)
        {
            hl = (ushort)((hl & 0xFF00) | lowByte);
        }

        public void GetHL(out byte highByte, out byte lowByte)
        {
            highByte = (byte)(hl >> 8);
            lowByte = (byte)(hl & 0xFF);
        }

        public void GetH(out byte highByte)
        {
            highByte = (byte)(hl >> 8);
        }

        public void GetL(out byte lowByte)
        {
            lowByte = (byte)(hl & 0xFF);
        }
    }
}
