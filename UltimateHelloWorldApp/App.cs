using HelloWorldLibrary.BusinessLogic;

namespace UltimateHelloWorldApp;

public class App
{
    private readonly IMessages _messages;

    public App(IMessages pMessages)
    {
        _messages = pMessages;
    }

    //UltimateHelloWorld.exe -lang=es
    public void Run(string[] args)
    {
        string lLanguage = "en";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].ToLower().StartsWith("lang="))
            {
                lLanguage = args[i].Substring(5);
                break;
            }
        }

        string lMessage = _messages.Greeting(lLanguage);
        
        Console.WriteLine(lMessage);
    }
}