using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAbleTarget : MonoBehaviour, ILever
{
    bool value;
    public GameObject SthToOpen;

    public bool returnValue()
    {
        return value;
    }

    public void setValue(bool value)
    {
        this.value = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spear")
        {
            Debug.Log("hit_target_trigerred");
            setValue(true);
            if(SthToOpen.GetComponent<IOpenable>() !=null)
            {
                SthToOpen.GetComponent<IOpenable>().open(true);
            }
            else
            {
                Debug.Log("fuckup");
            }
        }
    }
}
