using System.Collections.Generic;
using UnityEngine;
using System;

namespace UpdateManagers
{
    public class CountFrameUpdateManager : UpdateManagerBase
    {
        int _frameCnt;

        float[] _actDeltaTime;

        public CountFrameUpdateManager (GameObject newUpdateSource, bool scaledTime, int frameCnt) : 
            base(newUpdateSource, scaledTime)
        {
            _frameCnt = frameCnt;
            _actDeltaTime = new float[_frameCnt];
        }

        internal override void Update()
        {
            UpdateUpdatables(GetDeltaTime());
        }

        protected override bool IterateUpdatables(float deltaTime)
        {
            for (int i = 0; i < _frameCnt; i++)
                _actDeltaTime[i] += deltaTime;

            int actFrameCount = Time.frameCount % _frameCnt;

            bool haveNoNulls = true;
            int index = 0;
            foreach (IUpdatable updatable in _updatables)
            {
                if ((index % _frameCnt) == actFrameCount)
                    haveNoNulls &= UpdateUpdatable(updatable, _actDeltaTime[actFrameCount]);
                index++;
            }
            _actDeltaTime[actFrameCount] = 0f;
            return haveNoNulls;
        }
    }
}
