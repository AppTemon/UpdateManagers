using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpdateManagers;

public class TestGameObject : MonoBehaviour, IUpdatable
{
    public float test = 0f;
    public System.Action unscribe;

    public void FastUpdate(float time)
    {
        test += time;
        /*int load = 0;
        for (int i = 0; i < 100000; i++)
        {
            load++;
        }*/
    }
}
