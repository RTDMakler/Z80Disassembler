namespace Z80Dissassembler.Registers
{
    internal class AF : ISettingRegBytes, IGettingRegBytes
    {
        private ushort af;

        public void SetHL(byte highByte, byte lowByte)
        {
            af = (ushort)((highByte << 8) | lowByte);
        }

        public void SetH(byte highByte)
        {
            af = (ushort)((af & 0x00FF) | (highByte << 8));
        }

        public void SetL(byte lowByte)
        {
            af = (ushort)((af & 0xFF00) | lowByte);
        }

        public void GetHL(out byte highByte, out byte lowByte)
        {
            highByte = (byte)(af >> 8);
            lowByte = (byte)(af & 0xFF);
        }

        public void GetH(out byte highByte)
        {
            highByte = (byte)(af >> 8);
        }

        public void GetL(out byte lowByte)
        {
            lowByte = (byte)(af & 0x0F);
        }
    }
}
