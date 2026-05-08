using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{

    [SerializeField] private PlayerSoundType sound;
    [SerializeField, Range(0,1)] private float volume = 1f;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSoundManager.PlaySound(sound, volume);
    }

}
