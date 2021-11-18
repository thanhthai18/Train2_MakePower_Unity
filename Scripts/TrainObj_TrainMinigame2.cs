using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainObj_TrainMinigame2 : MonoBehaviour
{
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_BellRun, anim_Run;


    private void Start()
    {
        anim.state.Complete += AnimComplete;
        PlayAnim(anim, anim_Run, true);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == anim_BellRun)
        {
            PlayAnim(anim, anim_Run, true);
        }
        if(trackEntry.Animation.Name == anim_Run && GameController_TrainMinigame2.instance.isLose)
        {
            anim.state.TimeScale = 0;
        }
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    public void PlayAnimBell()
    {
        PlayAnim(anim, anim_BellRun, false);
    }
}
