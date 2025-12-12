using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class PersonProfessionTests
{
    private PersonProfession CreatePP(string? nconst, string? profession) => new PersonProfession
    {
        Nconst = nconst!,
        Profession = profession!
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
    [DataRow("nm999")]
    [DataRow("nm123456789012345678")] // within 20
    public void PersonProfession_Nconst_ShouldPass(string nconst)
    {
        // Arrange
        var model = CreatePP(nconst, "actor");
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
    public void PersonProfession_Nconst_ShouldFail(string? nconst)
    {
        // Arrange
        var model = CreatePP(nconst, "actor");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Profession only
    [DataTestMethod]
    [DataRow("a")] // min 1
    [DataRow("actor")]
    [DataRow(64)] // boundary
    public void PersonProfession_Profession_ShouldPass(object arg)
    {
        // Arrange
        var profession = arg is int len ? new string('a', len) : arg.ToString()!;
        var model = CreatePP("nm123", profession);
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
    public void PersonProfession_Profession_ShouldFail(object? arg)
    {
        // Arrange
        string? profession = arg switch
        {
            null => null,
            string s => s,
            int len => new string('a', len),
            _ => arg!.ToString()
        };
        var model = CreatePP("nm123", profession);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
