using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base class for all items
namespace TT {
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;

    }
}
