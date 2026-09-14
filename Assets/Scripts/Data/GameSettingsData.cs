using System;

namespace ArenaDuel.Data
{
    [Serializable]
    public sealed class GameSettingsData
    {
        public string selectedHero = "ember";
        public int qualityLevel = 2;
        public float masterVolume = 0.8f;
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.85f;
        public float joystickSensitivity = 1f;
        public float cameraSensitivity = 1f;
        public bool vibration = true;
    }
}
