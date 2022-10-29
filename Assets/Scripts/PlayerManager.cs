using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all updating here
namespace TT
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

       
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);
            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
         

           CheckForInteractableObject();
           
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        

        }


        private void LateUpdate()
        {
                //inputs to false in lateupdate so they get only called once per frame
                inputHandler.rollFlag = false;
              
                inputHandler.rb_Input = false;
                inputHandler.rt_Input = false;
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

        //TODO CHange to just ontriggerenter?
        public void CheckForInteractableObject()
        {
        
        RaycastHit hit;
            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                Debug.Log("Interact");
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



    }


}
