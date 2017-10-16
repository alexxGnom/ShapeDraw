using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeDraw
{
    public class MouseTrail : MonoBehaviour
    {
        [SerializeField]
        private bool _canShow = false;

        private ParticleSystem _particleSystem;


        public bool CanShow
        {
            set 
            {
                if (!_canShow && !value)
                    HideTrail(Vector3.zero);

                _canShow = value; 
            }
        }


        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystem.Stop();

            ControlAssistant.OnMouseDown += ShowTrail;
            ControlAssistant.OnMouseUp += HideTrail;
        }

        private void ShowTrail(Vector3 mousePos)
        {
            if (!_canShow) return;
            ControlAssistant.OnMousePressed += UpdateTrail;
            _particleSystem.Play();
        }

        private void UpdateTrail(Vector3 mousePos)
        {
            _particleSystem.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }

        private void HideTrail(Vector3 mousePos)
        {
            if(!_canShow) return;

            _particleSystem.Stop();
            ControlAssistant.OnMousePressed -= UpdateTrail;
        }
    }
}
