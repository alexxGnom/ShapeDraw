using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ShapeDraw
{
    public class DrawAssistantBase : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _pointPrefab;

        [SerializeField]
        protected GameObject _linePrefab;

        [SerializeField]
        protected Transform _container;


        protected bool _isStarted = false;

        protected List<Vector3> _vertices = new List<Vector3>();

        protected Shape _drawedShape;

        protected Transform _currentLine;

        protected Transform _currentPoint;


        public Action OnEndDraw;

        public Shape DrawedShape
        {
            get { return _drawedShape; }
        }


        protected virtual void AddPoint(Vector3 pos)
        {
            //var point = Instantiate(_pointPrefab, pos, Quaternion.identity, _container) as GameObject;
            var point = PoolManager.Instance.PointsPool.GetObject().Spawn(_container, pos);

            _vertices.Add(pos);

            _currentPoint = point.transform;
        }

        protected virtual void AddLine(Vector3 pos, Color color)
        {
            //var line = Instantiate(_linePrefab, pos, Quaternion.identity, _container) as GameObject;

            var line = PoolManager.Instance.LinesPool.GetObject().Spawn(_container, pos);

            _currentLine = line.transform;

            line.GetComponent<SpriteRenderer>().color = color;
        }

        protected virtual void AddLine(Vector3 pos)
        {
            AddLine(pos, Color.green);
        }

        protected virtual void UpdateCurrentLine(Vector3 pos)
        {
            if (_currentLine == null) return;

            var ab = pos - _currentLine.position;

            var rotationZ = (-Mathf.Atan2(ab.x, ab.y) * Mathf.Rad2Deg) + 90f;

            _currentLine.localEulerAngles = new Vector3(0, 0, rotationZ);

            var scaleX = Vector3.Distance(_currentLine.position, pos) * 3.1f;
            _currentLine.localScale = new Vector3(scaleX, 1, 1);
        }

        protected virtual void UpdateCurrentPoint(Vector3 pos)
        {
            if (_currentPoint == null) return;

            _currentPoint.localPosition = pos;
        }


        public virtual void StartDraw()
        {
            Clear();
            _isStarted = true;
        }

        public virtual void StopDraw()
        {
            _isStarted = false;
        }

        public void DrawShape(Shape shape, Color color, bool needClear = true)
        {
            if (shape == null) return;
            if (needClear)
                Clear();

            var vertices = shape.vertices;
            if (vertices != null && vertices.Count > 1)
            {
                for (var i = 0; i < vertices.Count - 1; i++)
                {
                    var pos = vertices[i];
                    AddPoint(pos);
                    AddLine(pos, color);
                    UpdateCurrentLine(vertices[i + 1]);
                }
                AddPoint(vertices[vertices.Count - 1]);
            }
        }

        public void DrawShape(Shape shape, bool needClear = true)
        {
            DrawShape(shape, Color.green, needClear);
        }

        public virtual void Clear()
        {
            var parts = _container.GetComponentsInChildren<ShapePart>();
            foreach (var p in parts)
            {
                // Destroy(p.gameObject);
                p.Despawn();
            }

            _vertices = new List<Vector3>();
            _drawedShape = null;
            
        }

       
    }
}
