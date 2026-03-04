# Clinical Trial Risk Analyzer

An AI-powered web application that analyzes patient data and generates structured 
clinical trial risk assessments. Built with ASP.NET Core, Razor Pages, and integrated 
with AI language models to support Risk-Based Quality Management (RBQM) workflows.

## 🚀 Live Demo
**App is live at:** https://clinical-risk-analyzer.azurewebsites.net

## Tech Stack

- **ASP.NET Core 10** — Web framework with Razor Pages
- **C#** — Backend logic and AI service integration
- **Markdig** — Markdown rendering for AI responses
- **Docker** — Multi-stage containerized deployment
- **Microsoft Azure** — Cloud hosting via App Service
- **Groq API** — Fast AI inference (LLaMA 3.3 70B model)

## Features

- AI-powered patient risk assessment using large language models
- Structured risk output: Risk Level, Key Factors, Monitoring Actions, Protocol Risks
- Automatic risk level detection with color-coded badges (Low / Medium / High)
- Clean side-by-side clinical UI — form left, results right
- Post/Redirect/Get pattern — no form resubmission on page refresh
- Graceful error handling for API failures and timeouts
- Pluggable AI provider architecture via interface pattern
- Secure API key management via environment variables
- Dockerized with multi-stage build for optimized image size

## AI Provider Architecture

The app uses an interface-based design pattern for AI providers — making it easy
to swap between different AI services without changing any business logic:
```csharp
public interface IAIService
{
    Task<string> AnalyzeAsync(string prompt);
}
```

The active implementation is `GroqService` using LLaMA 3.3 70B. To add a new 
provider, simply implement `IAIService` and register it in `Program.cs` — 
no other code changes needed.

```csharp
builder.Services.AddScoped<IAIService>(_ => new GroqService(apiKey));
```

## Getting Started

### Option 1: Live Demo
Visit the live app directly:
```
https://clinical-risk-analyzer.azurewebsites.net
```

### Option 2: Run with Docker
```bash
git clone https://github.com/adham1177/ClinicalRiskAnalyzer.git
cd ClinicalRiskAnalyzer
docker build -t clinical-risk-analyzer .
docker run -p 8080:8080 -e GroqApiKey="your-key-here" clinical-risk-analyzer
```
Then open: `http://localhost:8080`

### Option 3: Run Locally
```bash
git clone https://github.com/adham1177/ClinicalRiskAnalyzer.git
cd ClinicalRiskAnalyzer
```

Copy the example config and add your key:
```bash
cp appsettings.example.json appsettings.json
```

Edit `appsettings.json`:
```json
{
  "GroqApiKey": "your-groq-api-key-here"
}
```

Then run:
```bash
dotnet restore
dotnet run
```

## Configuration

| Setting | Description |
|---|---|
| `GroqApiKey` | Your Groq API key from console.groq.com |

Get a free Groq API key at: https://console.groq.com

## Risk Assessment Output

The AI generates a structured report covering:

1. **Overall Risk Level** — Low / Medium / High
2. **Key Risk Factors** — Patient-specific risks identified
3. **Trial Protocol Concerns** — Issues specific to the trial
4. **Recommended Monitoring Actions** — What to watch during the trial
5. **Protocol Deviation Risks** — Potential compliance issues
