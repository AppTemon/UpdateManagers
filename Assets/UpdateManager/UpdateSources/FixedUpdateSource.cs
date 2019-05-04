using UnityEngine;

namespace UpdateManagers
{
    [AddComponentMenu("UpdateManagers/FixedUpdateManager")]
    public class FixedUpdateSource : UpdateManagerSourceBase
    {
        void FixedUpdate()
        {
            if (_initialized)
            {
                _updateManager.Update();
            }
        }
    }
}