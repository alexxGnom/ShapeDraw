using UnityEngine;
using System;
using System.Collections.Generic;

namespace ShapeDraw
{
    public class BasePool { }

    public class Pool<T> : BasePool
    {
        private readonly Stack<T> _objects;
        public readonly Func<T> ObjectGenerator;

        public bool IsEmpty { get { return _objects.Count == 0; } }

        public Pool(Func<T> objGenerator)
        {
            if (objGenerator == null) throw new ArgumentNullException();
            _objects = new Stack<T>();
            ObjectGenerator = objGenerator;
        }

        public virtual void Precache(int count)
        {
            for (int i = 0; i < count; i++)
                PutObject(CreateObject());
        }

        public virtual T GetObject()
        {
            while (_objects.Count > 0)
            {
                var obj = _objects.Pop();
                if (obj == null)
                    continue;
                //Debug.Log(obj.GetType() +" Pool Getted");
                return obj;
            }
            //Debug.Log(" Pool new obj Created");
            return CreateObject();
        }

        public virtual void PutObject(T item)
        {
            _objects.Push(item);
        }

        public virtual T CreateObject()
        {
            return ObjectGenerator();
        }

        public virtual T[] GetObjectReferences()
        {
            return _objects.ToArray();
        }

        public virtual void Clear()
        {
            _objects.Clear();
        }
    }

    public interface IPoolable
    {
        void OnPoolEnter();
        void OnPoolExit();
    }

    public interface IUnityPoolable : IPoolable
    {
        Action DespawnAction { get; set; } //most poolable classes use Despawn() method instead this action
        GameObject gameObject { get; }
        void Despawn();
    }

    public sealed class UnityPool<T> : Pool<T> where T : UnityEngine.Object, IUnityPoolable
    {
        readonly Transform _parent;
        readonly bool _active;

        public UnityPool(Func<T> objGenerator, Transform parent, string name = null, bool active = false) : base(objGenerator)
        {
            if (string.IsNullOrEmpty(name))
                name = string.Format("[{0}]", typeof(T));

            _parent = new GameObject(name).transform;
            _parent.SetParent(parent, false);

            _active = active;
            _parent.gameObject.SetActive(_active);
        }

        public override T GetObject()
        {
            var obj = base.GetObject();

            if (_active)
                obj.gameObject.SetActive(true);

            obj.OnPoolExit();

            return obj;
        }

        public override void PutObject(T item)
        {
            if (_active)
                item.gameObject.SetActive(false);

            item.gameObject.transform.SetParent(_parent);

            base.PutObject(item);

            item.OnPoolEnter();
        }

        public override T CreateObject()
        {
            var obj = base.CreateObject();

            if (_active)
                obj.gameObject.SetActive(false);

            obj.gameObject.transform.SetParent(_parent);
            obj.DespawnAction = () => PutObject(obj);

            return obj;
        }

        public void Cache(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PutObject(CreateObject());
            }
        }

        public override void Clear()
        {
            base.Clear();
            //?Need destroy all childs;
            UnityEngine.Object.Destroy(_parent);
        }
    }
}