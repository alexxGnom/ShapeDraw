using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShapeDraw
{
    public class ControlAssistant : MonoSingleton<ControlAssistant>
    {
        [SerializeField]
        private Camera _controlCamera;

        [SerializeField]
        private float _mouseSensitivity = 0.2f;

        private Vector3 _pressPoint;

        private bool _locked = true;

        private Action<Vector3> _onMousePressed;
        private Action<Vector3> _onMouseDown;
        private Action<Vector3> _onMouseUp;


        public static Action<Vector3> OnMousePressed
        {
            set { Instance._onMousePressed = value; }
            get { return Instance._onMousePressed; }
        }

        public static Action<Vector3> OnMouseDown
        {
            set { Instance._onMouseDown = value; }
            get { return Instance._onMouseDown; }
        }

        public static Action<Vector3> OnMouseUp
        {
            set { Instance._onMouseUp = value; }
            get { return Instance._onMouseUp; }
        }


        private void Start()
        {
            if (_controlCamera == null)
                _controlCamera = Camera.main;

            _locked = false;
        }

        private void Update()
        {
            if (_locked || Time.timeScale == 0)
                return;

            _pressPoint = _controlCamera.ScreenToWorldPoint(Input.mousePosition);
            _pressPoint.z = 0f;

            if (Input.GetMouseButtonDown(0) && _onMouseDown != null)
            {
                _onMouseDown(_pressPoint);
            }

            if (Input.GetMouseButton(0) && _onMousePressed != null)
            {
                _onMousePressed(_pressPoint);
            }

            if (Input.GetMouseButtonUp(0) && _onMouseUp != null)
            {
                _onMouseUp(_pressPoint);
            }
        }


        public static bool IsInputLocked()
        {
            return Instance._locked;
        }

        public static void LockInput()
        {
            if (Instance != null)
                Instance._locked = true;
        }

        public static void UnlockInput()
        {
            if (Instance != null)
                Instance._locked = false;
        }

        public static Vector3 GetMousePosition()
        {
            return Instance != null ? Instance._pressPoint : Vector3.zero ;
        }

        public static bool IsMouseMove()
        {
            return Mathf.Abs(Input.GetAxis("Mouse X")) > Instance._mouseSensitivity || Mathf.Abs(Input.GetAxis("Mouse Y")) > Instance._mouseSensitivity;
        }
    }
}
