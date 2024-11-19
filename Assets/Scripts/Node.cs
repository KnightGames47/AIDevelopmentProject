using UnityEngine;

namespace BTree
{
    public abstract class Node : ScriptableObject
    {
        public enum NodeState
        {
            RUNNING,
            FAILURE,
            SUCCESS
        }

        public NodeState state = NodeState.RUNNING;
        public bool started = false;

        public NodeState Evaluate()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnEvaluate();

            if (state == NodeState.FAILURE || state == NodeState.SUCCESS)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnEvaluate();
    }
}
