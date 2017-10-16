using UnityEngine;

namespace ShapeDraw
{
    public class UIWindowBase : MonoBehaviour
    {
        public string id;

        public enum State { open, close, rolledUp}

        public State currentState = State.close;

        protected virtual void Awake()
        {
            CloseWindow();
        }

        public virtual void ShowWindow()
        {
            currentState = State.open;
            gameObject.SetActive(true);
        }

        public virtual void CloseWindow()
        {
            currentState = State.close;
            gameObject.SetActive(false);
        }

        public virtual void RollUpWindow()
        {
            currentState = State.rolledUp;
            gameObject.SetActive(false);
        }

    }
}
