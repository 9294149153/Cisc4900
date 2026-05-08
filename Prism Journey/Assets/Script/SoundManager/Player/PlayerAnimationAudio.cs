using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationAudio : MonoBehaviour
{
   
    public void AnimEvent_PlayFootstep()
    {

        PlayerSoundManager.PlaySound(PlayerSoundType.Run, 0.1f);
        
    }

    public void AnimEvent_PlayFootstepDash()
    {
        PlayerSoundManager.PlaySound(PlayerSoundType.Dash, 0.12f);
    }


}
