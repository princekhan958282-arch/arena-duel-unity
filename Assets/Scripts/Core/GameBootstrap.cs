using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArenaDuel.Core
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            SaveService.Load();
        }

        void Start()
        {
            if (SceneManager.GetActiveScene().name == "Boot")
                SceneManager.LoadScene("MainMenu");
        }
    }
}
