using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShapeDraw
{
    public class UIEndGameWindow : UIWindowBase
    {

        [SerializeField]
        private Text _description;

        public override void ShowWindow()
        {
            base.ShowWindow();

            _description.text = string.Format("You scored {0} points", GameLogic.Instance.Score);
        }

        public void ToMenu()
        {
            CloseWindow();

            UIMainController.ShowButtons();
        }

        public void StartAgain()
        {
            CloseWindow();
            UIMainController.Instance.StartGame();
        }
    }
}
