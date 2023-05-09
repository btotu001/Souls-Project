using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        PlayerStats playerStats;

        [Header("Skin Equipment Model Changers")]
        SkinModelChanger skinModelChanger;

        [Header("Default Naked Models")]
        public string nakedSkinModel;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();    
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerStats = GetComponentInParent<PlayerStats>();

            skinModelChanger = GetComponentInChildren<SkinModelChanger>();

            
        }

        private void Start()
        {
            EquipAllSKinModelsOnStart();
        }


        private void EquipAllSKinModelsOnStart()
        {
            //unequips all skin models
            skinModelChanger.UnEquipAllSkinModels();

            if(playerInventory.currentSkinEquipment != null)
            {
                //equips only current skin 
                skinModelChanger.EquipSkinModelByName(playerInventory.currentSkinEquipment.skinModelName);

                //get dmg absorption from playerstats
                playerStats.physicalDamageAbsorption = playerInventory.currentSkinEquipment.physicalDefense;
            }
            else
            {
                //Equip default skin (Naked)
                skinModelChanger.EquipSkinModelByName(nakedSkinModel);

                //set dmg absorption to 0 if naked
                playerStats.physicalDamageAbsorption = 0;

            }

           
        }

        public void OpenBlockingCollider()
        {
            //if player is two handing the right weapon, that weapon becomes "shield" when blocking
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsoption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsoption(playerInventory.leftWeapon);
            }
           
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
