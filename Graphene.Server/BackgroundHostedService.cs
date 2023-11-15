using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;

namespace Graphene.Server;

public class BackgroundHostedService(ILogger<BackgroundHostedService> logger, IMemoryCache memoryCache,
    JokeService jokeService) : BackgroundService
{
    private readonly ILogger<BackgroundHostedService> logger = logger;
    private readonly IMemoryCache memoryCache = memoryCache;
    private readonly JokeService jokeService = jokeService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var joke = jokeService.GetJoke();
            logger.LogWarning("{Joke}", joke);

            memoryCache.Set("joke", joke);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}

public sealed class JokeService
{
    public Joke GetJoke()
    {
        var joke = _jokes.ElementAt(
            Random.Shared.Next(_jokes.Count));

        return joke;
    }

    // Programming jokes borrowed from:
    // https://github.com/eklavyadev/karljoke/blob/main/source/jokes.json
    private readonly ImmutableHashSet<Joke> _jokes =
    [
        new("What's the best thing about a Boolean?", "Even if you're wrong, you're only off by a bit."),
        new("What's the object-oriented way to become wealthy?", "Inheritance"),
        new("Why did the programmer quit their job?", "Because they didn't get arrays."),
        new("Why do programmers always mix up Halloween and Christmas?", "Because Oct 31 == Dec 25"),
        new("How many programmers does it take to change a lightbulb?", "None that's a hardware problem"),
        new("If you put a million monkeys at a million keyboards, one of them will eventually write a Java program", "the rest of them will write Perl"),
        new("['hip', 'hip']", "(hip hip array)"),
        new("To understand what recursion is...", "You must first understand what recursion is"),
        new("There are 10 types of people in this world...", "Those who understand binary and those who don't"),
        new("Which song would an exception sing?", "Can't catch me - Avicii"),
        new("Why do Java programmers wear glasses?", "Because they don't C#"),
        new("How do you check if a webpage is HTML5?", "Try it out on Internet Explorer"),
        new("A user interface is like a joke.", "If you have to explain it then it is not that good."),
        new("I was gonna tell you a joke about UDP...", "...but you might not get it."),
        new("The punchline often arrives before the set-up.", "Do you know the problem with UDP jokes?"),
        new("Why do C# and Java developers keep breaking their keyboards?", "Because they use a strongly typed language."),
        new("Knock-knock.", "A race condition. Who is there?"),
        new("What's the best part about TCP jokes?", "I get to keep telling them until you get them."),
        new("A programmer puts two glasses on their bedside table before going to sleep.", "A full one, in case they gets thirsty, and an empty one, in case they don’t."),
        new("There are 10 kinds of people in this world.", "Those who understand binary, those who don't, and those who weren't expecting a base 3 joke."),
        new("What did the router say to the doctor?", "It hurts when IP."),
        new("An IPv6 packet is walking out of the house.", "He goes nowhere."),
        new("3 SQL statements walk into a NoSQL bar. Soon, they walk out", "They couldn't find a table.")
    ];
}

public readonly record struct Joke(string Setup, string Punchline)
{
    public override string ToString() => $"{Setup}{Environment.NewLine}{Punchline}";
}