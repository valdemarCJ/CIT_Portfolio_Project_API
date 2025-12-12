using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class MovieTests
{
    private Movie CreateMovie(string? tconst, string? title) => new Movie
    {
        Tconst = tconst!,
        Title = title
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Tconst: required, regex tt\d{3,}, length <= 20
    [DataTestMethod]
    [DataRow("tt123")] // min regex
    [DataRow("tt000")] // min digits with zeros
    [DataRow("tt1234")] // more digits still valid
    [DataRow("tt123456789012345678")] // <= 20 chars
    public void Movie_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreateMovie(tconst, "The Dark Knight");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("nm123")] // wrong prefix
    [DataRow("tt12")] // too short
    [DataRow("tt12345678901234567890")] // > 20
    [DataRow("TT123")] // uppercase prefix should fail regex
    [DataRow(" tt123")] // leading space invalid
    [DataRow("tt123 ")] // trailing space invalid
    [DataRow("tt12x")] // non-digit character
    public void Movie_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreateMovie(tconst, "x");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Title: optional, length <= 512
    [DataTestMethod]
    [DataRow("")]
    [DataRow("123")]
    [DataRow("valid title")]
    [DataRow("x")]
    [DataRow(null)]
    public void Movie_Title_ShouldPass(string? value)
    {
        // Arrange
        var model = CreateMovie("tt123", value);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(513)]
    [DataRow(1024)]
    public void Movie_Title_ShouldFail(int length)
    {
        // Arrange
        var title = new string('a', length);
        var model = CreateMovie("tt123", title);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    
}
