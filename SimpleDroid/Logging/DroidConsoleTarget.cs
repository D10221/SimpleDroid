using NLog;
using NLog.Targets;
using Log = Android.Util.Log;
namespace SimpleDroid.Logging
{
    [Target("DroidConsole")]
    public sealed class DroidConsoleTarget : TargetWithLayout
    {
        
        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = this.Layout.Render(logEvent);

            if (logEvent.Level.Equals(LogLevel.Info))
            {
                Log.Info(logEvent.LoggerName, logMessage);
                return;
            }
            if (logEvent.Level.Equals(LogLevel.Debug))
            {
                Log.Debug(logEvent.LoggerName, logMessage);
                return;
            }
            if (logEvent.Level.Equals(LogLevel.Warn))
            {
                Log.Warn(logEvent.LoggerName, logMessage);
                return;
            }
            if (logEvent.Level.Equals(LogLevel.Error))
            {
                Log.Error(logEvent.LoggerName, logMessage);
                return;
            }
        }        
        
    }

}