using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpdateManagers;

public class Test : MonoBehaviour
{
    public int testSize = 10000;
    public UpdateSourceType testSource;
    public bool testScaleTime;

    [Space(5)]
    public TestGameObject testPrefab;
    public TestSimpleGameObject testSimplePrefab;

    [Space(5)]
    public bool subscribeKnownManager;
    public UpdateManagerSourceBase updateManagerSource;

    [Space(5)]
    public bool clearGos;

    [Space(5)]
    public bool simpleTest;
    [Space(5)]
    public bool subscribeEveryFrame;
    [Space(5)]
    public float time;
    public bool subscribeTimed;
    [Space(5)]
    public int count;
    public bool subscribeCountFrame;

    List<TestGameObject> _objs = new List<TestGameObject>();

    void Clear()
    {
        foreach (var obj in _objs)
            obj.unscribe();
        _objs.Clear();
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    List<TestGameObject> Create()
    {
        var newList = new List<TestGameObject>(testSize);
        for (int i = 0; i < testSize; i++)
            newList.Add(Instantiate(testPrefab, transform, false));
        return newList;
    }

    // Update is called once per frame
    void Update()
    {
        if (simpleTest)
        {
            for (int i = 0; i < testSize; i++)
                Instantiate(testSimplePrefab, transform, false);
            simpleTest = false;
        }

        if (clearGos)
        {
            Clear();
            clearGos = false;
        }

        if (subscribeKnownManager)
        {
            var newList = Create();
            foreach (var obj in newList)
            {
                updateManagerSource.updateManager.Subscribe(obj);
                obj.unscribe = () =>
                {
                    updateManagerSource.updateManager.Unscribe(obj);
                };
            }
            _objs.AddRange(newList);
            subscribeKnownManager = false;
        }

        if (subscribeEveryFrame)
        {
            var newList = Create();
            UpdateSourceType buffSource = testSource;
            bool buffScaleTime = testScaleTime;
            foreach (var obj in newList)
            {
                GlobalUpdateManager.instance.SubscribeEveryFrame(obj, buffSource, buffScaleTime);
                obj.unscribe = () =>
                {
                    GlobalUpdateManager.instance.UnscribeEveryFrame(obj, buffSource, buffScaleTime);
                };
            }
            _objs.AddRange(newList);
            subscribeEveryFrame = false;
        }

        if (subscribeTimed)
        {
            var newList = Create();
            float buffTime = time;
            UpdateSourceType buffSource = testSource;
            bool buffScaleTime = testScaleTime;
            foreach (var obj in newList)
            {
                GlobalUpdateManager.instance.SubscribeTimed(buffTime, obj, buffSource, buffScaleTime);
                obj.unscribe = () =>
                {
                    GlobalUpdateManager.instance.UnscribeTimed(buffTime, obj, buffSource, buffScaleTime);
                };
            }
            _objs.AddRange(newList);
            subscribeTimed = false;
        }

        if (subscribeCountFrame)
        {
            var newList = Create();
            int buffCount = count;
            UpdateSourceType buffSource = testSource;
            bool buffScaleTime = testScaleTime;
            foreach (var obj in newList)
            {
                GlobalUpdateManager.instance.SubscribeCountFrame(buffCount, obj, buffSource, buffScaleTime);
                obj.unscribe = () =>
                {
                    GlobalUpdateManager.instance.UnscribeCountFrame(buffCount, obj, buffSource, buffScaleTime);
                };
            }
            _objs.AddRange(newList);
            subscribeCountFrame = false;
        }
    }
}
