
using System;
using System.Runtime.CompilerServices;
using Simulator.Dosing;

namespace Simulator.Dosing
{
    public static class EnumExtension
    {

        public static string Name(this Medicine.VolumeUnitEnum self)
        {
            switch (self)
            {
                case Medicine.VolumeUnitEnum.L:
                    return "L";
                case Medicine.VolumeUnitEnum.mL:
                    return "ml";
                case Medicine.VolumeUnitEnum.None:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static string Name(this Medicine.WeightUnitEnum self)
        {
            switch (self)
            {
                case Medicine.WeightUnitEnum.g:
                    return "g";
                case Medicine.WeightUnitEnum.mg:
                    return "mg"; ;
                case Medicine.WeightUnitEnum.ug:
                    return "μg";
                case Medicine.WeightUnitEnum.ng:
                    return "ng";
                case Medicine.WeightUnitEnum.None:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static double Factor(this Medicine.VolumeUnitEnum self)
        {
            switch (self)
            {
                case Medicine.VolumeUnitEnum.L:
                case Medicine.VolumeUnitEnum.mL:
                    return (int)self / 1000;
                case Medicine.VolumeUnitEnum.None:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static double Factor(this Medicine.WeightUnitEnum self)
        {
            switch (self)
            {
                case Medicine.WeightUnitEnum.g:
                case Medicine.WeightUnitEnum.mg:
                case Medicine.WeightUnitEnum.ug:
                case Medicine.WeightUnitEnum.ng:
                    return (int)self / 1000;
                case Medicine.WeightUnitEnum.None:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

    }
}