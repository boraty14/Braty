using Braty.Core.Runtime.Scripts.BUI.Core;

namespace Demo.DemoScroll
{
    public class DemoScrollListView : BScrollListView<DemoScrollListItem,DemoScrollListItemView>
    {
        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < 9; i++)
            {
                AddItem(new DemoScrollListItem(2,i+1));
            }
        }
    }
}