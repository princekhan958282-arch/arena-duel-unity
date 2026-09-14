using System;
using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Characters
{
    public sealed class HealthController : MonoBehaviour
    {
        public HeroDefinition definition;
        public float Current { get; private set; }
        public float Max => definition != null ? definition.maxHealth : 1000f;
        public bool IsDead { get; private set; }
        public float Normalized => Max <= 0 ? 0 : Current / Max;
        public event Action<float, float> Changed;
        public event Action<HealthController> Died;

        void Awake() => Current = Max;

        public void Configure(HeroDefinition hero)
        {
            definition = hero;
            Current = Max;
            IsDead = false;
            Changed?.Invoke(Current, Max);
        }

        public void TakeDamage(float rawDamage, float attackerPower = 100f)
        {
            if (IsDead) return;
            float defense = definition != null ? definition.defense : 0f;
            float multiplier = 100f / (100f + Mathf.Max(0f, defense));
            Current = Mathf.Max(0f, Current - rawDamage * (attackerPower / 100f) * multiplier);
            Changed?.Invoke(Current, Max);
            if (Current <= 0f)
            {
                IsDead = true;
                Died?.Invoke(this);
            }
        }
    }
}
