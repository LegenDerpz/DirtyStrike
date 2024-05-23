using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public Animator anim;

    void Update(){
        if(anim.GetBool("IsMoving")){
            PlayFootsteps();
        }else{
            StopFootsteps();
        }
    }

    void PlayFootsteps(){
        if(gameObject.CompareTag("Purifier")){
            StartCoroutine(FindObjectOfType<AudioManager>().PlayCoroutine("Footsteps_P"));
        }else if(gameObject.CompareTag("TerroDirt")){
            StartCoroutine(FindObjectOfType<AudioManager>().PlayCoroutine("Footsteps_T"));
        }
    }

    void StopFootsteps(){
        if(gameObject.CompareTag("Purifier")){
            FindObjectOfType<AudioManager>().Stop("Footsteps_P");
        }else if(gameObject.CompareTag("TerroDirt")){
            FindObjectOfType<AudioManager>().Stop("Footsteps_T");
        }
    }
    
}
