using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TT
{
    public class DeathPopUpUI : MonoBehaviour
    {
        CanvasGroup canvas;
        public GameObject FadeOut;


        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
        }

        public void DisplayDeathPopUp()
        {
            StartCoroutine(FadeInPopUp());
        }

        IEnumerator FadeInPopUp()
        {
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(true);

            for (float fade = 0.05f; fade < 1; fade = fade + 0.05f)
            {
                canvas.alpha = fade;

                if (fade > 0.9f)
                {
                    StartCoroutine(Death());
                   
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator Death()
        {
            yield return new WaitForSeconds(2f);
            FadeOut.SetActive(true);
            yield return new WaitForSeconds(2f);
            //FOR DEMO
            Debug.Log("dead");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
