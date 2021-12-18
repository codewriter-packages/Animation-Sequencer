using System;
using UnityEngine;

namespace BrunoMikoski.AnimationSequencer
{
    [Serializable]
    public sealed class AnchoredPositionMoveToPositionDOTweenActionBase : AnchoredPositionMoveDOTweenActionBase
    {
        [SerializeField]
        private Vector2 position;
        public override string DisplayName => "Move To Anchored Position";
        
        public AnchoredPositionMoveToPositionDOTweenActionBase()
        {
        }

        public AnchoredPositionMoveToPositionDOTweenActionBase(Vector2 position, bool isRelative) : base(isRelative)
        {
            this.position = position;
            
        }

        protected override Vector2 GetPosition()
        {
            return position;
        }
    }
}
