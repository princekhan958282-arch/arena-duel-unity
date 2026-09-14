using ArenaDuel.Core;
using UnityEngine;

namespace ArenaDuel.Audio
{
    public sealed class AudioManager : MonoBehaviour
    {
        static AudioManager instance;
        void Awake()
        {
            if (instance && instance != this) { Destroy(gameObject); return; }
            instance = this; DontDestroyOnLoad(gameObject); ApplyVolumes();
        }
        public void ApplyVolumes() => AudioListener.volume = Mathf.Clamp01(SaveService.Data.masterVolume);
    }
}
