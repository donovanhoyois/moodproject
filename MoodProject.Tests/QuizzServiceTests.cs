using MoodProject.Core;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.Out;
using MoodProject.Services;
using Moq;

namespace MoodProject.Tests;

public class QuizzServiceTests
{
    private IAppApi FakeApi;
    [SetUp]
    public void Setup()
    {
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
            .Setup(api => api.GetSymptomsWithHistory(It.IsAny<string>()))
            .ReturnsAsync(fakeSymptoms);
        FakeApi = mock.Object;
    }

    [Test]
    public async Task GenerateQuizz_ShouldReturnAQuizz()
    {
        var quizzService = new QuizzService(FakeApi);
        var r = await quizzService.Generate(string.Empty);
        Assert.That(r, Is.Not.Null);
        Assert.That(r.Count(), Is.Not.Zero);
    }
}