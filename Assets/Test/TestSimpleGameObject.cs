using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSimpleGameObject : MonoBehaviour
{
    public float test = 0f;

    public void Update()
    {
        test += Time.deltaTime;
        /*int load = 0;
        for (int i = 0; i < 100000; i++)
        {
            load++;
        }*/
    }
}
