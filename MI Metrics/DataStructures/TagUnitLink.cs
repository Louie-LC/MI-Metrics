using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class TagUnitLink
    {
        private int tagID;
        private int unitID;

        public int TagID { get => tagID; set => tagID = value; }
        public int UnitID { get => unitID; set => unitID = value; }

        public TagUnitLink(int tagID, int unitID)
        {
            TagID = tagID;
            UnitID = unitID;
        }
    }
}
