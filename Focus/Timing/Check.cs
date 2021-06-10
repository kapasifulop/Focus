using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.timing
{
    public static class Timing
    {
        public static int Check()
        {
            Timingset.TimingDataTable timingTable = new Timingset.TimingDataTable();
            timingTable.ReadXml(Paths.XML);

            foreach (DataRow drow in timingTable.Rows)
            {
                int Id = (int)drow["Id"] - 1;

                DateTime startTime = (DateTime)drow["StartTime"];
                DateTime endTime = (DateTime)drow["EndTime"];
                bool enabled = (bool)drow["Enabled"];
                int mode = (int)drow["Mode"];



                if (DateTime.Now > startTime && DateTime.Now < endTime && enabled)
                {
                    //Inside startLoop
                    timingTable.Rows[Id]["Enabled"] = false;
                    timingTable.WriteXml(Paths.XML);
                    return mode;
                }
            }

            return Modes.Default;
        }
    }
}
