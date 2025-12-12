using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class UserSearchHistoryTests
{
    /// <summary>
    /// Opretter en UserSearchHistory-instans med fornuftige standardværdier til brug i tests.
    /// </summary>
    /// <param name="userId">Brugerens id (skal være >= 1 for at være gyldigt).</param>
    /// <param name="text">Søgeteksten (krævet, maks 512 tegn).</param>
    /// <param name="searchedAt">Tidspunkt for søgningen.</param>
    /// <returns>En initialiseret <see cref="UserSearchHistory"/> klar til validering.</returns>
    private UserSearchHistory CreateUserSearchHistory(int userId, string? text, DateTime searchedAt)
    {
        return new UserSearchHistory
        {
            UserId = userId,
            Text = text!,
            SearchedAt = searchedAt
        };
    }

    /// <summary>
    /// Forsøger at validere et <see cref="UserSearchHistory"/> objekt og returnerer valideringsresultaterne.
    /// Denne metode anvendes til at sikre, at objektet opfylder alle definerede valideringskrav
    /// før det behandles yderligere (f.eks. gemmes i databasen).
    /// </summary>
    /// <param name="model">UserSearchHistory-objektet, der skal valideres.</param>
    /// <param name="results">Returnerer en liste af valideringsfejl, hvis der findes nogen.</param>
    /// <returns>Returnerer true, hvis objektet opfylder alle valideringsregler, ellers false.</returns>
    private bool TryValidateModel(UserSearchHistory model, out ICollection<ValidationResult> results)
    {
        // Opretter et ValidationContext objekt, som indkapsler den kontekst, valideringen skal foregå i.
        var ctx = new ValidationContext(model);
        // Initialiserer en tom liste til at holde valideringsresultaterne.
        results = new List<ValidationResult>();
        // Anvender Validator-klassen til at prøve at validere objektet og indsamle eventuelle fejl.
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Id property has no validation constraints (typically DB-generated)
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(42)]
    [DataRow(1000)]
    [DataRow(int.MaxValue)]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(-2)]
    [DataRow(-3)]
    [DataRow(-10)]
    [DataRow(int.MinValue)]
    public void UserSearchHistory_Id_ShouldPass(int id)
    {
        // Arrange
        var model = CreateUserSearchHistory(1, "a", DateTime.UtcNow);
        model.Id = id;
        // Act
        var result = TryValidateModel(model, out var validationResults);
        // Assert (stadig gyldigt da der ikke er attributter på Id)
        Assert.IsTrue(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    // Positive Test Cases for UserId (Range: [1, int.MaxValue])
    [DataTestMethod]
    [DataRow(1)] // minimum boundary
    [DataRow(2)] // next to boundary
    [DataRow(3)] // next to boundary
    [DataRow(10)] // small positive
    [DataRow(100)] // larger value
    [DataRow(int.MaxValue - 2)]
    [DataRow(int.MaxValue - 1)]
    [DataRow(int.MaxValue)] // extreme positive
    /// <summary>
    /// Positive testcases for UserId (Range: [1, int.MaxValue]).
    /// </summary>
    public void UserSearchHistory_UserId_ShouldPass(int userId)
    {
        // Arrange
        var model = CreateUserSearchHistory(userId, "a", DateTime.UtcNow);
        // Act
        var result = TryValidateModel(model, out var validationResults);
        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    // Negative Test Cases for UserId
    [DataTestMethod]
    [DataRow(0)] // just below minimum
    [DataRow(-1)] // negative
    [DataRow(-2)] // negative
    [DataRow(-3)] // negative
    [DataRow(-10)] // negative
    [DataRow(-100)] // negative
    /// <summary>
    /// Negative testcases for UserId (værdier under 1 skal fejle validering).
    /// </summary>
    public void UserSearchHistory_UserId_ShouldFail(int userId)
    {
        // Arrange
        var model = CreateUserSearchHistory(userId, "a", DateTime.UtcNow);
        // Act
        var result = TryValidateModel(model, out var validationResults);
        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Count > 0);
    }

    // Positive Test Cases for Text (Required, StringLength <= 512)
    [DataTestMethod]
    [DataRow(1)] // minimum effective length
    [DataRow(2)] // near minimum
    [DataRow(3)] // near minimum
    [DataRow(256)] // middle of range
    [DataRow(511)] // near max
    [DataRow(512)] // maximum boundary
    /// <summary>
    /// Positive testcases for Text (påkrævet, maks længde 512 tegn).
    /// </summary>
    public void UserSearchHistory_Text_ShouldPass(int len)
    {
        // Arrange
        var text = new string('a', len);
        var model = CreateUserSearchHistory(1, text, DateTime.UtcNow);
        // Act
        var result = TryValidateModel(model, out var validationResults);
        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    // Negative Test Cases for Text
    [DataTestMethod]
    [DataRow(null)] // null not allowed by [Required]
    [DataRow("")] // empty not allowed by [Required]
    [DataRow(513)] // above StringLength(512)
    [DataRow(514)] // above StringLength(512)
    [DataRow(1024)] // well above max
    [DataRow(2048)] // significantly above max
    /// <summary>
    /// Negative testcases for Text (null/empty eller længder over 512 skal fejle).
    /// </summary>
    public void UserSearchHistory_Text_ShouldFail(object? textArg)
    {
        // Arrange
        string? text = textArg switch
        {
            null => null,
            string s => s,
            int len => new string('a', len),
            _ => textArg!.ToString()
        };
        var model = CreateUserSearchHistory(1, text, DateTime.UtcNow);
        // Act
        var result = TryValidateModel(model, out var validationResults);
        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Count > 0);
    }

    // Positive Test Cases for SearchedAt (no validation attributes on this property)
    [DataTestMethod]
    [DataRow("0001-01-01T00:00:00Z")] // earliest
    [DataRow("1800-01-01T00:00:00Z")] // older historical
    [DataRow("1900-01-01T00:00:00Z")] // historical
    [DataRow("1970-01-01T00:00:00Z")] // epoch
    [DataRow("2000-02-29T00:00:00Z")] // leap day
    [DataRow("2025-11-04T12:00:00Z")] // typical value
    [DataRow("2100-12-31T23:59:59Z")] // future date valid
    [DataRow("2999-12-31T23:59:59Z")] // future extreme valid
    [DataRow("3000-01-01T00:00:00Z")] // far future (safe)
    /// <summary>
    /// Positive testcases for SearchedAt (der er ingen valideringsattributter på denne egenskab).
    /// </summary>
    public void UserSearchHistory_SearchedAt_ShouldPass(string iso)
    {
        // Arrange
        var dt = DateTime.Parse(iso);
        var model = CreateUserSearchHistory(1, "a", dt);
        // Act
        var result = TryValidateModel(model, out var validationResults);
        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    
}
