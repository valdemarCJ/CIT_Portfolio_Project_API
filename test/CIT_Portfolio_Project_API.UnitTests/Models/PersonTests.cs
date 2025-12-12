using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class PersonTests
{
    private Person CreatePerson(string? nconst, string? name) => new Person
    {
        Nconst = nconst!,
        Name = name
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Nconst: required, regex nm\d{3,}, length <= 20
    [DataTestMethod]
    [DataRow("nm123")]
    [DataRow("nm123456789012345678")]
    public void Person_Nconst_ShouldPass(string nconst)
    {
        // Arrange
        var model = CreatePerson(nconst, "Christian Bale");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("tt123")] // wrong prefix
    [DataRow("nm12")]  // too short
    [DataRow("nm12345678901234567890")] // > 20
    public void Person_Nconst_ShouldFail(string? nconst)
    {
        // Arrange
        var model = CreatePerson(nconst, "x");
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Name: optional, length <= 256
    [DataTestMethod]
    [DataRow("")]
    [DataRow("A")]
    [DataRow(null)]
    public void Person_Name_ShouldPass(string? value)
    {
        // Arrange
        var model = CreatePerson("nm123", value);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(257)]
    [DataRow(300)]
    public void Person_Name_ShouldFail(int length)
    {
        // Arrange
        var name = new string('a', length);
        var model = CreatePerson("nm123", name);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
