using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class MoviePersonTests
{
    private MoviePerson CreateMoviePerson(string? tconst, string? nconst, string? category) => new MoviePerson
    {
        Tconst = tconst!,
        Nconst = nconst!,
        Category = category
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
    [DataRow("tt123456789012345678")] // within 20
    public void MoviePerson_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreateMoviePerson(tconst, "nm123", null);
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
    [DataRow("nm123")] // wrong prefix
    [DataRow("TT123")] // uppercase
    [DataRow(" tt123")] // leading space
    [DataRow("tt123 ")] // trailing space
    [DataRow("tt12x")] // non-digit
    [DataRow("tt123456789012345678901")] // > 20
    public void MoviePerson_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreateMoviePerson(tconst, "nm123", null);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Nconst only
    [DataTestMethod]
    [DataRow("nm123")]
    [DataRow("nm999")]
    [DataRow("nm000")]
    [DataRow("nm123456789012345678")] // within 20
    public void MoviePerson_Nconst_ShouldPass(string nconst)
    {
        // Arrange
        var model = CreateMoviePerson("tt123", nconst, null);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("nm12")]
    [DataRow("tt123")] // wrong prefix
    [DataRow("NM123")] // uppercase
    [DataRow(" nm123")] // leading space
    [DataRow("nm123 ")] // trailing space
    [DataRow("nm12x")] // non-digit
    [DataRow("nm123456789012345678901")] // > 20
    public void MoviePerson_Nconst_ShouldFail(string? nconst)
    {
        // Arrange
        var model = CreateMoviePerson("tt123", nconst, null);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Category only
    [DataTestMethod]
    [DataRow(null)] // optional
    [DataRow("")] // empty allowed
    [DataRow("actor")]
    [DataRow("director")]
    [DataRow(64)] // boundary
    public void MoviePerson_Category_ShouldPass(object? arg)
    {
        // Arrange
        string? category = arg switch
        {
            null => null,
            string s => s,
            int len => new string('a', len),
            _ => arg!.ToString()
        };
        var model = CreateMoviePerson("tt123", "nm123", category);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(65)]
    [DataRow(128)]
    public void MoviePerson_Category_ShouldFail(int len)
    {
        // Arrange
        var cat = new string('a', len);
        var model = CreateMoviePerson("tt123", "nm123", cat);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
