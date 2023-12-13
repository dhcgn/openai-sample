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
    public string InitialPrompt { get; } = @"
I want to play a round Tic-Tac-Toe.
I'm X you are O. Make your move and evaluate if the game is over, and who the winner is. 
Comment each move with in a funny comment to challenge me. Do your best to win the game!

Respond only with a new data structure of the new state including your next move and comment.

```json
%%REPLACE%% 
```
";
    private bool firstMove = true;
    public TicTacToeGame MakeNextMove(TicTacToeGame game)
    {
        var prompt = "";
        if (firstMove)
        {
            firstMove = false;
            prompt = InitialPrompt.Replace("%%REPLACE%%", JsonConvert.SerializeObject(game));
        }
        else
        {
            prompt = JsonConvert.SerializeObject(game);
        }

        game = GetNextGameStateFromOpenAIApi(prompt, game);

        return game;
    }

    private TicTacToeGame GetNextGameStateFromOpenAIApi(string prompt, TicTacToeGame game)
    {
        messagesStorage.Add(new Message { Role = "user", Content = prompt });
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
