namespace StatePattern
{
    public abstract class State
    {
        protected int animationHashName;
        protected float stateTime;
        
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
        public virtual void Exit() { }
    }
}
