using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class SkinModelChanger : MonoBehaviour
    {
        public List<GameObject> skinModels;

        private void Awake()
        {
            GetAllSkinModels();
        }

        private void GetAllSkinModels()
        {
            int childrenGameObjects = transform.childCount;

            //add each child to the list
            for(int i = 0; i < childrenGameObjects; i++)
            {
                skinModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllSkinModels()
        {
            foreach(GameObject skinModel in skinModels)
            {
                skinModel.SetActive(false);
            }
        }

        public void EquipSkinModelByName(string skinName)
        {
            for(int i  = 0; i < skinModels.Count; i++)
            {
                if (skinModels[i].name == skinName)
                {
                    skinModels[i].SetActive(true);
                }
            }
        }
    }
}
