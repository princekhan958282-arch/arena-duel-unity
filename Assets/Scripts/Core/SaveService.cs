using System.IO;
using ArenaDuel.Data;
using ArenaDuel.Utilities;
using UnityEngine;

namespace ArenaDuel.Core
{
    public static class SaveService
    {
        public static GameSettingsData Data { get; private set; } = new GameSettingsData();
        static string SavePath => Path.Combine(Application.persistentDataPath, "arena_duel_save.json");

        public static void Load()
        {
            try
            {
                if (File.Exists(SavePath))
                    Data = JsonUtility.FromJson<GameSettingsData>(File.ReadAllText(SavePath)) ?? new GameSettingsData();
            }
            catch { Data = new GameSettingsData(); }
            Apply();
        }

        public static void Save()
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(Data, true));
            Apply();
        }

        public static void Apply()
        {
            GraphicsQualityController.Apply(Data.qualityLevel);
            AudioListener.volume = Mathf.Clamp01(Data.masterVolume);
        }
    }
}
