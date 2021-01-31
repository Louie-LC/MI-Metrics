using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class UnitFrequencyPair
    {
        // This class is used as a container for the ViewPageViewModel to help with filtering the DeviceHistoryRecord datagrid.

        public Unit Unit { get => unit; set => unit = value; }
        private Unit unit;

        public int Frequency { get => frequency; set => frequency = value; }
        private int frequency;

        public UnitFrequencyPair(Unit unit, int frequency)
        {
            Unit = unit;
            Frequency = frequency;
        }
    }
}
