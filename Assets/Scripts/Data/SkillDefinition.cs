using UnityEngine;

namespace ArenaDuel.Data
{
    public enum SkillKind { Melee, Projectile, DashStrike, Area }
    public enum StatusKind { None, Burn, Slow, Stun }

    [CreateAssetMenu(menuName = "Arena Duel/Skill Definition")]
    public sealed class SkillDefinition : ScriptableObject
    {
        public string skillId;
        public string displayName;
        [TextArea] public string description;
        public SkillKind kind;
        public float damage = 100f;
        public float cooldown = 1f;
        public float range = 2f;
        public float radius = 1.25f;
        public float windup = 0.12f;
        public float movementLock = 0.2f;
        public float knockback = 2f;
        public float projectileSpeed = 14f;
        public StatusKind status;
        public float statusDuration;
        public float statusMagnitude;
    }
}
