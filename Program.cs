using System;
using System.IO;
using System.Reflection.Emit;
using Z80Dissassembler.Registers;

namespace Z80Dissassembler
{
    class Program
    {
        private static AF _AF;
        private static DE _DE;
        private static HL _HL;
        private static BC _BC;
        private static ushort pointer;
        private static ushort maxReadBytes=1000;

        static Program()
        {
            _AF = new AF();
            _DE = new DE();
            _HL = new HL();
            _BC = new BC();
        }

        static int Main(string[] args)
        {
            string fileName = "asm.bin";
            pointer = 0x8000;
            return ReadBinaryFileBitwise(fileName);
        }

        static int ReadBinaryFileBitwise(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..", fileName);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    Console.WriteLine($"ORG &{pointer:X4}");
                    return CheckOpcodes(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1;
            }
        }

        static int CheckOpcodes(BinaryReader reader)
        {
            reader.BaseStream.Seek(pointer, SeekOrigin.Begin);
            byte H;
            byte L;
            byte Byte;
            int amountOfReadBytes = 0;
            while (true)
            {
                
                int readByte = reader.ReadByte();
                if (amountOfReadBytes++ == maxReadBytes || readByte == -1)
                    return 0;
                switch ((byte)readByte)
                {
                    case 0x00:
                        Console.WriteLine("nop");
                        break;
                    case 0x01:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        _BC.SetHL(H, L);
                        Console.WriteLine($"\tld bc, &{H:X2}{L:X2}");
                        break;
                    case 0x02:
                        Console.WriteLine($"\tld (bc),a");
                        break;
                    case 0x03:
                        Console.WriteLine("\tinc bc");
                        break;
                    case 0x04:
                        Console.WriteLine("\tinc b");
                        break;
                    case 0x05:
                        Console.WriteLine("\tdec b");
                        break;
                    case 0x06:
                        Byte  = reader.ReadByte();
                        Console.WriteLine($"\tld b,&{Byte:X2}");
                        break;
                    case 0x07:
                        Console.WriteLine("rlca");
                        break;
                    case 0x08:
                        Console.WriteLine("\tex af,af'");
                        break;
                    case 0x09:
                        Console.WriteLine("\tadd hl,bc");
                        break;
                    case 0x0A:
                        Console.WriteLine("\tld a,(bc)");
                        break;
                    case 0x0B:
                        Console.WriteLine("\tdec bc");
                        break;
                    case 0x0C:
                        Console.WriteLine("\tinc c");
                        break;
                    case 0x0D:
                        Console.WriteLine("\tdec c");
                        break;
                    case 0x0E:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tld c,&{Byte:X2}");
                        break;
                    case 0x0F:
                        Console.WriteLine("\trrca");
                        break;
                    case 0x10:
                        Console.WriteLine("\tdjnz d");
                        break;
                    case 0x11:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        _DE.SetHL(H, L);
                        Console.WriteLine($"\tld DE, &{H:X2}{L:X2}");
                        break;
                    case 0x12:
                        Console.WriteLine("\tld (de),a");
                        break;
                    case 0x13:
                        Console.WriteLine("\tinc de");
                        break;
                    case 0x14:
                        Console.WriteLine("\tinc d");
                        break;
                    case 0x15:
                        Console.WriteLine("\tdec d");
                        break;
                    case 0x16:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tld d,&{Byte:X2}");
                        break;
                    case 0x17:
                        Console.WriteLine($"\trla");
                        break;
                    case 0x18:
                        Console.WriteLine($"\tjr d");
                        break;
                    case 0x19:
                        Console.WriteLine($"\tadd hl, de");
                        break;
                    case 0x1A:
                        Console.WriteLine($"\tld a,(de)");
                        break;
                    case 0x1B:
                        Console.WriteLine($"\tdec de");
                        break;
                    case 0x1C:
                        Console.WriteLine($"\tinc e");
                        break;
                    case 0x1D:
                        Console.WriteLine($"\tdec e");
                        break;
                    case 0x1E:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tld e,&{Byte:X2}");
                        break;
                    case 0x1F:
                        Console.WriteLine($"\trra");
                        break;
                    case 0x20:
                        Console.WriteLine($"\tjr nz,d");
                        break;
                    case 0x21:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        _HL.SetHL(H, L);
                        Console.WriteLine($"\tld hl,&{H:X2}{L:X2}");
                        break;
                    case 0x22:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tld (&{H:X2}{L:X2}),hl");
                        break;
                    case 0x23:
                        Console.WriteLine($"\tinc hl");
                        break;
                    case 0x24:
                        Console.WriteLine($"\tinc h");
                        break;
                    case 0x25:
                        Console.WriteLine($"\tdec h");
                        break;
                    case 0x26:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tld h,&{Byte:X2}");
                        break;
                    case 0x27:
                        Console.WriteLine($"\tdaa");
                        break;
                    case 0x28:
                        Console.WriteLine($"\tjr z,d");
                        break;
                    case 0x29:
                        Console.WriteLine($"\tadd hl,hl");
                        break;
                    case 0x2A:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tld hl,(&{H:X2}{L:X2})");
                        break;
                    case 0x2B:
                        Console.WriteLine($"\tdec hl");
                        break;
                    case 0x2C:
                        Console.WriteLine($"\tinc l");
                        break;
                    case 0x2D:
                        Console.WriteLine($"\tdec l");
                        break;
                    case 0x2E:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tld l,&{Byte:X2}");
                        break;
                    case 0x2F:
                        Console.WriteLine($"\tcpl");
                        break;
                    case 0x30:
                        Console.WriteLine($"\tjr nc,d");
                        break;
                    case 0x31:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tld sp,&{H:X2}{L:X2}");
                        break;
                    case 0x32:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tld (&{H:X2}{L:X2}),a");
                        break;
                    case 0x33:
                        Console.WriteLine($"\tinc sp");
                        break;
                    case 0x34:
                        Console.WriteLine($"\tinc (hl)");
                        break;
                    case 0x35:
                        Console.WriteLine($"\tdec (hl)");
                        break;
                    case 0x36:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tld (hl),&{Byte:X2}");
                        break;
                    case 0x37:
                        Console.WriteLine($"\tscf");
                        break;
                    case 0x38:
                        Console.WriteLine($"\tjr c,d");
                        break;
                    case 0x39:
                        Console.WriteLine($"\tadd hl,sp");
                        break;
                    case 0x3A:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tld a,(&{H:X2}{L:X2})");
                        break;
                    case 0x3B:
                        Console.WriteLine($"\tdec sp");
                        break;
                    case 0x3C:
                        Console.WriteLine($"\tinc a");
                        break;
                    case 0x3D:
                        Console.WriteLine($"\tdec a");
                        break;
                    case 0x3E:
                        Byte = reader.ReadByte();
                        _AF.SetH(Byte);
                        Console.WriteLine($"\tld a,&{Byte:X2}");
                        break;
                    case 0x3F:
                        Console.WriteLine($"\tccf");
                        break;
                    case 0x40:
                        Console.WriteLine($"\tld b,b");
                        break;
                    case 0x41:
                        Console.WriteLine($"\tld b,c");
                        break;
                    case 0x42:
                        Console.WriteLine($"\tld b,d");
                        break;
                    case 0x43:
                        Console.WriteLine($"\tld b,e");
                        break;
                    case 0x44:
                        Console.WriteLine($"\tld b,h");
                        break;
                    case 0x45:
                        Console.WriteLine($"\tld b,l");
                        break;
                    case 0x46:
                        Console.WriteLine($"\tld b,(hl)");
                        break;
                    case 0x47:
                        Console.WriteLine($"\tld b,a");
                        break;
                    case 0x48:
                        Console.WriteLine($"\tld c,b");
                        break;
                    case 0x49:
                        Console.WriteLine($"\tld c,c");
                        break;
                    case 0x4A:
                        Console.WriteLine($"\tld c,d");
                        break;
                    case 0x4B:
                        Console.WriteLine($"\tld c,e");
                        break;
                    case 0x4C:
                        Console.WriteLine($"\tld c,h");
                        break;
                    case 0x4D:
                        Console.WriteLine($"\tld c,l");
                        break;
                    case 0x4E:
                        Console.WriteLine($"\tld c,(hl)");
                        break;
                    case 0x4F:
                        Console.WriteLine($"\tld c,a");
                        break;
                    case 0x50:
                        Console.WriteLine($"\tld d,b");
                        break;
                    case 0x51:
                        Console.WriteLine($"\tld d,c");
                        break;
                    case 0x52:
                        Console.WriteLine($"\tld d,d");
                        break;
                    case 0x53:
                        Console.WriteLine($"\tld d,e");
                        break;
                    case 0x54:
                        Console.WriteLine($"\tld d,h");
                        break;
                    case 0x55:
                        Console.WriteLine($"\tld d,l");
                        break;
                    case 0x56:
                        Console.WriteLine($"\tld d,(hl)");
                        break;
                    case 0x57:
                        Console.WriteLine($"\tld d,a");
                        break;
                    case 0x58:
                        Console.WriteLine($"\tld e,b");
                        break;
                    case 0x59:
                        Console.WriteLine($"\tld e,c");
                        break;
                    case 0x5A:
                        Console.WriteLine($"\tld e,d");
                        break;
                    case 0x5B:
                        Console.WriteLine($"\tld e,e");
                        break;
                    case 0x5C:
                        Console.WriteLine($"\tld e,h");
                        break;
                    case 0x5D:
                        Console.WriteLine($"\tld e,l");
                        break;
                    case 0x5E:
                        Console.WriteLine($"\tld e,(hl)");
                        break;
                    case 0x5F:
                        Console.WriteLine($"\tld e,a");
                        break;
                    case 0x60:
                        Console.WriteLine($"\tld h,b");
                        break;
                    case 0x61:
                        Console.WriteLine($"\tld h,c");
                        break;
                    case 0x62:
                        Console.WriteLine($"\tld h,d");
                        break;
                    case 0x63:
                        Console.WriteLine($"\tld h,e");
                        break;
                    case 0x64:
                        Console.WriteLine($"\tld h,h");
                        break;
                    case 0x65:
                        Console.WriteLine($"\tld h,l");
                        break;
                    case 0x66:
                        Console.WriteLine($"\tld h,(hl)");
                        break;
                    case 0x67:
                        Console.WriteLine($"\tld h,a");
                        break;
                    case 0x68:
                        Console.WriteLine($"\tld l,b");
                        break;
                    case 0x69:
                        Console.WriteLine($"\tld l,c");
                        break;
                    case 0x6A:
                        Console.WriteLine($"\tld l,d");
                        break;
                    case 0x6B:
                        Console.WriteLine($"\tld l,e");
                        break;
                    case 0x6C:
                        Console.WriteLine($"\tld l,h");
                        break;
                    case 0x6D:
                        Console.WriteLine($"\tld l,l");
                        break;
                    case 0x6E:
                        Console.WriteLine($"\tld l,(hl)");
                        break;
                    case 0x6F:
                        Console.WriteLine($"\tld l,a");
                        break;
                    case 0x70:
                        Console.WriteLine($"\tld (hl),b");
                        break;
                    case 0x71:
                        Console.WriteLine($"\tld (hl),c");
                        break;
                    case 0x72:
                        Console.WriteLine($"\tld (hl),d");
                        break;
                    case 0x73:
                        Console.WriteLine($"\tld (hl),e");
                        break;
                    case 0x74:
                        Console.WriteLine($"\tld (hl),h");
                        break;
                    case 0x75:
                        Console.WriteLine($"\tld (hl),l");
                        break;
                    case 0x76:
                        Console.WriteLine($"\thalt");
                        break;
                    case 0x77:
                        //ld (hl), a
                        Console.WriteLine($"\tld (hl),a");
                        break;
                    case 0x78:
                        Console.WriteLine($"\tld a,b");
                        break;
                    case 0x79:
                        Console.WriteLine($"\tld a,c");
                        break;
                    case 0x7A:
                        Console.WriteLine($"\tld a,d");
                        break;
                    case 0x7B:
                        Console.WriteLine($"\tld a,e");
                        break;
                    case 0x7C:
                        Console.WriteLine($"\tld a,h");
                        break;
                    case 0x7D:
                        Console.WriteLine($"\tld a,l");
                        break;
                    case 0x7E:
                        Console.WriteLine($"\tld a,(hl)");
                        break;
                    case 0x7F:
                        Console.WriteLine($"\tld a,a");
                        break;
                    case 0x80:
                        Console.WriteLine($"\tadd a,b");
                        break;
                    case 0x81:
                        Console.WriteLine($"\tadd a,c");
                        break;
                    case 0x82:
                        Console.WriteLine($"\tadd a,d");
                        break;
                    case 0x83:
                        Console.WriteLine($"\tadd a,e");
                        break;
                    case 0x84:
                        Console.WriteLine($"\tadd a,h");
                        break;
                    case 0x85:
                        Console.WriteLine($"\tadd a,l");
                        break;
                    case 0x86:
                        Console.WriteLine($"\tadd a,(hl)");
                        break;
                    case 0x87:
                        Console.WriteLine($"\tadd a,a");
                        break;
                    case 0x88:
                        Console.WriteLine($"\tadc a,b");
                        break;
                    case 0x89:
                        Console.WriteLine($"\tadc a,c");
                        break;
                    case 0x8A:
                        Console.WriteLine($"\tadc a,d");
                        break;
                    case 0x8B:
                        Console.WriteLine($"\tadc a,e");
                        break;
                    case 0x8C:
                        Console.WriteLine($"\tadc a,h");
                        break;
                    case 0x8D:
                        Console.WriteLine($"\tadc a,l");
                        break;
                    case 0x8E:
                        Console.WriteLine($"\tadc a,(hl)");
                        break;
                    case 0x8F:
                        Console.WriteLine($"\tadc a,a");
                        break;
                    case 0x90:
                        Console.WriteLine($"\tsub b");
                        break;
                    case 0x91:
                        Console.WriteLine($"\tsub c");
                        break;
                    case 0x92:
                        Console.WriteLine($"\tsub d");
                        break;
                    case 0x93:
                        Console.WriteLine($"\tsub e");
                        break;
                    case 0x94:
                        Console.WriteLine($"\tsub h");
                        break;
                    case 0x95:
                        Console.WriteLine($"\tsub l");
                        break;
                    case 0x96:
                        Console.WriteLine($"\tsub (hl)");
                        break;
                    case 0x97:
                        Console.WriteLine($"\tsub a");
                        break;
                    case 0x98:
                        Console.WriteLine($"\tsbc a,b");
                        break;
                    case 0x99:
                        Console.WriteLine($"\tsbc a,c");
                        break;
                    case 0x9A:
                        Console.WriteLine($"\tsbc a,d");
                        break;
                    case 0x9B:
                        Console.WriteLine($"\tsbc a,e");
                        break;
                    case 0x9C:
                        Console.WriteLine($"\tsbc a,h");
                        break;
                    case 0x9D:
                        Console.WriteLine($"\tsbc a,l");
                        break;
                    case 0x9E:
                        Console.WriteLine($"\tsbc a,(hl)");
                        break;
                    case 0x9F:
                        Console.WriteLine($"\tsbc a,a");
                        break;
                    case 0xA0:
                        Console.WriteLine($"\tand b");
                        break;
                    case 0xA1:
                        Console.WriteLine($"\tand c");
                        break;
                    case 0xA2:
                        Console.WriteLine($"\tand d");
                        break;
                    case 0xA3:
                        Console.WriteLine($"\tand e");
                        break;
                    case 0xA4:
                        Console.WriteLine($"\tand h");
                        break;
                    case 0xA5:
                        Console.WriteLine($"\tand l");
                        break;
                    case 0xA6:
                        Console.WriteLine($"\tand (hl)");
                        break;
                    case 0xA7:
                        Console.WriteLine($"\tand a");
                        break;
                    case 0xA8:
                        Console.WriteLine($"\txor b");
                        break;
                    case 0xA9:
                        Console.WriteLine($"\txor c");
                        break;
                    case 0xAA:
                        Console.WriteLine($"\txor d");
                        break;
                    case 0xAB:
                        Console.WriteLine($"\txor e");
                        break;
                    case 0xAC:
                        Console.WriteLine($"\txor h");
                        break;
                    case 0xAD:
                        Console.WriteLine($"\txor l");
                        break;
                    case 0xAE:
                        Console.WriteLine($"\txor (hl)");
                        break;
                    case 0xAF:
                        Console.WriteLine($"\txor a");
                        break;
                    case 0xB0:
                        Console.WriteLine($"\tor b");
                        break;
                    case 0xB1:
                        Console.WriteLine($"\tor c");
                        break;
                    case 0xB2:
                        Console.WriteLine($"\tor d");
                        break;
                    case 0xB3:
                        Console.WriteLine($"\tor e");
                        break;
                    case 0xB4:
                        Console.WriteLine($"\tor h");
                        break;
                    case 0xB5:
                        Console.WriteLine($"\tor l");
                        break;
                    case 0xB6:
                        Console.WriteLine($"\tor (hl)");
                        break;
                    case 0xB7:
                        Console.WriteLine($"\tor a");
                        break;
                    case 0xB8:
                        Console.WriteLine($"\tcp b");
                        break;
                    case 0xB9:
                        Console.WriteLine($"\tcp c");
                        break;
                    case 0xBA:
                        Console.WriteLine($"\tcp d");
                        break;
                    case 0xBB:
                        Console.WriteLine($"\tcp e");
                        break;
                    case 0xBC:
                        Console.WriteLine($"\tcp h");
                        break;
                    case 0xBD:
                        Console.WriteLine($"\tcp l");
                        break;
                    case 0xBE:
                        Console.WriteLine($"\tcp (hl)");
                        break;
                    case 0xBF:
                        Console.WriteLine($"\tcp a");
                        break;
                    case 0xC0:
                        Console.WriteLine($"\tret nz");
                        break;
                    case 0xC1:
                        Console.WriteLine($"\tpop bc");
                        break;
                    case 0xC2:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp nz, &{H:X2}{L:X2}");
                        break;
                    case 0xC3:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp &{H:X2}{L:X2}");
                        break;
                    case 0xC4:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall nz,&{H:X2}{L:X2}");
                        break;
                    case 0xC5:
                        Console.WriteLine($"\tpush bc");
                        break;
                    case 0xC6:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tadd a,&{Byte:X2}");
                        break;
                    case 0xC7:
                        Console.WriteLine($"\trst 00h");
                        break;
                    case 0xC8:
                        Console.WriteLine($"\tret z");
                        break;
                    case 0xC9:
                        Console.WriteLine("ret");
                        //return 0;
                        break;
                    case 0xCA:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp z,&{H:X2}{L:X2}");
                        break;
                    case 0xCB:
                        switch(reader.ReadByte())
                        {
                            case 0x00:
                                Console.WriteLine("rlc b");
                                break;
                            case 0x01:
                                Console.WriteLine("rlc c");
                                break;
                            case 0x02:
                                Console.WriteLine("rlc d");
                                break;
                            case 0x03:
                                Console.WriteLine("rlc e");
                                break;
                            case 0x04:
                                Console.WriteLine("rlc h");
                                break;
                            case 0x05:
                                Console.WriteLine("rlc l");
                                break;
                            case 0x06:
                                Console.WriteLine("rlc (hl)");
                                break;
                            case 0x07:
                                Console.WriteLine("rlc a");
                                break;
                            case 0x08:
                                Console.WriteLine("rrc b");
                                break;
                            case 0x09:
                                Console.WriteLine("rrc c");
                                break;
                            case 0x0A:
                                Console.WriteLine("rrc d");
                                break;
                            case 0x0B:
                                Console.WriteLine("rrc e");
                                break;
                            case 0x0C:
                                Console.WriteLine("rrc h");
                                break;
                            case 0x0D:
                                Console.WriteLine("rrc l");
                                break;
                            case 0x0E:
                                Console.WriteLine("rrc (hl)");
                                break;
                            case 0x0F:
                                Console.WriteLine("rrc a");
                                break;
                            case 0x10:
                                Console.WriteLine("rl b");
                                break;
                            case 0x11:
                                Console.WriteLine("rl c");
                                break;
                            case 0x12:
                                Console.WriteLine("rl d");
                                break;
                            case 0x13:
                                Console.WriteLine("rl e");
                                break;
                            case 0x14:
                                Console.WriteLine("rl h");
                                break;
                            case 0x15:
                                Console.WriteLine("rl l");
                                break;
                            case 0x16:
                                Console.WriteLine("rl (hl)");
                                break;
                            case 0x17:
                                Console.WriteLine("rl a");
                                break;
                            case 0x18:
                                Console.WriteLine("rr b");
                                break;
                            case 0x19:
                                Console.WriteLine("rr c");
                                break;
                            case 0x1A:
                                Console.WriteLine("rr d");
                                break;
                            case 0x1B:
                                Console.WriteLine("rr e");
                                break;
                            case 0x1C:
                                Console.WriteLine("rr h");
                                break;
                            case 0x1D:
                                Console.WriteLine("rr l");
                                break;
                            case 0x1E:
                                Console.WriteLine("rr (hl)");
                                break;
                            case 0x1F:
                                Console.WriteLine("rr a");
                                break;
                            case 0x20:
                                Console.WriteLine("sla b");
                                break;
                            case 0x21:
                                Console.WriteLine("sla c");
                                break;
                            case 0x22:
                                Console.WriteLine("sla d");
                                break;
                            case 0x23:
                                Console.WriteLine("sla e");
                                break;
                            case 0x24:
                                Console.WriteLine("sla h");
                                break;
                            case 0x25:
                                Console.WriteLine("sla l");
                                break;
                            case 0x26:
                                Console.WriteLine("sla (hl)");
                                break;
                            case 0x27:
                                Console.WriteLine("sla a");
                                break;
                            case 0x28:
                                Console.WriteLine("sra b");
                                break;
                            case 0x29:
                                Console.WriteLine("sra c");
                                break;
                            case 0x2A:
                                Console.WriteLine("sra d");
                                break;
                            case 0x2B:
                                Console.WriteLine("sra e");
                                break;
                            case 0x2C:
                                Console.WriteLine("sra h");
                                break;
                            case 0x2D:
                                Console.WriteLine("sra l");
                                break;
                            case 0x2E:
                                Console.WriteLine("sra (hl)");
                                break;
                            case 0x2F:
                                Console.WriteLine("sra a");
                                break;
                            //8 undocumented OpCodes
                            case 0x38:
                                Console.WriteLine("srl b");
                                break;
                            case 0x39:
                                Console.WriteLine("srl c");
                                break;
                            case 0x3A:
                                Console.WriteLine("srl d");
                                break;
                            case 0x3B:
                                Console.WriteLine("srl e");
                                break;
                            case 0x3C:
                                Console.WriteLine("srl h");
                                break;
                            case 0x3D:
                                Console.WriteLine("srl l");
                                break;
                            case 0x3E:
                                Console.WriteLine("srl (hl)");
                                break;
                            case 0x3F:
                                Console.WriteLine("srl a");
                                break;
                            case 0x40:
                                Console.WriteLine("bit 0,b");
                                break;
                            case 0x41:
                                Console.WriteLine("bit 0,c");
                                break;
                            case 0x42:
                                Console.WriteLine("bit 0,d");
                                break;
                            case 0x43:
                                Console.WriteLine("bit 0,e");
                                break;
                            case 0x44:
                                Console.WriteLine("bit 0,h");
                                break;
                            case 0x45:
                                Console.WriteLine("bit 0,1");
                                break;
                            case 0x46:
                                Console.WriteLine("bit 0,(hl)");
                                break;
                            case 0x47:
                                Console.WriteLine("bit 0,a");
                                break;
                            case 0x48:
                                Console.WriteLine("bit 1,b");
                                break;
                            case 0x49:
                                Console.WriteLine("bit 1,c");
                                break;
                            case 0x4A:
                                Console.WriteLine("bit 1,d");
                                break;
                            case 0x4B:
                                Console.WriteLine("bit 1,e");
                                break;
                            case 0x4C:
                                Console.WriteLine("bit 1,h");
                                break;
                            case 0x4D:
                                Console.WriteLine("bit 1,l");
                                break;
                            case 0x4E:
                                Console.WriteLine("bit 1,(hl)");
                                break;
                            case 0x4F:
                                Console.WriteLine("bit 1,a");
                                break;
                            case 0x50:
                                Console.WriteLine("bit 2,b");
                                break;
                            case 0x51:
                                Console.WriteLine("bit 2,c");
                                break;
                            case 0x52:
                                Console.WriteLine("bit 2,d");
                                break;
                            case 0x53:
                                Console.WriteLine("bit 2,e");
                                break;
                            case 0x54:
                                Console.WriteLine("bit 2,h");
                                break;
                            case 0x55:
                                Console.WriteLine("bit 2,l");
                                break;
                            case 0x56:
                                Console.WriteLine("bit 2,(hl)");
                                break;
                            case 0x57:
                                Console.WriteLine("bit 2,a");
                                break;
                            case 0x58:
                                Console.WriteLine("bit 3,b");
                                break;
                            case 0x59:
                                Console.WriteLine("bit 3,c");
                                break;
                            case 0x5A:
                                Console.WriteLine("bit 3,d");
                                break;
                            case 0x5B:
                                Console.WriteLine("bit 3,e");
                                break;
                            case 0x5C:
                                Console.WriteLine("bit 3,h");
                                break;
                            case 0x5D:
                                Console.WriteLine("bit 3,l");
                                break;
                            case 0x5E:
                                Console.WriteLine("bit 3,(hl)");
                                break;
                            case 0x5F:
                                Console.WriteLine("bit 3,a");
                                break;
                            case 0x60:
                                Console.WriteLine("bit 4,b");
                                break;
                            case 0x61:
                                Console.WriteLine("bit 4,c");
                                break;
                            case 0x62:
                                Console.WriteLine("bit 4,d");
                                break;
                            case 0x63:
                                Console.WriteLine("bit 4,e");
                                break;
                            case 0x64:
                                Console.WriteLine("bit 4,h");
                                break;
                            case 0x65:
                                Console.WriteLine("bit 4,l");
                                break;
                            case 0x66:
                                Console.WriteLine("bit 4,(hl)");
                                break;
                            case 0x67:
                                Console.WriteLine("bit 4,a");
                                break;
                            case 0x68:
                                Console.WriteLine("bit 5,b");
                                break;
                            case 0x69:
                                Console.WriteLine("bit 5,c");
                                break;
                            case 0x6A:
                                Console.WriteLine("bit 5,d");
                                break;
                            case 0x6B:
                                Console.WriteLine("bit 5,e");
                                break;
                            case 0x6C:
                                Console.WriteLine("bit 5,h");
                                break;
                            case 0x6D:
                                Console.WriteLine("bit 5,l");
                                break;
                            case 0x6E:
                                Console.WriteLine("bit 5,(hl)");
                                break;
                            case 0x6F:
                                Console.WriteLine("bit 5,a");
                                break;
                            case 0x70:
                                Console.WriteLine("bit 6,b");
                                break;
                            case 0x71:
                                Console.WriteLine("bit 6,c");
                                break;
                            case 0x72:
                                Console.WriteLine("bit 6,d");
                                break;
                            case 0x73:
                                Console.WriteLine("bit 6,e");
                                break;
                            case 0x74:
                                Console.WriteLine("bit 6,h");
                                break;
                            case 0x75:
                                Console.WriteLine("bit 6,l");
                                break;
                            case 0x76:
                                Console.WriteLine("bit 6,(hl)");
                                break;
                            case 0x77:
                                Console.WriteLine("bit 6,a");
                                break;
                            case 0x78:
                                Console.WriteLine("bit 7,b");
                                break;
                            case 0x79:
                                Console.WriteLine("bit 7,c");
                                break;
                            case 0x7A:
                                Console.WriteLine("bit 7,d");
                                break;
                            case 0x7B:
                                Console.WriteLine("bit 7,e");
                                break;
                            case 0x7C:
                                Console.WriteLine("bit 7,h");
                                break;
                            case 0x7D:
                                Console.WriteLine("bit 7,l");
                                break;
                            case 0x7E:
                                Console.WriteLine("bit 7,(hl)");
                                break;
                            case 0x7F:
                                Console.WriteLine("bit 7,a");
                                break;
                            case 0x80:
                                Console.WriteLine("res 0,b");
                                break;
                            case 0x81:
                                Console.WriteLine("res 0,c");
                                break;
                            case 0x82:
                                Console.WriteLine("res 0,d");
                                break;
                            case 0x83:
                                Console.WriteLine("res 0,e");
                                break;
                            case 0x84:
                                Console.WriteLine("res 0,h");
                                break;
                            case 0x85:
                                Console.WriteLine("res 0,l");
                                break;
                            case 0x86:
                                Console.WriteLine("res 0,(hl)");
                                break;
                            case 0x87:
                                Console.WriteLine("res 0,a");
                                break;
                            case 0x88:
                                Console.WriteLine("res 1,b");
                                break;
                            case 0x89:
                                Console.WriteLine("res 1,c");
                                break;
                            case 0x8A:
                                Console.WriteLine("res 1,d");
                                break;
                            case 0x8B:
                                Console.WriteLine("res 1,e");
                                break;
                            case 0x8C:
                                Console.WriteLine("res 1,h");
                                break;
                            case 0x8D:
                                Console.WriteLine("res 1,l");
                                break;
                            case 0x8E:
                                Console.WriteLine("res 1,(hl)");
                                break;
                            case 0x8F:
                                Console.WriteLine("res 1,a");
                                break;
                            case 0x90:
                                Console.WriteLine("res 2,b");
                                break;
                            case 0x91:
                                Console.WriteLine("res 2,c");
                                break;
                            case 0x92:
                                Console.WriteLine("res 2,d");
                                break;
                            case 0x93:
                                Console.WriteLine("res 2,e");
                                break;
                            case 0x94:
                                Console.WriteLine("res 2,h");
                                break;
                            case 0x95:
                                Console.WriteLine("res 2,l");
                                break;
                            case 0x96:
                                Console.WriteLine("res 2,(hl)");
                                break;
                            case 0x97:
                                Console.WriteLine("res 2,a");
                                break;
                            case 0x98:
                                Console.WriteLine("res 3,b");
                                break;
                            case 0x99:
                                Console.WriteLine("res 3,c");
                                break;
                            case 0x9A:
                                Console.WriteLine("res 3,d");
                                break;
                            case 0x9B:
                                Console.WriteLine("res 3,e");
                                break;
                            case 0x9C:
                                Console.WriteLine("res 3,h");
                                break;
                            case 0x9D:
                                Console.WriteLine("res 3,l");
                                break;
                            case 0x9E:
                                Console.WriteLine("res 3,(hl)");
                                break;
                            case 0x9F:
                                Console.WriteLine("res 3,a");
                                break;
                            case 0xA0:
                                Console.WriteLine("res 4,b");
                                break;
                            case 0xA1:
                                Console.WriteLine("res 4,c");
                                break;
                            case 0xA2:
                                Console.WriteLine("res 4,d");
                                break;
                            case 0xA3:
                                Console.WriteLine("res 4,e");
                                break;
                            case 0xA4:
                                Console.WriteLine("res 4,h");
                                break;
                            case 0xA5:
                                Console.WriteLine("res 4,l");
                                break;
                            case 0xA6:
                                Console.WriteLine("res 4,(hl)");
                                break;
                            case 0xA7:
                                Console.WriteLine("res 4,a");
                                break;
                            case 0xA8:
                                Console.WriteLine("res 5,b");
                                break;
                            case 0xA9:
                                Console.WriteLine("res 5,c");
                                break;
                            case 0xAA:
                                Console.WriteLine("res 5,d");
                                break;
                            case 0xAB:
                                Console.WriteLine("res 5,e");
                                break;
                            case 0xAC:
                                Console.WriteLine("res 5,h");
                                break;
                            case 0xAD:
                                Console.WriteLine("res 5,l");
                                break;
                            case 0xAE:
                                Console.WriteLine("res 5,(hl)");
                                break;
                            case 0xAF:
                                Console.WriteLine("res 5,a");
                                break;
                            case 0xB0:
                                Console.WriteLine("res 6,b");
                                break;
                            case 0xB1:
                                Console.WriteLine("res 6,c");
                                break;
                            case 0xB2:
                                Console.WriteLine("res 6,d");
                                break;
                            case 0xB3:
                                Console.WriteLine("res 6,e");
                                break;
                            case 0xB4:
                                Console.WriteLine("res 6,h");
                                break;
                            case 0xB5:
                                Console.WriteLine("res 6,l");
                                break;
                            case 0xB6:
                                Console.WriteLine("res 6,(hl)");
                                break;
                            case 0xB7:
                                Console.WriteLine("res 6,a");
                                break;
                            case 0xB8:
                                Console.WriteLine("res 7,b");
                                break;
                            case 0xB9:
                                Console.WriteLine("res 7,c");
                                break;
                            case 0xBA:
                                Console.WriteLine("res 7,d");
                                break;
                            case 0xBB:
                                Console.WriteLine("res 7,e");
                                break;
                            case 0xBC:
                                Console.WriteLine("res 7,h");
                                break;
                            case 0xBD:
                                Console.WriteLine("res 7,l");
                                break;
                            case 0xBE:
                                Console.WriteLine("res 7,(hl)");
                                break;
                            case 0xBF:
                                Console.WriteLine("res 7,a");
                                break;
                            case 0xC0:
                                Console.WriteLine("set 0,b");
                                break;
                            case 0xC1:
                                Console.WriteLine("set 0,c");
                                break;
                            case 0xC2:
                                Console.WriteLine("set 0,d");
                                break;
                            case 0xC3:
                                Console.WriteLine("set 0,e");
                                break;
                            case 0xC4:
                                Console.WriteLine("set 0,h");
                                break;
                            case 0xC5:
                                Console.WriteLine("set 0,l");
                                break;
                            case 0xC6:
                                Console.WriteLine("set 0,(hl)");
                                break;
                            case 0xC7:
                                Console.WriteLine("set 0,a");
                                break;
                            case 0xC8:
                                Console.WriteLine("set 1,b");
                                break;
                            case 0xC9:
                                Console.WriteLine("set 1,c");
                                break;
                            case 0xCA:
                                Console.WriteLine("set 1,d");
                                break;
                            case 0xCB:
                                Console.WriteLine("set 1,e");
                                break;
                            case 0xCC:
                                Console.WriteLine("set 1,h");
                                break;
                            case 0xCD:
                                Console.WriteLine("set 1,l");
                                break;
                            case 0xCE:
                                Console.WriteLine("set 1,(hl)");
                                break;
                            case 0xCF:
                                Console.WriteLine("set 1,a");
                                break;
                            case 0xD0:
                                Console.WriteLine("set 2,b");
                                break;
                            case 0xD1:
                                Console.WriteLine("set 2,c");
                                break;
                            case 0xD2:
                                Console.WriteLine("set 2,d");
                                break;
                            case 0xD3:
                                Console.WriteLine("set 2,e");
                                break;
                            case 0xD4:
                                Console.WriteLine("set 2,h");
                                break;
                            case 0xD5:
                                Console.WriteLine("set 2,l");
                                break;
                            case 0xD6:
                                Console.WriteLine("set 2,(hl)");
                                break;
                            case 0xD7:
                                Console.WriteLine("set 2,a");
                                break;
                            case 0xD8:
                                Console.WriteLine("set 3,b");
                                break;
                            case 0xD9:
                                Console.WriteLine("set 3,c");
                                break;
                            case 0xDA:
                                Console.WriteLine("set 3,d");
                                break;
                            case 0xDB:
                                Console.WriteLine("set 3,e");
                                break;
                            case 0xDC:
                                Console.WriteLine("set 3,h");
                                break;
                            case 0xDD:
                                Console.WriteLine("set 3,l");
                                break;
                            case 0xDE:
                                Console.WriteLine("set 3,(hl)");
                                break;
                            case 0xDF:
                                Console.WriteLine("set 3,a");
                                break;
                            case 0xE0:
                                Console.WriteLine("set 4,b");
                                break;
                            case 0xE1:
                                Console.WriteLine("set 4,c");
                                break;
                            case 0xE2:
                                Console.WriteLine("set 4,d");
                                break;
                            case 0xE3:
                                Console.WriteLine("set 4,e");
                                break;
                            case 0xE4:
                                Console.WriteLine("set 4,h");
                                break;
                            case 0xE5:
                                Console.WriteLine("set 4,l");
                                break;
                            case 0xE6:
                                Console.WriteLine("set 4,(hl)");
                                break;
                            case 0xE7:
                                Console.WriteLine("set 4,a");
                                break;
                            case 0xE8:
                                Console.WriteLine("set 5,b");
                                break;
                            case 0xE9:
                                Console.WriteLine("set 5,c");
                                break;
                            case 0xEA:
                                Console.WriteLine("set 5,d");
                                break;
                            case 0xEB:
                                Console.WriteLine("set 5,e");
                                break;
                            case 0xEC:
                                Console.WriteLine("set 5,h");
                                break;
                            case 0xED:
                                Console.WriteLine("set 5,l");
                                break;
                            case 0xEE:
                                Console.WriteLine("set 5,(hl)");
                                break;
                            case 0xEF:
                                Console.WriteLine("set 5,a");
                                break;
                            case 0xF0:
                                Console.WriteLine("set 6,b");
                                break;
                            case 0xF1:
                                Console.WriteLine("set 6,c");
                                break;
                            case 0xF2:
                                Console.WriteLine("set 6,d");
                                break;
                            case 0xF3:
                                Console.WriteLine("set 6,e");
                                break;
                            case 0xF4:
                                Console.WriteLine("set 6,h");
                                break;
                            case 0xF5:
                                Console.WriteLine("set 6,l");
                                break;
                            case 0xF6:
                                Console.WriteLine("set 6,(hl)");
                                break;
                            case 0xF7:
                                Console.WriteLine("set 6,a");
                                break;
                            case 0xF8:
                                Console.WriteLine("set 7,b");
                                break;
                            case 0xF9:
                                Console.WriteLine("set 7,c");
                                break;
                            case 0xFA:
                                Console.WriteLine("set 7,d");
                                break;
                            case 0xFB:
                                Console.WriteLine("set 7,e");
                                break;
                            case 0xFC:
                                Console.WriteLine("set 7,h");
                                break;
                            case 0xFD:
                                Console.WriteLine("set 7,l");
                                break;
                            case 0xFE:
                                Console.WriteLine("set 7,(hl)");
                                break;
                            case 0xFF:
                                Console.WriteLine("set 7,a");
                                break;
                            default:
                                Console.WriteLine("Undocumented OpCode. Close.");
                                return -1;
                        }
                        break;
                    case 0xCC:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall z,&{H:X2}{L:X2}");
                        break;
                    case 0xCD:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall &{H:X2}{L:X2}");
                        break;
                    case 0xCE:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tadc a,&{Byte:X2}");
                        break;
                    case 0xCF:
                        Console.WriteLine("\trst 08h");
                        break;
                    case 0xD0:
                        Console.WriteLine("\tret nc");
                        break;
                    case 0xD1:
                        Console.WriteLine("\tpop de");
                        break;
                    case 0xD2:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp nc,&{H:X2}{L:X2}");
                        break;
                    case 0xD3:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tout (&{Byte:X2}),a");
                        break;
                    case 0xD4:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall nc,&{H:X2}{L:X2}");
                        break;
                    case 0xD5:
                        Console.WriteLine("\tpush de");
                        break;
                    case 0xD6:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tsub &{Byte:X2}");
                        break;
                    case 0xD7:
                        Console.WriteLine("\trst 10h");
                        break;
                    case 0xD8:
                        Console.WriteLine("\tret c");
                        break;
                    case 0xD9:
                        Console.WriteLine("\texx");
                        break;
                    case 0xDA:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp c,&{H:X2}{L:X2}");
                        break;
                    case 0xDB:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tin a,(&{Byte:X2})");
                        break;
                    case 0xDC:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall c,&{H:X2}{L:X2}");
                        break;
                    case 0xDD:
                        switch(reader.ReadByte())
                        {
                            case 0x09:
                                Console.WriteLine("\tadd ix,bc");
                                break;
                            case 0x19:
                                Console.WriteLine("\tadd ix,de");
                                break;
                            case 0x21:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld ix,&{H:X2}{L:X2}");
                                break;
                            case 0x22:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld (&{H:X2}{L:X2}),ix");
                                break;
                            case 0x23:
                                Console.WriteLine("\tinc ix");
                                break;
                            case 0x29:
                                Console.WriteLine("\tadd ix,ix");
                                break;
                            case 0x2A:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld ix,(&{H:X2}{L:X2})");
                                break;
                            case 0x2B:
                                Console.WriteLine("\tdec ix");
                                break;
                            case 0x34:
                                Console.WriteLine("\tinc (ix+d)");
                                break;
                            case 0x35:
                                Console.WriteLine("\tdec (ix+d)");
                                break;
                            case 0x36:
                                Byte = reader.ReadByte();
                                Console.WriteLine($"\tld (ix+d),&{Byte:X2}");
                                break;
                            case 0x39:
                                Console.WriteLine("\tadd ix,sp");
                                break;
                            case 0x46:
                                Console.WriteLine("\tld b,(ix+d)");
                                break;
                            case 0x4E:
                                Console.WriteLine("\tld c,(ix+d)");
                                break;
                            case 0x56:
                                Console.WriteLine("\tld d,(ix+d)");
                                break;
                            case 0x5E:
                                Console.WriteLine("\tld e,(ix+d)");
                                break;
                            case 0x66:
                                Console.WriteLine("\tld h,(ix+d)");
                                break;
                            case 0x6E:
                                Console.WriteLine("\tld l,(ix+d)");
                                break;
                            case 0x70:
                                Console.WriteLine("\tld (ix+d),b");
                                break;
                            case 0x71:
                                Console.WriteLine("\tld (ix+d),c");
                                break;
                            case 0x72:
                                Console.WriteLine("\tld (ix+d),d");
                                break;
                            case 0x73:
                                Console.WriteLine("\tld (ix+d),e");
                                break;
                            case 0x74:
                                Console.WriteLine("\tld (ix+d),h");
                                break;
                            case 0x75:
                                Console.WriteLine("\tld (ix+d),l");
                                break;
                            case 0x77:
                                Console.WriteLine("\tld (ix+d),a");
                                break;
                            case 0x7E:
                                Console.WriteLine("\tld a,(ix+d)");
                                break;
                            case 0x86:
                                Console.WriteLine("\tadd a,(ix+d)");
                                break;
                            case 0x8E:
                                Console.WriteLine("\tadc a,(ix+d)");
                                break;
                            case 0x96:
                                Console.WriteLine("\tsub (ix+d)");
                                break;
                            case 0x9E:
                                Console.WriteLine("\tsbc a,(ix+d)");
                                break;
                            case 0xA6:
                                Console.WriteLine("\tand (ix+d)");
                                break;
                            case 0xAE:
                                Console.WriteLine("\txor (ix+d)");
                                break;
                            case 0xB6:
                                Console.WriteLine("\tor (ix+d)");
                                break;
                            case 0xBE:
                                Console.WriteLine("\tcp (ix+d)");
                                break;
                            case 0xCB:
                                switch(reader.ReadByte())
                                {
                                    case 0x06:
                                        Console.WriteLine("\trlc (ix+d)");
                                        break;
                                    case 0x16:
                                        Console.WriteLine("\trl (ix+d)");
                                        break;
                                    case 0x26:
                                        Console.WriteLine("\tsla (ix+d)");
                                        break;
                                    case 0x46:
                                        Console.WriteLine("\tbit 0,(ix+d)");
                                        break;
                                    case 0x56:
                                        Console.WriteLine("\tbit 2,(ix+d)");
                                        break;
                                    case 0x66:
                                        Console.WriteLine("\tbit 4,(ix+d)");
                                        break;
                                    case 0x76:
                                        Console.WriteLine("\tbit 6,(ix+d)");
                                        break;
                                    case 0x86:
                                        Console.WriteLine("\tres 0,(ix+d)");
                                        break;
                                    case 0x96:
                                        Console.WriteLine("\tres 2,(ix+d)");
                                        break;
                                    case 0xA6:
                                        Console.WriteLine("\tres 4,(ix+d)");
                                        break;
                                    case 0xB6:
                                        Console.WriteLine("\tres 6,(ix+d)");
                                        break;
                                    case 0xC6:
                                        Console.WriteLine("\tset 0,(ix+d)");
                                        break;
                                    case 0xD6:
                                        Console.WriteLine("\tset 2,(ix+d)");
                                        break;
                                    case 0xE6:
                                        Console.WriteLine("\tset 4,(ix+d)");
                                        break;
                                    case 0xF6:
                                        Console.WriteLine("\tset 6,(ix+d)");
                                        break;
                                    case 0x0E:
                                        Console.WriteLine("\trrc (ix+d)");
                                        break;
                                    case 0x1E:
                                        Console.WriteLine("\trr (ix+d)");
                                        break;
                                    case 0x2E:
                                        Console.WriteLine("\tsra (ix+d)");
                                        break;
                                    case 0x3E:
                                        Console.WriteLine("\tsrl (ix+d)");
                                        break;
                                    case 0x4E:
                                        Console.WriteLine("\tbit 1,(ix+d)");
                                        break;
                                    case 0x5E:
                                        Console.WriteLine("\tbit 3,(ix+d)");
                                        break;
                                    case 0x6E:
                                        Console.WriteLine("\tbit 5,(ix+d)");
                                        break;
                                    case 0x7E:
                                        Console.WriteLine("\tbit 7,(ix+d)");
                                        break;
                                    case 0x8E:
                                        Console.WriteLine("\tres 1,(ix+d)");
                                        break;
                                    case 0x9E:
                                        Console.WriteLine("\tres 3,(ix+d)");
                                        break;
                                    case 0xAE:
                                        Console.WriteLine("\tres 5,(ix+d)");
                                        break;
                                    case 0xBE:
                                        Console.WriteLine("\tres 7,(ix+d)");
                                        break;
                                    case 0xCE:
                                        Console.WriteLine("\tset 1,(ix+d)");
                                        break;
                                    case 0xDE:
                                        Console.WriteLine("\tset 3,(ix+d)");
                                        break;
                                    case 0xEE:
                                        Console.WriteLine("\tset 5,(ix+d)");
                                        break;
                                    case 0xFE:
                                        Console.WriteLine("\tset 7,(ix+d)");
                                        break;
                                    default:
                                        Console.WriteLine("Undocumented OpCode");
                                        return -1;
                                }
                                break;
                            case 0xE1:
                                Console.WriteLine("\tpop ix");
                                break;
                            case 0xE3:
                                Console.WriteLine("\tex (sp),ix");
                                break;
                            case 0xE5:
                                Console.WriteLine("\tpush ix");
                                break;
                            case 0xE9:
                                Console.WriteLine("\tjp (ix)");
                                break;
                            case 0xF9:
                                Console.WriteLine("\tld sp,ix");
                                break;
                            default:
                                Console.WriteLine("Undocumented OpCode\\Empty OpCode");
                                return -1;
                        }
                        break;
                    case 0xDE:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tsbc a,&{Byte:X2}");
                        break;
                    case 0xDF:
                        Console.WriteLine("\trst 18h");
                        break;
                    case 0xE0:
                        Console.WriteLine("\tret po");
                        break;
                    case 0xE1:
                        Console.WriteLine("\tpop hl");
                        break;
                    case 0xE2:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp po,&{H:X2}{L:X2}");
                        break;
                    case 0xE3:
                        Console.WriteLine("\tex (sp),hl");
                        break;
                    case 0xE4:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall po,&{H:X2}{L:X2}");
                        break;
                    case 0xE5:
                        Console.WriteLine("\tpush hl");
                        break;
                    case 0xE6:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tand a,(&{Byte:X2})");
                        break;
                    case 0xE7:
                        Console.WriteLine("\trst 20h");
                        break;
                    case 0xE8:
                        Console.WriteLine("\tret pe");
                        break;
                    case 0xE9:
                        Console.WriteLine("\tjp (hl)");
                        break;
                    case 0xEA:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp pe,&{H:X2}{L:X2}");
                        break;
                    case 0xEB:
                        Console.WriteLine("\tex de,hl");
                        break;
                    case 0xEC:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall pe,&{H:X2}{L:X2}");
                        break;
                    case 0xED:
                        switch(reader.ReadByte())
                            {
                            case 0x40:
                                Console.WriteLine("\tin b,(c)");
                                break;
                            case 0x41:
                                Console.WriteLine("\tout (c),b");
                                break;
                            case 0x42:
                                Console.WriteLine("\tsbc hl,bc");
                                break;
                            case 0x43:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld (&{H:X2}{L:X2}),bc");
                                break;
                            case 0x44:
                                Console.WriteLine("\tneg");
                                break;
                            case 0x45:
                                Console.WriteLine("\tretn");
                                break;
                            case 0x46:
                                Console.WriteLine("\tim 0");
                                break;
                            case 0x47:
                                Console.WriteLine("\tld i,a");
                                break;
                            case 0x48:
                                Console.WriteLine("\tin c,(c)");
                                break;
                            case 0x49:
                                Console.WriteLine("\tout (c),c");
                                break;
                            case 0x4A:
                                Console.WriteLine("\tadc hl,bc");
                                break;
                            case 0x4B:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld bc,(&{H:X2}{L:X2})");
                                break;
                            case 0x4D:
                                Console.WriteLine("\treti");
                                break;
                            case 0x4F:
                                Console.WriteLine("\tld r,a");
                                break;
                            case 0x50:
                                Console.WriteLine("\tin d,(c)");
                                break;
                            case 0x51:
                                Console.WriteLine("\tout (c),d");
                                break;
                            case 0x52:
                                Console.WriteLine("\tsbc hl,de");
                                break;
                            case 0x53:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld (&{H:X2}{L:X2}),de");
                                break;
                            case 0x56:
                                Console.WriteLine("\tim 1");
                                break;
                            case 0x57:
                                Console.WriteLine("\tld a,i");
                                break;
                            case 0x58:
                                Console.WriteLine("\tin e,(c)");
                                break;
                            case 0x59:
                                Console.WriteLine("\tout (c),e");
                                break;
                            case 0x5A:
                                Console.WriteLine("\tadc hl,de");
                                break;
                            case 0x5B:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld de,(&{H:X2}{L:X2})");
                                break;
                            case 0x5E:
                                Console.WriteLine("\tim 2");
                                break;
                            case 0x5F:
                                Console.WriteLine("\tld a,r");
                                break;
                            case 0x60:
                                Console.WriteLine("\tin h,(c)");
                                break;
                            case 0x61:
                                Console.WriteLine("\tout (c),h");
                                break;
                            case 0x62:
                                Console.WriteLine("\tsbc hl,hl");
                                break;
                            case 0x67:
                                Console.WriteLine("\trrd");
                                break;
                            case 0x68:
                                Console.WriteLine("\tin l,(c)");
                                break;
                            case 0x69:
                                Console.WriteLine("\tout (c),l");
                                break;
                            case 0x6A:
                                Console.WriteLine("\tadc hl,hl");
                                break;
                            case 0x6F:
                                Console.WriteLine("\trld");
                                break;
                            case 0x72:
                                Console.WriteLine("\tsbc hl,sp");
                                break;
                            case 0x73:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld (&{H:X2}{L:X2}),sp");
                                break;
                            case 0x78:
                                Console.WriteLine("\tin a,(c)");
                                break;
                            case 0x79:
                                Console.WriteLine("\tout (c),a");
                                break;
                            case 0x7A:
                                Console.WriteLine("\tadc hl,sp");
                                break;
                            case 0x7B:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld sp,(&{H:X2}{L:X2})");
                                break;
                            case 0xA0:
                                Console.WriteLine("\tldi");
                                break;
                            case 0xA1:
                                Console.WriteLine("\tcpi");
                                break;
                            case 0xA2:
                                Console.WriteLine("\tini");
                                break;
                            case 0xA3:
                                Console.WriteLine("\touti");
                                break;
                            case 0xA8:
                                Console.WriteLine("\tldd");
                                break;
                            case 0xA9:
                                Console.WriteLine("\tcpd");
                                break;
                            case 0xAA:
                                Console.WriteLine("\tind");
                                break;
                            case 0xAB:
                                Console.WriteLine("\toutd");
                                break;
                            case 0xB0:
                                Console.WriteLine("\tldir");
                                break;
                            case 0xB1:
                                Console.WriteLine("\tcpir");
                                break;
                            case 0xB2:
                                Console.WriteLine("\tinir");
                                break;
                            case 0xB3:
                                Console.WriteLine("\totir");
                                break;
                            case 0xB8:
                                Console.WriteLine("\tlddr");
                                break;
                            case 0xB9:
                                Console.WriteLine("\tcpdr");
                                break;
                            case 0xBA:
                                Console.WriteLine("\tindr");
                                break;
                            case 0xBB:
                                Console.WriteLine("\totdr");
                                break;
                            default:
                                Console.WriteLine("No OpCode/Undocumented OpCode");
                                return -1;
                            }
                        break;
                    case 0xEE:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\txor &{Byte:X2}");
                        break;
                    case 0xEF:
                        Console.WriteLine("\trst 28h");
                        break;
                    case 0xF0:
                        Console.WriteLine("\tret p");
                        break;
                    case 0xF1:
                        Console.WriteLine("\tpop af");
                        break;
                    case 0xF2:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp p,&{H:X2}{L:X2}");
                        break;
                    case 0xF3:
                        Console.WriteLine("\tdi");
                        break;
                    case 0xF4:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall p,&{H:X2}{L:X2}");
                        break;
                    case 0xF5:
                        Console.WriteLine("\tpush af");
                        break;
                    case 0xF6:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tor &{Byte:X2}");
                        break;
                    case 0xF7:
                        Console.WriteLine("\trst 30h");
                        break;
                    case 0xF8:
                        Console.WriteLine("\tret m");
                        break;
                    case 0xF9:
                        Console.WriteLine("\tld sp,hl");
                        break;
                    case 0xFA:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tjp m,&{H:X2}{L:X2}");
                        break;
                    case 0xFB:
                        Console.WriteLine("\tei");
                        break;
                    case 0xFC:
                        L = reader.ReadByte();
                        H = reader.ReadByte();
                        Console.WriteLine($"\tcall m,&{H:X2}{L:X2}");
                        break;
                    case 0xFD:
                        switch (reader.ReadByte())
                        {
                            case 0x09:
                                Console.WriteLine("\tadd iy,bc");
                                break;
                            case 0x19:
                                Console.WriteLine("\tadd iy,de");
                                break;
                            case 0x21:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld iy,&{H:X2}{L:X2}");
                                break;
                            case 0x22:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld (&{H:X2}{L:X2}),iy");
                                break;
                            case 0x23:
                                Console.WriteLine("\tinc iy");
                                break;
                            case 0x29:
                                Console.WriteLine("\tadd iy,iy");
                                break;
                            case 0x2A:
                                L = reader.ReadByte();
                                H = reader.ReadByte();
                                Console.WriteLine($"\tld iy,(&{H:X2}{L:X2})");
                                break;
                            case 0x2B:
                                Console.WriteLine("\tdec iy");
                                break;
                            case 0x34:
                                Console.WriteLine("\tinc (iy+d)");
                                break;
                            case 0x35:
                                Console.WriteLine("\tdec (iy+d)");
                                break;
                            case 0x36:
                                Byte = reader.ReadByte();
                                Console.WriteLine($"\tld (iy+d),&{Byte:X2}");
                                break;
                            case 0x39:
                                Console.WriteLine("\tadd iy,sp");
                                break;
                            case 0x46:
                                Console.WriteLine("\tld b,(iy+d)");
                                break;
                            case 0x4E:
                                Console.WriteLine("\tld c,(iy+d)");
                                break;
                            case 0x56:
                                Console.WriteLine("\tld d,(iy+d)");
                                break;
                            case 0x5E:
                                Console.WriteLine("\tld e,(iy+d)");
                                break;
                            case 0x66:
                                Console.WriteLine("\tld h,(iy+d)");
                                break;
                            case 0x6E:
                                Console.WriteLine("\tld l,(iy+d)");
                                break;
                            case 0x70:
                                Console.WriteLine("\tld (iy+d),b");
                                break;
                            case 0x71:
                                Console.WriteLine("\tld (iy+d),c");
                                break;
                            case 0x72:
                                Console.WriteLine("\tld (iy+d),d");
                                break;
                            case 0x73:
                                Console.WriteLine("\tld (iy+d),e");
                                break;
                            case 0x74:
                                Console.WriteLine("\tld (iy+d),h");
                                break;
                            case 0x75:
                                Console.WriteLine("\tld (iy+d),l");
                                break;
                            case 0x77:
                                Console.WriteLine("\tld (iy+d),a");
                                break;
                            case 0x7E:
                                Console.WriteLine("\tld a,(iy+d)");
                                break;
                            case 0x86:
                                Console.WriteLine("\tadd a,(iy+d)");
                                break;
                            case 0x8E:
                                Console.WriteLine("\tadc a,(iy+d)");
                                break;
                            case 0x96:
                                Console.WriteLine("\tsub (iy+d)");
                                break;
                            case 0x9E:
                                Console.WriteLine("\tsbc a,(iy+d)");
                                break;
                            case 0xA6:
                                Console.WriteLine("\tand (iy+d)");
                                break;
                            case 0xAE:
                                Console.WriteLine("\txor (iy+d)");
                                break;
                            case 0xB6:
                                Console.WriteLine("\tor (iy+d)");
                                break;
                            case 0xBE:
                                Console.WriteLine("\tcp (iy+d)");
                                break;
                            case 0xCB:
                                switch (reader.ReadByte())
                                {
                                    case 0x06:
                                        Console.WriteLine("\trlc (iy+d)");
                                        break;
                                    case 0x16:
                                        Console.WriteLine("\trl (iy+d)");
                                        break;
                                    case 0x26:
                                        Console.WriteLine("\tsla (iy+d)");
                                        break;
                                    case 0x46:
                                        Console.WriteLine("\tbit 0,(iy+d)");
                                        break;
                                    case 0x56:
                                        Console.WriteLine("\tbit 2,(iy+d)");
                                        break;
                                    case 0x66:
                                        Console.WriteLine("\tbit 4,(iy+d)");
                                        break;
                                    case 0x76:
                                        Console.WriteLine("\tbit 6,(iy+d)");
                                        break;
                                    case 0x86:
                                        Console.WriteLine("\tres 0,(iy+d)");
                                        break;
                                    case 0x96:
                                        Console.WriteLine("\tres 2,(iy+d)");
                                        break;
                                    case 0xA6:
                                        Console.WriteLine("\tres 4,(iy+d)");
                                        break;
                                    case 0xB6:
                                        Console.WriteLine("\tres 6,(iy+d)");
                                        break;
                                    case 0xC6:
                                        Console.WriteLine("\tset 0,(iy+d)");
                                        break;
                                    case 0xD6:
                                        Console.WriteLine("\tset 2,(iy+d)");
                                        break;
                                    case 0xE6:
                                        Console.WriteLine("\tset 4,(iy+d)");
                                        break;
                                    case 0xF6:
                                        Console.WriteLine("\tset 6,(iy+d)");
                                        break;
                                    case 0x0E:
                                        Console.WriteLine("\trrc (iy+d)");
                                        break;
                                    case 0x1E:
                                        Console.WriteLine("\trr (iy+d)");
                                        break;
                                    case 0x2E:
                                        Console.WriteLine("\tsra (iy+d)");
                                        break;
                                    case 0x3E:
                                        Console.WriteLine("\tsrl (iy+d)");
                                        break;
                                    case 0x4E:
                                        Console.WriteLine("\tbit 1,(iy+d)");
                                        break;
                                    case 0x5E:
                                        Console.WriteLine("\tbit 3,(iy+d)");
                                        break;
                                    case 0x6E:
                                        Console.WriteLine("\tbit 5,(iy+d)");
                                        break;
                                    case 0x7E:
                                        Console.WriteLine("\tbit 7,(iy+d)");
                                        break;
                                    case 0x8E:
                                        Console.WriteLine("\tres 1,(iy+d)");
                                        break;
                                    case 0x9E:
                                        Console.WriteLine("\tres 3,(iy+d)");
                                        break;
                                    case 0xAE:
                                        Console.WriteLine("\tres 5,(iy+d)");
                                        break;
                                    case 0xBE:
                                        Console.WriteLine("\tres 7,(iy+d)");
                                        break;
                                    case 0xCE:
                                        Console.WriteLine("\tset 1,(iy+d)");
                                        break;
                                    case 0xDE:
                                        Console.WriteLine("\tset 3,(iy+d)");
                                        break;
                                    case 0xEE:
                                        Console.WriteLine("\tset 5,(iy+d)");
                                        break;
                                    case 0xFE:
                                        Console.WriteLine("\tset 7,(iy+d)");
                                        break;
                                    default:
                                        Console.WriteLine("Undocumented OpCode");
                                        return -1;
                                }
                                break;
                            case 0xE1:
                                Console.WriteLine("\tpop iy");
                                break;
                            case 0xE3:
                                Console.WriteLine("\tex (sp),iy");
                                break;
                            case 0xE5:
                                Console.WriteLine("\tpush iy");
                                break;
                            case 0xE9:
                                Console.WriteLine("\tjp (iy)");
                                break;
                            case 0xF9:
                                Console.WriteLine("\tld sp,iy");
                                break;
                            default:
                                Console.WriteLine("Undocumented OpCode\\Empty OpCode");
                                return -1;
                        }
                        break;
                    case 0xFE:
                        Byte = reader.ReadByte();
                        Console.WriteLine($"\tCP &{Byte:X2}");
                        break;
                    case 0xFF:
                        Console.WriteLine($"\trst 38h");
                        break;
                    default:;
                }
            }
        }
    }
}
