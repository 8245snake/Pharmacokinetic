using System;

namespace WebTCI.Services
{
    public class TimePointingService
    {
        private DateTime _pointingTime;

        public DateTime PointingTime
        {
            get => _pointingTime;
            set
            {
                _pointingTime = value;
                PointingTimeChanged?.Invoke(this, value);
            }
        }

        public event PointingTimeChangedHandler PointingTimeChanged;

        public delegate void PointingTimeChangedHandler(TimePointingService sender, DateTime pointingTime);

    }
}