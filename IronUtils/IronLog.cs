/*************************************
     作者： Iron
     邮箱： sdcxzaqweds@126.com
     日期： 2021/12/22 1:28
     功能： 日志工具核心类
 *************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

public static class ExtensinMethonds
{
        //this object为原始类型扩展方法 主要是用来我们平时经常用到的类型，需要用动时只需new object.Log(...)
        public static void Log(this object obj, string log, params object[] args)
        {
            IronUtils.IronLog.Log(string.Format(log,args));
        }
        
        public static void Log(this object obj, params object[] args)
        {
            IronUtils.IronLog.Log(obj);
        }

        public static void ColorLog(this object obj, IronUtils.LogColor color, string log, params object[] args)
        {
            IronUtils.IronLog.ColorLog(color, String.Format(log,args));
        }
        
        public static void ColorLog(this object obj, IronUtils.LogColor color, object log)
        {
            IronUtils.IronLog.ColorLog(color, log);
        }
        
        public static void Trace(this object obj, string log, params object[] args)
        {
            IronUtils.IronLog.Trace(string.Format(log,args));
        }
        
        public static void Trace(this object obj, object log)
        {
            IronUtils.IronLog.Trace(log);
        }

        public static void Warn(this object obj, string log, params object[] args)
        {
            IronUtils.IronLog.Warn(string.Format(log,args));
        }
        
        public static void Warn(this object obj, object log)
        {
            IronUtils.IronLog.Warn(log);
        }
        
        public static void Error(this object obj, string log, params object[] args)
        {
            IronUtils.IronLog.Error(string.Format(log,args));
        }
        
        public static void Error(this object obj, object log)
        {
            IronUtils.IronLog.Warn(log);
        }
}

namespace IronUtils
{
    public static class IronLog
    {
        class UnityLogger:ILogger
        {
            private Type type = Type.GetType("UnityEngine.Debug, UnityEngine");
            public void Log(string msg, LogColor logColor = LogColor.None)
            {
                if (logColor != LogColor.None)
                {
                    msg = ColorUnityLog(msg, logColor);
                }
                type.GetMethod("Log", new Type[] {typeof(object)})?.Invoke(null, new object[] {msg});
            }
            public void Warn(string msg)
            {
                type.GetMethod("LogWarning", new Type[] {typeof(object)})?.Invoke(null, new object[] {msg});
            }
            public void Error(string msg)
            {
                type.GetMethod("LogError", new Type[] {typeof(object)})?.Invoke(null, new object[] {msg});
            }

            private string ColorUnityLog(string msg, LogColor color)
            {
                switch (color)
                {
                    case LogColor.None:
                        break;
                    case LogColor.Red:
                        msg = $"<color=#FF0000>{msg}</color>";
                        break;
                    case LogColor.Green:
                        msg = $"<color=#00FF00>{msg}</color>";
                        break;
                    case LogColor.Blue:
                        msg = $"<color=#0000FF>{msg}</color>";
                        break;
                    case LogColor.Cyan:
                        msg = $"<color=#00FFFF>{msg}</color>";
                        break;
                    case LogColor.Magenta:
                        msg = $"<color=#FF00FF>{msg}</color>";
                        break;
                    case LogColor.Yellow:
                        msg = $"<color=#FFFF00>{msg}</color>";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(color), color, null);
                }

                return msg;
            }
        }
        
        class ConsoleLogger: ILogger
        {
            public void Log(string msg, LogColor logColor = LogColor.None)
            {
                WriteConsoleLog(msg, logColor);
            }
            public void Warn(string msg)
            {
                WriteConsoleLog(msg, LogColor.Yellow);
            }
            public void Error(string msg)
            {
                WriteConsoleLog(msg, LogColor.Red);
            }

            private void WriteConsoleLog(string msg, LogColor color)
            {
                switch (color)
                {
                    case LogColor.None:
                        PrintConsole(ConsoleColor.Gray, msg);
                        break;
                    case LogColor.Red:
                        PrintConsole(ConsoleColor.DarkRed, msg);
                        break;
                    case LogColor.Green:
                        PrintConsole(ConsoleColor.Green, msg);
                        break;
                    case LogColor.Blue:
                        PrintConsole(ConsoleColor.Blue, msg);
                        break;
                    case LogColor.Cyan:
                        PrintConsole(ConsoleColor.Cyan, msg);
                        break;
                    case LogColor.Magenta:
                        PrintConsole(ConsoleColor.Magenta, msg);
                        break;
                    case LogColor.Yellow:
                        PrintConsole(ConsoleColor.DarkYellow, msg);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(color), color, null);
                }
            }
            
            static void PrintConsole(ConsoleColor color, string msg)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static LogConfig cfg;
        private static ILogger _logger;
        private static StreamWriter _logFileWriter = null;
        public static void InitSetting(LogConfig cfg=null)
        {
            if (cfg == null)
            {
                cfg = new LogConfig();
            }

            IronLog.cfg = cfg;
            if (cfg.loggerType == LoggerType.Console) {
                _logger = new ConsoleLogger();
            }
            else
            {
                _logger = new UnityLogger();
            }
            //自动保存
            if (!cfg.enableSave) return;

            if (cfg.enableCover)
            {
                string path = cfg.SavePath + cfg.SaveName;
                try
                {
                    if (Directory.Exists(cfg.SavePath))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(cfg.SavePath);
                    }
                    _logFileWriter = File.AppendText(path);
                    _logFileWriter.AutoFlush = true;
                }
                catch
                {
                    _logFileWriter = null;
                }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-ss");
                string path = cfg.SavePath + prefix + cfg.SaveName;
                try
                {
                    if (Directory.Exists(cfg.SavePath))
                    {
                        Directory.CreateDirectory(cfg.SavePath);
                    }

                    _logFileWriter = File.AppendText(path);
                    _logFileWriter.AutoFlush = true;
                }
                catch (Exception e)
                {
                    _logFileWriter = null;
                }
            }
        }

        private static string DecorateLog(string msg, bool isTrace = false)
        {
            StringBuilder sb = new StringBuilder(cfg.LogPrefix, 100);
            if (cfg.enableTime)
            {
                sb.AppendFormat("{0:hh:mm:ss--fff}  ", DateTime.Now);
            }

            if (cfg.enableThreadID)
            {
                sb.AppendFormat("{0}", GetThreadId());
            }

            sb.AppendFormat("{0} {1}   ", cfg.logSeparate, msg);
            if (isTrace)
            {
                sb.AppendFormat("\nStackTrace:{0}", GetLogTrace());
            }

            return sb.ToString();
        }

        private static string GetThreadId()
        {
            return $"ThreadID:{Thread.CurrentThread.ManagedThreadId}";
        }

        private static string GetLogTrace()
        {
            StackTrace st =new StackTrace(3,true);

            string traceInfo = "";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                traceInfo += $"\n    {sf.GetFileName()}::{sf.GetMethod()}   line:{sf.GetFileLineNumber()}";
            }
            return traceInfo;
        }

        private static void writeToFile(string msg)
        {
            if (cfg.enableSave && _logFileWriter != null)
            {
                try
                {
                    _logFileWriter.WriteLine(msg);
                }
                catch (Exception e)
                {
                    _logFileWriter = null;
                }
            }
        }

        #region 对外接口
        public static void Log(string msg, params object[] args)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            msg = DecorateLog(string.Format(msg, args));
            _logger.Log(msg);
            if (cfg.enableSave)
            {
                writeToFile($"[L]{msg}");
            }
        }
        
        public static void Log(object obj, params object[] args)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            string msg = DecorateLog(obj.ToString());
            _logger.Log(msg);
            if (cfg.enableSave)
            {
                writeToFile($"[L]{msg}");
            }
        }

        public static void ColorLog(LogColor color, string msg, params object[] args)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            msg = DecorateLog(string.Format(msg, args));
            _logger.Log(msg, color);
            if (cfg.enableSave)
            {
                writeToFile($"[L]{msg}");
            }
        }
        
        public static void ColorLog(LogColor color, object obj)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            string msg = DecorateLog(obj.ToString());
            _logger.Log(msg, color);
            if (cfg.enableSave)
            {
                writeToFile($"[L]{msg}");
            }
        }
        
        public static void Trace(string msg, params object[] args)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            msg = DecorateLog(string.Format(msg, args), cfg.enableTrace);
            _logger.Log(msg, LogColor.Magenta);
            if (cfg.enableSave)
            {
                writeToFile($"[T]{msg}");
            }
        }
        
        public static void Trace(object obj)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            string msg = DecorateLog(obj.ToString(), cfg.enableTrace);
            _logger.Log(msg, LogColor.Magenta);
            if (cfg.enableSave)
            {
                writeToFile($"[T]{msg}");
            }
        }

        public static void Warn(string msg, params object[] args)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            msg = DecorateLog(string.Format(msg, args));
            _logger.Log(msg);
            if (cfg.enableSave)
            {
                writeToFile($"[T]{msg}");
            }
        }
        
        public static void Warn(object obj)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            string msg = DecorateLog(obj.ToString());
            _logger.Warn(msg);
            if (cfg.enableSave)
            {
                writeToFile($"[T]{msg}");
            }
        }
        
        public static void Error(string msg, params object[] args)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            msg = DecorateLog(string.Format(msg, args), cfg.enableTrace);
            _logger.Error(msg);
            if (cfg.enableSave)
            {
                writeToFile($"[E]{msg}");
            }
        }
        
        public static void Error(object obj)
        {
            if (cfg.enableLog==false)
            {
                return;
            }
            string msg = DecorateLog(obj.ToString(), cfg.enableTrace);
            _logger.Error(msg);
            if (cfg.enableSave)
            {
                writeToFile($"[E]{msg}");
            }
        }
        #endregion
    }
}