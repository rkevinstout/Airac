using System;
namespace Airac;

public class Cycle
{
    private static TimeSpan Duration => TimeSpan.FromDays(28);

    /// <summary>
    /// The date from which cycles are calculated relative to
    /// </summary>
    private static DateOnly Epoch => new(1901, 1, 10);

    /// <summary>
    /// The number of cycles since the epoch
    /// </summary>
    private readonly int _serial;

    /// <summary>
    /// The start date of a 28 day AIRAC cycle
    /// </summary>
    public DateOnly EffectiveDate => Epoch.AddDays(_serial * Duration.Days);

    /// <summary>
    /// The position of cycle in the series of cycles
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
        : this((date.DayNumber - Epoch.DayNumber) / Duration.Days)
    { }

    /// <summary>
    /// Constructs a cycle given an offset from the epoch
    /// </summary>
    /// <param name="serial">
    /// the number of cycles since the epoch
    /// </param>
    private Cycle(int serial) => _serial = serial;

    public static Cycle FromIdentifier(string identifier)
    {
        //if (!int.TryParse(identifier, out int integer))
        //{
        //    throw new ArgumentException($"Unable to parse {nameof(identifier)}", nameof(identifier));
        //}

        var integer = int.Parse(identifier, System.Globalization.NumberStyles.Integer);

        var ordinal = integer % 100;
        var year = 2000 + ((integer - ordinal) / 100);

        var endOfYear = new DateOnly(year - 1, 12, 31);
        var lastCycleOfYear = new Cycle(endOfYear);

        var serial = lastCycleOfYear._serial + ordinal;

        var cycle = new Cycle(serial);

        if (cycle.EffectiveDate.Year != year)
        {
            throw new ArgumentOutOfRangeException(
                nameof(identifier),
                $"{year} does not have {ordinal} cycles"
                );
        }

        return cycle;
    }
}

