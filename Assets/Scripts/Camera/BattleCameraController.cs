using UnityEngine;

namespace ArenaDuel.CameraSystem
{
    public sealed class BattleCameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 6.5f, -8.5f);
        public float followSmoothing = 7f;
        public float lookHeight = 1.2f;
        Vector3 shake;

        void LateUpdate()
        {
            if (!target) return;
            Vector3 wanted = target.position + target.rotation * offset;
            transform.position = Vector3.Lerp(transform.position, wanted + shake, 1f - Mathf.Exp(-followSmoothing * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position + Vector3.up * lookHeight - transform.position), 10f * Time.deltaTime);
            shake = Vector3.Lerp(shake, Vector3.zero, 12f * Time.deltaTime);
        }

        public void Shake(float strength) => shake = Random.insideUnitSphere * strength;
    }
}
