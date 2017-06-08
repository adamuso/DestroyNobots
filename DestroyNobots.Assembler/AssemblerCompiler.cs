using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    public class AssemblerCompiler
    {
        IInstructionSetProvider instructionSetProvider;
        Dictionary<string, AssemblerOpcodeSet> instructions;
        Dictionary<string, int> registers;
        Dictionary<string, int> constants;
        Dictionary<string, int> labels;

        // parser info
        int line;

        public AssemblerCompiler(IInstructionSetProvider instructionSetProvider)
        {
            this.instructionSetProvider = instructionSetProvider;
            instructions = new Dictionary<string, AssemblerOpcodeSet>();
            registers = new Dictionary<string, int>();
            constants = new Dictionary<string, int>();
            labels = new Dictionary<string, int>();
            line = 1;
        }

        public void SetInstruction(string name, byte opcode, int params_number, params AssemblerParameters[] parameters)
        {
            if (!instructions.ContainsKey(name.ToLower()))
                instructions[name.ToLower()] = new AssemblerOpcodeSet(instructionSetProvider);

            instructions[name.ToLower()].Add(opcode);

            if (params_number == 0 && parameters.Length == 0)
                instructions[name.ToLower()].Default = opcode;

            AssemblerInstruction instructon = instructionSetProvider.InstructionSet[opcode];
            instructon.SetParameters(parameters);
        }

        public void SetConstant(string name, int value)
        {
            constants[name] = value;
        }

        public void SetRegister(string name, int regindex)
        {
            registers[name.ToLower()] = regindex;
        }

        private void PreProcess(System.IO.Stream stream)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            int address = 0;

            if (reader.PeekChar() == 65279)         // UTF-8 identify char
                reader.ReadChar();

            List<string> labelNames = PreProcessLabels(stream);

            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

            while (reader.BaseStream.Length != reader.BaseStream.Position)
            {
                #region Init, Process comments, labels, reserve
                if (reader.PeekChar() == 65279)         // UTF-8 identify char
                    reader.ReadChar();

                string instruction = "";
                skipSpaces(stream);

                if (reader.BaseStream.Length == reader.BaseStream.Position)
                    break;

                if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
                    while (char.IsLetterOrDigit((char)reader.PeekChar()) || (char)reader.PeekChar() == ':' || (char)reader.PeekChar() == '_')
                    {
                        instruction += reader.ReadChar();
                    }

                if (skipComments(reader))
                    continue;

                if (instruction.EndsWith(":"))
                {
                    string label = instruction.Substring(0, instruction.Length - 1).ToLower();

                    if (!instructions.ContainsKey(label) && !constants.ContainsKey(label) && !registers.ContainsKey(label))
                    {
                        labels.Add(label, address);
                    }
                    else
                        throw new Exception("This word is reserved!");

                    continue;
                }

                #region Reserve
                if (instruction.ToLower() == "reserve")
                {
                    skipSpaces(stream);

                    int size;
                    if (!readIntValue(stream, reader, out size))
                        throw new Exception("Expected size!");

                    skipSpaces(stream);

                    if ((char)reader.PeekChar() == ',')
                        reader.ReadChar();
                    else
                        throw new Exception("Expected ','!");

                    skipSpaces(stream);

                    int mult = 0;
                    if (!readIntValue(stream, reader, out mult))
                        throw new Exception("Expected multiplier!");

                    address += size * mult;

                    continue;
                }
                #endregion

                #region ConstToMemory
                if (instruction.ToLower() == "memory")
                {
                    int size;

                    do
                    {
                        size = 0;

                        if ((char)reader.PeekChar() == ',')
                            reader.ReadChar();

                        skipSpaces(stream);

                        if ((char)reader.PeekChar() == '[')
                        {
                            reader.ReadChar();
                            readIntValue(stream, reader, out size);

                            if ((char)reader.PeekChar() == ']')
                                reader.ReadChar();
                            else
                                throw new Exception("Expected ']'");
                        }

                        skipSpaces(stream);

                        if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
                        {
                            string reg = readIdentifier(stream, reader);

                            if (constants.ContainsKey(reg))
                                address += 4;
                            else if (labels.ContainsKey(reg))
                                address += 4;
                            else
                                throw new Exception("Identifier not found!");
                        }

                        if (char.IsDigit((char)reader.PeekChar()))
                        {
                            int val = readIntLiteral(stream, reader);

                            if ((val < byte.MaxValue && size == 0) || size == 1)
                                address += 1;
                            else if ((val < short.MaxValue && size == 0) || size == 2)
                                address += 2;
                            else if ((val < int.MaxValue && size == 0) || size == 4)
                                address += 4;
                            else
                                throw new Exception("Too big number!");
                        }

                        if ((char)reader.PeekChar() == '"')
                        {
                            string lit = readStringLiteral(stream, reader);
                            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(lit);

                            address += bytes.Length;
                        }

                        if ((char)reader.PeekChar() == '\'')
                        {
                            char lit = readCharLiteral(stream, reader);
                            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("" + lit);

                            address += bytes.Length;
                        }

                        skipSpaces(stream);
                    }
                    while ((char)reader.PeekChar() == ',');

                    continue;
                }
                #endregion

                #endregion

                #region Read parameters
                AssemblerOpcodeSet opcodes = instructions[instruction.ToLower()];      // get opcode from instruction

                List<int> args = new List<int>();
                List<AssemblerParameters> _params = new List<AssemblerParameters>();
                int parnum = 4;

                if (opcodes.Count == 1)
                    parnum = instructionSetProvider.InstructionSet[opcodes.Opcodes[0]].ParametersCount;

                for (int i = 0; i < parnum; i++)
                {
                    skipSpaces(stream);

                    bool pointer = false;

                    if ((char)reader.PeekChar() == '[')
                    {
                        pointer = true;
                        reader.ReadChar();
                    }

                    if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
                    {
                        // parameter is register/const
                        string reg = readIdentifier(stream, reader);

                        if (constants.ContainsKey(reg))
                        {
                            args.Add(constants[reg]);
                            _params.Add(pointer ? AssemblerParameters.Pointer : AssemblerParameters.Value);
                        }
                        else if (labelNames.Contains(reg))
                        {
                            args.Add(int.MaxValue);
                            _params.Add(pointer ? AssemblerParameters.Pointer : AssemblerParameters.Address);
                        }
                        else
                        {
                            args.Add(registers[reg.ToLower()]);
                            _params.Add(pointer ? AssemblerParameters.PointerInRegister : AssemblerParameters.Register);
                        }
                    }
                    else if (char.IsDigit((char)reader.PeekChar()))
                    {
                        // parameter is value/number

                        args.Add(readIntLiteral(stream, reader));
                        _params.Add(pointer ? AssemblerParameters.Pointer : AssemblerParameters.Value);
                    }

                    if (pointer)
                    {
                        if ((char)reader.PeekChar() == ']')
                            reader.ReadChar();
                        else
                            throw new Exception("Expected ']'!");
                    }

                    skipSpaces(stream);

                    if ((char)reader.PeekChar() == ',')
                        reader.ReadChar();
                    else
                        break;
                }
                #endregion

                #region Memory usage calculating
                byte opcode = opcodes.Find(_params.ToArray());
                AssemblerInstruction asm = InstructionSetProvider.InstructionSet[opcode];
                byte paramstypes = 0;

                //writer.Write(opcode);
                address += 1;

                //writer.Write(paramstypes);
                address += 1;

                for (int i = 0; i < args.Count; i++)
                {
                    if (_params[i] == AssemblerParameters.Register)
                    {
                        paramstypes |= (byte)(0x00 << (i * 2));
                    }
                    else if ((_params[i] & AssemblerParameters.Value) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Pointer) != 0) // Address
                        {
                            paramstypes |= (byte)(0x02 << (i * 2));
                        }
                        else
                        {
                            if (args[i] < byte.MaxValue)
                                paramstypes |= (byte)(0x00 << (i * 2));
                            else if (args[i] < short.MaxValue)
                                paramstypes |= (byte)(0x01 << (i * 2));
                            else
                                paramstypes |= (byte)(0x02 << (i * 2));
                        }
                    }
                    else if ((_params[i] & AssemblerParameters.Pointer) != 0)
                    {                   
                        if ((_params[i] & AssemblerParameters.Register) != 0)
                            paramstypes |= (byte)(0x01 << (i * 2));
                        else
                            paramstypes |= (byte)(0x00 << (i * 2));
                    }
                }

                for (int i = 0; i < args.Count; i++)
                {
                    if (_params[i] == AssemblerParameters.Register)
                        address += 1;
                    else if ((_params[i] & AssemblerParameters.Value) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Pointer) != 0) // Address
                        {
                            address += 4;
                        }
                        else
                        {
                            if (args[i] < byte.MaxValue)
                                address += 1;
                            else if (args[i] < short.MaxValue)
                                address += 2;
                            else
                                address += 4;
                        }
                    }
                    else if ((_params[i] & AssemblerParameters.Pointer) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Register) != 0)
                            address += 1;
                        else
                            address += 4;
                    }
                }
                #endregion
            }

            stream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        private List<string> PreProcessLabels(System.IO.Stream stream)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            List<string> labels = new List<string>();

            while (reader.BaseStream.Length != reader.BaseStream.Position)
            {
                string instruction = "";
                skipSpaces(stream);

                if (reader.BaseStream.Length == reader.BaseStream.Position)
                    break;

                if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
                    while (char.IsLetterOrDigit((char)reader.PeekChar()) || (char)reader.PeekChar() == ':' || (char)reader.PeekChar() == '_')
                    {
                        instruction += reader.ReadChar();
                    }

                while ((char)reader.PeekChar() == ',' || char.IsDigit((char)reader.PeekChar()) || (char)reader.PeekChar() == '[' || (char)reader.PeekChar() == ']')
                    reader.ReadChar();

                if ((char)reader.PeekChar() == '"')
                {
                    reader.ReadChar();
                    while (reader.ReadChar() != '"') ;
                }

                if ((char)reader.PeekChar() == '\'')
                {
                    reader.ReadChar();
                    while (reader.ReadChar() != '\'') ;
                }

                if (skipComments(reader))
                    continue;

                if (instruction.EndsWith(":"))
                {
                    string label = instruction.Substring(0, instruction.Length - 1).ToLower();

                    if (!instructions.ContainsKey(label) && !constants.ContainsKey(label) && !registers.ContainsKey(label))
                    {
                        labels.Add(label);
                    }
                    else
                        throw new Exception("This word is reserved!");

                    continue;
                }
            }

            return labels;
        }

        public byte[] Compile(string text)
        {
            return Compile(new System.IO.MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(text)));
        }

        public byte[] Compile(System.IO.Stream stream)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);
            System.IO.MemoryStream buffer = new System.IO.MemoryStream();
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(buffer);

            line = 1;

            int address = 0;

            PreProcess(stream);

            while (reader.BaseStream.Length != reader.BaseStream.Position)
            {
                if (reader.PeekChar() == 65279)         // UTF-8 identify char
                    reader.ReadChar();

                string instruction = "";
                skipSpaces(stream);

                if (reader.BaseStream.Length == reader.BaseStream.Position)
                    break;

                if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
                    while (char.IsLetterOrDigit((char)reader.PeekChar()) || (char)reader.PeekChar() == ':' || (char)reader.PeekChar() == '_')
                    {
                        instruction += reader.ReadChar();
                    }

                if (skipComments(reader))                   // skip comments
                    continue;

                if (instruction.EndsWith(":"))              // check if a label
                    continue;

                #region Reserve
                if (instruction.ToLower() == "reserve")
                {
                    skipSpaces(stream);

                    int size;
                    if (!readIntValue(stream, reader, out size))
                        throw new Exception("Expected size!");

                    skipSpaces(stream);

                    if ((char)reader.PeekChar() == ',')
                        reader.ReadChar();
                    else
                        throw new Exception("Expected ','!");

                    skipSpaces(stream);

                    int mult = 0;
                    if (!readIntValue(stream, reader, out mult))
                        throw new Exception("Expected multiplier!");

                    writer.Write(new byte[size * mult], 0, size * mult);
                    address += size * mult;

                    continue;
                }
                #endregion

                #region ConstToMemory
                if (instruction.ToLower() == "memory")
                {
                    int size = 0;

                    do
                    {
                        size = 0;

                        if ((char)reader.PeekChar() == ',')
                            reader.ReadChar();

                        skipSpaces(stream);

                        if ((char)reader.PeekChar() == '[')
                        {
                            reader.ReadChar();
                            readIntValue(stream, reader, out size);

                            if ((char)reader.PeekChar() == ']')
                                reader.ReadChar();
                            else
                                throw new Exception("Expected ']'");
                        }

                        skipSpaces(stream);

                        if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
                        {
                            string reg = readIdentifier(stream, reader);

                            if (constants.ContainsKey(reg))
                            {
                                writer.Write(constants[reg]);
                                address += 4;
                            }
                            else if (labels.ContainsKey(reg))
                            {
                                writer.Write(labels[reg]);
                                address += 4;
                            }
                            else
                            {
                                throw new Exception("Identifier not found!");
                            }
                        }

                        if (char.IsDigit((char)reader.PeekChar()))
                        {
                            int val = readIntLiteral(stream, reader);

                            if ((val < byte.MaxValue && size == 0) || size == 1)
                            {
                                writer.Write((byte)val);
                                address += 1;
                            }
                            else if ((val < short.MaxValue && size == 0) || size == 2)
                            {
                                writer.Write((short)val);
                                address += 2;
                            }
                            else if ((val < int.MaxValue && size == 0) || size == 4)
                            {
                                writer.Write(val);
                                address += 4;
                            }
                            else
                                throw new Exception("Too big number!");
                        }

                        if ((char)reader.PeekChar() == '"')
                        {
                            string lit = readStringLiteral(stream, reader);
                            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(lit);

                            writer.Write(bytes, 0, bytes.Length);
                            address += bytes.Length;
                        }

                        if ((char)reader.PeekChar() == '\'')
                        {
                            char lit = readCharLiteral(stream, reader);
                            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("" + lit);

                            writer.Write(bytes, 0, bytes.Length);
                            address += bytes.Length;
                        }

                        skipSpaces(stream);
                    }
                    while ((char)reader.PeekChar() == ',');

                    continue;
                }
                #endregion

                AssemblerOpcodeSet opcodes = instructions[instruction.ToLower()];      // get opcode from instruction

                List<int> args = new List<int>();
                List<AssemblerParameters> _params = new List<AssemblerParameters>();
                int parnum = 4;

                if (opcodes.Count == 1)
                    parnum = instructionSetProvider.InstructionSet[opcodes.Opcodes[0]].ParametersCount;

                for (int i = 0; i < parnum; i++)
                {
                    skipSpaces(stream);

                    bool pointer = false;

                    if ((char)reader.PeekChar() == '[')
                    {
                        pointer = true;
                        reader.ReadChar();
                    }

                    if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_' || (char)reader.PeekChar() == '[')
                    {
                        // parameter is register/const

                        string reg = readIdentifier(stream, reader);

                        if (constants.ContainsKey(reg))
                        {
                            args.Add(constants[reg]);
                            _params.Add(pointer ? AssemblerParameters.Pointer : AssemblerParameters.Value);
                        }
                        else if (labels.ContainsKey(reg))
                        {
                            args.Add(labels[reg]);
                            _params.Add(pointer ? AssemblerParameters.Pointer : AssemblerParameters.Address);
                        }
                        else
                        {
                            args.Add(registers[reg.ToLower()]);
                            _params.Add(pointer ? AssemblerParameters.PointerInRegister : AssemblerParameters.Register);
                        }
                    }
                    else if (char.IsDigit((char)reader.PeekChar()))
                    {
                        // parameter is value/number

                        args.Add(readIntLiteral(stream, reader));
                        _params.Add(pointer ? AssemblerParameters.Pointer : AssemblerParameters.Value);
                    }

                    if (pointer)
                    {
                        if ((char)reader.PeekChar() == ']')
                            reader.ReadChar();
                        else
                            throw new Exception("Expected ']'!");
                    }

                    skipSpaces(stream);

                    if ((char)reader.PeekChar() == ',')
                        reader.ReadChar();
                    else
                        break;
                }

                byte opcode = opcodes.Find(_params.ToArray());
                AssemblerInstruction asm = instructionSetProvider.InstructionSet[opcode];
                byte paramstypes = 0;
                int startaddress = address;

                writer.Write(opcode);
                address += 1;

                for (int i = 0; i < args.Count; i++)
                {
                    if (_params[i] == AssemblerParameters.Register)
                    {
                        paramstypes |= (byte)(0x00 << (i * 2));
                    }
                    else if ((_params[i] & AssemblerParameters.Value) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Pointer) != 0) // Address
                        {
                            paramstypes |= (byte)(0x02 << (i * 2));
                        }
                        else
                        {
                            if (args[i] < byte.MaxValue)
                                paramstypes |= (byte)(0x00 << (i * 2));
                            else if (args[i] < short.MaxValue)
                                paramstypes |= (byte)(0x01 << (i * 2));
                            else
                                paramstypes |= (byte)(0x02 << (i * 2));
                        }
                    }
                    else if ((_params[i] & AssemblerParameters.Pointer) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Register) != 0)
                            paramstypes |= (byte)(0x01 << (i * 2));
                        else
                            paramstypes |= (byte)(0x00 << (i * 2));
                    }
                }

                writer.Write(paramstypes);
                address += 1;

                for (int i = 0; i < args.Count; i++)
                {
                    if (_params[i] == AssemblerParameters.Register)
                    {
                        writer.Write((byte)args[i]);
                        address += 1;
                    }
                    else if ((_params[i] & AssemblerParameters.Value) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Pointer) != 0) // Address
                        {
                            address += 4;

                            if (asm.ConvertLabelsToOffsets)
                                writer.Write(args[i] - address);
                            else
                                writer.Write(args[i]);
                        }
                        else
                        { 
                            if (args[i] < byte.MaxValue)
                            {
                                writer.Write((byte)args[i]);
                                address += 1;
                            }
                            else if (args[i] < short.MaxValue)
                            {
                                writer.Write((short)args[i]);
                                address += 2;
                            }
                            else
                            {
                                writer.Write(args[i]);
                                address += 4;
                            }
                        }
                    }
                    else if ((_params[i] & AssemblerParameters.Pointer) != 0)
                    {
                        if ((_params[i] & AssemblerParameters.Register) != 0)
                        {
                            writer.Write((byte)args[i]);

                            address += 1;
                        }
                        else
                        {
                            writer.Write(args[i]);

                            address += 4;
                        }
                    }
                }
            }

            byte[] zero = BitConverter.GetBytes((int)0);
            buffer.Write(zero, 0, zero.Length);

            //instructionSetProvider.Memory.Write(instructionSetProvider.ProgramMemory, buffer.ToArray());
            return buffer.ToArray();
        }

        private int labelOffset(int value)
        {
            return ((value < 0 ? (-value << 1 | 1) : (value << 1)));
        }

        private bool readIntValue(System.IO.Stream stream, System.IO.BinaryReader reader, out int value)
        {
            int add = 0;

            return readIntValue(stream, reader, ref add, out value);
        }

        private bool readIntValue(System.IO.Stream stream, System.IO.BinaryReader reader, ref int address, out int value)
        {
            skipSpaces(stream);

            if (char.IsLetter((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
            {
                string reg = readIdentifier(stream, reader);
                skipSpaces(stream);

                if (constants.ContainsKey(reg))
                {
                    address += 4;
                    value = constants[reg];
                    return true;
                }
                else if (labels.ContainsKey(reg))
                {
                    address += 4;
                    value = labels[reg];
                    return true;
                }
                else
                {
                    throw new Exception("Identifier not found!");
                }
            }

            if (char.IsDigit((char)reader.PeekChar()))
            {
                int val = readIntLiteral(stream, reader);
                skipSpaces(stream);

                if (val < byte.MaxValue)
                {
                    address += 1;
                    value = (byte)val;
                    return true;
                }
                else if (val < short.MaxValue)
                {
                    address += 2;
                    value = (short)val;
                    return true;
                }
                else if (val < int.MaxValue)
                {
                    address += 4;
                    value = val;
                    return true;
                }
                else
                    throw new Exception("Too big number!");
            }

            value = 0;
            return false;
        }

        private string readIdentifier(System.IO.Stream stream, System.IO.BinaryReader reader)
        {
            string str = "";

            skipSpaces(stream);

            while (char.IsLetterOrDigit((char)reader.PeekChar()) || (char)reader.PeekChar() == '_')
            {
                str += (char)stream.ReadByte();
            }

            skipSpaces(stream);

            return str;
        }

        private string readStringLiteral(System.IO.Stream stream, System.IO.BinaryReader reader)
        {
            string str = "";

            skipSpaces(stream);

            if ((char)reader.PeekChar() == '"')
                reader.ReadChar();

            while ((char)reader.PeekChar() != '"')
            {
                str += (char)stream.ReadByte();
            }

            if ((char)reader.PeekChar() == '"')
                reader.ReadChar();

            skipSpaces(stream);

            return str;
        }

        private char readCharLiteral(System.IO.Stream stream, System.IO.BinaryReader reader)
        {
            char c = '\0';

            skipSpaces(stream);

            if ((char)reader.PeekChar() == '\'')
                reader.ReadChar();

            c = reader.ReadChar();

            if ((char)reader.PeekChar() == '\'')
                reader.ReadChar();

            skipSpaces(stream);

            return c;
        }

        private int readIntLiteral(System.IO.Stream stream, System.IO.BinaryReader reader)
        {
            // parameter is value/number

            string val = "";

            skipSpaces(stream);

            while (char.IsDigit((char)reader.PeekChar()) || (char)reader.PeekChar() == 'x' || (char)reader.PeekChar() == 'b')
            {
                val += (char)stream.ReadByte();
            }

            skipSpaces(stream);

            if (val.StartsWith("0x"))
                return Convert.ToInt32(val, 16);

            if (val.EndsWith("b"))
                return Convert.ToInt32(val.Substring(0, val.Length - 1), 2);

            return int.Parse(val);
        }

        private void skipSpaces(System.IO.Stream stream)
        {
            char space = (char)stream.ReadByte();
            while (Char.IsWhiteSpace(space))
            {
                if (space == '\n' || space == '\r')
                {
                    line++;
                    space = (char)stream.ReadByte();

                    if (space == '\n' || space == '\r')
                        space = (char)stream.ReadByte();
                }
                else
                    space = (char)stream.ReadByte();
            }

            if (stream.Length != stream.Position)
                stream.Seek(-1, System.IO.SeekOrigin.Current);
        }

        private bool skipComments(System.IO.BinaryReader reader)
        {
            string instruction = "";

            while ((char)reader.PeekChar() == ';' || (char)reader.PeekChar() == '/' || (char)reader.PeekChar() == '*')
                instruction += reader.ReadChar();

            if (instruction.StartsWith(";") || instruction.StartsWith("//"))
            {
                while ((char)reader.PeekChar() != '\n' && (char)reader.PeekChar() != '\r') reader.ReadByte();
                return true;
            }

            if (instruction.StartsWith("/*"))
            {
                back:
                while ((char)reader.PeekChar() != '*')
                {
                    reader.ReadChar();
                }

                string end = "*";

                if ((char)reader.PeekChar() == '/')
                {
                    end += reader.ReadChar();
                    return true;
                }

                goto back;
            }

            return false;
        }

        public IInstructionSetProvider InstructionSetProvider { get { return instructionSetProvider; } }
    }
}
