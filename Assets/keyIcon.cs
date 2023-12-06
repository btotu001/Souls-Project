using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TT
{
    public class keyIcon : MonoBehaviour
    {
       
        void Start()
        {
            if (SPAWN.instance.hasKey)
            {
                gameObject.GetComponent<Image>().enabled = true;
            }

        }

        public void GotKey()
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
    }
}
