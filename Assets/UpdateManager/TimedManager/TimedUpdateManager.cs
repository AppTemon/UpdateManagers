using UnityEngine;

namespace UpdateManagers
{
    public class TimedUpdateManager : UpdateManagerBase
    {
        float _updateTime;

        float _timer;

        public TimedUpdateManager(GameObject newUpdateSource, bool scaledTime, float time) : 
            base(newUpdateSource, scaledTime)
        {
            _updateTime = time;
        }

        internal override void Update()
        {
            _timer += GetDeltaTime();
            while (_timer >= _updateTime)
            {
                _timer -= _updateTime;
                UpdateUpdatables(_updateTime);
            }
        }
    }
}
