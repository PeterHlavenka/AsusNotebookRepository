using System;
using Android.Views;
using Com.Tomergoldst.Tooltips;
using ToolTipSupportProject;
using ToolTipSupportProject.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Capi")]
[assembly: ExportEffect(typeof(DroidTooltipEffect), nameof(TooltipEffect))]
namespace ToolTipSupportProject.Android
{
    public class DroidTooltipEffect : PlatformEffect 
    {
        ToolTip m_toolTipView;
        readonly ToolTipsManager m_toolTipsManager;

        public DroidTooltipEffect()
        {
            ToolTipsManager.ITipListener listener = new TipListener();
            m_toolTipsManager = new ToolTipsManager(listener);
        }

        void OnTap(object sender, EventArgs e)
        {
            var control = Control ?? Container;

            var text = TooltipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
               ToolTip.Builder builder;
                var parentContent = control.RootView;
               
                var position = TooltipEffect.GetPosition(Element);
                switch (position)
                {
                    case TooltipPosition.Top:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(80, ' '), ToolTip.PositionAbove);
                        break;
                    case TooltipPosition.Left:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(80, ' '), ToolTip.PositionLeftTo);
                        break;
                    case TooltipPosition.Right:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(80, ' '), ToolTip.PositionRightTo);
                        break;
                    default:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(80, ' '), ToolTip.PositionBelow);
                        break;
                }

                builder.SetAlign(ToolTip.AlignLeft);
                builder.SetBackgroundColor(TooltipEffect.GetBackgroundColor(Element).ToAndroid());
                builder.SetTextColor(TooltipEffect.GetTextColor(Element).ToAndroid());
             
                m_toolTipView = builder.Build();
               
                m_toolTipsManager?.Show(m_toolTipView);
            }

        }


        protected override void OnAttached()
        {
            var control = Control ?? Container;
            control.Click += OnTap;
        }


        protected override void OnDetached()
        {
            var control = Control ?? Container;
            control.Click -= OnTap;
            m_toolTipsManager.FindAndDismiss(control);
        }

        class TipListener : Java.Lang.Object, ToolTipsManager.ITipListener
        {
            public void OnTipDismissed(global::Android.Views.View p0, int p1, bool p2)
            {
             
            }
        }
    }
}