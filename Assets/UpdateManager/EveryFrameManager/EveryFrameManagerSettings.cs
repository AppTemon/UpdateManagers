using UnityEngine;

namespace UpdateManagers
{
    [CreateAssetMenu(fileName = "UpdateManagerSettings", menuName = "UpdateManagers/EveryFrameSettings")]
    public class EveryFrameManagerSettings : UpdateManagerSettingsBase
    {
        public override UpdateManagerBase CreateManagerFromSettings(GameObject updateSource)
        {
            return new EveryFrameUpdateManager(updateSource, scaledTime);
        }
    }
}
