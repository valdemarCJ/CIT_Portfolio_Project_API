using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class MovieGenreTests
{
    private MovieGenre CreateMovieGenre(string? tconst, string? genre) => new MovieGenre
    {
        Tconst = tconst!,
        Genre = genre!
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
    [DataRow("tt999")]
    [DataRow("tt000")]
    [DataRow("tt1234")]
    [DataRow("tt123456789012345678")] // within 20
    public void MovieGenre_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreateMovieGenre(tconst, "action");
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
    [DataRow("tt123456789012345678901")] // > 20
    public void MovieGenre_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreateMovieGenre(tconst, "action");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Genre only
    [DataTestMethod]
    [DataRow("a")] // min 1
    [DataRow("action")]
    [DataRow("Comedy-Drama")] // hyphen allowed
    [DataRow("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb")] // 64 chars
    public void MovieGenre_Genre_ShouldPass(string genre)
    {
        // Arrange
        var model = CreateMovieGenre("tt123", genre);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")] // required
    [DataRow(65)]
    [DataRow(128)]
    public void MovieGenre_Genre_ShouldFail(object? arg)
    {
        // Arrange
        string? genre = arg switch
        {
            null => null,
            string s => s,
            int len => new string('a', len),
            _ => arg!.ToString()
        };
        var model = CreateMovieGenre("tt123", genre);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
