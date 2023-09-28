using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TT
{
    /// <summary>
    /// This script in parent object of health bar so we can disable the slider and still access it
    /// </summary>
    public class UIBossHealthBar : MonoBehaviour
    {
        public TextMeshProUGUI bossName;
        Slider slider;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void SetUIHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
        }

        public void SetUIHealthBarToInactive()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
        public void SetBossCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
        
    }
}
