using Braty.Core.Runtime.Scripts.MonoEcs;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public abstract class PanelBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            MonoManager.GetSystem<PanelManager>().AddPanel(this);
        }

        protected virtual void OnDestroy()
        {
            MonoManager.GetSystem<PanelManager>().RemovePanel(this);
        }
    }
}