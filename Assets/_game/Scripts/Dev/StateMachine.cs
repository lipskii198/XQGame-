namespace _game.Scripts.Dev
{
    public abstract class StateMachine
    {
        private StateBase currentStateBase;

        public void SetState(StateBase stateBase)
        {
            currentStateBase?.OnExit();
            currentStateBase = stateBase;
            currentStateBase.OnEnter();
        }

        public StateBase GetState()
        {
            return currentStateBase;
        }

        private void Update()
        {
            currentStateBase?.Update();
        }

        private void FixedUpdate()
        {
            currentStateBase?.FixedUpdate();
        }
    }

    public abstract class StateBase
    {
        protected StateMachine stateMachine;

        protected StateBase(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}