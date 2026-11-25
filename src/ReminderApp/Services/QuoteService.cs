namespace ReminderApp.Services;

public static class QuoteService
{
    private static readonly string[] Quotes =
    {
        "The secret of getting ahead is getting started.",
        "Small steps every day add up to big results.",
        "Done is better than perfect.",
        "You don't have to see the whole staircase, just take the first step.",
        "Focus on progress, not perfection.",
        "Well begun is half done.",
        "What you do today can improve all your tomorrows.",
        "Discipline is choosing what you want most over what you want now.",
        "Believe you can and you're halfway there.",
        "Success is the sum of small efforts repeated day in and day out.",
        "The secret of getting ahead is getting started.",
        "Great things are done by a series of small things brought together.",
        "It always seems impossible until it's done."
    };

    public static string GetRandom() => Quotes[Random.Shared.Next(Quotes.Length)];
}
