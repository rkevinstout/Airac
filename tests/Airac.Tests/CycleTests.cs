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

    [Theory]
    [InlineData(2020, 12, 31)]
    [InlineData(2043, 12, 31)]
    public void SomeYearsHaveFourteenCycles(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);

        var cycle = new Cycle(date);

        cycle.Ordinal.Should().Be(14);
    }

    [Fact]
    public void CycleMayBeginOnJan1()
    {
        var date = new DateOnly(2043, 1, 1);

        var cycle = new Cycle(date);

        cycle.Ordinal.Should().Be(1);
        cycle.EffectiveDate.Should().Be(date);
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
        
        var result = cycle1.Equals(cycle2);

        result.Should().BeTrue();
    }

    [Fact]
    public void InvalidCastShouldNotBeEqual()
    {
        var date = new DateOnly(2020, 1, 2);

        var cycle = new Cycle(date);

        var result = cycle.Equals("foo");

        result.Should().BeFalse();    }

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
            // 2020 was a rare 14 cycle year
            {  1, new DateOnly(2020, 1, 2) },
            {  2, new DateOnly(2020, 1, 30) },
            {  3, new DateOnly(2020, 2, 27) },
            {  4, new DateOnly(2020, 3, 26) },
            {  5, new DateOnly(2020, 4, 23) },
            {  6, new DateOnly(2020, 5, 21) },
            {  7, new DateOnly(2020, 6, 18) },
            {  8, new DateOnly(2020, 7, 16) },
            {  9, new DateOnly(2020, 8, 13) },
            { 10, new DateOnly(2020, 9, 10) },
            { 11, new DateOnly(2020, 10, 8) },
            { 12, new DateOnly(2020, 11, 5) },
            { 13, new DateOnly(2020, 12, 3) },
            { 14, new DateOnly(2020, 12, 31) },

            // 2021 is particularly interesting as it follows
            // a rare 14 cycle year

            {  1, new DateOnly(2021, 1, 28) },
            {  2, new DateOnly(2021, 2, 25) },
            {  3, new DateOnly(2021, 3, 25) },
            {  4, new DateOnly(2021, 4, 22) },
            {  5, new DateOnly(2021, 5, 20) },
            {  6, new DateOnly(2021, 6, 17) },
            {  7, new DateOnly(2021, 7, 15) },
            {  8, new DateOnly(2021, 8, 12) },
            {  9, new DateOnly(2021, 9, 9) },
            { 10, new DateOnly(2021, 10, 7) },
            { 11, new DateOnly(2021, 11, 4) },
            { 12, new DateOnly(2021, 12, 2) },
            { 13, new DateOnly(2021, 12, 30) },

            {  1, new DateOnly(2022, 1, 27) },
            {  2, new DateOnly(2022, 2, 24) },
            {  3, new DateOnly(2022, 3, 24) },
            {  4, new DateOnly(2022, 4, 21) },
            {  5, new DateOnly(2022, 5, 19) },
            {  6, new DateOnly(2022, 6, 16) },
            {  7, new DateOnly(2022, 7, 14) },
            {  8, new DateOnly(2022, 8, 11) },
            {  9, new DateOnly(2022, 9, 8) },
            { 10, new DateOnly(2022, 10, 6) },
            { 11, new DateOnly(2022, 11, 3) },
            { 12, new DateOnly(2022, 12, 1) },
            { 13, new DateOnly(2022, 12, 29) },

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
