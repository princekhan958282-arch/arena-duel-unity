using ArenaDuel.Data;
using UnityEngine;

namespace ArenaDuel.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementController : MonoBehaviour
    {
        public HeroDefinition definition;
        public Transform cameraTransform;
        public float arenaRadius = 12f;
        public bool IsGrounded => controller.isGrounded;
        public bool Locked { get; set; }
        public float SpeedMultiplier { get; set; } = 1f;

        CharacterController controller;
        Vector3 planarVelocity;
        float verticalVelocity;
        Vector3 externalVelocity;

        void Awake() => controller = GetComponent<CharacterController>();

        public void Tick(Vector2 input, bool jump)
        {
            float speed = definition != null ? definition.movementSpeed : 5.5f;
            float acceleration = definition != null ? definition.acceleration : 20f;
            Vector3 forward = cameraTransform ? Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized : Vector3.forward;
            Vector3 right = cameraTransform ? cameraTransform.right : Vector3.right;
            Vector3 desired = Locked ? Vector3.zero : (forward * input.y + right * input.x).normalized * speed * SpeedMultiplier;
            planarVelocity = Vector3.MoveTowards(planarVelocity, desired, acceleration * Time.deltaTime);

            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
            if (!Locked && jump && controller.isGrounded)
                verticalVelocity = definition != null ? definition.jumpForce : 7.5f;
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

            if (desired.sqrMagnitude > 0.03f)
            {
                Quaternion target = Quaternion.LookRotation(desired);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, 14f * Time.deltaTime);
            }

            Vector3 motion = planarVelocity + Vector3.up * verticalVelocity + externalVelocity;
            controller.Move(motion * Time.deltaTime);
            externalVelocity = Vector3.MoveTowards(externalVelocity, Vector3.zero, 14f * Time.deltaTime);

            Vector3 flat = transform.position; flat.y = 0f;
            if (flat.magnitude > arenaRadius)
            {
                Vector3 clamped = flat.normalized * arenaRadius;
                transform.position = new Vector3(clamped.x, transform.position.y, clamped.z);
            }
        }

        public void AddImpulse(Vector3 impulse) => externalVelocity += impulse;
        public void Dash(Vector3 direction, float distance) => controller.Move(direction.normalized * distance);
    }
}
