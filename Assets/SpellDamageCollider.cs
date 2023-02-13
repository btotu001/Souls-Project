using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterStats spellTarget;
        Rigidbody rigidBody;

        Vector3 impactNormal; //Used to rotate the impact particles


        private void Awake()
        {
            //bugfix for collisionenter?
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            //Do projectile particles
            if(projectileParticles != null)
            {
                projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
                projectileParticles.transform.parent = transform;
            }

            if (muzzleParticles)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f); //<- timer how long these particles last
            }

           
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!hasCollided)
            {
                spellTarget = other.transform.GetComponent<CharacterStats>(); //character that collides with this

                if(spellTarget != null)
                {
                    spellTarget.TakeDamage(currentWeaponDamage);
                }
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 5f);
                Destroy(gameObject);
            }
        }
    }
}
