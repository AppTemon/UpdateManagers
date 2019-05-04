using System.Collections.Generic;
using UnityEngine;
using System;

namespace UpdateManagers
{
    public class GlobalUpdateManager : MonoBehaviour
    {
        static GlobalUpdateManager _globalUpdateManager;


        List<UpdateManagerDescription> _everyFrameUpdateList;
        Dictionary<float, List<UpdateManagerDescription>> _timedUpdateLists;
        Dictionary<int, List<UpdateManagerDescription>> _countFrameUpdateLists;

        bool _everyFrameDirty;
        bool _timedDirty;
        bool _countFrameDity;

        public static GlobalUpdateManager instance
        {
            get
            {
                if (_globalUpdateManager == null)
                {
                    var globalUpdateMangerGO = new GameObject("_GlobalUpdateManager_", typeof(GlobalUpdateManager));
                    _globalUpdateManager = globalUpdateMangerGO.GetComponent<GlobalUpdateManager>();
                    _globalUpdateManager.Init();
                }
                return _globalUpdateManager;
            }
        }

        void Init()
        {
            _everyFrameUpdateList = new List<UpdateManagerDescription>();
            _timedUpdateLists = new Dictionary<float, List<UpdateManagerDescription>>();
            _countFrameUpdateLists = new Dictionary<int, List<UpdateManagerDescription>>();
            DontDestroyOnLoad(gameObject);
        }

