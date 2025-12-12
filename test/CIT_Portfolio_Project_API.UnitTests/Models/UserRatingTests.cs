using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class UserRatingTests
{
    private UserRating CreateUserRating(int userId, string? tconst, int value) => new UserRating
    {
        UserId = userId,
        Tconst = tconst!,
        Value = value
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Id: no constraints
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(42)]
    [DataRow(int.MaxValue)]
    public void UserRating_Id_ShouldPass(int id)
    {
        // Arrange
        var model = CreateUserRating(1, "tt123", 5);
        model.Id = id;
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    // UserId only
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(int.MaxValue - 2)]
    [DataRow(int.MaxValue - 1)]
    [DataRow(int.MaxValue)]
    public void UserRating_UserId_ShouldPass(int userId)
    {
        // Arrange
        var model = CreateUserRating(userId, "tt123", 5);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(-2)]
    public void UserRating_UserId_ShouldFail(int userId)
    {
        // Arrange
        var model = CreateUserRating(userId, "tt123", 5);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Tconst only
    [DataTestMethod]
    [DataRow("tt123")]
    [DataRow("tt000")]
    [DataRow("tt123456789012345678")] // <=20
    public void UserRating_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreateUserRating(1, tconst, 5);
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
    [DataRow("tt123456789012345678901")] // >20
    public void UserRating_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreateUserRating(1, tconst, 5);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Value only [1..10]
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(5)]
    [DataRow(10)]
    public void UserRating_Value_ShouldPass(int value)
    {
        // Arrange
        var model = CreateUserRating(1, "tt123", value);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(11)]
    public void UserRating_Value_ShouldFail(int value)
    {
        // Arrange
        var model = CreateUserRating(1, "tt123", value);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
