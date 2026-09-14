using UnityEngine;

namespace ArenaDuel.Characters
{
    public struct FighterInput
    {
        public Vector2 move;
        public bool jump;
        public bool basic;
        public bool skill1;
        public bool skill2;
        public bool ultimate;
    }

    public interface IFighterInputSource
    {
        FighterInput ReadInput();
        void ConsumeTransientInput();
    }
}
