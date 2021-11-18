using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TrainMinigame2 : MonoBehaviour
{
    public float duration;


    private void Start()
    {
        if(GameController_TrainMinigame2.instance.background.speed > 4 && GameController_TrainMinigame2.instance.background.speed < 5)
        {
            duration = 3;
        }
        else if(GameController_TrainMinigame2.instance.background.speed >= 5)
        {
            duration = 2;
        }
        else if(GameController_TrainMinigame2.instance.background.speed <= 4)
        {
            duration = 4;
        }
        transform.DOMoveX(GameController_TrainMinigame2.instance.trainObj.transform.position.x, duration).SetEase(Ease.Linear);
    }

    public void RunAway()
    {
        transform.DOLocalMove(new Vector2(12.87f, 7.29f), 1).SetEase(Ease.Linear).OnComplete(() => 
        {
            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController_TrainMinigame2.instance.Lose();
            transform.DOMoveX(collision.transform.position.x + 5, 0.5f);
        }
    }
}
