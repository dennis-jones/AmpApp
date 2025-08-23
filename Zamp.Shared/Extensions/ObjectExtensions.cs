using System.Reflection;
using System.Text.Json;

namespace Zamp.Extensions;

public static class GenericExtensions
{
    public static string ToJson(this object? obj)
    {
        return JsonSerializer.Serialize(obj);
    }
    
    public static T? Clone<T>(this T obj)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize<T>(obj));
    }
    
    public static bool HasProperty(this object obj, string propertyName)
    {
	    return obj.GetType().GetProperty(propertyName) != null;
    }

    public static T? GetPropertyValue<T>(this object? obj, string propertyName)
	{
		try
		{
			var val = obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null);
			return (T?)val;
		}
		catch
		{
			return default;
		}
    }

    public static object? ConvertWhiteSpaceStringsToNull(this object? obj)
	{
		if (obj == null)
			return null;

		foreach (PropertyInfo prop in obj.GetType().GetProperties())
		{
			if (prop.PropertyType == typeof(string))
			{
				string? val = prop.GetValue(obj)?.ToString();
				if (string.IsNullOrWhiteSpace(val))
				{
					prop.SetValue(obj, null);
				}
			}
		}
		return obj;
	}

	public static void SetAllPropertiesToNull(this object? obj, List<string>? exclude = null)
	{
		if (obj == null)
			return;

		foreach (PropertyInfo prop in obj.GetType().GetProperties())
		{
			if ((exclude ?? []).Contains(prop.Name))
				continue;
			
			if (prop.GetSetMethod() is not null)
				prop.SetValue(obj, null);
		}
	}
}
