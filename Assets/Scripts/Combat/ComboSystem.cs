using System;
using UnityEngine;

namespace ArenaDuel.Combat
{
    public sealed class ComboSystem : MonoBehaviour
    {
        public int Count { get; private set; }
        public float comboWindow = 1.6f;
        float expiresAt;
        public event Action<int> Changed;

        void Update()
        {
            if (Count > 0 && Time.time > expiresAt) { Count = 0; Changed?.Invoke(Count); }
        }

        public void RegisterHit()
        {
            Count++; expiresAt = Time.time + comboWindow; Changed?.Invoke(Count);
        }
    }
}
