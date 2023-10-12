using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TT
{
    public class SPAWN : MonoBehaviour
    {
        public static SPAWN instance;

        //public Transform startingPos;
        //public Transform cpPos;

        public GameObject Player;
        public bool cpActivated = false;

        private bool created;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (instance == null)
            {
                instance = this;

            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {

            MovePlayer();
           
        }

        public void MovePlayer()
        {
            if (cpActivated == false)
            {
                Player.gameObject.transform.position = new Vector3(12f, 1.2f, -22.5f);
            }
            else
            {
                Player.gameObject.transform.position = new Vector3(12f, 1.2f, -18.5f);
            }
        }
    }
}
