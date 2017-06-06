using System;
using Microsoft.Xna.Framework;

namespace DestroyNobots
{
    public class Timer : IUpdateable
    {
        private int accumulator;
        private int interval;
        private bool timeout;
        private bool running;

        public int Interval { get { return interval; } }
        public bool IsTimeout { get { return timeout; } }

        public event EventHandler<TimerEventArgs> Tick;

        public Timer()
        {
            interval = 0;
            accumulator = 0;
            timeout = false;
            running = false;
        }

        public Timer(int interval, bool timeout)
            : this()
        {
            Set(interval, timeout);
        }

        public void Set(int interval, bool timeout)
        {
            this.interval = interval;
            this.timeout = timeout;
            Start();
        }

        public void Prepare(int interval, bool timeout)
        {
            this.interval = interval;
            this.timeout = timeout;
        }

        public void Start()
        {
            running = true;
        }

        public void Stop()
        {
            running = false;
        }

        public void Update(GameTime gt)
        {
            if(Interval > 0 && running)
            {
                accumulator += (int)gt.ElapsedGameTime.TotalMilliseconds;

                if (Tick != null)
                {
                    while (accumulator > Interval)
                    {
                        Tick(this, new TimerEventArgs());
                        accumulator -= Interval;

                        if (timeout)
                            running = false;
                    }
                }
            }   
        }

        public bool FetchTick()
        {
            if(Tick == null)
            {
                if (running && Interval > 0)
                {
                    if (accumulator > Interval)
                    {
                        accumulator -= Interval;
                        
                        if (timeout)
                            running = false;

                        return true;
                    }
                }

                return false;
            }

            throw new Exception("You should not use Tick event and this method at once!");
        }
    }
}
