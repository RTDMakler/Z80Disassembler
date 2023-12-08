namespace Z80Dissassembler.Registers
{
    internal class DE : ISettingRegBytes, IGettingRegBytes
    {
        private ushort de;

        public void SetHL(byte highByte, byte lowByte)
        {
            de = (ushort)((highByte << 8) | lowByte);
        }

        public void SetH(byte highByte)
        {
            de = (ushort)((de & 0x00FF) | (highByte << 8));
        }

        public void SetL(byte lowByte)
        {
            de = (ushort)((de & 0xFF00) | lowByte);
        }

        public void GetHL(out byte highByte, out byte lowByte)
        {
            highByte = (byte)(de >> 8);
            lowByte = (byte)(de & 0xFF);
        }

        public void GetH(out byte highByte)
        {
            highByte = (byte)(de >> 8);
        }

        public void GetL(out byte lowByte)
        {
            lowByte = (byte)(de & 0xFF);
        }
    }
}
