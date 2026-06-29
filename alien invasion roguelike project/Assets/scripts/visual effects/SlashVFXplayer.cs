using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class SlashVFXplayer : MonoBehaviour
{

    public GameObject SlashEffect;

    public Transform SlashPositionRight;
    public Transform SlashPositionLeft;

    bool eventRight;
    public void slashEffect(string s)
    {
        if(s == "right")
        {
            eventRight = true;
        }
        else
        {
            eventRight = false;
        }

        if (eventRight)
        {
            GameObject SE = Instantiate(SlashEffect, SlashPositionRight.position, SlashPositionRight.rotation);

            Destroy(SE, 0.5f);
        }
        else
        {
            GameObject SE = Instantiate(SlashEffect, SlashPositionLeft.position, SlashPositionLeft.rotation);

            Destroy(SE, 0.5f);
        }
    }
}
