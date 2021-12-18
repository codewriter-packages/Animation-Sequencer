using System;
using UnityEngine;

namespace BrunoMikoski.AnimationSequencer
{
    [Serializable]
    public abstract class GameObjectAnimationStep : AnimationStepBase
    {
        [SerializeField]
        protected GameObject target;
        public GameObject Target => target;

        [SerializeField]
        protected float duration = 1;

        protected GameObjectAnimationStep()
        {
        }

        protected GameObjectAnimationStep(GameObject target)
        {
            this.target = target;
        }

        protected GameObjectAnimationStep(GameObject target, float duration)
        {
            this.target = target;
            this.duration = duration;
        }
        
        protected GameObjectAnimationStep(GameObject target, float duration, float delay) : base(delay)
        {
            this.target = target;
            this.duration = duration;
        }
        public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }
    }
}
