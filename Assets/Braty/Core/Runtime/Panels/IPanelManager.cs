using System;
using Cysharp.Threading.Tasks;

namespace Braty.Core.Runtime.Panels
{
    public interface IPanelManager
    {
        event Action<IPanel> OnPanelOpening;
        event Action<IPanel> OnPanelOpened;
        event Action<IPanel> OnPanelClosing;
        event Action<IPanel> OnPanelClosed;

        UniTask ShowPanel<T>(bool isSafeArea = true) where T : IPanel;
        UniTask HidePanel<T>(bool unloadPanel = false) where T : IPanel;
        T GetPanel<T>() where T : IPanel;
    }
}