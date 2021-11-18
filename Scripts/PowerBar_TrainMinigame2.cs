using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PowerBar_TrainMinigame2 : MonoBehaviour
{
    public Image Fill;
    public float currentPower = 0, maxPower = 100;
    public float prePowerUp = 0, prePowerDown = 0;


    private void Start()
    {
        Fill.fillAmount = currentPower / maxPower;
    }

    public void DecreasingPower()
    {
        if (currentPower >= 0)
        {
            if((-currentPower + prePowerDown) <= 5)
            {
                currentPower -= Time.deltaTime * 50;
                Fill.fillAmount = currentPower / maxPower;
            }
            else
            {
                GameController_TrainMinigame2.instance.isDecreasingPower = false;
            }      
        }
        else
        {
            currentPower = 0;
            Fill.fillAmount = currentPower / maxPower;
            GameController_TrainMinigame2.instance.isDecreasingPower = false;
            transform.GetChild(2).transform.DOPunchScale(Vector3.one, 0.5f);
            GameController_TrainMinigame2.instance.Lose();
        }
    }

    public void IncreasingPower()
    {
        if (currentPower < maxPower)
        {
            if ((currentPower - prePowerUp) <= 20)
            {
                currentPower += Time.deltaTime * 50;
                Fill.fillAmount = currentPower / maxPower;
            }
            else
            {
                GameController_TrainMinigame2.instance.isIncreasingPower = false;
            }
        }
        else
        {
            currentPower = maxPower;
            Fill.fillAmount = currentPower / maxPower;
            GameController_TrainMinigame2.instance.isIncreasingPower = false;
            transform.GetChild(1).transform.DOPunchScale(Vector3.one, 0.5f);
            GameController_TrainMinigame2.instance.Lose();

        }
    }
}
