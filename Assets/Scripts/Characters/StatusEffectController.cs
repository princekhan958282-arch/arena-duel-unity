using System.Collections.Generic;
using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Characters
{
    public sealed class StatusEffectController : MonoBehaviour
    {
        sealed class Active { public StatusKind kind; public float remaining; public float magnitude; public float tick; }
        readonly List<Active> active = new List<Active>();
        MovementController movement;
        HealthController health;
        public bool IsStunned { get; private set; }

        void Awake() { movement = GetComponent<MovementController>(); health = GetComponent<HealthController>(); }

        public void Apply(StatusKind kind, float duration, float magnitude)
        {
            if (kind == StatusKind.None || duration <= 0) return;
            active.Add(new Active { kind = kind, remaining = duration, magnitude = magnitude, tick = 0f });
        }

        void Update()
        {
            IsStunned = false;
            float slow = 1f;
            for (int i = active.Count - 1; i >= 0; i--)
            {
                Active e = active[i]; e.remaining -= Time.deltaTime;
                if (e.kind == StatusKind.Stun) IsStunned = true;
                if (e.kind == StatusKind.Slow) slow = Mathf.Min(slow, Mathf.Clamp01(1f - e.magnitude));
                if (e.kind == StatusKind.Burn)
                {
                    e.tick -= Time.deltaTime;
                    if (e.tick <= 0f) { health.TakeDamage(Mathf.Max(1f, e.magnitude)); e.tick = 0.5f; }
                }
                if (e.remaining <= 0f) active.RemoveAt(i);
            }
            if (movement) movement.SpeedMultiplier = slow;
        }
    }
}
