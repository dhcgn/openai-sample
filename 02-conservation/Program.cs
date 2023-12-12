using System.Net.Http.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;

// Instructions for the quiz
string prompt = @"Ask an easy C# question and give me 4 possible answers, only one of them correct. 
Don't tell me which one is correct, I'll figure it out.
Only if my answer is wrong, be very sarcastic and insulting in a funny way.";

// Want to use anothe langague? Uncomment the line below and comment the line above.
// prompt += "Write in German.";

// Get the API key from the file
string apiKey = File.ReadAllText("../../../../OPENAITOKEN.txt");
HttpClient httpClient = new HttpClient { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiKey) } };

var request = new
{
  model = "gpt-3.5-turbo",
  messages = new[] { new { role = "user", content = prompt } }
};
var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);

var json = await response.Content.ReadAsStringAsync();
dynamic result = JsonConvert.DeserializeObject<dynamic>(json);

// Write the quiz to the console
Console.WriteLine(result.choices[0].message.content);

Console.WriteLine("What is the correct answer? (type and press enter)");
// Get the correct answer from the user
string correctAnswer = Console.ReadLine();

// Create the conversation
request = new
{
  model = "gpt-3.5-turbo",
  messages = new[] {
      new { role = "user", content = prompt } ,
      new { role = (string)result.choices[0].message.role, content = (string)result.choices[0].message.content } ,
      new { role = "user", content = $"I think the answer is '{correctAnswer}'. Is this right?" } ,
      }
};
// Send the whole conversation to the API
response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);

json = await response.Content.ReadAsStringAsync();
result = JsonConvert.DeserializeObject<dynamic>(json);

// Write the answer to the console
Console.WriteLine(result.choices[0].message.content);