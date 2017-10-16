using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ShapeDraw
{
    public class UICreatorController : MonoSingleton<UICreatorController>
    {
        [SerializeField]
        private GameObject _buttonContainer;

        [SerializeField]
        private Button _bttnLoadGame;

        [SerializeField]
        private Button _bttnDraw;

        [SerializeField]
        private Button _bttnSave;

        [SerializeField]
        private Button _bttnLoad;

        [SerializeField]
        private InputField _levelNumInput;

        public static Button ButtonDraw
        { 
            get { return Instance._bttnDraw; }
        }

        public static Button ButtonSave
        {
            get { return Instance._bttnSave; }
        }

        public static Button ButtonLoad
        {
            get { return Instance._bttnLoad; }
        }

        public static string LevelNum
        {
            get { return Instance._levelNumInput.text; }
        }

        private void Start()
        {
            _bttnLoadGame.onClick.AddListener(() => { if (SceneLoadController.HasInstance) SceneLoadController.LoadGameScene(); });
        }

        public static void HideButtons()
        {
            Instance._buttonContainer.SetActive(false);
        }

        public static void ShowButtons()
        {
            Instance._buttonContainer.SetActive(true);
        }
    }
}
