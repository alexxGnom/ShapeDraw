using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ShapeDraw
{
    public class UIMainController : MonoSingleton<UIMainController>
    {

        [SerializeField]
        private Button _bttnStartGame;

        [SerializeField]
        private Button _bttnSettings;

        [SerializeField]
        private DrawAssistantBase _drawAssistant;

        [SerializeField]
        private GameObject _gameInfoContainer;

        [SerializeField]
        private Text _scoreText;

        [SerializeField]
        private Text _timerText;

        [SerializeField]
        private UIWindowBase[] _windows; 

        public DrawAssistantBase CurrentDrawer
        {
            get { return _drawAssistant; }
        }


        private void Start()
        {
            _bttnStartGame.onClick.AddListener(StartGame);
            _bttnSettings.onClick.AddListener(OpenSettings);
            HideGameInfo();
        }

        private void OpenSettings()
        {
            var settingsWindow = GetWindowById(Constants.UI_SETTINGS_WINDOW);
            if (settingsWindow != null)
                settingsWindow.ShowWindow();
        }


        public void StartGame()
        {
            HideButtons();
            ShowGameInfo();
            GameLogic.Instance.StartGame();
        }

        public void StopGame()
        {
            HideGameInfo();
            ShowButtons();
        }


        public static void HideButtons()
        {
            Instance._bttnStartGame.gameObject.SetActive(false);
            Instance._bttnSettings.gameObject.SetActive(false);
        }

        public static void ShowButtons()
        {
            Instance._bttnStartGame.gameObject.SetActive(true);
            Instance._bttnSettings.gameObject.SetActive(true);
        }

        public static void HideGameInfo()
        {
            Instance._gameInfoContainer.SetActive(false);
        }

        public static void ShowGameInfo()
        {
            Instance._gameInfoContainer.SetActive(true);
        }

        public static UIWindowBase GetWindowById(string id)
        {
            foreach (var w in Instance._windows)
            {
                if (w.id == id)
                    return w;
            }
            return null;
        }

        public static void SetScore(int score)
        {
            Instance._scoreText.text = "Score: " + score;
        }

        public static void SetTimer(float time)
        {
            Instance._timerText.text = string.Format("Time: {0:0.0}",time);
        }



    }
}
