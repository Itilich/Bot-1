using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using var cts = new CancellationTokenSource();

var bot = new TelegramBotClient("7780123339:AAEbkPzZ1tDeKs_jOg4HqSovlz5MMXm45q8", cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnMessage += OnMessage;
bot.OnError += OnError;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();
IReplyMarkup Buttons()
{
    var Keyboard = new List<List<KeyboardButton>>()
    {
        new List<KeyboardButton> {new KeyboardButton{Text = "Первое голосовое" }, new KeyboardButton{Text = "Второе голосовое" } }
    };
    return new ReplyKeyboardMarkup(Keyboard);
}
async Task OnMessage(Message msg, UpdateType type)
{
    await bot.SendTextMessageAsync(msg.Chat.Id, msg.Text, replyMarkup: Buttons());
    switch (msg.Text)
    {
        case "Первое голосовое":
            {
                await using Stream stream = System.IO.File.OpenRead("1.ogg");
                var message = await bot.SendVoice(msg.Chat.Id, stream, duration: 96);
            }
            break;
        case "Второе голосовое":
            {
                await using Stream stream = System.IO.File.OpenRead("2.ogg");
                var message = await bot.SendVoice(msg.Chat.Id, stream, duration: 167);
            }
            break;
    }
    
}

async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); 
}

async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) 
    {
        await bot.AnswerCallbackQuery(query.Id, $"You picked {query.Data}");
        await bot.SendMessage(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
    }
}