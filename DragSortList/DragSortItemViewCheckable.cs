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

    /**
     * Lightweight ViewGroup that wraps list items obtained from user's
     * ListAdapter. ItemView expects a single child that has a definite
     * height (i.e. the child's layout height is not MATCH_PARENT).
     * The width of
     * ItemView will always match the width of its child (that is,
     * the width MeasureSpec given to ItemView is passed directly
     * to the child, and the ItemView measured width is set to the
     * child's measured width). The height of ItemView can be anything;
     * the 
     * 
     *
     * The purpose of this class is to optimize slide
     * shuffle animations.
     */
    public class DragSortItemViewCheckable : DragSortItemView, ICheckable {

        public DragSortItemViewCheckable(Context context) 
            :base(context){
        }

        public bool Checked {
            get
            {
                View child = GetChildAt(0);
                if (child is ICheckable)
                    return ((ICheckable) child).Checked;
                else
                    return false;
            }
            set
            {
                View child = GetChildAt(0);
                if (child is ICheckable)
                ((ICheckable) child).Checked = value;
            }
        }
        
        public void Toggle() {
            View child = GetChildAt(0);
            if (child is ICheckable)
                ((ICheckable) child).Toggle();
        }
    }

}