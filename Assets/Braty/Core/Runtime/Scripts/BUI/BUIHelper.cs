using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public class BUIHelper : MonoBehaviour
    {
        public Renderer _renderer;
        
        [ContextMenu("Print stencil")]
        void DoSomething()
        {
            Material mat = _renderer.sharedMaterial;
            if (mat.HasProperty("_Stencil"))
            {
                int stencilRef = mat.GetInt("_Stencil");
                Debug.Log("Stencil Ref: " + stencilRef);
            }
        }
    }
}