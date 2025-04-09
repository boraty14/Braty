using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public interface IPanel
    {
        CanvasGroup CanvasGroup { get;}
        RectTransform RectTransform { get;}
        bool IsShown { get; }

        UniTask Show();
        UniTask Hide();
    }
}