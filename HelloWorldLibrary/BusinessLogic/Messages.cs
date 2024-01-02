using System.Text.Json;
using HelloWorldLibrary.models;
using Microsoft.Extensions.Logging;

namespace HelloWorldLibrary.BusinessLogic;

public class Messages : IMessages
{
    private readonly ILogger<Messages> _logger;

    public Messages(ILogger<Messages> pLogger)
    {
        _logger = pLogger;
    }

    public string Greeting(string pLanguage)
    {
        string lOutput = LookUpCustomText("Greeting", pLanguage);
        return lOutput;
    }

    private string LookUpCustomText(string pKey, string pLanguage)
    {
        JsonSerializerOptions lOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        
        try
        {
            List<CustomText>? lMessageSets = JsonSerializer
                .Deserialize<List<CustomText>>
                (
                    File.ReadAllText("CustomText.json"), lOptions
                );

            CustomText? lMessages = lMessageSets.Where(x => x.Language == pLanguage).First();

            if (lMessages == null)
            {
                throw new NullReferenceException("The specified language was not found in the json file");
            }

            return lMessages.Translations[pKey];
        }
        catch (Exception lException)
        {
            _logger.LogError("Error looking up the custom text", lException);
            throw;
        }
        
        
    }
}