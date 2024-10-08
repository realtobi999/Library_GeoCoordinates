﻿using FluentAssertions;
using GeoCoordinates.Core;

namespace GeoCoordinates.Tests;

public class CoordinateTests
{
    [Fact]
    public void Coordinate_ShouldCreateInstance()
    {
        // prepare
        double latitude = 45.0;
        double longitude = 90.0;
        double elevation = 100.0;

        // act & assert
        var coordinate = new Coordinate(latitude, longitude, elevation);

        coordinate.Latitude.Should().Be(latitude);
        coordinate.Longitude.Should().Be(longitude);
        coordinate.Elevation.Should().Be(elevation);
    }

    [Fact]
    public void Coordinate_LongitudeLatitudeElevationValidationWorks()
    {
        // act & assert
        Assert.Throws<FormatException>(() => new Coordinate(99999, 90, 100));
        Assert.Throws<FormatException>(() => new Coordinate(45, 9999, 100));
        Assert.Throws<FormatException>(() => new Coordinate(45, 90, 10000));
    }

    [Fact]
    public void ToString_ReturnCorrectFormat()
    {
        // prepare
        var coordinate = new Coordinate(45, 90, 100);

        // act & assert
        var expected = "45|90|100";

        coordinate.ToString().Should().Be(expected);
    }

    [Fact]
    public void Parse_WorksAndReturnsCorrectInstance()
    {
        // prepare
        var coordinateString = "45|90|100";

        // act & assert
        var expected = new Coordinate(45, 90, 100);

        Coordinate.Parse(coordinateString).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Parse_ValidationWorks()
    {
        Assert.Throws<FormatException>(() => Coordinate.Parse("hello world!"));
        Assert.Throws<FormatException>(() => Coordinate.Parse("45||0"));
        Assert.Throws<FormatException>(() => Coordinate.Parse("45|"));
        Assert.Throws<FormatException>(() => Coordinate.Parse("45||"));
        Assert.Throws<FormatException>(() => Coordinate.Parse("45|90|hello"));
        Assert.Throws<FormatException>(() => Coordinate.Parse("45|hello|0"));
    }

    [Fact]
    public void IsWithinDistanceTo_TrueWhenWithinRange()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0); // Berlin
        var coord2 = new Coordinate(52.5200, 13.4060, 0); // close to Berlin
        double range = 1000; // 1 km

        // act & assert
        var result = coord1.IsWithinDistanceTo(coord2, range);

        Assert.True(result);
    }

    [Fact]
    public void IsWithinDistanceTo_FalseWhenOutsideRange()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0); // Berlin
        var coord2 = new Coordinate(48.8566, 2.3522, 0);  // Paris
        double range = 500000; // 500 km

        // act & assert
        var result = coord1.IsWithinDistanceTo(coord2, range);

        Assert.False(result);
    }

    [Fact]
    public void IsWithinDistanceTo_ThrowsArgumentException_WhenRangeIsZeroOrNegative()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0);
        var coord2 = new Coordinate(48.8566, 2.3522, 0);

        // act & assert
        Assert.Throws<ArgumentException>(() => coord1.IsWithinDistanceTo(coord2, 0));
        Assert.Throws<ArgumentException>(() => coord1.IsWithinDistanceTo(coord2, -10));
    }

    [Fact]
    public void ToPrettyString_ShouldFormatCorrectly()
    {
        // prepare
        var coordinate = new Coordinate(1.40339, -122.4194, 15.0);

        // act & assert
        var result = coordinate.ToPrettyString();

        result.Should().Be("1°24'12.2\"N 122°25'9.8\"W Elevation: 15 meters");
    }
}
