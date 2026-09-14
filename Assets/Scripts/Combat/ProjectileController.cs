using ArenaDuel.Characters;
using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Combat
{
    public sealed class ProjectileController : MonoBehaviour
    {
        HealthController owner;
        ComboSystem combo;
        SkillDefinition skill;
        float power;
        Vector3 direction;
        float remaining;

        public void Initialize(HealthController source, ComboSystem comboSystem, SkillDefinition definition, float attackPower, Vector3 travelDirection)
        {
            owner = source; combo = comboSystem; skill = definition; power = attackPower; direction = travelDirection.normalized;
            remaining = Mathf.Max(0.5f, skill.range / Mathf.Max(0.1f, skill.projectileSpeed));
        }

        void Update()
        {
            transform.position += direction * skill.projectileSpeed * Time.deltaTime;
            remaining -= Time.deltaTime;
            if (remaining <= 0) Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            HealthController target = other.GetComponentInParent<HealthController>();
            if (!target || target == owner) return;
            DamageSystem.Apply(target.gameObject, skill.damage, power, direction, skill.knockback, skill.status, skill.statusDuration, skill.statusMagnitude);
            combo?.RegisterHit();
            Destroy(gameObject);
        }
    }
}
