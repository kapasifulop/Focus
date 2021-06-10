using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus
{
    public static class Paths
    {
        public static string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string Focus = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\focus";

        public static string XML = Focus + @"\service.data";
    }
}
