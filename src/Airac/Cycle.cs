using System;
namespace Airac;

public class Cycle
{
    private static TimeSpan Duration => TimeSpan.FromDays(28);

    /// <summary>
    /// The date from which cycles are calculated relative to
    /// </summary>
    private static DateOnly _epoch => new DateOnly(1901, 1, 10);

    /// <summary>
    /// The number of cycles since the epoch
    /// </summary>
    private int _serial;

    /// <summary>
    /// The start date of a 28 day AIRAC cycle
    /// </summary>
    public DateOnly EffectiveDate => _epoch.AddDays(_serial * Duration.Days);

    /// <summary>
    /// Represents the position of cycle in the series of cycles
    /// for the given year
    /// </summary>
    public int Ordinal => (EffectiveDate.DayOfYear / Duration.Days) + 1;

    /// <summary>
    /// Commonly used representation of the cycle in the format YYoo
    /// </summary>
    public string Identifier => $"{EffectiveDate.Year % 100}{Ordinal:D2}";

    /// <summary>
    /// Constructs a cycle for the current date (and time UTC)
    /// </summary>
    public Cycle() : this(DateTime.UtcNow)
    { }

    /// <summary>
    /// Constructs a cycle for the given date
    /// </summary>
    /// <param name="dateTime"></param>
    public Cycle(DateTime dateTime) : this(DateOnly.FromDateTime(dateTime))
    { }

    /// <summary>
    /// Constructs a cycle for the given date
    /// </summary>
    /// <param name="date"></param>
    public Cycle(DateOnly date)
        : this((date.DayNumber - _epoch.DayNumber) / Duration.Days)
    { }

    /// <summary>
    /// Constructs a cycle for the given identifier
    /// </summary>
    /// <param name="identifier">
    /// representation of the cycle in the format YYoo
    /// </param>
    public Cycle(string identifier)
    {

    }
    /// <summary>
    /// Constructs a cycle given an offset from the epoch
    /// </summary>
    /// <param name="serial">
    /// the number of cycles since the epoch
    /// </param>
    private Cycle(int serial) => _serial = serial;
}

