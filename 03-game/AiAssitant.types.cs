using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class ChatSession
{
    [JsonProperty("model")]
    public string Model { get; set; } = "gpt-3.5-turbo";
    
    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }

    [JsonProperty("temperature")]
    public int Temmperature { get; set; } = 1;

    [JsonProperty("max_tokens")]
    public int MaxTokens { get; set; } = 256;

    [JsonProperty("top_p")]
    public int TopP { get; set; } = 1;

    [JsonProperty("frequency_penalty")]
    public int FrequencyPenalty { get; set; } = 0;

    [JsonProperty("presence_penalty")]
    public int PresencePenalty { get; set; } = 0;
}

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
}

public class ChatCompletion
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("object")]
    public string Object { get; set; }
    
    [JsonProperty("created")]
    public long Created { get; set; }
    
    [JsonProperty("model")]
    public string Model { get; set; }
    
    [JsonProperty("system_fingerprint")]
    public string SystemFingerprint { get; set; }
    
    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }
    
    [JsonProperty("usage")]
    public UsageData Usage { get; set; }
}

public class Choice
{
    [JsonProperty("index")]
    public int Index { get; set; }
    
    [JsonProperty("message")]
    public Message Message { get; set; }
    
    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; }
}

public class UsageData
{
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }
    
    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }
    
    [JsonProperty("total_tokens")]
    public int TotalTokens { get; set; }
}