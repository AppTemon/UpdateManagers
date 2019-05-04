using System.Collections.Generic;
using UnityEngine;

namespace UpdateManagers
{
    public class UpdateManagerBase
    {
        public GameObject updateSource => _updateSource;
        public bool isEmpty => _updatables == null || _updatables.Count == 0;

        protected bool _scaledTime;
        protected HashSet<IUpdatable> _updatables;

        protected GameObject _updateSource;

        HashSet<IUpdatable> _addUpdatables;
        HashSet<IUpdatable> _removeUpdatables;

        internal UpdateManagerBase(GameObject newUpdateSource, bool scaledTime)
        {
            _scaledTime = scaledTime;
            _updateSource = newUpdateSource;
            _updatables = new HashSet<IUpdatable>();
            _addUpdatables = new HashSet<IUpdatable>();
            _removeUpdatables = new HashSet<IUpdatable>();
        }

        public void Subscribe(IUpdatable updatable)
        {
            _addUpdatables.Add(updatable);
        }

        public void Unscribe(IUpdatable updatable)
        {
            _removeUpdatables.Add(updatable);
        }

        protected float GetDeltaTime()
        {
            if (Time.inFixedTimeStep)
                return _scaledTime ? Time.fixedDeltaTime : Time.fixedUnscaledDeltaTime;
            else
                return _scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
        }

        internal virtual void Update()
        {
            UpdateUpdatables(GetDeltaTime());
        }

        internal void ManageAddRemoveUpdatables()
        {
            _updatables.SymmetricExceptWith(_addUpdatables);
            _addUpdatables.Clear();
            
            _updatables.ExceptWith(_removeUpdatables);
            _removeUpdatables.Clear();
        }

        internal void FixNulls()
        {
            if (_updatables.RemoveWhere((updatable) => updatable.Equals(null)) > 0)
                Debug.LogWarning("UpdateManager expects null updatebles");
        }

        protected bool UpdateUpdatable(IUpdatable updatable, float deltaTime)
        {
            if (updatable.Equals(null))
                return false;
            updatable.FastUpdate(deltaTime);
            return true;
        }

        protected void UpdateUpdatables(float deltaTime)
        {
            ManageAddRemoveUpdatables();
            if (!IterateUpdatables(deltaTime))
                FixNulls();
        }

        protected virtual bool IterateUpdatables(float deltaTime)
        {
            bool haveNoNulls = true;
            foreach (IUpdatable updatable in _updatables)
                haveNoNulls &= UpdateUpdatable(updatable, deltaTime);
            return haveNoNulls;
        }
    }
}
