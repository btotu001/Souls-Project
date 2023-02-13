using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class PlayerAttacker : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerAnimatorManager animatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        LayerMask backStabLayer = 1 << 13; //searches layer 13, wich is the backstab layer in unity
        LayerMask riposteLayer = 1 << 14;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();  
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            //Check if we have stamina, if we do not , return
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {

                animatorHandler.anim.SetBool("canDoCombo", false);


                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if(lastAttack == weapon.TH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            //Check if we have stamina, if we do not , return
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                lastAttack = weapon.TH_Light_Attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            //Check if we have stamina, if we do not , return
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                //todo two handed heavy attack (no animation for that yet)
            }
            else
            {
              
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                lastAttack = weapon.OH_Heavy_Attack_1;
            }
        }

        #region Input Actions
        public void HandleRBAction()
        {
            //if melee weapon equipped
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                //Handle melee action
                PerformRBMeleeAction();
            }
            //if spell caster equipped (magick, faith, pyro)
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                //Handle spell action
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
           
           
        }

        public void HandleLBAction()
        {
            PerformLBBlockAction();
        }

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShieldWeapon)
            {
                //perform shield weapon art
                PerformLTWeaponArt(inputHandler.twoHandFlag);

            }
            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                //do a light attack
            }
        }

        #endregion


        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                //check to not do animations when interacting or can do combo
                if (playerManager.isInteracting)
                {
                    return;
                }
                if (playerManager.canDoCombo)
                {
                    return;
                }

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

       

        //from weapon, we can check if its spell, faith or magic caster
        private void PerformRBMagicAction(WeaponItem weapon)
        {
            //to avoid spell spam
            if (playerManager.isInteracting)
                return;

            if (weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    //CHeck for fp
                    if(playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        //attempt to cast spell
                        playerInventory.currentSpell.AttempToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Shrugging", true);
                    }
                }
            }

            else if (weapon.isPyroCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    //CHeck for fp
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        //attempt to cast spell
                        playerInventory.currentSpell.AttempToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                    else
                    {     
                        animatorHandler.PlayTargetAnimation("Shrugging", true);
                    }
                }
            }

        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler,playerStats, cameraHandler, weaponSlotManager);
            animatorHandler.anim.SetBool("isFiringSpell", true);
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

          
            if (isTwoHanding)
            {
                //if we are two handing perform weapon art for right weapon
               
            }
            else
            {
                //else perform weapon art for left weapon
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);

            }
        }

        #endregion

        #region Defense Actions
        private void PerformLBBlockAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            animatorHandler.PlayTargetAnimation("BlockStart", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true; //goes to false only after letting off LB

        }
        #endregion

        public void AttemptBackStabOrRiposte()
        {
            //Check if we have stamina, if we do not , return
            if (playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;

            //if raycast finds backstab
            if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider; //

                if(enemyCharacterManager != null)
                {
                    //CHeck for "team" i.d (so you cant back stab friends or urself)
                    //Pull into a transform behind the enemy so the backstab looks clean
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;

                    //rotate us towards that transform
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                     playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    //play animation
                    animatorHandler.PlayTargetAnimation("BackStab", true);
                    //make enemy play animation
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("BackStabbed", true);
                    //do damage (animation event)
                }
            }
            //if raycast finds riposte
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                //(For multiplayer) check team I.D
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("RiposteStab", true);
                    Debug.Log("RIPOSTED");
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("RiposteStabbed", true);
                }

                
            }

        }
    }

}
