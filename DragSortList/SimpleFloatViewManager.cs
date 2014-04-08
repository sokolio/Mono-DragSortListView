using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DragSortList
{
    ///<summary>
    ///Simple implementation of the FloatViewManager class. 
    ///<para>Uses list items as they appear in the ListView to create the floating View.</para>
    ///</summary>
    public class SimpleFloatViewManager : Java.Lang.Object, IFloatViewManager {

        private Bitmap mFloatBitmap;

        private ImageView mImageView;

        private Color mFloatBGColor = Color.Black;

        private ListView mListView;

        public SimpleFloatViewManager(ListView lv) {
            mListView = lv;
        }

        public void SetBackgroundColor(Color color)
        {
            mFloatBGColor = color;
        }

        
        /// <summary>
        /// This simple implementation creates a Bitmap copy of the
        /// list item currently shown at ListView <c>position</c>.       
        /// </summary>
        public virtual View OnCreateFloatView(int position) {
            // Guaranteed that this will not be null? I think so. Nope, got
            // a NullPointerException once...
            View v = mListView.GetChildAt(position + mListView.HeaderViewsCount - mListView.FirstVisiblePosition);
            if (v == null) {
                return null;
            }

            v.Pressed = false;

            // Create a copy of the drawing cache so that it does not get
            // recycled by the framework when the list tries to clean up memory
            //v.setDrawingCacheQuality(View.DRAWING_CACHE_QUALITY_HIGH);
            v.DrawingCacheEnabled = true;
            mFloatBitmap = Bitmap.CreateBitmap(v.DrawingCache);
            v.DrawingCacheEnabled = false;

            if (mImageView == null) {
                mImageView = new ImageView(mListView.Context);
            }
            mImageView.SetBackgroundColor(mFloatBGColor);
            mImageView.SetPadding(0, 0, 0, 0);
            mImageView.SetImageBitmap(mFloatBitmap);
            mImageView.LayoutParameters = new ViewGroup.LayoutParams(v.Width, v.Height);

            return mImageView;
        }

        
         // This does nothing 
        public virtual void OnDragFloatView(View floatView, Point position, Point touch)
        {
            // do nothing
        }

        
        // Removes the Bitmap from the ImageView created in
        // onCreateFloatView() and tells the system to recycle it.
        public void OnDestroyFloatView(View floatView) {
            ((ImageView) floatView).SetImageDrawable(null);
            
            mFloatBitmap.Recycle();
            mFloatBitmap = null;
        }
    }

    /// <summary>
    /// Interface for customization of the floating View appearance and dragging behavior.
    /// <para>
    /// Implement your own and pass it to  <see cref="DragSortListView.SetFloatViewManager"/>. 
    /// If your own is not passed, the default <see cref="SimpleFloatViewManager"/>
    /// implementation is used.
    /// </para>
    /// </summary>
    public interface IFloatViewManager
    {


        // You can help DSLV by setting some ViewGroup.LayoutParams on this View;
        // otherwise it will set some for you (with a width of FILL_PARENT
        // and a height of WRAP_CONTENT).

        // (NOTE: <code>position</code> excludes header Views; thus, if you
        // want to call  ListView.GetChildAt(int), you will need
        // to add ListView.GetHeaderViewsCount() to the index).

        /// <summary>
        /// Return the floating View for item at <c>position</c>.
        /// <para>
        /// DragSortListView will measure and layout this View for you, so feel free to just inflate it.
        /// </para>
        /// </summary>
        /// <param name="position">Position of item to drag
        /// </param>
        /// <returns>The View you wish to display as the floating View.</returns>
        View OnCreateFloatView(int position);


        // Called whenever the floating View is dragged. Float View
        // properties can be changed here. Also, the upcoming location
        // of the float View can be altered by setting
        // <code>location.x</code> and <code>location.y</code>.
        //
        // @param floatView The floating View.
        // @param location The location (top-left; relative to DSLV
        // top-left) at which the float
        // View would like to appear, given the current touch location
        // and the offset provided in {@link DragSortListView#startDrag}.
        // @param touch The current touch location (relative to DSLV
        // top-left).
        // @param pendingScroll 

        void OnDragFloatView(View floatView, Point location, Point touch);


        // Called when the float View is dropped; lets you perform
        // any necessary cleanup. The internal DSLV floating View
        // reference is set to null immediately after this is called.
        //
        // @param floatView The floating View passed to
        // {@link #onCreateFloatView(int)}.

        void OnDestroyFloatView(View floatView);
    }
}