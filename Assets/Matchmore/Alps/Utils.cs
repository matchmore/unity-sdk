using System;
using System.Text;
using Newtonsoft.Json;

public class Utils
{
    private class ApiKeyObject
    {
        public string Sub { get; set; }
    }

    public static string ExtractWorldId(string apiKey)
    {
        try
        {
            var subjectData = Convert.FromBase64String(apiKey.Split('.')[1]);
            var subject = Encoding.UTF8.GetString(subjectData);
            var deserializedApiKey = JsonConvert.DeserializeObject<ApiKeyObject>(subject);

            return deserializedApiKey.Sub;
        }
        catch (Exception e)
        {
            throw new ArgumentException("Api key was invalid", e);
        }
    }
}

public class Tuple<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}

public static class Tuple
{
    public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
    {
        var tuple = new Tuple<T1, T2>(first, second);
        return tuple;
    }
}