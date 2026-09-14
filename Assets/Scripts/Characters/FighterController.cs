using ArenaDuel.Combat;
using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Characters
{
    [RequireComponent(typeof(MovementController), typeof(HealthController), typeof(CombatController))]
    public sealed class FighterController : MonoBehaviour
    {
        public HeroDefinition hero;
        public MonoBehaviour inputSource;
        public Transform modelRoot;

        MovementController movement;
        HealthController health;
        StatusEffectController effects;
        CombatController combat;
        IFighterInputSource input;

        public HealthController Health => health;
        public CombatController Combat => combat;

        void Awake()
        {
            movement = GetComponent<MovementController>();
            health = GetComponent<HealthController>();
            effects = GetComponent<StatusEffectController>();
            combat = GetComponent<CombatController>();
        }

        void Start()
        {
            Configure(hero);
            SetInputSource(inputSource);
        }

        public void Configure(HeroDefinition definition)
        {
            hero = definition;
            movement.definition = definition;
            health.Configure(definition);
            combat.Configure(definition);
        }

        public void SetInputSource(MonoBehaviour source)
        {
            inputSource = source;
            input = source as IFighterInputSource;
        }

        public void SetCamera(Transform cameraTransform) => movement.cameraTransform = cameraTransform;

        void Update()
        {
            if (health.IsDead) { movement.Locked = true; return; }
            if (input == null && inputSource) input = inputSource as IFighterInputSource;
            FighterInput state = input != null ? input.ReadInput() : default;
            movement.Locked = combat.IsBusy || (effects && effects.IsStunned);
            movement.Tick(state.move, state.jump);
            if (!movement.Locked)
            {
                if (state.basic) combat.TryUse(hero.basicAttack);
                else if (state.skill1) combat.TryUse(hero.skill1);
                else if (state.skill2) combat.TryUse(hero.skill2);
                else if (state.ultimate) combat.TryUse(hero.ultimate);
            }
            input?.ConsumeTransientInput();
        }
    }
}
