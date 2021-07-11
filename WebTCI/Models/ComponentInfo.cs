using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTCI.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ComponentInfo
    {
        public string ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int ScrollLeft { get; set; }
        public int ScrollTop { get; set; }

        public double Right => X + Width;
        public double Bottom => Y + Height;

        public override string ToString()
        {
            return $"{nameof(ID)}: {ID}, {nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Height)}: {Height}, {nameof(Width)}: {Width}";
        }

        public bool IsSame(ComponentInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Width == Width && other.Height == Height;
        }

        public ComponentInfo DeepCopy()
        {
            var clone = new ComponentInfo()
            {
                ID = ID,
                X = X,
                Y = Y,
                Height = Height,
                Width = Width,
                ScrollLeft = ScrollLeft,
                ScrollTop = ScrollTop
            };
            return clone;
        }
    }
}