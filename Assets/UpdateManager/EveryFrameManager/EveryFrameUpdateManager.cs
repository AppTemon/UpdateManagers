using UnityEngine;

namespace UpdateManagers
{
    public class EveryFrameUpdateManager : UpdateManagerBase
    {
        public EveryFrameUpdateManager(GameObject newUpdateSource, bool scaledTime) : 
            base (newUpdateSource, scaledTime)
        {

        }

        internal override void Update()
        {
            UpdateUpdatables(GetDeltaTime());
        }
    }
}
