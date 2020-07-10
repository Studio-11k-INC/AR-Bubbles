using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTip : MonoBehaviour
{

    public Image thisImage;

    public Sprite tip1;
    public Sprite tip2;

    private float tipCount = 2;
    private float currentTip = 1;

    public float delay;


    void Start()
    {
        InvokeRepeating("CycleTextures", delay, delay);
    }

    // Update is called once per frame
    void CycleTextures()
    {
        float ct = currentTip % tipCount;
        currentTip++;

        switch (ct)
        {
            case 0:
                thisImage.sprite = tip2;
                break;
            case 1:
                thisImage.sprite = tip1;
                break;
        }



    }

}
