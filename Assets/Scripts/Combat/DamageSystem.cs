using ArenaDuel.Characters;
using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Combat
{
    public static class DamageSystem
    {
        public static void Apply(GameObject target, float damage, float attackerPower, Vector3 direction, float knockback, StatusKind status, float duration, float magnitude)
        {
            HealthController health = target.GetComponentInParent<HealthController>();
            if (!health || health.IsDead) return;
            health.TakeDamage(damage, attackerPower);
            MovementController move = health.GetComponent<MovementController>();
            if (move && knockback > 0) move.AddImpulse(direction.normalized * knockback);
            StatusEffectController effects = health.GetComponent<StatusEffectController>();
            if (effects) effects.Apply(status, duration, magnitude);
        }
    }
}
