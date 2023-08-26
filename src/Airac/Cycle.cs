using System.Globalization;

namespace Airac;

/// <summary>
/// The AIRAC cycle governs the production schedule of Aeronautical
/// Information Publications
/// </summary>
/// <remarks>
/// Cycles do not overlap with each begining on a Thursday (UTC) with a
/// duration of 28 days
/// </remarks>
/// <see cref="https://www.icao.int/airnavigation/information-management/Pages/AIRAC.aspx"/>
/// <seealso cref="https://en.wikipedia.org/wiki/Aeronautical_Information_Publication/>
public class Cycle
{
    /// <summary>
    /// The duration of each cycle as defined by ICAO
    /// </summary>
    private static TimeSpan Duration => TimeSpan.FromDays(28);

    /// <summary>
    /// The arbitrary date from which cycles are calculated relative to
    /// </summary>
    /// <remarks>
    /// The AIRAC system was introduced in 1964.  As such, dates prior
    /// are academic
    /// </remarks>
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
    public int Ordinal => ((EffectiveDate.DayOfYear - 1) / Duration.Days) + 1;

    /// <summary>
    /// Commonly used human readable representation of the cycle in
    /// the format YYoo
    /// </summary>
    public string Identifier => $"{EffectiveDate:yy}{Ordinal:D2}";

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

    /// <summary>
    /// Produces a cycle represented by the given identifier
    /// </summary>
    /// <param name="identifier">Four digit identifier in the format YYoo</param>
    /// <param name="yearConverter">Converts 2 digit year to 4 digits</param>
    /// <returns>a cycle represented by the given <paramref name="identifier"/></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the ordinal specified by <paramref name="identifier"/>
    /// is not valid for the given year
    /// </exception>
    /// <exception cref="FormatException">
    /// Thrown when <paramref name="identifier"/> can not be parsed as an integer
    /// </exception>
    public static Cycle FromIdentifier(
        string identifier, 
        Func<int, int>? yearConverter = null
        )
    {
        var integer = int.Parse(identifier, NumberStyles.Integer);

        var (yy, ordinal) = Math.DivRem(integer, 100);

        yearConverter ??= ConvertYear;
        var year = yearConverter.Invoke(yy);

        var previousYearEnd = new DateOnly(year - 1, 12, 31);
        var lastCycleOfYear = new Cycle(previousYearEnd);

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

    /// <summary>
    /// Date windowing function biased on the current century
    /// </summary>
    /// <param name="twoDigitYear">the two low-order digits of a year </param>
    /// <returns>a four digit year</returns>
    /// <seealso cref="https://en.wikipedia.org/wiki/Date_windowing"/>
    private static int ConvertYear(int twoDigitYear)
    {        
        var hundreds = DateTime.UtcNow.Year / 100;
        var year = hundreds * 100 + twoDigitYear;

        return year;
    }

    public override string ToString() => Identifier;

    public override bool Equals(object? obj) => Equals(obj as Cycle);

    public bool Equals(Cycle? other) => _serial == other?._serial;

    public override int GetHashCode() => _serial;
}