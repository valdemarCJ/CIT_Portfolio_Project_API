using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class UserTests
{
    private User CreateUser(object usernameArg, string? email, int passLen)
    {
        var username = usernameArg is int len ? new string('a', len) : usernameArg.ToString()!;
        return new User
        {
            Username = username,
            Email = email!,
            PasswordHash = new string('x', passLen)
        };
    }

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Id: no validation attributes – any value should validate
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(42)]
    public void User_Id_ShouldPass(int id)
    {
        // Arrange
        var model = CreateUser("bob", "bob@example.com", 20);
        model.Id = id;
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    // Positive: Username [3..64], Email valid, PasswordHash [20..200]
    [DataTestMethod]
    [DataRow("bob", "bob@example.com", 20)]
    [DataRow("a_very_long_username_but_valid", "a@b.co", 50)]
    [DataRow(64, "ok@example.com", 200)]
    public void User_Username_ShouldPass(object usernameArg, string email, int passLen)
    {
        // Arrange
        var model = CreateUser(usernameArg, email, passLen);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    // Username only
    [DataTestMethod]
    [DataRow("ab")]
    [DataRow("")]
    [DataRow(65)]
    public void User_Username_ShouldFail(object usernameArg)
    {
        // Arrange
        var model = CreateUser(usernameArg, "ok@example.com", 20);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Email only
    [DataTestMethod]
    [DataRow("bob@example.com")]
    [DataRow("a@b.co")]
    public void User_Email_ShouldPass(string email)
    {
        // Arrange
        var model = CreateUser("bob", email, 20);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow("bad-email")]
    [DataRow("")]
    [DataRow("invalid@")]
    public void User_Email_ShouldFail(string email)
    {
        // Arrange
        var model = CreateUser("bob", email, 20);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // PasswordHash only
    [DataTestMethod]
    [DataRow(20)]
    [DataRow(50)]
    [DataRow(200)]
    public void User_PasswordHash_ShouldPass(int passLen)
    {
        // Arrange
        var model = CreateUser("bob", "bob@example.com", passLen);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(19)]
    [DataRow(201)]
    public void User_PasswordHash_ShouldFail(int passLen)
    {
        // Arrange
        var model = CreateUser("bob", "bob@example.com", passLen);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
