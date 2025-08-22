using Braty.Core.Runtime.Scripts.BUI.Core;

namespace Demo.DemoScroll
{
    public class DemoScrollListView : BScrollListView<DemoScrollListItem,DemoScrollListItemView>
    {
        protected override void Start()
        {
            base.Start();
            AddItem(new DemoScrollListItem(2));
            AddItem(new DemoScrollListItem(2));
            AddItem(new DemoScrollListItem(2));
        }
    }
}