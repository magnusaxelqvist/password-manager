using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class FileDictionary
{
    private string filePath;
    private Dictionary<string, string> dictionary;

    public FileDictionary(string filePath)
    {
        this.filePath = filePath;
        this.dictionary = new Dictionary<string, string>();
    }

    public FileDictionary Load()
    {
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);

            Console.WriteLine("Load:" + fileContent);

            dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent) ?? new Dictionary<string, string>();
        }
        else
        {
            dictionary = new Dictionary<string, string>();
        }

        return this;
    }

    public void Save()
    {
        string fileContent = JsonSerializer.Serialize(dictionary);

        Console.WriteLine("Save:" + fileContent);

        File.WriteAllText(filePath, fileContent);
    }

    public string? Get(string key)
    {
        return this.dictionary.TryGetValue(key, out string? value) ? value : null;// Or throw an exception, based on your error handling preference
    }

    public void Set(string key, string value)
    {
        dictionary[key] = value;
    }

    public bool Delete(string key)
    {
        return dictionary.Remove(key);
    }

    public IEnumerable<string> GetAllKeys()
    {
        return dictionary.Keys;
    }
}

