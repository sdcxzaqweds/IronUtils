using System;
using IronUtils;

namespace ServerLogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IronLog.InitSetting();
            IronLog.Log("{0} START...","SeverIronLog");
            IronLog.ColorLog(LogColor.Red, "Color Log:{0}", LogColor.Red.ToString());
            IronLog.ColorLog(LogColor.Blue, "Color Log:{0}", LogColor.Blue.ToString());
            IronLog.ColorLog(LogColor.Green, "Color Log:{0}", LogColor.Green.ToString());
            IronLog.ColorLog(LogColor.Cyan, "Color Log:{0}", LogColor.Cyan.ToString());
            IronLog.ColorLog(LogColor.Magenta, "Color Log:{0}", LogColor.Magenta.ToString());
            IronLog.ColorLog(LogColor.Yellow, "Color Log:{0}", LogColor.Yellow.ToString());
            
            Root root = new Root();
            root.Init();
        }
    }
}