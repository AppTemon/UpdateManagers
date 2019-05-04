using UnityEngine;

namespace UpdateManagers
{
    [AddComponentMenu("UpdateManagers/SimpleUpdateManager")]
    public class SimpleUpdateSource : UpdateManagerSourceBase
    {
        void Update()
        {
            if (_initialized)
            {
                _updateManager.Update();
            }
        }
    }
}
