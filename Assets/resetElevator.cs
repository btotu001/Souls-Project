using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class resetElevator : MonoBehaviour
    {
       public  ElevatorInteractable elevator;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();
            if (character != null)
            {
                elevator.ActivateElevatorOnly();
            }
        }
    }
}
