using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Vault
{
    private Dictionary<string, string> dict;

    public Vault()
    {
        dict = new Dictionary<string, string>();
    }

    // Public property to expose the dictionary for serialization
    [JsonPropertyName("Items")]
    public Dictionary<string, string> Items
    {
        get { return dict; }
        set { dict = value; } // Allow deserialization to set the dictionary
    }
    
    public void Set(string property, string value)
    {
        dict[property] = value;
    }

    public string? Get(string key)
    {
        if (dict.TryGetValue(key, out string? value))
        {
            return value;
        }
        return null;
    }
}
