using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class EventColliderStartBossfight : MonoBehaviour
    {
        WorldEventManager worldEventManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                worldEventManager.ActivateBossFight();
            }
        }
    }
}
