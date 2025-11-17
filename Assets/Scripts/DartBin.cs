using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBin : MonoBehaviour
{
    public bool playerInBinZone;
    public GameObject packOfDarts;

    [SerializeField]
    public int numberOfDartsAvailable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInBinZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBinZone = false;
        }
    }
}
