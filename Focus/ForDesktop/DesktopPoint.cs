using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Focus.ForDesktop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DesktopPoint
    {
        public int X;
        public int Y;

        public DesktopPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public struct NamedDesktopPoint
    {
        public string Name;
        public int X;
        public int Y;

        public NamedDesktopPoint(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }
    }
}
