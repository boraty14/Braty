using Braty.Core.Runtime.Scripts.BUI;
using UnityEngine;

namespace Demo.DemoScroll
{
    public class DemoScrollListView : BScrollListView<DemoScrollListItem,DemoScrollListItemView>
    {
        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < 9; i++)
            {
                AddItem(new DemoScrollListItem(Random.Range(1,3),i+1));
            }
        }
    }
}