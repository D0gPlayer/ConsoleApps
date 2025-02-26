using ConsoleApps.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ConsoleApps.ProgressGame
{
    //Create progress bar that reflects task progress
    public class ProgressBar
    {
        const char _fill = '■';
        const char _back = '\b';
        const char _empty = ' ';
        const char _start = '[';
        const char _end = ']';
        const double _baseAcceleration = 0.02;
        

        private Timer _timer;
        private ConsoleColor _color;
        private int _progress;
        private int _cursorRow;
        private readonly object _consoleSyncContext;
        private readonly string[] _arrows = new string[]{ "▄", "■", "▀"};
        private double _distance;
        private double _acceleration;
        private double _maxSpeed;
        private double _speed = 10;

        public ProgressBar(object syncContext, int count, double acceleration, double maxSpeed)
        {
            _consoleSyncContext = syncContext;
            _acceleration = acceleration;
            _maxSpeed = maxSpeed;
            Console.WriteLine();
            _cursorRow = count;
        }

        public void StartProgress(double distanceInMeters)
        {
            _distance = distanceInMeters;
            _color = (ConsoleColor)Random.Shared.Next(1,16);
            _timer = new Timer(GetTimerIntervalFromSpeed(_distance, 60));
            _timer.Elapsed += ProcessTimer;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private double GetMpsFromKph(double kph)
        {
            return kph * 1000 / 60 / 60;
        }

        private double GetTimerIntervalFromSpeed(double distanceInMeters, double speedKmH)
        {
            return TimeSpan.FromSeconds(distanceInMeters / GetMpsFromKph(speedKmH)).TotalMilliseconds / 100;
        }

        private void ProcessTimer(Object source, ElapsedEventArgs e)
        {
            if(_progress >= 100) _timer.Stop();

            _progress++;
            if(_speed < _maxSpeed) 
                _speed += ((_maxSpeed - _speed) * _baseAcceleration) * _acceleration * (_timer.Interval / 1000);

            _timer.Interval = GetTimerIntervalFromSpeed(_distance, _speed);
            DrawProgressBar(_progress);
        }

        private void DrawProgressBar(int percent)
        {      
            if(percent > 100) percent = 100;
            
            var progressBarString = new StringBuilder();
            progressBarString.Append(_start);
            progressBarString.Append(new string(_fill, percent));
            if(percent < 100) progressBarString.Append(_arrows[percent % 3]);
            progressBarString.Append(new string(_empty, 100 - percent));
            progressBarString.Append(_end);
            //progressBarString.Append($" {percent}%");
            progressBarString.Append($" {(int)_speed} km/h");
            progressBarString.Append(new string(_back, progressBarString.Length));

            lock (_consoleSyncContext)
            {
                Console.CursorTop = _cursorRow;
                ConsoleClr.Write(progressBarString.ToString(), _color);
            }
        }

        
    }
}
