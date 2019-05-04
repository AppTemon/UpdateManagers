using UnityEngine;

namespace UpdateManagers
{
    public abstract class UpdateManagerSettingsBase : ScriptableObject
    {
        public bool scaledTime;

        public abstract UpdateManagerBase CreateManagerFromSettings(GameObject updateSource);
    }
}
