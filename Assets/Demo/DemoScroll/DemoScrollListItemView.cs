using Braty.Core.Runtime.Scripts.BUI;
using TMPro;
using UnityEngine;

namespace Demo.DemoScroll
{
    public class DemoScrollListItemView : BScrollListItemView<DemoScrollListItem>
    {
        [SerializeField] private TMP_Text _text;

        public override void Render(DemoScrollListItem item, Vector2 position)
        {
            base.Render(item, position);
            _text.text = item.Count.ToString();
        }
    }
}