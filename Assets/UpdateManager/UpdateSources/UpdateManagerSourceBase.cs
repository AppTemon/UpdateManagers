using UnityEngine;

namespace UpdateManagers
{
    public abstract class UpdateManagerSourceBase : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        UpdateManagerSettingsBase _managerSettings;
#pragma warning restore CS0649

        protected UpdateManagerBase _updateManager;
        protected bool _initialized;

        public UpdateManagerBase updateManager {
            get {
                Init();
                return _updateManager;
            } }

        private void Awake()
        {
            Init();
        }

        void Init()
        {
            if (_initialized || _managerSettings == null)
                return;
            _updateManager = _managerSettings.CreateManagerFromSettings(gameObject);
            _initialized = true;
        }

        public EveryFrameUpdateManager InitForEveryFrame(bool scaledTime)
        {
            if (_initialized)
                Debug.LogWarning($"{gameObject.name} manager was reinitialized");

            var newManager = new EveryFrameUpdateManager(gameObject, scaledTime);
            _updateManager = newManager;
            _initialized = true;
            return newManager;
        }

        public TimedUpdateManager InitForTimed(bool scaledTime, float time)
        {
            if (_initialized)
                Debug.LogWarning($"{gameObject.name} manager was reinitialized");

            var newManager = new TimedUpdateManager(gameObject, scaledTime, time);
            _updateManager = newManager;
            _initialized = true;
            return newManager;
        }

        public CountFrameUpdateManager InitForCountFrame(bool scaledTime, int count)
        {
            if (_initialized)
                Debug.LogWarning($"{gameObject.name} manager was reinitialized");

            var newManager = new CountFrameUpdateManager(gameObject, scaledTime, count);
            _updateManager = newManager;
            _initialized = true;
            return newManager;
        }
    }

    public enum UpdateSourceType
    {
        SimpleUpdate,
        FixedUpdate,
        LateUpdate
    }
}
