using Focus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Focus_Service
{
    public partial class FocusService : ServiceBase
    {
        Thread Worker;
        AutoResetEvent StopRequest = new AutoResetEvent(false);

        Timingset.TimingDataTable timingTable = new Timingset.TimingDataTable();

        public FocusService()
        {
            InitializeComponent();

            if (!Directory.Exists(Paths.Focus))
            {
                Directory.CreateDirectory(Paths.Focus);
            }
        }

        protected override void OnCustomCommand(int command)
        {
            switch (command)
            {
                case 1:
                    //Refresh time datas
                    if (File.Exists(Paths.XML))
                    {
                        timingTable.ReadXml(Paths.XML);
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {

        }

        private void DoWork(object arg)
        {
            // Worker thread loop
            for (; ; )
            {
                // Run this code once every 10 seconds or stop right away if the service 
                // is stopped
                if (StopRequest.WaitOne(10000)) return;
                // Do work...
                //...
                foreach(DataRow drow in timingTable.Rows)
                {
                    int Id = (int)drow["Id"] - 1;

                    DateTime startTime = (DateTime)drow["StartTime"];
                    DateTime endTime = (DateTime)drow["EndTime"];
                    bool enabled = (bool)drow["Enabled"];
                    int mode = (int)drow["Mode"];



                    if(DateTime.Now > startTime && DateTime.Now < endTime && enabled)
                    {
                        //Inside startLoop
                        timingTable.Rows[Id]["Enabled"] = false;
                        timingTable.WriteXml(Paths.XML);
                    }
                }
            }
        }
    }
}
