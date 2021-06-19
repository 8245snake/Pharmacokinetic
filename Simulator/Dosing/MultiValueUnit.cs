using System.Collections.Generic;
using static Simulator.Dosing.ValueUnit;

namespace Simulator.Dosing
{
    public class MultiValueUnit : ValueUnit
    {
        private List<WeightUnitEnum> _WeightUnits = new List<WeightUnitEnum>();
        private List<WeightUnitEnum> _WeightUnitsInverse = new List<WeightUnitEnum>();

        private List<VolumeUnitEnum> _VolumeUnits = new List<VolumeUnitEnum>();
        private List<VolumeUnitEnum> _VolumeUnitsInverse = new List<VolumeUnitEnum>();

        private List<TimeUnitEnum> _TimeUnits = new List<TimeUnitEnum>();
        private List<TimeUnitEnum> _TimeUnitsInverse = new List<TimeUnitEnum>();


    }
}