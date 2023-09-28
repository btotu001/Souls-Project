using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator animator;
        public bool canRotate;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate); //disable rotation when doing animation
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true); //disable rotation when doing animation
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);

        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {

        }
    }
}
