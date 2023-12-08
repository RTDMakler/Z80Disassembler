namespace Z80Dissassembler.Registers
{
    internal class BC : ISettingRegBytes, IGettingRegBytes
    {
        private ushort bc;

        public void SetHL(byte highByte, byte lowByte)
        {
            bc = (ushort)((highByte << 8) | lowByte);
        }

        public void SetH(byte highByte)
        {
            bc = (ushort)((bc & 0x00FF) | (highByte << 8));
        }

        public void SetL(byte lowByte)
        {
            bc = (ushort)((bc & 0xFF00) | lowByte);
        }

        public void GetHL(out byte highByte, out byte lowByte)
        {
            highByte = (byte)(bc >> 8);
            lowByte = (byte)(bc & 0xFF);
        }

        public void GetH(out byte highByte)
        {
            highByte = (byte)(bc >> 8);
        }

        public void GetL(out byte lowByte)
        {
            lowByte = (byte)(bc & 0xFF);
        }
    }
}
