using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace MouseInverter
{
    class Inverter
    {
        private Point currentPosition;

        private bool running;

        private bool exit;

        static int Main(string[] args)
        {

            Inverter inverter = new Inverter();

            Console.CancelKeyPress += delegate
            {
                inverter.Stop();
            };

            inverter.Start();
            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        public bool Running
        {
            get
            {
                return this.running;
            }
        }
                
        private void MouseLoop()
        {
            Thread.CurrentThread.IsBackground = true;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            while (!this.exit)
            {
                Point newPosition = Cursor.Position;

                int bottom = this.currentPosition.Y - (newPosition.X - this.currentPosition.X);
                int maxHeight = SystemInformation.VirtualScreen.Height;
                if (bottom > maxHeight)
                {
                    bottom = maxHeight;
                }
                else if (bottom < 0)
                {
                    bottom = 0;
                }

                int right = this.currentPosition.X + (newPosition.Y - this.currentPosition.Y);
                int maxWidth = SystemInformation.VirtualScreen.Width;
                if (right > maxWidth)
                {
                    right = maxWidth;
                }
                else if (right < 0)
                {
                    right = 0;
                }
                
                Cursor.Position = new Point(right, bottom);
                this.currentPosition = Cursor.Position;
                Thread.Sleep(1);
            }
            this.exit = false;
        }

        public void Start()
        {
            this.currentPosition = Cursor.Position;
            this.running = true;
            (new Thread(new ThreadStart(this.MouseLoop))).Start();
        }

        public void Stop()
        {
            this.running = false;
            this.exit = true;
        }
    }
}