        private void Awake()
        {
            if (_globalUpdateManager == null)
            {
                _globalUpdateManager = this;
                Init();
            }
            else if (_globalUpdateManager != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        void OnDestroy()
        {
            if (_globalUpdateManager == this)
                _globalUpdateManager = null;
        }

        GameObject CreateUpdateSource(UpdateSourceType sourceType, string name)
        {
            Type sourceTypeOf = null;
            switch (sourceType)
            {
                case UpdateSourceType.SimpleUpdate:
                    sourceTypeOf = typeof(SimpleUpdateSource);
                    break;
                case UpdateSourceType.FixedUpdate:
                    sourceTypeOf = typeof(FixedUpdateSource);
                    break;
                case UpdateSourceType.LateUpdate:
                    sourceTypeOf = typeof(LateUpdateSource);
                    break;
            }
            var go = new GameObject($"{sourceType}_{name}", sourceTypeOf);
            go.transform.SetParent(transform);
            return go;
        }

        UpdateManagerDescription FindManagerDescription(List<UpdateManagerDescription> list, UpdateSourceType sourceType, bool scaledTime)
        {
            return list.Find((description) =>
            {
                return (description.sourceType == sourceType) && (description.scaledTime == scaledTime);
            });
        }

        public EveryFrameUpdateManager GetEveryFrameUpdateList(UpdateSourceType sourceType, bool scaledTime)
        {

            var managerDescription = FindManagerDescription(_everyFrameUpdateList, sourceType, scaledTime);
            if (managerDescription == null)
            {
                string scaledName = scaledTime ? "Scaled" : "Unscaled";
                GameObject go = CreateUpdateSource(sourceType, $"EveryFrame_{scaledName}");
                managerDescription = new UpdateManagerDescription
                {
                    sourceType = sourceType,
                    scaledTime = scaledTime,
                    updateManager = go.GetComponent<UpdateManagerSourceBase>().InitForEveryFrame(scaledTime)
                };
                _everyFrameUpdateList.Add(managerDescription);
            }
            return managerDescription.updateManager as EveryFrameUpdateManager;
        }

        public void SubscribeEveryFrame(IUpdatable updatable, UpdateSourceType sourceType = UpdateSourceType.SimpleUpdate, bool scaledTime = true)
        {
            GetEveryFrameUpdateList(sourceType, scaledTime).Subscribe(updatable);
        }

        public void UnscribeEveryFrame(IUpdatable updatable, UpdateSourceType sourceType = UpdateSourceType.SimpleUpdate, bool scaledTime = true)
        {
            var managerDescription = FindManagerDescription(_everyFrameUpdateList, sourceType, scaledTime);
            if (managerDescription != null)
            {
                managerDescription.updateManager.Unscribe(updatable);
                _everyFrameDirty = true;
            }
        }

        public TimedUpdateManager GetTimedUpdateList(float time, UpdateSourceType sourceType, bool scaledTime)
        {
            if (!_timedUpdateLists.TryGetValue(time, out List<UpdateManagerDescription> updateList))
            {
                updateList = new List<UpdateManagerDescription>();
                _timedUpdateLists.Add(time, updateList);
            }
            var managerDescription = FindManagerDescription(_everyFrameUpdateList, sourceType, scaledTime);
            if (managerDescription == null)
            {
                string scaledName = scaledTime ? "Scaled" : "Unscaled";
                GameObject go = CreateUpdateSource(sourceType, $"Timed_{scaledName}_{time}");
                managerDescription = new UpdateManagerDescription
                {
                    sourceType = sourceType,
                    scaledTime = scaledTime,
                    updateManager = go.GetComponent<UpdateManagerSourceBase>().InitForTimed(scaledTime, time)
                };
                updateList.Add(managerDescription);
            }
            return managerDescription.updateManager as TimedUpdateManager;
        }

        public void SubscribeTimed(float time, IUpdatable updatable, 
            UpdateSourceType sourceType = UpdateSourceType.SimpleUpdate, bool scaledTime = true)
        {
            GetTimedUpdateList(time, sourceType, scaledTime).Subscribe(updatable);
        }

        public void UnscribeTimed(float time, IUpdatable updatable, 
            UpdateSourceType sourceType = UpdateSourceType.SimpleUpdate, bool scaledTime = true)
        {
            if (_timedUpdateLists.TryGetValue(time, out List<UpdateManagerDescription> updateList))
            {
                var managerDescription = FindManagerDescription(updateList, sourceType, scaledTime);
                if (managerDescription != null)
                {
                    managerDescription.updateManager.Unscribe(updatable);
                    _timedDirty = true;
                }
            }
        }

        public CountFrameUpdateManager GetCountFrameUpdateList(int count, UpdateSourceType sourceType, bool scaledTime)
        {
            if (!_countFrameUpdateLists.TryGetValue(count, out List<UpdateManagerDescription> updateList))
            {
                updateList = new List<UpdateManagerDescription>();
                _countFrameUpdateLists.Add(count, updateList);
            }
            var managerDescription = FindManagerDescription(_everyFrameUpdateList, sourceType, scaledTime);
            if (managerDescription == null)
            {
                string scaledName = scaledTime ? "Scaled" : "Unscaled";
                GameObject go = CreateUpdateSource(sourceType, $"CountFrame_{scaledName}_{count}");
                managerDescription = new UpdateManagerDescription
                {
                    sourceType = sourceType,
                    scaledTime = scaledTime,
                    updateManager = go.GetComponent<UpdateManagerSourceBase>().InitForCountFrame(scaledTime, count)
                };
                updateList.Add(managerDescription);
            }
            return managerDescription.updateManager as CountFrameUpdateManager;
        }

        public void SubscribeCountFrame(int count, IUpdatable upadatable,
            UpdateSourceType sourceType = UpdateSourceType.SimpleUpdate, bool scaledTime = true)
        {
            GetCountFrameUpdateList(count, sourceType, scaledTime).Subscribe(upadatable);
        }

        public void UnscribeCountFrame(int count, IUpdatable updatable,
            UpdateSourceType sourceType = UpdateSourceType.SimpleUpdate, bool scaledTime = true)
        {
            if (_countFrameUpdateLists.TryGetValue(count, out List<UpdateManagerDescription> updateList))
            {
                var managerDescription = FindManagerDescription(updateList, sourceType, scaledTime);
                if (managerDescription != null)
                {
                    managerDescription.updateManager.Unscribe(updatable);
                    _countFrameDity = true;
                }
            }
        }

        void ManageUnnecessaryManagers(List<UpdateManagerDescription> list)
        {
            List<UpdateManagerDescription> removeList = null;
            foreach (var item in list)
            {
                item.updateManager.ManageAddRemoveUpdatables();
                if (item.updateManager.isEmpty)
                {
                    Destroy(item.updateManager.updateSource);
                    if (removeList == null)
                        removeList = new List<UpdateManagerDescription>();
                    removeList.Add(item);
                }
            }
            if (removeList != null)
                foreach (var item in removeList)
                    list.Remove(item);
        }

        void LateUpdate()
        {
            if (_everyFrameDirty)
            {
                ManageUnnecessaryManagers(_everyFrameUpdateList);
                _everyFrameDirty = false;
            }

            if (_timedDirty)
            {
                List<float> removeKeys = null;
                foreach (var updateList in _timedUpdateLists)
                {
                    ManageUnnecessaryManagers(updateList.Value);
                    if (updateList.Value.Count == 0)
                    {
                        if (removeKeys == null)
                            removeKeys = new List<float>();
                        removeKeys.Add(updateList.Key);
                    }
                }
                if (removeKeys != null)
                    foreach (var key in removeKeys)
                        _timedUpdateLists.Remove(key);
                _timedDirty = false;
            }

            if (_countFrameDity) {
                List<int> removeKeys = null;
                foreach (var updateList in _countFrameUpdateLists)
                {
                    ManageUnnecessaryManagers(updateList.Value);
                    if (updateList.Value.Count == 0)
                    {
                        if (removeKeys == null)
                            removeKeys = new List<int>();
                        removeKeys.Add(updateList.Key);
                    }
                }
                if (removeKeys != null)
                    foreach (var key in removeKeys)
                        _countFrameUpdateLists.Remove(key);
                _countFrameDity = false;
            }
        }

        class UpdateManagerDescription
        {
            public UpdateSourceType sourceType;
            public bool scaledTime;
            public UpdateManagerBase updateManager;
        }
    }
}
