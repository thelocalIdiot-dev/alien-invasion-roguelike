using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventReceiver : MonoBehaviour
{
    public bool isOn;

    public void colliderOn()
    {
        isOn = true;
    }

    public void colliderOff()
    {
       isOn = false;
    }

    public void endAttack()
    {
        ChainSwords.instance.ResetAttack();
    }
}
