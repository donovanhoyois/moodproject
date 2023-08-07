using MoodProject.Core;

namespace MoodProject.Api;

public class SymptomEntity : Symptom
{
    public bool isDisabled { get; set; }

    public SymptomEntity(Symptom symptom)
    {
        Id = symptom.Id;
        UserId = symptom.UserId;
        Type = symptom.Type;
        TypeId = symptom.TypeId;
        ValuesHistory = symptom.ValuesHistory;
        isDisabled = false;
    }

    public SymptomEntity()
    {
        
    }
}