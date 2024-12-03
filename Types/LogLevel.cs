namespace Novelog.Types;

/// <summary>
/// Represents the severity level of the log.
/// </summary>
public enum LogLevel : byte
{
    /// <summary>
    /// Information that is useful for debugging
    /// </summary>
    DEBUG,
    
    /// <summary>
    /// Information that is useful for monitoring
    /// </summary>
    INFO,
    
    /// <summary>
    /// When something unexpected happened
    /// </summary>
    WARN,
    
    /// <summary>
    /// When server did not crash but stopped something from executing properly
    /// </summary>
    ERROR,
    
    /// <summary>
    /// When application crashes
    /// </summary>
    CRIT,
}