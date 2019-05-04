using UnityEngine;

namespace UpdateManagers
{
    [CreateAssetMenu(fileName = "UpdateManagerSettings", menuName = "UpdateManagers/TimedSettings")]
    public class TimedManagerSettings : UpdateManagerSettingsBase
    {
        public float time;

        public override UpdateManagerBase CreateManagerFromSettings(GameObject updateSource)
        {
            return new TimedUpdateManager(updateSource, scaledTime, time);
        }
    }
}
