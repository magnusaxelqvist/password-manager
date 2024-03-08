using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class FileDictionary
{
    private string filePath;
    private Dictionary<string, string> dictionary = new Dictionary<string, string>();

    public FileDictionary(string filePath)
    {
        this.filePath = filePath;
        Load();
    }

    private void Load()
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
    }

    public void Save()
    {
        string fileContent = JsonSerializer.Serialize(dictionary);

        Console.WriteLine("Save:" + fileContent);

        File.WriteAllText(filePath, fileContent);
    }

    public string? GetProperty(string key)
    {
        return this.dictionary.TryGetValue(key, out string? value) ? value : null;// Or throw an exception, based on your error handling preference
    }

    public void SetProperty(string key, string value)
    {
        dictionary[key] = value;
    }

    public bool RemoveProperty(string key)
    {
        return dictionary.Remove(key);
    }

    public IEnumerable<string> GetAllKeys()
    {
        return dictionary.Keys;
    }
}

