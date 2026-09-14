using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaDuel.Characters
{
    public sealed class PlayerInputController : MonoBehaviour, IFighterInputSource
    {
        public FighterInput ReadInput()
        {
            FighterInput result = default;
            Keyboard k = Keyboard.current;
            Gamepad g = Gamepad.current;
            if (k != null)
            {
                result.move = new Vector2((k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0), (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0));
                result.jump = k.spaceKey.wasPressedThisFrame;
                result.basic = k.jKey.wasPressedThisFrame;
                result.skill1 = k.kKey.wasPressedThisFrame;
                result.skill2 = k.lKey.wasPressedThisFrame;
                result.ultimate = k.uKey.wasPressedThisFrame;
            }
            if (g != null)
            {
                result.move = Vector2.ClampMagnitude(result.move + g.leftStick.ReadValue(), 1f);
                result.jump |= g.buttonSouth.wasPressedThisFrame;
                result.basic |= g.buttonWest.wasPressedThisFrame;
                result.skill1 |= g.rightShoulder.wasPressedThisFrame;
                result.skill2 |= g.leftShoulder.wasPressedThisFrame;
                result.ultimate |= g.buttonNorth.wasPressedThisFrame;
            }
            MobileInputController mobile = MobileInputController.Instance;
            if (mobile != null) result = mobile.Merge(result);
            return result;
        }

        public void ConsumeTransientInput() => MobileInputController.Instance?.ConsumeTransientInput();
    }
}
