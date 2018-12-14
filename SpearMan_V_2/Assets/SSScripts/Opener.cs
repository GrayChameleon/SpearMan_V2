using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour ,IOpenable
{
    public GameObject SthToOpen;

    public void open(bool value)
    {
        //apen door anim or sth cool 

        SthToOpen.SetActive(!value);
    }
}
