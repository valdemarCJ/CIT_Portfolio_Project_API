using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class UserBookmarkTests
{
    private UserBookmark CreateBookmark(int userId, string? tconst, string? note) => new UserBookmark
    {
        UserId = userId,
        Tconst = tconst!,
        Note = note
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Id: no validation attributes – unconstrained
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(999)]
    public void UserBookmark_Id_ShouldPass(int id)
    {
        // Arrange
        var model = CreateBookmark(1, "tt123", null);
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
    [DataRow(10)]
    [DataRow(int.MaxValue - 2)]
    [DataRow(int.MaxValue - 1)]
    [DataRow(int.MaxValue)]
    public void UserBookmark_UserId_ShouldPass(int userId)
    {
        // Arrange
        var model = CreateBookmark(userId, "tt123", null);
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
    public void UserBookmark_UserId_ShouldFail(int userId)
    {
        // Arrange
        var model = CreateBookmark(userId, "tt123", null);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Tconst only
    [DataTestMethod]
    [DataRow("tt123")]
    [DataRow("tt999999")]
    [DataRow("tt123456789012345678")] // <=20
    public void UserBookmark_Tconst_ShouldPass(string tconst)
    {
        // Arrange
        var model = CreateBookmark(1, tconst, null);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("nm123")]
    [DataRow("tt12")]
    [DataRow("tt123456789012345678901")] // >20
    public void UserBookmark_Tconst_ShouldFail(string? tconst)
    {
        // Arrange
        var model = CreateBookmark(1, tconst, null);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Note only (optional, <=512)
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("note")]
    [DataRow(512)]
    public void UserBookmark_Note_ShouldPass(object? arg)
    {
        // Arrange
        string? note = arg switch
        {
            null => null,
            string s => s,
            int len => new string('a', len),
            _ => arg!.ToString()
        };
        var model = CreateBookmark(1, "tt123", note);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(513)]
    [DataRow(1024)]
    public void UserBookmark_Note_ShouldFail(int len)
    {
        // Arrange
        var note = new string('a', len);
        var model = CreateBookmark(1, "tt123", note);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
