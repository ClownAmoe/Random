// Tests/BLL/GameExtensionsTests.cs

using Xunit;
using GameOverDose.BLL.Extensions;
using GameOverDose.DAL.Entities;
using System;

namespace GameOverDose.Tests.BLL;

public class GameExtensionsTests
{
    [Fact]
    public void GetPositiveRatingPercentage_ValidRating_ReturnsCorrectPercentage()
    {
        // Arrange
        var game = new Game
        {
            Rating = 4.0,
            RatingTop = 5
        };

        // Act
        var result = game.GetPositiveRatingPercentage();

        // Assert
        Assert.Equal(80.0, result);
    }

    [Fact]
    public void GetPositiveRatingPercentage_NullRating_ReturnsZero()
    {
        // Arrange
        var game = new Game
        {
            Rating = null,
            RatingTop = 5
        };

        // Act
        var result = game.GetPositiveRatingPercentage();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void IsNewRelease_RecentGame_ReturnsTrue()
    {
        // Arrange
        var game = new Game
        {
            Release = DateTime.Now.AddMonths(-6)
        };

        // Act
        var result = game.IsNewRelease();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNewRelease_OldGame_ReturnsFalse()
    {
        // Arrange
        var game = new Game
        {
            Release = DateTime.Now.AddYears(-2)
        };

        // Act
        var result = game.IsNewRelease();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetPlatformsShort_ManyPlatforms_ReturnsTruncated()
    {
        // Arrange
        var game = new Game
        {
            Platforms = "PC, PS5, Xbox, Switch, Mobile"
        };

        // Act
        var result = game.GetPlatformsShort();

        // Assert
        Assert.Contains("...", result);
        Assert.Contains("PC", result);
    }

    [Fact]
    public void GetPlatformsShort_EmptyPlatforms_ReturnsNA()
    {
        // Arrange
        var game = new Game
        {
            Platforms = ""
        };

        // Act
        var result = game.GetPlatformsShort();

        // Assert
        Assert.Equal("N/A", result);
    }
}