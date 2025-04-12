using System;
using System.Collections.Generic;
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Scope
{
    [RequireComponent(typeof(SceneScope))]
    public abstract class ScopeBase : MonoBehaviour
    {
        private readonly List<IDisposable> _systems = new();
        private Container _sceneContainer;
        
        private void Start()
        {
            _sceneContainer = gameObject.scene.GetSceneContainer(); 
            LoadScope();
        }

        protected abstract void LoadScope();

        protected void AddSystem<T>() where T : IDisposable
        {
            _systems.Add(_sceneContainer.Construct<T>());
        }
        
        private void OnDestroy()
        {
            foreach (var system in _systems)
            {
                system.Dispose();
            }
        }
    }
}