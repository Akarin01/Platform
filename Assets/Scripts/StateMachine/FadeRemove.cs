using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemove : StateMachineBehaviour
{
    public float fadeDelayTime;
    public float fadeTime;

    float timerDelay;
    float timerFade;

    Color startColor;
    SpriteRenderer sr;
    GameObject gameObject;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sr = animator.GetComponent<SpriteRenderer>();
        startColor = sr.color;
        gameObject = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timerDelay += Time.deltaTime;
        if (timerDelay > fadeDelayTime)
        {
            timerFade += Time.deltaTime;
            if (timerFade > fadeTime)
            {
                Destroy(gameObject);
            }

            float newAlpha = startColor.a * (1f - timerFade / fadeTime);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        }

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
