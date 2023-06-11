using _game.Scripts.Managers;
using UnityEngine;

namespace _game.Scripts.UI
{
    // This is needed so it reroutes the call to LevelManager because for some fucking reason Unity's OnClick event forgets the assigned object after switching scenes because its a singleton
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject levelsMenu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject creditsMenu;

        public void StartGame()
        {
            mainMenu.SetActive(false);
            levelsMenu.SetActive(true);
        }

        public void OpenSettings()
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenMain()
        {
            levelsMenu.SetActive(false);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        
        public void LoadLevel(string levelName)
        {
            GameManager.Instance.GetLevelManager.LoadLevel(levelName);
        }
    }
}