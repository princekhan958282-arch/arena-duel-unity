using ArenaDuel.Characters;
using UnityEngine;

namespace ArenaDuel.AI
{
    public sealed class ArenaAIController : MonoBehaviour, IFighterInputSource
    {
        public FighterController owner;
        public FighterController target;
        public float decisionInterval = 0.16f;
        FighterInput decision;
        float nextDecision;

        public FighterInput ReadInput()
        {
            if (Time.time >= nextDecision) Decide();
            return decision;
        }

        void Decide()
        {
            nextDecision = Time.time + decisionInterval + Random.Range(-0.03f, 0.05f);
            decision = default;
            if (!owner || !target || owner.Health.IsDead || target.Health.IsDead) return;
            Vector3 delta = target.transform.position - owner.transform.position;
            float distance = delta.magnitude;
            Vector3 local = owner.transform.InverseTransformDirection(delta.normalized);
            bool retreat = owner.Health.Normalized < 0.2f && distance < 4f;
            decision.move = retreat ? new Vector2(-local.x, -local.z) : new Vector2(local.x, local.z);
            if (distance < 2.2f)
            {
                decision.move *= 0.2f;
                float roll = Random.value;
                if (roll < 0.44f) decision.basic = true;
                else if (roll < 0.66f) decision.skill2 = true;
                else if (roll < 0.84f) decision.skill1 = true;
                else decision.ultimate = true;
            }
            else if (distance < 8f && Random.value < 0.35f) decision.skill1 = true;
            if (Random.value < 0.035f) decision.jump = true;
        }

        public void ConsumeTransientInput()
        {
            decision.jump = decision.basic = decision.skill1 = decision.skill2 = decision.ultimate = false;
        }
    }
}
