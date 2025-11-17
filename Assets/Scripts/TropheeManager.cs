using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TropheeManager : MonoBehaviour
{
    public static TropheeManager Instance;
    public TextMeshProUGUI tropheeCountText;
    public GameObject tropheeIcon;
    private Animator tropheeIconAnimator;
    private Animator tropheeTextAnimator;
    private int tropheeCount = 0;
    private bool firstUpdate = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tropheeIconAnimator = tropheeIcon.GetComponent<Animator>();
        tropheeTextAnimator = tropheeCountText.GetComponent<Animator>();

        UpdateTropheeUI();
        firstUpdate = false;
    }

    public void AddTropheeCount()
    {
        tropheeCount += 1;
        UpdateTropheeUI();
    }

    public void UpdateTropheeUI()
    {
        tropheeCountText.text = tropheeCount.ToString();

        if (!firstUpdate)
        {
            tropheeIconAnimator.SetTrigger("TropheeUpdateBoing");
            tropheeTextAnimator.SetTrigger("TropheeScoreUpdate");
        }

    }

}
