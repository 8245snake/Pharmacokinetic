﻿
using System;
using System.Runtime.CompilerServices;
using Simulator.Dosing;

namespace Simulator.Dosing
{
    public static class EnumExtension
    {

        public static string Name(this ValueUnit.VolumeUnitEnum self)
        {
            switch (self)
            {
                case ValueUnit.VolumeUnitEnum.L:
                    return "L";
                case ValueUnit.VolumeUnitEnum.mL:
                    return "ml";
                case ValueUnit.VolumeUnitEnum.None:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static string Name(this ValueUnit.WeightUnitEnum self)
        {
            switch (self)
            {
                case ValueUnit.WeightUnitEnum.g:
                    return "g";
                case ValueUnit.WeightUnitEnum.mg:
                    return "mg"; ;
                case ValueUnit.WeightUnitEnum.ug:
                    return "μg";
                case ValueUnit.WeightUnitEnum.ng:
                    return "ng";
                case ValueUnit.WeightUnitEnum.None:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }


    }
}