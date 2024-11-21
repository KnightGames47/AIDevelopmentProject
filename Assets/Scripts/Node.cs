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
        [HideInInspector] public BlackBoard blackboard;
        [HideInInspector] public AiAgent agent; // This has the information about the AI agent that we want to track
        //The information here is information that the whole tree will be able to see and change
        [TextArea] public string description;

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
