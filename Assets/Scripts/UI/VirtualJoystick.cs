using ArenaDuel.Characters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArenaDuel.UI
{
    public sealed class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform knob;
        RectTransform area;
        float radius;
        void Awake() { area = transform as RectTransform; radius = Mathf.Min(area.rect.width, area.rect.height) * .34f; }
        public void OnPointerDown(PointerEventData e) => OnDrag(e);
        public void OnDrag(PointerEventData e)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(area, e.position, e.pressEventCamera, out Vector2 local)) return;
            Vector2 value = Vector2.ClampMagnitude(local / Mathf.Max(1f, radius), 1f);
            if (knob) knob.anchoredPosition = value * radius;
            if (MobileInputController.Instance) MobileInputController.Instance.Move = value;
        }
        public void OnPointerUp(PointerEventData e)
        {
            if (knob) knob.anchoredPosition = Vector2.zero;
            if (MobileInputController.Instance) MobileInputController.Instance.Move = Vector2.zero;
        }
    }
}
