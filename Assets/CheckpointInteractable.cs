using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class CheckpointInteractable : Interactable
    {
        //Location of grave (for teleporting)

        [Header("Grave Teleport Transform")]
        public Transform graveTeleportTransform;

        [Header("Activation Status")]
        public bool hasBeenActivated;

        //Bonfire unique ID (for saving wich cps you have activated)

        [Header("Grave Effects")]
        public ParticleSystem activationFx;
        public ParticleSystem fireFx;
        public AudioClip graveActivationSFX;

        AudioSource audioSource;

        private void Awake()
        {
            //todo saving?
            //if the bonfire has been already activated by the player, play fx when loading in
            if (hasBeenActivated || SPAWN.instance.cpActivated)
            {
                fireFx.gameObject.SetActive(true);
                fireFx.Play();
                interactableText = "Rest";
            }
            else
            {
                interactableText = "Activate Grave";
            }

            //get audio source
            //audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            Debug.Log("Bonfire");

            if (hasBeenActivated)
            {
                //open teleport menu
            }
            else
            {
                //activate cp
                playerManager.playerAnimatorManager.PlayTargetAnimation("Grave", true);
                playerManager.uIManager.ActivateGravePopUp();
                hasBeenActivated = true;
                interactableText = "Rest";
                activationFx.gameObject.SetActive(true);
                activationFx.Play();
                fireFx.gameObject.SetActive(true);
                fireFx.Play();
                //play audio
                //audioSource.PlayOneShot();
                SPAWN.instance.cpActivated = true; //for demo spawn
            }
        }
    }
}
