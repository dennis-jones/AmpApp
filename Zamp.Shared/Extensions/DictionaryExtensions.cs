namespace Zamp.Extensions;

public static class DictionaryExtensions
{
    public static void AddCssClass(this Dictionary<string, object> dict, string className)
    {
        if (dict.TryGetValue("class", out object? value))
            dict["class"] = $"{value} {className}";
        else
            dict.Add("class", className);
    }
    
    public static void AddCssStyle(this Dictionary<string, object> dict, string propertyName, string propertyValue)
    {
        dict.AddCssStyle($"{propertyName}: {propertyValue}");
    }
            
    public static void AddCssStyle(this Dictionary<string, object> dict, string propertyNameAndValue)
    {
        if (dict.TryGetValue("style", out object? value))
            dict["style"] = $"{value}; {propertyNameAndValue}";
        else
            dict.Add("style", $"{propertyNameAndValue}");
    }
    
    public static KeyValuePair<TKey, TValue> GetEntry<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
    }
}
