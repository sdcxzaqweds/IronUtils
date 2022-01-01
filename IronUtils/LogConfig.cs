/*************************************
     作者： Iron
     邮箱： sdcxzaqweds@126.com
     日期： 2021/12/22 1:28
     功能： 日志配置
 *************************************/

using System;

namespace IronUtils
{
    public enum LogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }
    
    public enum LoggerType
    {
        Unity,
        Console
    }
    
    public class LogConfig
    {
        public bool enableLog = true;
        public string LogPrefix = "#";
        public bool enableTime = true;
        public string logSeparate = ">>";
        public bool enableThreadID = true;
        public bool enableTrace = true;
        public bool enableSave = true;
        public bool enableCover = true;
        public string SavePath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory);
        public string SaveName = "ConsoleIronLog.txt";
        public LoggerType loggerType = LoggerType.Console;
    }

    interface ILogger
    {
        void Log(string msg, LogColor logColor = LogColor.None);
        void Warn(string msg);
        void Error(string msg);
    }
}