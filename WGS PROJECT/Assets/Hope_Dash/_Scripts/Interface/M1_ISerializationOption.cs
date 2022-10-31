using M1_SimpleJSON;

namespace RunMinigames.Interface
{
    public interface M1_ISerializationOption
    {
        string ContentType { get; }
        T Deserialize<T>(string text);

        M1_JSONNode Deserialize(string text);
    }
}
