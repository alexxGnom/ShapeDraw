using UnityEngine;

namespace ShapeDraw
{
    public class UISettingsWindow : UIWindowBase
    {

        public void LoadCreator()
        {
            if (SceneLoadController.HasInstance)
                SceneLoadController.LoadCreatorScene();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
