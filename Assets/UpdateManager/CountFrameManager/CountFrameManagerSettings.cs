using UnityEngine;

namespace UpdateManagers
{
    [CreateAssetMenu(fileName = "UpdateManagerSettings", menuName = "UpdateManagers/CountFrameSettings")]
    public class CountFrameManagerSettings : UpdateManagerSettingsBase
    {
        public int count;

        public override UpdateManagerBase CreateManagerFromSettings(GameObject updateSource)
        {
            return new CountFrameUpdateManager(updateSource, scaledTime, count);
        }
    }
}
