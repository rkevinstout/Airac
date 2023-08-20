using FluentAssertions;

namespace Airac.Tests;

public class CycleTests
{
    [Fact]
    public void DefaultCtorReturnsValidCycle()
    {
        var cycle = new Cycle();

        cycle.Should().NotBeNull();
        cycle.EffectiveDate.DayOfWeek.Should().Be(DayOfWeek.Thursday);
        cycle.EffectiveDate
            .Should()
            .BeBefore(DateOnly.FromDateTime(DateTime.UtcNow));
        cycle.Identifier.Length.Should().Be(4);
    }

    [Fact]
    public void SomeYearsHaveFourteenCycles()
    {
        var date = new DateOnly(2020, 12, 31);

        var cycle = new Cycle(date);

        cycle.Ordinal.Should().Be(14);
    }

    [Fact]
    public void CanParseIdentifier()
    {
        string identifier = "2304";

        var cycle = Cycle.FromIdentifier(identifier);

        cycle.Ordinal.Should().Be(4);
        cycle.EffectiveDate.Year.Should().Be(2023);
        cycle.Identifier.Should().Be(identifier);
    }

    [Fact]
    public void InvalidIdentifierFormatShouldThrow()
    {
        string identifier = "Garbage";

        Action act = () => Cycle.FromIdentifier(identifier);

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void InvalidOrdinalShouldThrow()
    {
        string identifier = "2314";

        Action act = () => Cycle.FromIdentifier(identifier);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IdenticalCyclesAreEqual()
    {
        var date = new DateOnly(2020, 1, 2);

        var cycle1 = new Cycle(date);
        var cycle2 = new Cycle(date.AddDays(14));

        cycle1.Should().BeEquivalentTo(cycle2);
    }

    [Fact]
    public void InvalidCastShouldNotBeEqual()
    {
        var date = new DateOnly(2020, 1, 2);

        var cycle = new Cycle(date);

        cycle.Should().NotBeEquivalentTo("foo");
    }

    [Theory]
    [MemberData(nameof(AirNavData))]
    public void EffectiveDateAndOrdinalsAreCorrect(int ordinal, DateOnly effectiveDate)
    {
        var cycle = new Cycle(effectiveDate.AddDays(27));

        cycle.Ordinal.Should().Be(ordinal);
        cycle.EffectiveDate.Should().Be(effectiveDate);
    }

    /// <summary>
    /// Tuples of ordinals and effective dates
    /// sourced from https://ais.airnav.ge/en/airac-cycles
    /// </summary>
    public static TheoryData<int, DateOnly> AirNavData => new()
        {
            {  1, new DateOnly(2023, 1, 26) },
            {  2, new DateOnly(2023, 2, 23) },
            {  3, new DateOnly(2023, 3, 23) },
            {  4, new DateOnly(2023, 4, 20) },
            {  5, new DateOnly(2023, 5, 18) },
            {  6, new DateOnly(2023, 6, 15) },
            {  7, new DateOnly(2023, 7, 13) },
            {  8, new DateOnly(2023, 8, 10) },
            {  9, new DateOnly(2023, 9, 7) },
            { 10, new DateOnly(2023, 10, 5) },
            { 11, new DateOnly(2023, 11, 2) },
            { 12, new DateOnly(2023, 11, 30) },
            { 13, new DateOnly(2023, 12, 28) },
            {  1, new DateOnly(2024, 1, 25) },
            {  2, new DateOnly(2024, 2, 22) },
            {  3, new DateOnly(2024, 3, 21) },
            {  4, new DateOnly(2024, 4, 18) },
            {  5, new DateOnly(2024, 5, 16) },
            {  6, new DateOnly(2024, 6, 13) },
            {  7, new DateOnly(2024, 7, 11) },
            {  8, new DateOnly(2024, 8, 8) },
            {  9, new DateOnly(2024, 9, 5) },
            { 10, new DateOnly(2024, 10, 3) },
            { 11, new DateOnly(2024, 10, 31) },
            { 12, new DateOnly(2024, 11, 28) },
            { 13, new DateOnly(2024, 12, 26) },
        };
}
