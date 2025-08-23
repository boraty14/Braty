using Braty.Core.Runtime.Scripts.BUI;

namespace Demo.DemoScroll
{
    public class DemoScrollListItem : BScrollListItem
    {
        public readonly int Count;
        
        public DemoScrollListItem(float size, int count) : base(size)
        {
            Count = count;
        }
    }
}