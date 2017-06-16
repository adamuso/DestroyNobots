namespace DestroyNobots.Assembler
{
    public interface IPointer
    {
        Address Address { get; set; }
        int Size { get; }
    }
}