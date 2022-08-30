using UnityEngine.UIElements;

namespace DS.Windows
{

    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }

        public SplitView()
        {
        }

        public SplitView(int fixedPaneIndex, float fixedPaneStartDimension, TwoPaneSplitViewOrientation orientation) : base(fixedPaneIndex, fixedPaneStartDimension, orientation)
        {
        }
    }
}