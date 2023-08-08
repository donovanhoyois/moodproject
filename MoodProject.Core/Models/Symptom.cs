﻿namespace MoodProject.Core.Models;

public class Symptom
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public SymptomType Type { get; set; }
    public int TypeId { get; set; }
    public IEnumerable<FactorValue> ValuesHistory { get; set; }

    public Symptom()
    {
        
    }

    public Symptom(SymptomType type, IEnumerable<FactorValue> valuesHistory)
    {
        Type = type;
        ValuesHistory = valuesHistory;
    }
}