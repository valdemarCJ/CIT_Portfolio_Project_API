using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class PersonKnownForTests
{
    private PersonKnownFor CreatePK(string? nconst, string? tconst) => new PersonKnownFor
    {
        Nconst = nconst!,
        Tconst = tconst!
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Nconst only
    [DataTestMethod]
    [DataRow("nm123")]
    [DataRow("nm123456789012345678")] // within 20
    public void PersonKnownFor_Nconst_ShouldPass(string nconst)
    {
        // Arrange
        var model = CreatePK(nconst, "tt123");
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
    [DataRow("nm123456789012345678901")] // > 20
    public void PersonKnownFor_Nconst_ShouldFail(string? nconst)
    {
        // Arrange
        var model = CreatePK(nconst, "tt123");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Tconst only
    [DataTestMethod]
    [DataRow("tt123")]
    [DataRow("tt123456789012345678")] // within 20
    public void PersonKnownFor_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreatePK("nm123", tconst);
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
    [DataRow("tt123456789012345678901")] // > 20
    public void PersonKnownFor_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreatePK("nm123", tconst);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
