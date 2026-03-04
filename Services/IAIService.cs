namespace ClinicalRiskAnalyzer.Services;

public interface IAIService
{
    Task<string> AnalyzeAsync(string prompt);
}