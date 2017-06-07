namespace DestroyNobots.Assembler.Emulator.Peripherals
{
    public delegate void PeripheralPortOut();
    public delegate void PeripheralPortIn();

    public class PeripheralPortHandler
    {
        public PeripheralPortIn In { get; set; }
        public PeripheralPortOut Out { get; set; }
    }

}