using DestroyNobots.Assembler.Emulator.Peripherals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyNobots.Assembler.Emulator
{
    public class Computer : IRuntimeContext
    {
        private bool powerStatus;
        private IMemory rom;
        private LinkedList<IPeripheral> peripherals;
        private RAMMemory physicalMemory;

        public IMemory Memory { get { return physicalMemory; } }
        public IProcessorBase Processor { get; private set; }
        public Dictionary<ushort, PeripheralPortHandler> Ports { get; private set; }

        public Computer(IProcessorBase processor, IMemory memory, IMemory rom = null)
        {
            Ports = new Dictionary<ushort, PeripheralPortHandler>();
            peripherals = new LinkedList<Peripherals.IPeripheral>();

            this.rom = rom;
            this.Processor = processor;
            this.Processor.Computer = this;
            this.physicalMemory = new RAMMemory(memory);

            powerStatus = false;
        }

        public void SwitchROM(IMemory newRom)
        {
            rom = newRom;
        }

        public void ConnectPeripheral(IPeripheral peripheral)
        {
            peripheral.Install();
            peripherals.AddLast(peripheral);
        }

        public void DisconnectPeripheral(IPeripheral peripheral)
        {
            peripheral.Uninstall();
            peripherals.Remove(peripheral);
        }

        public T GetSpecificProcessor<T>()
            where T : IProcessorBase
        {
            return (T)Processor;
        }

        #region Synchronous locking user thread 
        public void PowerUpAndRun()
        {
            if (!powerStatus)
            {
                PowerUp();
                Processor.Run();
            }
        }

        public void Run()
        {
            if (powerStatus)
                Processor.Run();
        }
        #endregion

        #region Synchronous
        public void PowerUp()
        {
            if (!powerStatus)
            {
                powerStatus = true;

                physicalMemory.PowerUp();

                if(rom != null)
                    Memory.Write(0, rom.Read(0, (uint)rom.MemorySize));

                Processor.Reset();
            }
        }

        public void Step()
        {
            if (powerStatus)
                Processor.Step();
        }

        public void Reset()
        {
            if(powerStatus)
            {
                PowerDown();
                PowerUp();
            }
        }

        public void PowerDown()
        {
            if (powerStatus)
            {
                powerStatus = false;
                physicalMemory.PowerDown();
                Processor.Abort();
            }
        }
        #endregion

        #region Asynchronous
        public async Task PauseAsync()
        {
            if (powerStatus)
            {
                Processor.Pause();
                while (Processor.Running) { await Task.Delay(1); }
            }
        }

        public async Task RunAsync()
        {
            if (powerStatus)
                await Task.Run(() => Processor.Run());
        }

        public async Task ResetAsync()
        {
            if (powerStatus)
            {
                PowerDown();
                while(Processor.Running) { await Task.Delay(1); }
                PowerUp();
            }
        }

        T IRuntimeContext.GetContext<T>()
        {
            return (T)(object)this;
        }
        #endregion
    }
}
