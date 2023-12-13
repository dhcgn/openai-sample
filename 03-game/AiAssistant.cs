using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AiAssistant
{
    private readonly HttpClient httpClient;
    private List<Message> messagesStorage = new List<Message>();
    public AiAssistant()
    {
        var apiKey = File.ReadAllText("../../../../OPENAITOKEN.txt");
        httpClient = new HttpClient { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiKey) } };
    }
    public string InitialPrompt { get; } = @"I send you my move, and you answer with your turn by using the same data structure. 

1. Always evaluate if there is a winner.
2. Comment each move with in a funny comment to challenge me. 
3. Do your best to win the game!

My move:

```json
%%REPLACE%% 
```

Respond only in this json format, no other respond is allowed.
";
    private bool firstMove = true;
    public TicTacToeGame MakeNextMove(TicTacToeGame game)
    {
     
        if (firstMove)
        {
            firstMove = false;
            
            var prompt = InitialPrompt.Replace("%%REPLACE%%", JsonConvert.SerializeObject(game));
            messagesStorage.Add(new Message { Role = "system", Content = "Let us play a game of tic-tac-toe. You, the human, are `X` and I, the AI, am `O`." });
            messagesStorage.Add(new Message { Role = "user", Content = prompt });
        }
        else
        {
               var prompt = JsonConvert.SerializeObject(game);
            messagesStorage.Add(new Message { Role = "user", Content = prompt });
        }

        game = GetNextGameStateFromOpenAIApi(game);

        return game;
    }

    private TicTacToeGame GetNextGameStateFromOpenAIApi(TicTacToeGame game)
    {
        var chatSession = new ChatSession { Messages = messagesStorage };

        var json = JsonConvert.SerializeObject(chatSession);

        var response = httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", chatSession).GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
        var chatCompletion = response.Content.ReadFromJsonAsync<ChatCompletion>().GetAwaiter().GetResult();

        messagesStorage.Add(chatCompletion.Choices.Single().Message);
        var lastMessage = messagesStorage.Last().Content;

        game = JsonConvert.DeserializeObject<TicTacToeGame>(lastMessage);

        return game;
    }
}
