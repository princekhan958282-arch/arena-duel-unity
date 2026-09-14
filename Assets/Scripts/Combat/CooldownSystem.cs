using System.Collections.Generic;
using UnityEngine;

namespace ArenaDuel.Combat
{
    public sealed class CooldownSystem : MonoBehaviour
    {
        readonly Dictionary<string, float> readyAt = new Dictionary<string, float>();
        public bool IsReady(string id) => !readyAt.TryGetValue(id, out float t) || Time.time >= t;
        public void StartCooldown(string id, float duration) => readyAt[id] = Time.time + duration;
        public float Remaining(string id) => readyAt.TryGetValue(id, out float t) ? Mathf.Max(0f, t - Time.time) : 0f;
        public float Normalized(string id, float duration) => duration <= 0 ? 0 : Mathf.Clamp01(Remaining(id) / duration);
    }
}
