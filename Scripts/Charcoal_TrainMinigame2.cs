using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcoal_TrainMinigame2 : MonoBehaviour
{
    public bool isOnFurnace;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            isOnFurnace = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            isOnFurnace = false;
        }
    }
}
