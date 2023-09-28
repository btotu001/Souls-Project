using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        PlayerWeaponSlotManager playerWeaponSlotManager;

        [Header("Quick Slots")]
        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumable;

        [Header("Current Equipment/Skin")]
        public TorsoEquipment currentSkinEquipment;


        public WeaponItem[] weaponsInRightHandSlots = new  WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        //-1 = unarmed
        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        public List<WeaponItem> weaponsInventory;

        private void Awake()
        {
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        private void Start()
        {
            /* OLD
            //set weapons before loading
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            */
            rightWeapon = weaponsInRightHandSlots[0];
            leftWeapon = weaponsInLeftHandSlots[0];
            playerWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            playerWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            //if weapon at index 0 is not null, load it
            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            //if there is no weapon in that slot, check next index
            else if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            //equip unarmed
            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = playerWeaponSlotManager.unarmedWeapon;
                playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, false);
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            //if weapon at index 0 is not null, load it
            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            //if there is no weapon in that slot, check next index
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            //equip unarmed
            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = playerWeaponSlotManager.unarmedWeapon;
                playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, true);
            }
        }
    }

}
