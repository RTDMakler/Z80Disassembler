using System;
using System.IO;
using Z80Dissassembler.Registers;

namespace Z80Dissassembler
{
    class Program
    {
        private static AF _AF;
        private static DE _DE;
        private static HL _HL;
        private static BC _BC;
        private static ushort _Pointer;

        static Program()
        {
            _AF = new AF();
            _DE = new DE();
            _HL = new HL();
            _BC = new BC();
        }

        static int Main()
        {
            string fileName = "_asm.bin";
            _Pointer = 0x8200;
            ReadBinaryFileBitwise(fileName);
            return 0;
        }

        static void ReadBinaryFileBitwise(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..", fileName);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    Console.WriteLine($"ORG &{_Pointer:X4}");
                    CheckOpcodes(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void CheckOpcodes(BinaryReader reader)
        {
            reader.BaseStream.Seek(_Pointer, SeekOrigin.Begin);
            byte H;
            byte L;
            byte Byte;
            while (true)
            {
                int readByte = reader.ReadByte();
                switch ((byte)readByte)
                {
                    case 0x3E:
                        //_Pointer++;
                        Byte = reader.ReadByte();
                        _AF.SetH(Byte);
                        Console.WriteLine($"\tld A, &{Byte:X2}");
                        break;
                    case 0x21:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        _HL.SetHL(H, L);
                        Console.WriteLine($"\tld HL, &{H:X2}{L:X2}");
                        break;
                    case 0x11:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        _DE.SetHL(H, L);
                        Console.WriteLine($"\tld DE, &{H:X2}{L:X2}");
                        break;
                    case 0x01:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        _BC.SetHL(H, L);
                        Console.WriteLine($"\tld BC, &{H:X2}{L:X2}");
                        break;
                    default:
                        Console.WriteLine("Missed OpCode");
                        return;
                }
            }
        }
    }
}
