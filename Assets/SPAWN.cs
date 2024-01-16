using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace TT
{
    public class SPAWN : MonoBehaviour
    {
        public static SPAWN instance;

        //public Transform startingPos;
        //public Transform cpPos;

        public GameObject Player;
        public bool cpActivated = false;
   
        public bool hasKey = false;
        public bool usedKey = false;

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


        private void Update()
        {
           
        }


        public void MovePlayer()
        {
            if (cpActivated == false)
            {
                //Player.gameObject.transform.position = new Vector3(12f, 1.2f, -22.5f);
            }
            else
            {
                Player.gameObject.transform.position = new Vector3(60.2f, 1.2f, -23.9f);
            }

          
        }
    }
}
