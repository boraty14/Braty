using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public abstract class PanelBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            PanelManager.AddPanel(this);
        }

        protected virtual void OnDestroy()
        {
            PanelManager.RemovePanel(this);
        }
    }
}