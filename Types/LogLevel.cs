using System.ComponentModel;

namespace Novelog.Types;

public enum LogLevel : byte
{
    [Description("Information that is useful for debugging")]
    DEBUG,
    
    [Description("Information that is useful for monitoring")]
    INFO,
    
    [Description("When something unexpected happened")]
    WARN,
    
    [Description("When server did not crash but stopped something from executing properly")]
    ERROR,
    
    [Description("When server crashed")]
    CRIT,
}