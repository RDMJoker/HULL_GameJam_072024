using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/SceneLoader", fileName = "NewSceneLoader")]
    public class SceneLoader : ScriptableObject
    {
        public EScenes scene;
        
        public void Load()
        {
            SceneManager.LoadScene((int)scene);
        }
    }
}