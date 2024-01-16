using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class ElevatorInteractable : Interactable
    {
        [Header("Interactable Collider")]
        [SerializeField] Collider interactableCollider;

        [Header("Destination")]
        [SerializeField] Vector3 destinationHigh; //Highest point elevator will travel
        [SerializeField] Vector3 destinationLow; //lowest point elevator will travel
        [SerializeField] bool isTraveling = false;
        [SerializeField] bool buttonIsReleased = true;

        [Header("Animator")]
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] string buttonPressAnimation = "Elevator_button_Press";
        [SerializeField] List<CharacterManager> charactersOnButton; //list for characters if multiplayer

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();
            if(character != null)
            {
                //add to list
                AddCharacterToList(character);

                if(!isTraveling && buttonIsReleased)
                {
                    ActivateElevator();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();
            if (character != null)
            {
                StartCoroutine(ReleaseButton(character));
            }
        }

        private void ActivateElevator()
        {
            elevatorAnimator.SetBool("isPressed", true);
            buttonIsReleased = false;
            elevatorAnimator.Play(buttonPressAnimation);

            if(transform.position == destinationHigh)
            {
                StartCoroutine(MoveElevator(destinationLow, 3));
            }
            if (transform.position == destinationLow)
            {
                StartCoroutine(MoveElevator(destinationHigh, 3));
            }

        }

        private IEnumerator MoveElevator(Vector3 finalPosition, float duration)
        {
            isTraveling = true;

            if (duration > 0)
            {
                float startTime = Time.time;
                float endTime = startTime + duration;

                Vector3 initialPosition = transform.position;

                while (Time.time < endTime)
                {
                    float t = (Time.time - startTime) / duration;
                    transform.position = Vector3.Lerp(initialPosition, finalPosition, t);
                    Vector3 movementVelocity = (finalPosition - initialPosition) / duration;

                    Vector3 characterMovementVelocity = new Vector3(0, movementVelocity.y, 0);

                    foreach (var character in charactersOnButton)
                    {
                        Rigidbody characterRigidbody = character.GetComponent<Rigidbody>();
                        if (characterRigidbody != null)
                        {
                            characterRigidbody.MovePosition(characterRigidbody.position + characterMovementVelocity * Time.deltaTime);
                        }
                        else
                        {
                            character.transform.position += characterMovementVelocity * Time.deltaTime;
                        }
                    }

                    yield return null;
                }

                transform.position = finalPosition;
                isTraveling = false;
            }
        }

        public void ActivateElevatorOnly()
        {
            elevatorAnimator.SetBool("isPressed", true);
            buttonIsReleased = false;
            elevatorAnimator.Play(buttonPressAnimation);

            if (transform.position == destinationHigh)
            {
                StartCoroutine(MoveElevatorOnly(destinationLow, 3));
            }
            if (transform.position == destinationLow)
            {
                StartCoroutine(MoveElevatorOnly(destinationHigh, 3));
            }

        }
        private IEnumerator MoveElevatorOnly(Vector3 finalPosition, float duration)
        {
            isTraveling = true;

            if (duration > 0)
            {
                float startTime = Time.time;
                float endTime = startTime + duration;

                Vector3 initialPosition = transform.position;

                while (Time.time < endTime)
                {
                    float t = (Time.time - startTime) / duration;
                    transform.position = Vector3.Lerp(initialPosition, finalPosition, t);
                   

                    yield return null;
                }

                transform.position = finalPosition;
                isTraveling = false;
            }
        }

        private IEnumerator ReleaseButton(CharacterManager character)
        {
            while (isTraveling)
                yield return null;

            yield return new WaitForSeconds(2);

            elevatorAnimator.SetBool("isPressed", false);
            buttonIsReleased = true;


        }

        //multiplayer update
        private void AddCharacterToList(CharacterManager character)
        {
            if (charactersOnButton.Contains(character))
            {
                return;
            }
            charactersOnButton.Add(character);
        }

        private void RemoveCharacterToList(CharacterManager character)
        {
            charactersOnButton.Remove(character);

            //Remocve null entry
            for (int i = charactersOnButton.Count - 1; i > -1; i--)
            {
                if(charactersOnButton[i] == null)
                {
                    charactersOnButton.RemoveAt(i);
                }
            }
        }
    }
}
