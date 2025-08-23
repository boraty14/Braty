using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public abstract class BScrollListItemView<T> : MonoBehaviour where T : BScrollListItem
    {
        public virtual void Render(T item, Vector2 position)
        {
            gameObject.SetActive(true);
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
        }

    }
}