using System.Net.Http.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;

string prompt = "Hello, how are you? And name a random city.";

// The apiKey is stored in a file called OPENAITOKEN.txt at project root
string apiKey = File.ReadAllText("../../../../OPENAITOKEN.txt");
HttpClient httpClient = new HttpClient { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiKey) } };

// Documentation: https://platform.openai.com/docs/guides/text-generation/chat-completions-api
var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", new
{
    model = "gpt-3.5-turbo",
    messages = new[] { new { role = "user", content = prompt } }
});

// I use dynamic here because I don't want to create a class for the response for the sake of simplicity
var json = await response.Content.ReadAsStringAsync();
dynamic result = JsonConvert.DeserializeObject<dynamic>(json);

Console.WriteLine(result.choices[0].message.content);

/*
{
  "id": "chatcmpl-8V452dWh3nvd0xiK8tSmT5TPsIVLt",
  "object": "chat.completion",
  "created": 1702414544,
  "model": "gpt-3.5-turbo-0613",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "Hello! I'm an AI language model, so I don't have feelings, but I'm here and ready to help. How can I assist you today?"
      },
      "finish_reason": "stop"
    }
  ],
*/