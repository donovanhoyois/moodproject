using Microsoft.IdentityModel.Tokens;

namespace MoodProject.Api.Services.Builder;

public class LuceneQueryBuilder
{
    private List<string>? FieldValues;
    private string? FieldName;
    private string? LogicalOperator;

    public LuceneQueryBuilder WithFieldName(string name)
    {
        FieldName = name;
        return this;
    }
    public LuceneQueryBuilder WithFieldValues(List<string> values)
    {
        FieldValues = values;
        return this;
    }

    public LuceneQueryBuilder WithLogicalOperator(string logicalOperator)
    {
        LogicalOperator = logicalOperator;
        return this;
    }
    
    
    public string Build()
    {
        var result = string.Empty;
        foreach (var field in FieldValues)
        {
            if (!result.IsNullOrEmpty())
            {
                result += $" {LogicalOperator} ";
            }
            result += $"{FieldName}:{field}";
        }

        return result;
    }
}