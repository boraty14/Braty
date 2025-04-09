namespace Braty.Core.Runtime.Scripts.Save
{
    public class SaveManager : ISaveManager
    {
        bool ISaveManager.HasKey(string key)
        {
            return HasKeyInternal(key);
        }

        bool ISaveManager.TryGetValue<T>(string key, out T value, T defaultValue)
        {
            if (!HasKeyInternal(key))
            {
                value = defaultValue;
                return false;
            }

            //value = ES3.Load<T>(key);
            value = default;
            return true;
        }

        void ISaveManager.Save<T>(string key, T value)
        {
            //ES3.Save<T>(key,value);
        }

        private bool HasKeyInternal(string key)
        {
            //return ES3.KeyExists(key);
            return default;
        }
    }
}