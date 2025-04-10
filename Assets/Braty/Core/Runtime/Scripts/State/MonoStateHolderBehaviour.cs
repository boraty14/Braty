using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.State
{
    public class MonoStateHolderBehaviour : MonoBehaviour
    {
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}