using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TT
{
    //Separate script for enemy HPBAR because (for example) need to deactivate it and fade away
    public class UIEnemyHealthBar : MonoBehaviour
    {
        public Slider slider;
        float timeUntilBarIsHidden = 0;
        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetCurrentHealth(int health)
        {
            slider.value = health;
            timeUntilBarIsHidden = 3;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

            if(slider != null) //dont update if slider is deactivated
            {
                if (timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);
                    }
                }

                //if enemy dies, destroy this slider and script
                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }
           
        }
        private void LateUpdate()
        {
            //makes hpbar look at camera
            transform.LookAt(Camera.main.transform);
        }

    }


}
