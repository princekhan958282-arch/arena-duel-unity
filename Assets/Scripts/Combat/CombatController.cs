using System.Collections;
using System.Collections.Generic;
using ArenaDuel.Characters;
using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Combat
{
    [RequireComponent(typeof(CooldownSystem))]
    public sealed class CombatController : MonoBehaviour
    {
        public HeroDefinition hero;
        public Transform attackOrigin;
        public Transform projectileOrigin;
        public LayerMask fighterMask = ~0;
        public bool IsBusy { get; private set; }

        CooldownSystem cooldowns;
        MovementController movement;
        HealthController health;
        ComboSystem combo;
        readonly Collider[] hits = new Collider[16];

        void Awake()
        {
            cooldowns = GetComponent<CooldownSystem>();
            movement = GetComponent<MovementController>();
            health = GetComponent<HealthController>();
            combo = GetComponent<ComboSystem>();
        }

        public void Configure(HeroDefinition definition)
        {
            hero = definition;
            if (!attackOrigin) attackOrigin = transform;
            if (!projectileOrigin) projectileOrigin = transform;
        }

        public bool TryUse(SkillDefinition skill)
        {
            if (!skill || IsBusy || (health && health.IsDead) || !cooldowns.IsReady(skill.skillId)) return false;
            cooldowns.StartCooldown(skill.skillId, skill.cooldown);
            StartCoroutine(Execute(skill));
            return true;
        }

        IEnumerator Execute(SkillDefinition skill)
        {
            IsBusy = true;
            if (skill.windup > 0) yield return new WaitForSeconds(skill.windup);
            if (!health || !health.IsDead)
            {
                switch (skill.kind)
                {
                    case SkillKind.Projectile: SpawnProjectile(skill); break;
                    case SkillKind.DashStrike:
                        movement.Dash(transform.forward, skill.range * 0.55f);
                        ApplyArea(skill, transform.position + transform.forward * 0.75f);
                        break;
                    case SkillKind.Area: ApplyArea(skill, transform.position); break;
                    default: ApplyArea(skill, (attackOrigin ? attackOrigin.position : transform.position) + transform.forward * skill.range * 0.55f); break;
                }
            }
            if (skill.movementLock > skill.windup)
                yield return new WaitForSeconds(skill.movementLock - skill.windup);
            IsBusy = false;
        }

        void ApplyArea(SkillDefinition skill, Vector3 center)
        {
            int count = Physics.OverlapSphereNonAlloc(center, skill.radius, hits, fighterMask, QueryTriggerInteraction.Collide);
            var damaged = new HashSet<HealthController>();
            for (int i = 0; i < count; i++)
            {
                HealthController target = hits[i].GetComponentInParent<HealthController>();
                if (!target || target == health || !damaged.Add(target)) continue;
                DamageSystem.Apply(target.gameObject, skill.damage, hero ? hero.attack : 100f, target.transform.position - transform.position,
                    skill.knockback, skill.status, skill.statusDuration, skill.statusMagnitude);
                combo?.RegisterHit();
            }
        }

        void SpawnProjectile(SkillDefinition skill)
        {
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = hero && hero.heroId == "ember" ? "EmberFireProjectile" : "Projectile";
            projectile.transform.position = (projectileOrigin ? projectileOrigin.position : transform.position + Vector3.up) + transform.forward * 0.7f;
            projectile.transform.localScale = Vector3.one * 0.42f;
            Collider col = projectile.GetComponent<Collider>(); col.isTrigger = true;
            Rigidbody body = projectile.AddComponent<Rigidbody>(); body.useGravity = false; body.isKinematic = true;
            ProjectileController controller = projectile.AddComponent<ProjectileController>();
            controller.Initialize(health, combo, skill, hero ? hero.attack : 100f, transform.forward);
            Renderer renderer = projectile.GetComponent<Renderer>();
            renderer.material.color = new Color(1f, 0.18f, 0.02f);
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", new Color(3f, 0.25f, 0.01f));
        }

        public CooldownSystem Cooldowns => cooldowns;
    }
}
