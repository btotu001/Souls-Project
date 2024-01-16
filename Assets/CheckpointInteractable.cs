using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        //demo 
        public bool isdoor = false;
        public keyIcon keyIcon;


        //in awake
        void Start()
        {
            //todo saving?
            //if the bonfire has been already activated by the player, play fx when loading in
            if (hasBeenActivated || SPAWN.instance.cpActivated && !isdoor)
            {
                fireFx.gameObject.SetActive(true);
                fireFx.Play();
                interactableText = "Active";
            }
            else
            {
              
                interactableText = "Activate Grave";
            }

            if (isdoor)
            {
                interactableText = "Locked";
            }
            if (SPAWN.instance.usedKey && isdoor)
                Destroy(gameObject);

            //get audio source
            //audioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
            if (SPAWN.instance.hasKey == true && isdoor)
                interactableText = "Use key";
        }
        public override void Interact(PlayerManager playerManager)
        {
            if (isdoor)
            {
                if (SPAWN.instance.hasKey == false)
                    return;

                playerManager.playerAnimatorManager.PlayTargetAnimation("Grave", true);
                keyIcon.GetComponent<Image>().enabled = false;
                interactableText = "Open";
                SPAWN.instance.usedKey = true;
                activationFx.Play();
                Destroy(this.gameObject, 2.3f);
            }
            else
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
                    interactableText = "Active";
                    activationFx.gameObject.SetActive(true);
                    activationFx.Play();
                    fireFx.gameObject.SetActive(true);
                    fireFx.Play();
                    //play audio
                    //audioSource.PlayOneShot();
                    SPAWN.instance.cpActivated = true; //for demo spawn

                    PlayerInventoryManager playerInventoryManager;
                    playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();
                    playerInventoryManager.currentConsumable.currentItemAmount = playerInventoryManager.currentConsumable.maxItemAmount;
                    UIManager uimanager;
                    uimanager = FindObjectOfType<UIManager>();
                    uimanager.flask_text.text = playerInventoryManager.currentConsumable.currentItemAmount.ToString();

                }
            }
            
        }

       
    }
}
