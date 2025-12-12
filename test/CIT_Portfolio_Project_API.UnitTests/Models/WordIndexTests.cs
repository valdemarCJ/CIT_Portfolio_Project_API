using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIT_Portfolio_Project_API.UnitTests.Models;

[TestClass]
public class WordIndexTests
{
    private WordIndex CreateWordIndex(string? word, int frequency) => new WordIndex
    {
        Word = word!,
        Frequency = frequency
    };

    private bool TryValidateModel(object model, out ICollection<ValidationResult> results)
    {
        var ctx = new ValidationContext(model);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    // Word only (required, <=128)
    [DataTestMethod]
    [DataRow("a")] // min length 1
    [DataRow("word")] 
    [DataRow(128)] // boundary
    public void WordIndex_Word_ShouldPass(object arg)
    {
        // Arrange
        var word = arg is int len ? new string('a', len) : arg.ToString()!;
        var model = CreateWordIndex(word, frequency: 0);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(129)] // too long
    public void WordIndex_Word_ShouldFail(object? arg)
    {
        // Arrange
        string? word = arg switch
        {
            null => null,
            string s => s,
            int len => new string('a', len),
            _ => arg!.ToString()
        };
        var model = CreateWordIndex(word, frequency: 0);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }

    // Frequency only (>=0)
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(100)]
    [DataRow(int.MaxValue - 2)]
    [DataRow(int.MaxValue - 1)]
    [DataRow(int.MaxValue)]
    public void WordIndex_Frequency_ShouldPass(int freq)
    {
        // Arrange
        var model = CreateWordIndex("word", freq);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsTrue(ok);
        Assert.AreEqual(0, results.Count);
    }

    [DataTestMethod]
    [DataRow(-1)]
    [DataRow(-100)]
    public void WordIndex_Frequency_ShouldFail(int freq)
    {
        // Arrange
        var model = CreateWordIndex("word", freq);
        // Act
        var ok = TryValidateModel(model, out var results);
        // Assert
        Assert.IsFalse(ok);
        Assert.IsTrue(results.Count > 0);
    }
}
