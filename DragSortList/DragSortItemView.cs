using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace DragSortList
{

    /// <summary>
    /// Lightweight ViewGroup that wraps list items obtained from user's ListAdapter. 
    /// <para>ItemView expects a single child that has a definite
    /// height (i.e. the child's layout height is not MATCH_PARENT).
    /// </para>
    /// <para> The width of ItemView will always match the width of its child (that is,
    /// the width MeasureSpec given to ItemView is passed directly
    /// to the child, and the ItemView measured width is set to the
    /// child's measured width). The height of ItemView can be anything;
    /// </para>
    /// </summary>
    // The purpose of this class is to optimize slide
    // shuffle animations.
    public class DragSortItemView : ViewGroup
    {
        private GravityFlags mGravity = GravityFlags.Top;

        public DragSortItemView(Context context) 
            :base(context) {
            // always init with standard ListView layout params
            LayoutParameters = new AbsListView.LayoutParams(
                    ViewGroup.LayoutParams.FillParent,
                    ViewGroup.LayoutParams.WrapContent);
            //setClipChildren(true);
        }

        public GravityFlags Gravity { 
            get{
                return mGravity;
            }
            set{
                mGravity = value;
            }
        }

        
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom) {
            View child = GetChildAt(0);
            if (child == null) {
                return;
            }

            if (mGravity == GravityFlags.Top) {
                child.Layout(0, 0, MeasuredWidth, child.MeasuredHeight);
            } else {
                child.Layout(0, MeasuredHeight - child.MeasuredHeight, MeasuredWidth, MeasuredHeight);
            }
        }

        
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            int width = MeasureSpec.GetSize(widthMeasureSpec);

            MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            View child = GetChildAt(0);
            if (child == null) {
                SetMeasuredDimension(0, width);
                return;
            }

            if (child.IsLayoutRequested) {
                // Always let child be as tall as it wants.
                MeasureChild(child, widthMeasureSpec,
                        MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
            }

            if (heightMode == MeasureSpecMode.Unspecified) {
                ViewGroup.LayoutParams lp = LayoutParameters;

                if (lp.Height > 0) {
                    height = lp.Height;
                } else {
                    height = child.MeasuredHeight;
                }
            }

            SetMeasuredDimension(width, height);
        }
    }
}