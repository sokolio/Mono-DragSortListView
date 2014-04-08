using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DragSortList
{

    public enum DragInitMode
    {
        ON_DOWN = 0,
        ON_DRAG = 1,
        ON_LONG_PRESS = 2,
    }
        
    public enum RemoveMode
    {
        CLICK_REMOVE = 0,
        FLING_REMOVE = 1,
    }

    
    /// <summary>
    /// Class that starts and stops item drags on a <see cref="DragSortListView"/> 
    /// based on touch gestures. 
    /// <para>This class also inherits from <see cref="SimpleFloatViewManager"/>,
    /// which provides basic float View creation.</para>
    /// </summary>
    // An instance of this class is meant to be passed to the methods
    // {DragSortListView.SetTouchListener()} and
    // {DragSortListView.SetFloatViewManager()} of your
    // {DragSortListView} instance.
    public class DragSortController : SimpleFloatViewManager, View.IOnTouchListener, GestureDetector.IOnGestureListener
    {

        private bool mIsRemoving = false;

        private GestureDetector mDetector;

        private GestureDetector mFlingRemoveDetector;

        private int mTouchSlop;

        public const int MISS = -1;

        private int mHitPos = MISS;
        private int mFlingHitPos = MISS;

        private int mClickRemoveHitPos = MISS;

        private int[] mTempLoc = new int[2];

        private int mItemX;
        private int mItemY;

        private int mCurrX;
        private int mCurrY;

        private bool mDragging = false;

        private float mFlingSpeed = 500f;

        private int mDragHandleId;

        private int mClickRemoveId;

        private int mFlingHandleId;
        private bool mCanDrag;

        private DragSortListView mDslv;
        private int mPositionX;

        
         /// <summary>
        /// Calls <see cref="DragSortController(DragSortListView, int, DragInitMode, DragInitMode)"/> with a
        /// 0 drag handle id, FLING_RIGHT_REMOVE remove mode, and ON_DOWN drag init. 
        /// <para>
        /// By default, sorting is enabled, and removal is disabled.
        /// </para>
        /// </summary>
        public DragSortController(DragSortListView dslv)
            : this(dslv, 0, DragInitMode.ON_DOWN, RemoveMode.FLING_REMOVE) {}

        public DragSortController(DragSortListView dslv, int dragHandleId, DragInitMode dragInitMode, RemoveMode removeMode)
            : this(dslv, dragHandleId, dragInitMode, removeMode, 0) {}

        public DragSortController(DragSortListView dslv, int dragHandleId, DragInitMode dragInitMode, RemoveMode removeMode, int clickRemoveId)
            : this(dslv, dragHandleId, dragInitMode, removeMode, clickRemoveId, 0) { }
        

        /// <summary>
        /// By default, sorting is enabled, and removal is disabled.
        /// </summary>
        /// <param name="dragHandleId">The resource id of the View that represents
        /// the drag handle in a list item.</param>
        /// <param name="dragInitMode"></param>
        public DragSortController(DragSortListView dslv, int dragHandleId, DragInitMode dragInitMode,
                RemoveMode removeMode, int clickRemoveId, int flingHandleId) :base (dslv){
            mDslv = dslv;
            mDetector = new GestureDetector(dslv.Context, this);
            //mFlingRemoveListener = new FlingRemoveListener(this);
            mFlingRemoveDetector = new GestureDetector(dslv.Context, new FlingRemoveListener(this));
            mFlingRemoveDetector.IsLongpressEnabled = false;
            mTouchSlop = ViewConfiguration.Get(dslv.Context).ScaledTouchSlop;
            mDragHandleId = dragHandleId;
            mClickRemoveId = clickRemoveId;
            mFlingHandleId = flingHandleId;
            RemoveMode = removeMode;
            DragInitMode = dragInitMode;
            SortEnabled = true;
            RemoveEnabled = false;
        }

        


        public DragInitMode DragInitMode {get; set;}
         
        /// <summary>
        /// Enable/Disable list item sorting. Disabling is useful if only item 
        /// removal is desired. Prevents drags in the vertical direction.
        /// </summary>
        public bool SortEnabled { get; set; } 
        
        ///<summary>
        ///The current remove mode.
        ///</summary>
        public RemoveMode RemoveMode { get; set; } 

        /// <summary>
        /// Enable/Disable item removal without affecting remove mode. 
        /// </summary>
        public bool RemoveEnabled { get; set; } 

        
        /// <summary>
        /// Set the resource id for the View that represents the drag handle in a list item.
        /// </summary>
        /// <param name="id">An android resource id</param>
        public void SetDragHandleId(int id) {
            mDragHandleId = id;
        }

        /// <summary>
        /// Set the resource id for the View that represents the fling handle in a list item.
        /// </summary>
        /// <param name="id">An android resource id</param>
        public void SetFlingHandleId(int id) {
            mFlingHandleId = id;
        }

        /// <summary>
        /// Set the resource id for the View that represents click removal button.
        /// </summary>
        /// <param name="id">An android resource id</param>
        public void setClickRemoveId(int id) {
            mClickRemoveId = id;
        }
        

        /// <summary>
        /// Sets flags to restrict certain motions of the floating View
        /// based on DragSortController settings (such as remove mode).
        /// Starts the drag on the DragSortListView.
        /// </summary>
        /// <param name="position">The list item position (includes headers)</param>
        /// <param name="deltaX">Touch x-coord minus left edge of floating View</param>
        /// <param name="deltaY">Touch y-coord minus top edge of floating View</param>
        /// <returns>True if drag started, false otherwise</returns>
        public bool StartDrag(int position, int deltaX, int deltaY) {

            int dragFlags = 0;
            if (SortEnabled && !mIsRemoving) {
                dragFlags |= DragSortListView.DRAG_POS_Y | DragSortListView.DRAG_NEG_Y;
            }
            if (RemoveEnabled && mIsRemoving) {
                dragFlags |= DragSortListView.DRAG_POS_X;
                dragFlags |= DragSortListView.DRAG_NEG_X;
            }

            mDragging = mDslv.StartDrag(position - mDslv.HeaderViewsCount, dragFlags, deltaX,
                    deltaY);
            return mDragging;
        }

        
        public bool OnTouch(View v, MotionEvent ev) {
            if (!mDslv.DragEnabled || mDslv.ListViewIntercepted) {
                return false;
            }

            mDetector.OnTouchEvent(ev);
            if (RemoveEnabled && mDragging && RemoveMode == RemoveMode.FLING_REMOVE) {
                mFlingRemoveDetector.OnTouchEvent(ev);
            }

            MotionEventActions action = ev.Action & MotionEventActions.Mask;
            switch (action) {
                case MotionEventActions.Down:
                    mCurrX = (int) ev.GetX();
                    mCurrY = (int) ev.GetY();
                    break;
                case MotionEventActions.Up:
                    Log.Debug(DragSortListView.LOG_TAG, "on touch: up");
                    if (RemoveEnabled && mIsRemoving) {
                        int x = mPositionX >= 0 ? mPositionX : -mPositionX;
                        int removePoint = mDslv.Width / 2;
                        if (x > removePoint) {
                            mDslv.StopDragWithVelocity(true, 0);
                        }
                    }
                    mIsRemoving = false; //workaround
                    mDragging = false; //workaround
                    break;
                case MotionEventActions.Cancel:
                    Log.Debug(DragSortListView.LOG_TAG, "on touch: cancel");
                    mIsRemoving = false;
                    mDragging = false;
                    break;
            }

            return false; 
        }

        
        // Overrides to provide fading when slide removal is enabled.
        public override void OnDragFloatView(View floatView, Point position, Point touch) {

            if (RemoveEnabled && mIsRemoving) {
                mPositionX = position.X;
            }
        }

        
        /// <summary>
        /// Get the position to start dragging based on the ACTION_DOWN
        /// MotionEvent.
        /// <para> This function simply calls <see cref="dragHandleHitPosition(MotionEvent)"/>.
        /// Override to change drag handle behavior
        /// </para>
        /// </summary>
        /// <param name="ev">The ACTION_DOWN MotionEvent</param>
        /// <returns>The list position to drag if a drag-init gesture is
        /// detected; MISS if unsuccessful</returns>
        private int startDragPosition(MotionEvent ev) {
            //* this function is called internally when an ACTION_DOWN
            //* event is detected.
            return dragHandleHitPosition(ev);
        }

        private int startFlingPosition(MotionEvent ev) {
            return RemoveMode == RemoveMode.FLING_REMOVE ? flingHandleHitPosition(ev) : MISS;
        }

        
        /// <summary>
        /// Checks for the touch of an item's drag handle (specified by <see cref="SetDragHandleId(int)"/>), 
        /// <para>and returns that item's position if a drag handle touch was detected.
        /// </para>
        /// </summary>
        /// <param name="ev">The ACTION_DOWN MotionEvent</param>
        /// <returns>The list position to drag if a drag-init gesture is
        /// detected; MISS if unsuccessful</returns>
        private int dragHandleHitPosition(MotionEvent ev) {
            return viewIdHitPosition(ev, mDragHandleId);
        }

        private int flingHandleHitPosition(MotionEvent ev) {
            return viewIdHitPosition(ev, mFlingHandleId);
        }

        private int viewIdHitPosition(MotionEvent ev, int id) {
            int x = (int) ev.GetX();
            int y = (int) ev.GetY();

            int touchPos = mDslv.PointToPosition(x, y); // includes headers/footers

            int numHeaders = mDslv.HeaderViewsCount;
            int numFooters = mDslv.FooterViewsCount;
            int count = mDslv.Count;

             Log.Debug(DragSortListView.LOG_TAG, "touch down on position " + id);
            // We're only interested if the touch was on an
            // item that's not a header or footer.
            if (touchPos != AdapterView.InvalidPosition && touchPos >= numHeaders
                    && touchPos < (count - numFooters)) {
                View item = mDslv.GetChildAt(touchPos - mDslv.FirstVisiblePosition);
                int rawX = (int) ev.RawX;
                int rawY = (int) ev.RawY;

                View dragBox = id == 0 ? item : (View) item.FindViewById(id);
                if (dragBox != null) {
                    dragBox.GetLocationOnScreen(mTempLoc);

                    if (rawX > mTempLoc[0] && rawY > mTempLoc[1] &&
                            rawX < mTempLoc[0] + dragBox.Width &&
                            rawY < mTempLoc[1] + dragBox.Height) {

                        mItemX = item.Left;
                        mItemY = item.Top;

                        return touchPos;
                    }
                }
            }

            return MISS;
        }

        public bool OnDown(MotionEvent ev) {
            Log.Debug(DragSortListView.LOG_TAG, "on down event");
            if (RemoveEnabled && RemoveMode == RemoveMode.CLICK_REMOVE) {
                mClickRemoveHitPos = viewIdHitPosition(ev, mClickRemoveId);
            }

            mHitPos = startDragPosition(ev);
            if (mHitPos != MISS && DragInitMode == DragInitMode.ON_DOWN) {
                StartDrag(mHitPos, (int) ev.GetX() - mItemX, (int) ev.GetY() - mItemY);
            }

            mIsRemoving = false;
            mCanDrag = true;
            mPositionX = 0;
            mFlingHitPos = startFlingPosition(ev);

            return true;
        }

        
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) {

            Log.Debug(DragSortListView.LOG_TAG, "on scroll event");
            int x1 = (int) e1.GetX();
            int y1 = (int) e1.GetY();
            int x2 = (int) e2.GetX();
            int y2 = (int) e2.GetY();
            int deltaX = x2 - mItemX;
            int deltaY = y2 - mItemY;

            if (mCanDrag && !mDragging && (mHitPos != MISS || mFlingHitPos != MISS)) {
                if (mHitPos != MISS) {
                    if (DragInitMode == DragInitMode.ON_DRAG && System.Math.Abs(y2 - y1) > mTouchSlop && SortEnabled) {
                        StartDrag(mHitPos, deltaX, deltaY);
                    }
                    else if (DragInitMode != DragInitMode.ON_DOWN && System.Math.Abs(x2 - x1) > mTouchSlop && RemoveEnabled)
                    {
                        mIsRemoving = true;
                        StartDrag(mFlingHitPos, deltaX, deltaY);
                    }
                } else if (mFlingHitPos != MISS) {
                    if (System.Math.Abs(x2 - x1) > mTouchSlop && RemoveEnabled) {
                        mIsRemoving = true;
                        StartDrag(mFlingHitPos, deltaX, deltaY);
                    } else if (System.Math.Abs(y2 - y1) > mTouchSlop) {
                        mCanDrag = false; // if started to scroll the list then
                                          // don't allow sorting nor fling-removing
                    }
                }
            }
            // return whatever
            return false;
        }

        public void OnLongPress(MotionEvent e) {
            Log.Debug(DragSortListView.LOG_TAG, "lift listener long pressed");

            if (mHitPos != MISS && DragInitMode == DragInitMode.ON_LONG_PRESS) {
                mDslv.PerformHapticFeedback(FeedbackConstants.LongPress);
                StartDrag(mHitPos, mCurrX - mItemX, mCurrY - mItemY);
            }
        }
        
        // complete the OnGestureListener interface
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) {
            return false;
        }

        // complete the OnGestureListener interface
        public bool OnSingleTapUp(MotionEvent ev) {
            Log.Debug(DragSortListView.LOG_TAG, "on single tap up");
            if (RemoveEnabled && RemoveMode == RemoveMode.CLICK_REMOVE) {
                if (mClickRemoveHitPos != MISS) {
                    mDslv.RemoveItem(mClickRemoveHitPos - mDslv.HeaderViewsCount);
                }
            }
            return true;
        }

        // complete the OnGestureListener interface
        public void OnShowPress(MotionEvent ev) {
            // do nothing
        }


        private class FlingRemoveListener : GestureDetector.SimpleOnGestureListener
        {
            DragSortController mDsc;
            public FlingRemoveListener(DragSortController dragSortController)
            {
                mDsc = dragSortController;
            }

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
 	             //return base.OnFling(e1, e2, velocityX, velocityY);
                Log.Debug(DragSortListView.LOG_TAG, "on fling event");
                if (mDsc.RemoveEnabled && mDsc.mIsRemoving)
                {
                    int w = mDsc.mDslv.Width;
                    int minPos = w / 5;
                    if (velocityX > mDsc.mFlingSpeed)
                    {
                        if (mDsc.mPositionX > -minPos)
                        {
                            mDsc.mDslv.StopDragWithVelocity(true, velocityX);
                        }
                    }
                    else if (velocityX < -mDsc.mFlingSpeed)
                    {
                        if (mDsc.mPositionX < minPos)
                        {
                            mDsc.mDslv.StopDragWithVelocity(true, velocityX);
                        }
                    }
                    mDsc.mIsRemoving = false;
                }
                return false;
            }
        }
        

    }
    
    

    
}