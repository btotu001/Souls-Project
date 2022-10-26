using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        //Called when player interacts with something
        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("Interacted with object.");
        }
    }
}
