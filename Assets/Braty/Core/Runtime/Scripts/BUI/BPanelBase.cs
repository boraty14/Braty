using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public abstract class BPanelBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            BPanelManager.AddPanel(this);
        }

        protected virtual void OnDestroy()
        {
            BPanelManager.RemovePanel(this);
        }
    }
}