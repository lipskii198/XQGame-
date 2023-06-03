using UnityEngine;

namespace _game.Scripts.Dev
{
    public class StateMachine : MonoBehaviour
    {
        private StateBase currentStateBase;

        public void SetState(StateBase stateBase)
        {
            Debug.Log($"[StateMachine] SetState: {stateBase.GetType().Name}");
            currentStateBase?.OnExit();
            currentStateBase = stateBase;
            currentStateBase.OnEnter();
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
        protected StateMachine stateMachine;

        protected StateBase(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Tick();
        public abstract void FixedTick();
    }
}