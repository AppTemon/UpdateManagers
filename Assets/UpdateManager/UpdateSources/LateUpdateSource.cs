using UnityEngine;

namespace UpdateManagers
{
    [AddComponentMenu("UpdateManagers/LateUpdateManager")]
    public class LateUpdateSource : UpdateManagerSourceBase
    {
        void LateUpdate()
        {
            if (_initialized)
            {
                _updateManager.Update();
            }
        }
    }
}