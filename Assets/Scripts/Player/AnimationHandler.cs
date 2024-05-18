using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;
    public Combat combat;

    void Update(){
        if(combat.isMoving){
            animator.SetBool("IsMoving", true);
        }else if(!combat.isMoving){
            animator.SetBool("IsMoving", false);
        }
    }
}
