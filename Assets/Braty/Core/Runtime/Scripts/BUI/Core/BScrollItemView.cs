using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    public abstract class BScrollItemView<T> : MonoBehaviour where T : BScrollItem
    {
        public virtual void Render(T scrollItem)
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}