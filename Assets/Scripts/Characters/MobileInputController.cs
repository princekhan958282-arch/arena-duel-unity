using UnityEngine;

namespace ArenaDuel.Characters
{
    public sealed class MobileInputController : MonoBehaviour
    {
        public static MobileInputController Instance { get; private set; }
        public Vector2 Move { get; set; }
        bool jump, basic, skill1, skill2, ultimate;
        void Awake() => Instance = this;
        void OnDestroy() { if (Instance == this) Instance = null; }

        public void PressJump() => jump = true;
        public void PressBasic() => basic = true;
        public void PressSkill1() => skill1 = true;
        public void PressSkill2() => skill2 = true;
        public void PressUltimate() => ultimate = true;

        public FighterInput Merge(FighterInput input)
        {
            if (Move.sqrMagnitude > input.move.sqrMagnitude) input.move = Move;
            input.jump |= jump; input.basic |= basic; input.skill1 |= skill1; input.skill2 |= skill2; input.ultimate |= ultimate;
            return input;
        }

        public void ConsumeTransientInput() => jump = basic = skill1 = skill2 = ultimate = false;
    }
}
