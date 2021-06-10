using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Focus_Service
{
    public partial class FocusService : ServiceBase
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string focusFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\focus";
        string comm_file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\focus\service.data";

        public FocusService()
        {
            InitializeComponent();

            if (!Directory.Exists(focusFolder))
            {
                Directory.CreateDirectory(focusFolder);
            }

            if (!File.Exists(comm_file))
            {
                File.WriteAllText(comm_file, "Autocreate");
            }
        }

        protected override void OnCustomCommand(int command)
        {
            switch (command)
            {
                case 1:
                    //Refresh time datas
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
    }
}
