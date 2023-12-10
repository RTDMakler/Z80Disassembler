using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80Dissassembler
{
    internal class HexCaseGenerator
    {
        public static void GenerateTxt()
        {
            string fileName = "..\\..\\..\\.."+"GeneratedCases.txt";
            GenerateCasesFile(fileName);
            Console.WriteLine($"File '{fileName}' generated successfully.");
        }

        static void GenerateCasesFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("switch (opcode)");
                writer.WriteLine("{");

                for (int opcode = 0x00; opcode <= 0xFF; opcode++)
                {
                    writer.WriteLine($"    case 0x{opcode:X2}:");
                    writer.WriteLine("        // Handle opcode 0x{opcode:X2}");
                    writer.WriteLine("        break;");
                }

                writer.WriteLine("    default:");
                writer.WriteLine("        // Handle default case (if needed)");
                writer.WriteLine("        break;");
                writer.WriteLine("}");
            }
        }
    }
}
