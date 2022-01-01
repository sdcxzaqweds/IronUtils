using System.Collections;
using System.Collections.Generic;
using IronUtils;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    { 
        LogConfig config = new LogConfig()
        {
            enableLog = true,
            LogPrefix = "",
            enableTime = true,
            logSeparate = ">",
            enableThreadID = true,
            enableTrace = false,
            enableSave = true,
            enableCover = true,
            loggerType = LoggerType.Unity,
            SavePath = Application.persistentDataPath + "/IronLog/",
            SaveName = "ClientPELog.txt"
        };
        
        IronLog.InitSetting(config);
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
