using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace ItemPipes.Framework
{
    class Printer
    {
        private static IMonitor _monitor;

        public static void SetMonitor(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public static void Info(String message)
        {
            _monitor.Log(message, LogLevel.Info);
        }

        public static void Warn(String message)
        {
            _monitor.Log(message, LogLevel.Warn);
        }

        public static void Debug(String message)
        {
            _monitor.Log(message, LogLevel.Debug);
        }

        public static void Trace(String message)
        {
            _monitor.Log(message, LogLevel.Trace);
        }
    }
}
