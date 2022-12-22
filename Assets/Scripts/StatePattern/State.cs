using UnityEngine;

namespace StatePattern
{
    public abstract class State
    {
        protected int animationHashName;
        protected float stateTime;
        private float _animationDuration;

        protected float AnimationDuration
        {
            set => _animationDuration = value;
            get
            {
                if (_animationDuration > 0f) return _animationDuration;
                Debug.LogWarning("Animation not found. Returning default duration (0.5)");
                return 0.5f;

            }
        }

        protected State() { }

        protected State(string animationName) => animationHashName = Animator.StringToHash(animationName);

        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Exit() { }
    }
}
