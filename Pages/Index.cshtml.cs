using ClinicalRiskAnalyzer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalRiskAnalyzer.Pages;

public class IndexModel : PageModel
{
    private readonly IAIService _aiService;
    public string RiskAssessment { get; set; } = string.Empty;
    public string RiskAssessmentHtml => Markdown.ToHtml(RiskAssessment);
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsLoading { get; set; } = false;

    public IndexModel(IAIService aiService)
    {
        _aiService = aiService;
    }

    public void OnGet()
    {
        if (TempData["RiskAssessment"] is string result)
            RiskAssessment = result;

        if (TempData["ErrorMessage"] is string error)
            ErrorMessage = error;  
    }

    public async Task<IActionResult> OnPostAsync(string PatientName, int Age, string Diagnosis, string Medications, string LabResults, string TrialProtocol)
    {
        if (Age < 1 || Age > 120)
        {
            TempData["ErrorMessage"] = "Please enter a valid age between 1 and 120.";
            return RedirectToPage();
        }

        try
        {
            var prompt = $"""
            You are a clinical trial risk assessment AI assistant.
            Analyze the following patient data and provide a structured risk assessment.

            Patient: {PatientName}
            Age: {Age}
            Diagnosis: {Diagnosis}
            Current Medications: {Medications}
            Recent Lab Results: {LabResults}
            Trial Protocol: {TrialProtocol}

            Please provide:
            1. Overall Risk Level (Low / Medium / High)
            2. Key Risk Factors identified
            3. Specific concerns for this trial protocol
            4. Recommended monitoring actions
            5. Any protocol deviation risks

            Be concise, clinical, and specific.
            """;

            RiskAssessment = await _aiService.AnalyzeAsync(prompt);

            if (string.IsNullOrWhiteSpace(RiskAssessment))
                TempData["ErrorMessage"] = "The AI returned an empty response. Please try again.";
            else
                TempData["RiskAssessment"] = RiskAssessment;

        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = "Could not connect to the AI service. Please check your connection and try again.";
        }
        catch (TaskCanceledException)
        {
            TempData["ErrorMessage"] = "The request timed out. The AI service may be busy — please try again.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
        }
       
        return RedirectToPage();
    }




}
