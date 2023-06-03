using UnityEngine;

namespace _game.Scripts.Utility
{
    public class StateMachine : MonoBehaviour
    {
        private StateBase currentStateBase;

        public void SetState(StateBase stateBase)
        {
            Debug.Log($"[StateMachine] SetState: {stateBase.GetType().Name}");
            currentStateBase?.OnExit();
            if (currentStateBase != null)
                Debug.Log($"[StateMachine] SetState: {currentStateBase?.GetType().Name} exited");
            currentStateBase = stateBase;
            currentStateBase.OnEnter();
            Debug.Log($"[StateMachine] SetState: {stateBase.GetType().Name} entered");
        }

        public StateBase GetState()
        {
            return currentStateBase;
        }

        public void Tick()
        {
            currentStateBase?.Tick();
        }

        public void FixedTick()
        {
            currentStateBase?.FixedTick();
        }
    }

    public abstract class StateBase
    {
        private string name;
        protected readonly StateMachine stateMachine;

        protected StateBase(StateMachine stateMachine, string name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
            
            Debug.Log($"[{GetType().Name}] State initialized");
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Tick();
        public abstract void FixedTick();
    }
}