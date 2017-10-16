using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeDraw
{
    public class GameLogic : MonoSingleton<GameLogic>
    {
        [SerializeField]
        private int _currentLevel = 1;

        [SerializeField]
        private int _levelStartTime = 30;

        [SerializeField] 
        private int _timePerLevelDelta = 3;

        private DrawAssistantBase _drawAssistant;

        private UIWindowBase _previewWindow;

        private Shape _taskShape;

        private int _score = 0;

        private Coroutine _timerLeftRoutine;

        private float _levelTime;

        public int Score
        {
            get { return _score; }
        }


        private void Start()
        {
            _drawAssistant = UIMainController.Instance.CurrentDrawer;
            UIMainController.SetScore(_score);

            _drawAssistant.OnEndDraw += CheckCompleteLevel;

            _previewWindow = UIMainController.GetWindowById(Constants.UI_PREVIEW_WINDOW);
        }

        private void CheckCompleteLevel()
        {
            StartCoroutine(CheckCompleteRoutine());
        }

        private IEnumerator CheckCompleteRoutine()
        {
            _drawAssistant.StopDraw();

            var drShape = _drawAssistant.DrawedShape;
            yield return null;
            Shape drawedShape;
            Shape templ;

            if (drShape.vertices.Count == 2)
                drawedShape = Utils.ShapeToLine(drShape);
            else
                drawedShape = Utils.ShapeToPoligon(drShape);

            if (_taskShape.vertices.Count == 2)
                templ = Utils.ShapeToLine(_taskShape);
            else
                templ = Utils.ShapeToPoligon(_taskShape);


            if (drawedShape.Equals(templ))
                StartCoroutine(CompleteRoutine());
            else
                _drawAssistant.StartDraw();
        }

        private IEnumerator CompleteRoutine()
        {
            if (_timerLeftRoutine != null)
                StopCoroutine(_timerLeftRoutine);
            _currentLevel++;
            _score++;
            _levelTime -= _timePerLevelDelta;
            yield return new WaitForSeconds(0.5f);
            UIMainController.SetScore(_score);
            yield return StartCoroutine(StartCurrentLevel());
        }


        public IEnumerator StartCurrentLevel()
        {
            _taskShape = StorageController.LoadShapeFromFile(_currentLevel.ToString());

            _previewWindow.ShowWindow();
            while (_previewWindow.currentState != UIWindowBase.State.rolledUp) yield return null;
            _drawAssistant.StartDraw();

            
            _timerLeftRoutine = StartCoroutine(TimeLeft(_levelTime));
        }

        public IEnumerator TimeLeft(float timeLeft = 30.0f)
        {
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;

                UIMainController.SetTimer(timeLeft);
                yield return null;
            }

            StopGame();
        }

        public void StartGame()
        {
            _score = 0;
            _currentLevel = 1;
            _levelTime = _levelStartTime;

            UIMainController.SetScore(_score);

            StartCoroutine(StartCurrentLevel());
        }

        public void StopGame()
        {
            _drawAssistant.StopDraw();
            _drawAssistant.Clear();
            UIMainController.HideGameInfo();

            _previewWindow.CloseWindow();

            var endWindow = UIMainController.GetWindowById(Constants.UI_END_GAME_WINDOW);
            if (endWindow != null)
                endWindow.ShowWindow();
        }

        public string GetCurrentLevel()
        {
            return _currentLevel.ToString();
        }

        public Shape GetTaskShape()
        {
            return _taskShape;
        }

    }
}
