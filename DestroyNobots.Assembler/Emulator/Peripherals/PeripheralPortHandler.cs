namespace DestroyNobots.Assembler.Emulator.Peripherals
{
    public delegate void PeripheralPortOut(long data, byte size);
    public delegate long PeripheralPortIn(byte size);

    public class PeripheralPortHandler
    {
        public PeripheralPortIn In { get; set; }
        public PeripheralPortOut Out { get; set; }
        public IPeripheral Peripheral { get; private set; }

        public PeripheralPortHandler(IPeripheral peripheral)
        {
            this.Peripheral = peripheral;
        }
    }

}