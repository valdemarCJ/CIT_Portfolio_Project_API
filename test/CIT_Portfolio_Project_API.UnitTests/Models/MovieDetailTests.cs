using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class MovieDetailTests
{
    private MovieDetail CreateMovieDetail(string? tconst, string? plot, int? seasonNumber, int? episodeNumber) => new MovieDetail
    {
        Tconst = tconst!,
        Plot = plot,
        SeasonNumber = seasonNumber,
        EpisodeNumber = episodeNumber
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Tconst only
    [DataTestMethod]
    [DataRow("tt123")]
    [DataRow("tt000")]
    [DataRow("tt1234")]
    [DataRow("tt123456789012345678")] // <= 20 chars
    public void MovieDetail_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreateMovieDetail(tconst, plot: null, seasonNumber: 1, episodeNumber: 1);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("tt12")]
    [DataRow("nm123")]
    [DataRow("TT123")]
    [DataRow(" tt123")]
    [DataRow("tt123 ")]
    [DataRow("tt12x")]
    [DataRow("tt123456789012345678901")] // > 20 chars
    public void MovieDetail_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreateMovieDetail(tconst, plot: null, seasonNumber: 1, episodeNumber: 1);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // RuntimeMinutes 
    [DataTestMethod]
    [DataRow(null)] // null allowed -> valid
    [DataRow(0)] // at lower boundary -> valid
    [DataRow(1)] // just above lower boundary -> valid
    [DataRow(2)] // near lower boundary -> valid
    [DataRow(90)] // mid-range typical -> valid
    [DataRow(120)] // mid-range typical -> valid
    [DataRow(150)] // mid-range typical -> valid
    [DataRow(int.MaxValue)] // at upper boundary -> valid
    [DataRow(int.MaxValue - 1)] // just below upper boundary -> valid
    [DataRow(int.MaxValue - 2)] // near upper boundary -> valid
    public void MovieDetail_RuntimeMinutes_ShouldPass(int? runtime)
    {
        // Arrange
        var model = CreateMovieDetail("tt123", plot: null, runtimeMinutes: runtime, startYear: 2000);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(-1)] // just below lower boundary -> invalid
    [DataRow(-2)] // moderately below lower boundary -> invalid
    [DataRow(-3)] // slightly further below lower boundary -> invalid
    [DataRow(-10)] // clearly below lower boundary -> invalid
    [DataRow(-50)] // mid-range negative -> invalid
    [DataRow(-500)] // deeper mid-range negative -> invalid
    [DataRow(-100)] // far below lower boundary -> invalid
    [DataRow(int.MinValue)] // extreme below lower boundary -> invalid
    public void MovieDetail_RuntimeMinutes_ShouldFail(int runtime)
    {
        // Arrange
        var model = CreateMovieDetail("tt123", plot: null, runtimeMinutes: runtime, startYear: 2000);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Plot only (no constraints)
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("short plot")]
    public void MovieDetail_Plot_ShouldPass(string? plot)
    {
        // Arrange
        var model = CreateMovieDetail("tt123", plot, seasonNumber: 1, episodeNumber: 1);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    // StartYear only (no constraints)
    [DataTestMethod]
    [DataRow(null)]
    [DataRow(-9999)]
    [DataRow(0)]
    [DataRow(1990)]
    [DataRow(2024)]
    public void MovieDetail_StartYear_ShouldPass(int? startYear)
    {
        // Arrange
        var model = CreateMovieDetail("tt123", plot: null, seasonNumber: 1, episodeNumber: startYear);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }
}
