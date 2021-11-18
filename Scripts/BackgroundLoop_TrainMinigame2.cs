using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop_TrainMinigame2 : MonoBehaviour
{
    public float speed = 0f;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        //LoopBackGround();
    }

    //void LoopBackGround()
    //{
    //    if (!GameController_TrainMinigame2.instance.isLose)
    //    {
    //        transform.DOMoveX(-28.29f, 7.0725f).SetEase(Ease.Linear).OnComplete(() =>
    //        {
    //            transform.position = startPos;
    //            LoopBackGround();
    //        });
    //    }
        
    //}

    private void Update()
    {
        if (!GameController_TrainMinigame2.instance.isLose && !GameController_TrainMinigame2.instance.isWin)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x < -28.29f)
            {
                transform.position = startPos;
            }
        }
    }
}
