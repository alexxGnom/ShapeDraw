using UnityEngine.SceneManagement;
using UnityEngine;

namespace ShapeDraw
{
    public class SceneLoadController : MonoSingleton<SceneLoadController>
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public static void LoadCreatorScene()
        {
            SceneManager.LoadScene(Constants.CREATOR_SCENE_NAME);
        }

        public static void LoadGameScene()
        {
            SceneManager.LoadScene(Constants.GAME_SCENE_NAME);
        }
    }
}
