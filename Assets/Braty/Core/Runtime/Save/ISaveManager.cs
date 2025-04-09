namespace Braty.Core.Runtime.Save
{
    public interface ISaveManager
    {
        bool HasKey(string key);
        bool TryGetValue<T>(string key, out T value, T defaultValue = default);
        void Save<T>(string key, T value);
    }
}