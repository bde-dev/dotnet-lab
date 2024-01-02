using HelloWorldLibrary.BusinessLogic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HelloWorldTests.BusinessLogic;

public class MessagesTests
{
    [Fact]
    public void Greeting_InEnglish()
    {
        ILogger <Messages> lLogger = new NullLogger<Messages>();
        Messages lMessages = new Messages(lLogger);

        string lExpected = "Hello World";
        string lActual = lMessages.Greeting("en");
        
        Assert.Equal(lExpected, lActual);
    }
    
    [Fact]
    public void Greeting_InSpanish()
    {
        ILogger <Messages> lLogger = new NullLogger<Messages>();
        Messages lMessages = new Messages(lLogger);

        string lExpected = "Hola Mundo";
        string lActual = lMessages.Greeting("es");
        
        Assert.Equal(lExpected, lActual);
    }
    
    [Fact]
    public void Greeting_InFrench()
    {
        ILogger <Messages> lLogger = new NullLogger<Messages>();
        Messages lMessages = new Messages(lLogger);

        string lExpected = "Salut tout le monde";
        string lActual = lMessages.Greeting("fr");
        
        Assert.Equal(lExpected, lActual);
    }
    
    [Fact]
    public void Greeting_Invalid()
    {
        ILogger <Messages> lLogger = new NullLogger<Messages>();
        Messages lMessages = new Messages(lLogger);

        Assert.Throws<InvalidOperationException>(
            () => lMessages.Greeting("invalid language")
            );
    }
}