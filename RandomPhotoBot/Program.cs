
using RandomPhotoBot;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

ITelegramBotClient bot = new TelegramBotClient("6423827881:AAHc5x0ZFfmPqNB8Viy5CWlQ8FCd79fspog");
var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { },
};

bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
Console.ReadLine();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Некоторые действия
    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
    
    if(update.Type == UpdateType.Message)
    {
        
        
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(
            new KeyboardButton[]
            {
                new KeyboardButton("Load image"),
                new KeyboardButton("button2")
            });
        
        var message = update.Message ?? throw new ArgumentNullException("message");
        var userMenuStateProvider = new UserMenuStateProvider();
        
        if (message.Text.ToLower() == "/start")
        {
            userMenuStateProvider.SetState(update.Message.UserShared.UserId.ToString(), MenuState.Default);
            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "за что мне это??",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
            return;
        }
        
        

        var user = update.Message.UserShared;
        if (user == null)
            return;
        
        var userMenuState = userMenuStateProvider.GetState(user.UserId.ToString());

        if (userMenuState == MenuState.SendImage)
        {
            
        }
        
        if (string.IsNullOrWhiteSpace(message.Text))
            throw new ArgumentNullException("message.Text");

        if (message.Text == "Load image")
        {
            userMenuStateProvider.SetState(user.UserId.ToString(), MenuState.SendImage);
            
            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Загрузи картинку",
                cancellationToken: cancellationToken);
        }
    }
}

async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    // Некоторые действия
    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
}

async void DownloadFile(string fileId, string path)
{
    try
    {
        var file = await bot.GetFileAsync(fileId);

        using (var saveImageStream = new FileStream(path, FileMode.Create))
        {
            await bot.DownloadFileAsync($"c:/bot_images/{file.FileId}.jpg", saveImageStream, cancellationToken);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error downloading: " + ex.Message);
    }
}