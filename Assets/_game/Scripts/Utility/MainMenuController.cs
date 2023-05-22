using _game.Scripts.Managers;
using UnityEngine;

namespace _game.Scripts.Utility
{
    // This is needed so it reroutes the call to LevelManager because for some fucking reason Unity's OnClick event forgets the assigned object after switching scenes because its a singleton
    public class MainMenuController : MonoBehaviour
    {
        public void LoadLevel(string levelName)
        {
            GameManager.Instance.GetLevelManager.LoadLevel(levelName);
        }
    }
}