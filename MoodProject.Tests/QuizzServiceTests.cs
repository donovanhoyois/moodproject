using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.Out;
using MoodProject.Services;
using Moq;

namespace MoodProject.Tests;

public class QuizzServiceTests
{
    private IAppApi FakeApi;
    private List<FactorValue> FakeHarmfulnessAscendingValues = new();
    private List<FactorValue> FakePresenceAscendingValues = new();
    private List<FactorValue> MixedTypesAscendingValues = new();
    
    [SetUp]
    public void Setup()
    {
        FakeHarmfulnessAscendingValues = new List<FactorValue>();
        FakePresenceAscendingValues = new List<FactorValue>();
        MixedTypesAscendingValues = new List<FactorValue>();
        var values = new[] { 0, 0.1f, 0.2f, 0.3f, 0.4f };
        
        // Harmfulness values
        for (var i = 0; i < 5; i++)
        {
            var newFactorValue = new FactorValue()
            {
                Id = i,
                SymptomId = 1,
                Type = FactorType.Harmfulness,
                Value = values[i],
                Timestamp = DateTime.Now - TimeSpan.FromDays(5-i)
            };
            FakeHarmfulnessAscendingValues.Add(newFactorValue);
            MixedTypesAscendingValues.Add(newFactorValue);
        }
        
        // Presence values
        for (var i = 0; i < 5; i++)
        {
            var newFactorValue = new FactorValue()
            {
                Id = i,
                SymptomId = 1,
                Type = FactorType.Presence,
                Value = values[i],
                Timestamp = DateTime.Now - TimeSpan.FromDays(5-i)
            };
            FakePresenceAscendingValues.Add(newFactorValue);
            MixedTypesAscendingValues.Add(newFactorValue);
        }
        
        MixedTypesAscendingValues.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        

        var fakeSymptoms = new List<Symptom>()
        {
            new()
            {
                Id = 1,
                Type = new(1, "Symptome 1"),
                TypeId = 1,
                UserId = "xxx",
                ValuesHistory = new List<FactorValue>()
                {
                    new()
                    {
                        Id = 1,
                        SymptomId = 1,
                        Timestamp = DateTime.Now - TimeSpan.FromDays(9),
                        Type = FactorType.Presence,
                        Value = 0.4f
                    },
                    new()
                    {
                        Id = 2,
                        SymptomId = 1,
                        Timestamp = DateTime.Now - TimeSpan.FromDays(11),
                        Type = FactorType.Presence,
                        Value = 0.5f
                    },
                    new()
                    {
                        Id = 3,
                        SymptomId = 1,
                        Timestamp = DateTime.Now - TimeSpan.FromDays(12),
                        Type = FactorType.Presence,
                        Value = 0.6f
                    },
                    new()
                    {
                        Id = 4,
                        SymptomId = 1,
                        Timestamp = DateTime.Now - TimeSpan.FromDays(9),
                        Type = FactorType.Harmfulness,
                        Value = -0.4f
                    },
                    new()
                    {
                        Id = 5,
                        SymptomId = 1,
                        Timestamp = DateTime.Now - TimeSpan.FromDays(11),
                        Type = FactorType.Harmfulness,
                        Value = -0.5f
                    },
                    new()
                    {
                        Id = 6,
                        SymptomId = 1,
                        Timestamp = DateTime.Now - TimeSpan.FromDays(12),
                        Type = FactorType.Harmfulness,
                        Value = -0.6f
                    },
                }
            }
        };

        var mock = new Mock<IAppApi>();
        mock
            .Setup(api => api.GetSymptomsWithHistoryByUserId(It.IsAny<string>()))
            .ReturnsAsync(fakeSymptoms);
        FakeApi = mock.Object;
    }

    [Test]
    public async Task GenerateQuizz_ShouldReturnAQuizz()
    {
        var quizzService = new QuizzService(FakeApi);
        var r = await quizzService.Generate(string.Empty);
        Assert.That(r, Is.Not.Null);
        Assert.That(r.Content.Count(), Is.Not.Zero);
    }

    [TestCase(FactorType.Harmfulness)]
    [TestCase(FactorType.Presence)]
    public void GetAverageValues_SpecificType_ShouldReturnCorrectAverage(FactorType factorType)
    {
        // Arrange
        var quizzService = new QuizzService(FakeApi);
        var symptom = new Symptom()
        {
            Type = new SymptomType()
            {
                Id = 1,
                Name = "Symptom"
            },
            Id = 1,
            isDisabled = false,
            TypeId = 1,
            UserId = "userid",
            ValuesHistory = factorType == FactorType.Harmfulness ? FakeHarmfulnessAscendingValues : FakePresenceAscendingValues
        };
        /*
        for (int i = 1; i <= 20; i++)
        {
            fakeSymptomValues.Add(new FactorValue()
            {
                Id = i,
                SymptomId = 1,
                Type = i%2 == 0 ? FactorType.Harmfulness : FactorType.Presence,
                Value = 0 + MathF.Round(i/20f, 2, MidpointRounding.ToEven),
                Timestamp = new DateTime((new TimeSpan(DateTime.Now.Ticks) - TimeSpan.FromDays(10 - Math.Round(i/2f, MidpointRounding.AwayFromZero))).Ticks)
            });
        }
        */

        // Act
        var results = new List<float>();
        for (var i = 0; i < symptom.ValuesHistory.Count(); i++)
        {
            results.Add(quizzService.GetAverageValues(symptom, symptom.ValuesHistory.Count()-i, 0, factorType));
        }

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Has.Count.EqualTo(5));
        for (var i = 0; i < results.Count; i++)
        {
            if (i + 1 < results.Count)
            {
                // Check that average is increasing
                Assert.That(results.ElementAt(i), Is.GreaterThan(results.ElementAt(i + 1)));
            }
        }
    }
    
    [Test]
    public void GetAverageValues_MixedTypes_ShouldReturnCorrectAverage()
    {
        // Arrange
        var quizzService = new QuizzService(FakeApi);
        var symptom = new Symptom()
        {
            Type = new SymptomType()
            {
                Id = 1,
                Name = "Symptom"
            },
            Id = 1,
            isDisabled = false,
            TypeId = 1,
            UserId = "userid",
            ValuesHistory = MixedTypesAscendingValues
        };
        /*
        for (int i = 1; i <= 20; i++)
        {
            fakeSymptomValues.Add(new FactorValue()
            {
                Id = i,
                SymptomId = 1,
                Type = i%2 == 0 ? FactorType.Harmfulness : FactorType.Presence,
                Value = 0 + MathF.Round(i/20f, 2, MidpointRounding.ToEven),
                Timestamp = new DateTime((new TimeSpan(DateTime.Now.Ticks) - TimeSpan.FromDays(10 - Math.Round(i/2f, MidpointRounding.AwayFromZero))).Ticks)
            });
        }
        */

        // Act
        var result = quizzService.GetAverageValues(symptom);

        // Assert
        Assert.That(result, Is.Not.EqualTo(float.NaN));
        Assert.That(result, !Is.GreaterThan(0.5f));
    }
}