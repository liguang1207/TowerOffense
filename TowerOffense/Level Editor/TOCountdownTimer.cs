using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerOffense.Level_Editor
{
    public class TOCountdownTimer
    {
        private int pSecondsLeft = 0;
        private Boolean pTimerRunning = false;
        private DateTime pLastTick;

        public TOCountdownTimer(int aSecondsLeft)
        {
            Start(aSecondsLeft);
        }

        public void Start(int aSecondsLeft)
        {
            pSecondsLeft = aSecondsLeft;
            pTimerRunning = true;
            pLastTick = DateTime.Now;
        }

        public void Stop()
        {
            pSecondsLeft = 0; ;
            pTimerRunning = false;
        }

        public void Update(GameTime aGameTime)
        {
            if (DateTime.Now.Subtract(pLastTick).TotalSeconds >= 1 && pSecondsLeft > 0)
            {
                pSecondsLeft--;
                pLastTick = DateTime.Now;
            }
            else if (pSecondsLeft == 0)
            {
                pTimerRunning = false;
            }
        }

        public Boolean Completed
        {
            get { return (!pTimerRunning && pSecondsLeft == 0); }
        }

        public Boolean Running
        {
            get { return pTimerRunning; }
        }

        public String TimeLeft
        {
            get
            {
                int Minutes = 0;
                int Seconds = pSecondsLeft;

                while (Seconds > 59)
                {
                    Minutes++;
                    Seconds -= 60;
                }

                return Minutes.ToString() + ":" + (Seconds < 10 ? "0" + Seconds.ToString() : Seconds.ToString());
            }
        }
    }
}
