using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarOnField : MonoBehaviour
{
    public static GameObject starObject;
    private Animator StarAnimator;

    private void Start(){
        Character.Red.MoveEvent.AddListener(GetStar);
        StarAnimator = GetComponent<Animator>();
        starObject = this.gameObject;
    }

    public void GetStar(){
        if(Character.Blue.transform.position == this.transform.position){
            StarAnimator.SetBool("_isDisappearing", true);
            Invoke("FinallyDisappear",1.0f);
        }
    }

    public void FinallyDisappear(){
        this.gameObject.SetActive(false);
    }
}
