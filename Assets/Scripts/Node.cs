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

        [HideInInspector] public NodeState state = NodeState.RUNNING;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

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

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnEvaluate();
    }
}
