using System;
using UnityEngine;

namespace ShapeDraw
{
    public class ShapePart : MonoBehaviour, IUnityPoolable
    {
        public Action DespawnAction { get; set; }

        public ShapePart Spawn(Transform parent, Vector3 pos)
        {
            transform.SetParent(parent, false);
            transform.position = pos;

            return this;
        }

        public void Despawn()
        {
            if (DespawnAction != null)
                DespawnAction();
            else
                Destroy(gameObject);
        }

        public void OnPoolEnter(){ }

        public void OnPoolExit() { }
    }
}
