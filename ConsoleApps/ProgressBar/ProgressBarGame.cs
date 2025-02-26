using ConsoleApps.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ConsoleApps.ProgressGame
{
    public class ProgressBarGame
    {
        private readonly object _consoleSyncContext = new object(); 
        private Timer _timer;
        private int _timeElapsed;

        public void Start()
        {
            _timer = new Timer(100);
            _timer.Elapsed += DrawUI;
            _timer.AutoReset = true;
            
            var count = 1;
            var distance = 1000;
            while (true)
            {
                Console.ReadKey();
                if(!_timer.Enabled) _timer.Start();
                
                new ProgressBar(_consoleSyncContext, count++, 6.5, 180).StartProgress(distance);
                new ProgressBar(_consoleSyncContext, count++, 2, 320).StartProgress(distance);
                new ProgressBar(_consoleSyncContext, count++, 2.5, 260).StartProgress(distance);
            }
        }

        private void DrawUI(Object source, ElapsedEventArgs e)
        {
            lock (_consoleSyncContext)
            {
                _timeElapsed += (int)_timer.Interval;
                var time = TimeSpan.FromMilliseconds(_timeElapsed);
                Console.CursorTop = 0;
                ConsoleClr.Write($" {time.Seconds}:{time.Milliseconds}");
                Console.CursorLeft = 0;
            }
        }
    }
}