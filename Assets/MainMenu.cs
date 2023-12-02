using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TT
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject fade_out;
        public GameObject controls;

        public Button ResumeBtn;
        public Button ControlsBtn;
        public Button BackBtn;

       
        public GameObject Player;
        public GameObject PauseMenu;
        public InputHandler inputHandler;
        public CameraHandler cameraHandler;
        UIManager uiManager;
        PlayerLocomotionManager playerloco;

        private void Start()
        {
            if(Player != null)
            {
                uiManager = FindObjectOfType<UIManager>();
                ResumeBtn.Select();
                inputHandler.enabled = false;
                cameraHandler.LookspeedReset();
                playerloco = Player.GetComponent<PlayerLocomotionManager>();
                playerloco.movementSpeed = 0;
            }
            
        }
        private void OnEnable()
        {
            if (Player != null)
            {
                uiManager = FindObjectOfType<UIManager>();
                ResumeBtn.Select();
                inputHandler.enabled = false;
                cameraHandler.LookspeedReset();
                playerloco.movementSpeed = 0;
            }
        }
        public void PlayGame()
        {
            StartCoroutine("Play");
        }
        public void QuitGame()
        {
            StartCoroutine("Quit");
        }
        public void ToggleControls()
        {
            if (!controls.activeInHierarchy)
            {
                controls.SetActive(true);
                BackBtn.Select();
            }
            else
            {
                controls.SetActive(false);
                ControlsBtn.Select();
            }
 
        }

        public void ResumeGame()
        {
            inputHandler.enabled = true;
            cameraHandler.LookspeedBack();
            PauseMenu.SetActive(false);
            //Time.timeScale = 1;
            uiManager.hudWindow.SetActive(true);
            playerloco.movementSpeed = 4;
        }

        private IEnumerator Play()
        {
          
            fade_out.SetActive(true);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        private IEnumerator Quit()
        {

            fade_out.SetActive(true);
            yield return new WaitForSeconds(2f);
            Application.Quit();
        }

    }
}
