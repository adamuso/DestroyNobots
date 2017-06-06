using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DestroyNobots
{
    public class TimerManager : IUpdateable
    {
        private LinkedList<Timer> timers;

        public TimerManager()
        {
            timers = new LinkedList<Timer>();
        }

        public Timer Create()
        {
            Timer t = new Timer();
            timers.AddLast(t);

            return t;
        }

        public Timer Create(int interval, bool timeout)
        {
            Timer t = new Timer(interval, timeout);
            timers.AddLast(t);

            return t;
        }

        public Timer CreateAndStart(int interval, bool timeout)
        {
            Timer t = Create(interval, timeout);
            t.Start();

            return t;
        }

        public void Update(GameTime gt)
        {
            foreach(Timer timer in timers)
            {
                timer.Update(gt);
            }
        }
    }
}
