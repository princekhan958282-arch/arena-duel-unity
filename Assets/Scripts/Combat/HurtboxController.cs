using ArenaDuel.Characters;
using UnityEngine;

namespace ArenaDuel.Combat
{
    [RequireComponent(typeof(Collider))]
    public sealed class HurtboxController : MonoBehaviour
    {
        public HealthController owner;
        void Awake() { if (!owner) owner = GetComponentInParent<HealthController>(); }
    }
}
