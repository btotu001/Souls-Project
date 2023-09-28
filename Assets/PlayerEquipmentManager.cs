using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;

        [Header("Skin Equipment Model Changers")]
        SkinModelChanger skinModelChanger;

        [Header("Default Naked Models")]
        public string nakedSkinModel;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();    
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

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

            if(playerInventoryManager.currentSkinEquipment != null)
            {
                //equips only current skin 
                skinModelChanger.EquipSkinModelByName(playerInventoryManager.currentSkinEquipment.skinModelName);

                //get dmg absorption from playerstats
                playerStatsManager.physicalDamageAbsorption = playerInventoryManager.currentSkinEquipment.physicalDefense;
            }
            else
            {
                //Equip default skin (Naked)
                skinModelChanger.EquipSkinModelByName(nakedSkinModel);

                //set dmg absorption to 0 if naked
                playerStatsManager.physicalDamageAbsorption = 0;

            }

           
        }

        public void OpenBlockingCollider()
        {
            //if player is two handing the right weapon, that weapon becomes "shield" when blocking
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsoption(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsoption(playerInventoryManager.leftWeapon);
            }
           
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
