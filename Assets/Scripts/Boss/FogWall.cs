using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class FogWall : MonoBehaviour
    {
        private void Awake()
        {
            //enable in future when saving or need performance
           // gameObject.SetActive(false);

        }

        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {

        }
    }
}
