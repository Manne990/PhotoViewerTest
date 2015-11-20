using System;
using Android.Views;

namespace PhotoViewerTest.Droid
{
	public interface IGestureDetector
	{
		 bool OnTouchEvent(MotionEvent ev);

		 bool IsScaling();

		 void SetOnGestureListener(IOnGestureListener listener);
	}
}

