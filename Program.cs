using System;
using System.IO;
using Z80Dissassembler.Registers;

namespace Z80Dissassembler
{
    class Program
    {
        private static int _pointer;



        static void Main()
        {
            string fileName = "_asm.bin";
            ReadBinaryFileBitwise(fileName);
        }

        static void ReadBinaryFileBitwise(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..", fileName);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    CheckOpcodes(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void CheckOpcodes(FileStream fs)
        {
            fs.Seek(_pointer, SeekOrigin.Begin);
            int readByte = fs.ReadByte();

            switch (readByte)
            {
                case 0x3E:
                    _pointer++;

                    break;
                case 0x02:

                    break;
                default:
                    Console.WriteLine("Missed OpCode");
                    return;
            }
        }
    }

}
