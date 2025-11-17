using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public bool isInSmallRoom = false;

    private void Awake()
    {
        Instance = this;
    }


    // RETIRER POUR MEILLEUR DEV
    
     void Update()
     {
         //Cursor.visible = false;
     }

    // SERA PEUT ETRE UTILE APRES

    // void Start()
    // {
    //     Application.targetFrameRate = 60;
    // }

    public void EnterSmallRoom()
    {
        isInSmallRoom = true;
    }

    public void ExitSmallRoom()
    {
        isInSmallRoom = false;
    }

    public void ChangeDartLilTargetCollision(bool condition)
    {
        int dartLayer = LayerMask.NameToLayer("Dart");
        int lilTargetLayer = LayerMask.NameToLayer("LilTarget");

        Physics2D.IgnoreLayerCollision(dartLayer, lilTargetLayer, condition);
    }

    
}
