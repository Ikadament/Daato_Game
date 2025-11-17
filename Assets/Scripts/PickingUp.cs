using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickingUp : MonoBehaviour
{
    public DartBin dartBinScript;
    public GameObject dartPrefab;

    [SerializeField]
    private Vector3 offset;

    public TextMeshProUGUI dartNumText;

    public int numberOfDart = 0;
    //private bool tookDarts = false;

    private void Update()
    {
        if(dartBinScript.playerInBinZone && Input.GetKeyUp(KeyCode.Space)) //&& !tookDarts)
        {
            numberOfDart = dartBinScript.numberOfDartsAvailable;
            Debug.Log(numberOfDart);
            //tookDarts = true;

            dartBinScript.packOfDarts.SetActive(false);
        }

        dartNumText.text = numberOfDart.ToString();
    }
}
