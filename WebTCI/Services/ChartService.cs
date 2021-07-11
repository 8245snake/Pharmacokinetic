using System;
using System.Drawing;
using Blazor.Extensions.Canvas.WebGL;
using Simulator.Models;
using static WebTCI.Services.ChartService;

namespace WebTCI.Services
{
    public class ChartService
    {
        public int CanvasWidth
        {
            get => _canvasWidth;
            set
            {
                _canvasWidth = value;
                ChartRect = GetChartRectangle();
            }
        }

        public int CanvasHeight
        {
            get => _canvasHeight;
            set
            {
                _canvasHeight = value;
                ChartRect = GetChartRectangle();
            }
        }

        public int PaddeingLeft
        {
            get => _paddeingLeft;
            set
            {
                _paddeingLeft = value;
                ChartRect = GetChartRectangle();
            }
        }

        public int PaddeingRight
        {
            get => _paddeingRight;
            set
            {
                _paddeingRight = value;
                ChartRect = GetChartRectangle();
            }
        }

        public int PaddeingTop
        {
            get => _paddeingTop;
            set
            {
                _paddeingTop = value;
                ChartRect = GetChartRectangle();
            }
        }

        public int PaddeingBottom
        {
            get => _paddeingBottom;
            set
            {
                _paddeingBottom = value;
                ChartRect = GetChartRectangle();
            }
        }

        public Rectangle ChartRect;

        private DateTime _startTime;
        private DateTime _endTime;
        private TimeScaleEnum _timeScale = TimeScaleEnum.OneHour;
        private int _canvasWidth = 1600;
        private int _canvasHeight = 600;
        private int _paddeingLeft = 100;
        private int _paddeingRight = 30;
        private int _paddeingTop = 30;
        private int _paddeingBottom = 30;
        private DateTime _simulationStartTime = DateTime.Now.AddHours(-2);

        public enum TimeScaleEnum
        {
            TenMinute = 0,
            ThirtyMinute = 1,
            OneHour = 2,
            FoirHour = 3,
            EightHour = 4,
            TwelveHour = 5,
        }

        public TimeScaleEnum TimeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
                SetTimeRange(_startTime, _startTime.AddMinutes(value.FullMinutes()));
            }
        }

        public DateTime SimulationStartTime
        {
            get => _simulationStartTime;
            set => _simulationStartTime = value;
        }

        public DateTime StartTime => _startTime;

        public DateTime EndTime => _endTime;

        public delegate void TimeRangeChangedHandler(DateTime startTime, DateTime endTime);

        public event TimeRangeChangedHandler TimeRangeChanged;

        public ChartService()
        {
            ChartRect = GetChartRectangle();
        }

        private Rectangle GetChartRectangle()
        {
            return new Rectangle(
                PaddeingLeft,
                PaddeingTop,
                CanvasWidth - PaddeingLeft - PaddeingRight,
                CanvasHeight - PaddeingTop - PaddeingBottom);
        }

        public void SetTimeRange(DateTime startTime, DateTime endTime)
        {
            _startTime = startTime.AddSeconds(-1 * startTime.Second);
            _endTime = endTime.AddSeconds(-1 * endTime.Second);
            OnTimeRangeChanged(startTime, endTime);
        }

        protected virtual void OnTimeRangeChanged(DateTime starttime, DateTime endtime)
        {
            TimeRangeChanged?.Invoke(starttime, endtime);
        }

        public PointF CalculatePoint(DateTime time, double concentration, WebTCI.Models.MedicineModel medicine)
        {

            var totalSeconds = (EndTime - StartTime).TotalSeconds;
            var pixelsBysecond = ChartRect.Width / totalSeconds;
            float x = (float)(ChartRect.X + (time - StartTime).TotalSeconds * pixelsBysecond);

            var pyByOne = ChartRect.Height / medicine.DisplayValueMax;
            float y = (float)(ChartRect.Bottom - (concentration * pyByOne));

            return new PointF(x, y);
        }
    }

    public static class TimeScaleEnumExtend
    {
        public static int HalfMinutes(this TimeScaleEnum scale)
        {
            switch (scale)
            {
                case TimeScaleEnum.TenMinute:
                    return 5;
                case TimeScaleEnum.ThirtyMinute:
                    return 15;
                case TimeScaleEnum.OneHour:
                    return 30;
                case TimeScaleEnum.FoirHour:
                    return 120;
                case TimeScaleEnum.EightHour:
                    return 240;
                case TimeScaleEnum.TwelveHour:
                    return 360;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scale), scale, null);
            }
        }

        public static int FullMinutes(this TimeScaleEnum scale)
        {
            return scale.HalfMinutes() * 2;
        }

        public static int StepMinutes(this TimeScaleEnum scale)
        {
            switch (scale)
            {
                case TimeScaleEnum.TenMinute:
                    return 1;
                case TimeScaleEnum.ThirtyMinute:
                    return 2;
                case TimeScaleEnum.OneHour:
                    return 5;
                case TimeScaleEnum.FoirHour:
                    return 20;
                case TimeScaleEnum.EightHour:
                    return 30;
                case TimeScaleEnum.TwelveHour:
                    return 60;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scale), scale, null);
            }
        }
    }
}