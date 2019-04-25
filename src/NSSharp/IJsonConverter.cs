namespace NSSharp
{
    public interface IJsonConverter
    {
        T Deserialize<T>(string value);
    }
}