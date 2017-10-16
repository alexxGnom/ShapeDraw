using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ShapeDraw
{
    public class DrawAssistantGame : DrawAssistantBase
    {
        [SerializeField]
        private MouseTrail _mouseTrail;

        private void Start()
        {
            ControlAssistant.OnMouseDown += OnMouseDown;
            ControlAssistant.OnMouseUp += OnMouseUp;
        }


        private void OnMouseDown(Vector3 mousePos)
        {
            if (!_isStarted) return;

            Clear();

            AddPoint(mousePos);

            _isStarted = true;
            ControlAssistant.OnMousePressed += UpdateDraw;
        }

        private void OnMouseUp(Vector3 mousePos)
        {
            if (!_isStarted) return;

            ControlAssistant.OnMousePressed -= UpdateDraw;

            var verticesSize = _vertices.Count;

            for (var i = 0; i < verticesSize; i++)
            {
                var v = _vertices[i];
                if (Vector3.Distance(mousePos, v) < 2f)
                {
                    _vertices[verticesSize - 1] = v;
                    break;
                }
            }

            if (verticesSize > 1)
            {
                UpdateCurrentLine(_vertices[verticesSize - 1]);
                UpdateCurrentPoint(_vertices[verticesSize - 1]);

                _drawedShape = new Shape(_vertices);

                if (OnEndDraw != null)
                    OnEndDraw();

               
            }
            else
            {
                Clear();
            }


            //  Utils.ClearLog();
            //foreach (var v in _vertices)
            //{
            //    Debug.Log(v.ToString());
            //}
        }

        private void UpdateDraw(Vector3 mousePos)
        {
            if (!_isStarted) return;

            var verticesSize = _vertices.Count;
            if (!ControlAssistant.IsMouseMove() && Vector3.Distance(_vertices[verticesSize - 1], mousePos) > 2f)
            {
                AddPoint(mousePos);
                AddLine(_vertices[verticesSize - 1]);
                UpdateCurrentLine(mousePos);
            }
        }


        public override void StartDraw()
        {
            base.StartDraw();

            _mouseTrail.CanShow = true;
        }

        public override void StopDraw()
        {
            base.StopDraw();

            _mouseTrail.CanShow = false;
        }
    }
}
