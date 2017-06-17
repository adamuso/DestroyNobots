namespace DestroyNobots.Assembler
{
    public enum RegisterType : byte
    {
        Full = 0x00,
        Lower8 = 0x01,
        Higher8 = 0x02,
        Lower16 = 0x03,
        Lower32 = 0x04
    }
}