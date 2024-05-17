using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;
    public Combat shooting;

    void Update(){
        if(shooting.isMoving){
            animator.SetBool("IsMoving", true);
        }else if(!shooting.isMoving){
            animator.SetBool("IsMoving", false);
        }
    }
}
