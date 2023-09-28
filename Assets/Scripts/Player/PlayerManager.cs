using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all updating here
namespace TT
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerLocomotionManager playerLocomotion;
        PlayerStatsManager playerStatsManager;
        PlayerAnimatorManager playerAnimatorManager;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;
       

        private void Awake()
        {
            Application.targetFrameRate = 60; //Move to game manager

            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();

            inputHandler = GetComponent<InputHandler>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            animator = GetComponent<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

       
       
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", playerStatsManager.isDead);
            

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerStatsManager.RegenerateStamina();

            
           CheckForInteractableObject();
           
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);
        

        }


        private void LateUpdate()
        {
                //inputs to false in lateupdate so they get only called once per frame
                inputHandler.rollFlag = false;
              
                inputHandler.rb_Input = false;
                inputHandler.rt_Input = false;
                inputHandler.lt_Input = false;
                inputHandler.d_Pad_Up = false;
                inputHandler.d_Pad_Down = false;
                inputHandler.d_Pad_Left = false;
                inputHandler.d_Pad_Right = false;
                inputHandler.a_Input = false;
                inputHandler.jump_Input = false;
                inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);

            }


            if (isInAir)
                {
                    playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
                }

        }

        #region Player Interactions

        //TODO CHange to just ontriggerenter?
        public void CheckForInteractableObject()
        {
        
        RaycastHit hit;
            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
               // Debug.Log("Interact");
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        //Set ui text to the interactable object text
                        //set the text pop up to true
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        //when pressing a, interact with the object
                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }
                if (interactableUIGameObject != null && inputHandler.a_Input)
                {
                    interactableUIGameObject.SetActive(false);
                }
            }


        }
        /*
        void OnTriggerEnterStay(Collider collider)
        {
            if (collider.tag == "Interactable")
            {
                Interactable interactableObject = collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    //Set ui text to the interactable object text
                    //set the text pop up to true

                    //when pressing a, interact with the object
                    if (inputHandler.a_Input)
                    {
                        collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        */

        public void OpenChestInteraction(Transform playerStandPoint)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero; // Stops the player from "ice skating"
            transform.position = playerStandPoint.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);

        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero; // Stops the player from "ice skating"

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            //Rotate over time so it does not look as rigid

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }

        #endregion



    }


}
