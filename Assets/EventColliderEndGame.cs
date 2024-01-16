using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TT
{
    public class EventColliderEndGame : MonoBehaviour
    {
        public GameObject EndUI;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                EndUI.SetActive(true);
                StartCoroutine("ToMenu");
            }
        }

        private IEnumerator ToMenu()
        {
            yield return new WaitForSeconds(10f);
            SceneManager.LoadScene("Menu");
        }
    }
}
