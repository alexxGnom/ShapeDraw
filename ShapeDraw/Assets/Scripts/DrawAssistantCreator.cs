using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ShapeDraw
{
    public class DrawAssistantCreator : DrawAssistantBase
    {
        [SerializeField]
        private int _maxPoints = 10;

        private void Start()
        {
            
            if (UICreatorController.HasInstance)
            {
                UICreatorController.ButtonDraw.onClick.AddListener(StartDraw);
                UICreatorController.ButtonSave.onClick.AddListener(Save);
                UICreatorController.ButtonLoad.onClick.AddListener(Load);
            }

            ControlAssistant.OnMouseDown += OnMouseDown;
        }

        private void OnMouseDown(Vector3 mousePos)
        {
            if (!_isStarted) return;

            var verticesSize = _vertices.Count;
           
            if (verticesSize >= 1)
            {
                for (var i = 0; i < verticesSize; i++)
                {
                    var v = _vertices[i];
                    if (Vector3.Distance(mousePos, v) < 2f)
                    {
                        if (verticesSize - i > 2)
                            AddPoint(v);

                        StopDraw();
                        return;
                    }
                }
            }

            AddPoint(mousePos);
            AddLine(mousePos);
        }

        private void LateUpdate()
        {
            if (!_isStarted) return;

            UpdateCurrentLine(ControlAssistant.GetMousePosition());
        }

        private void Load()
        {
            var levelNum = UICreatorController.LevelNum;

            if (string.IsNullOrEmpty(levelNum))
            {
                Debug.LogError("BAD INPUT!");
                return;
            }

            var shape = StorageController.LoadShapeFromFile(levelNum);
            DrawShape(shape);
        }

        private void Save()
        {
            var levelNum = UICreatorController.LevelNum;

            if (string.IsNullOrEmpty(levelNum))
            {
                Debug.LogError("BAD INPUT!");
                return;
            }

            if (StorageController.HasInstance)
            {
                StorageController.SaveShapeToFile(levelNum, new Shape(_vertices));
            }
        }

        public override void StartDraw()
        {
            base.StartDraw();

            if (UICreatorController.HasInstance)
                UICreatorController.HideButtons();
           
        }

        public override void StopDraw()
        {
            base.StopDraw();

            var verticesSize = _vertices.Count;

            if (verticesSize > 1)
                UpdateCurrentLine(_vertices[verticesSize - 1]);
            else
                Clear();

            if (UICreatorController.HasInstance)
                UICreatorController.ShowButtons();
        }
    }
}
