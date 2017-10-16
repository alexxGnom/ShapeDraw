using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ShapeDraw
{
    public class UIPreviewWindow : UIWindowBase
    {

        [SerializeField]
        private Image _previewImage;

        [SerializeField]
        private int _resWidth = 1920;

        [SerializeField]
        private int _resHeight = 1080;

        [SerializeField]
        private Animator _animator;

        private Camera _camera;

        private Texture2D _screenShot;

        private DrawAssistantBase _drawAssistant;

        private int _imgWidth;

        private int _imgHeight;


        protected override void Awake()
        {
            base.Awake();

            _camera = Camera.main;

            _resWidth = Screen.width;
            _resHeight = Screen.height;

            _imgWidth = (int)_previewImage.rectTransform.sizeDelta.x;
            _imgHeight = (int)_previewImage.rectTransform.sizeDelta.y;

            _drawAssistant = UIMainController.Instance.CurrentDrawer;
        }

        public override void ShowWindow()
        {
            base.ShowWindow();

            StartCoroutine(PreviewCurrentLevel());
        }

        public override void CloseWindow()
        {
            base.CloseWindow();
            _previewImage.color = new Color(1, 1, 1, 0);
        }

        public override void RollUpWindow()
        {
            _animator.SetTrigger("rollUp");
        }

        private IEnumerator GetScreenShot()
        {
            yield return new WaitForEndOfFrame();

            int textureH = (_resWidth * _imgHeight) / _imgWidth;
            RenderTexture rt = new RenderTexture(_resWidth, textureH, 24);
            _camera.targetTexture = rt;
            _screenShot = new Texture2D(_resWidth, textureH, TextureFormat.RGB24, false);
            _camera.Render();
            RenderTexture.active = rt;
            _screenShot.ReadPixels(new Rect(0, 0, _resWidth, textureH), 0, 0);
            _screenShot.Apply();
            _camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            Sprite tempSprite = Sprite.Create(_screenShot, new Rect(0, 0, _screenShot.width, _screenShot.height), new Vector2(0, 0));
            _previewImage.sprite = tempSprite;
            _previewImage.color = new Color(1,1,1,1);

            yield return new WaitForEndOfFrame();
        }

        private IEnumerator PreviewCurrentLevel()
        {
            Polygon poly = new Polygon( GameLogic.Instance.GetTaskShape().vertices);
            if (poly != null)
            {
                _drawAssistant.DrawShape(poly);
            }
            yield return null;

            yield return StartCoroutine(GetScreenShot());

            _drawAssistant.Clear();

            RollUpWindow();

            yield return new WaitForSeconds(0.5f);
            currentState = State.rolledUp;
        }

    }
}
