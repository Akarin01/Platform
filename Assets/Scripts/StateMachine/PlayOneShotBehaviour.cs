using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip clip;
    public float timeToDelay;
    float delayTimer = 0f;
    public bool playDelay;
    public bool playOnEnter, playOnExit;
    bool hasDelayedPlay;
    [Range(0f, 1f)] public float volume;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter && !playDelay)
        {
            AudioSource.PlayClipAtPoint(clip, animator.transform.position, volume);
        }

        hasDelayedPlay = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playDelay && !hasDelayedPlay)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= timeToDelay)
            {
                AudioSource.PlayClipAtPoint(clip, animator.transform.position, volume);
                hasDelayedPlay = true;
                delayTimer = 0f;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(clip, animator.transform.position, volume);
        }
    }

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
