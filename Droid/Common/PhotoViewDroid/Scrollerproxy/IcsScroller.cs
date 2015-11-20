using System;
using Android.Content;

namespace PhotoViewerTest.Droid
{
	public class IcsScroller:GingerScroller
	{
		public IcsScroller(Context context):base(context) {

		}
		public override bool ComputeScrollOffset ()
		{
			return mScroller.ComputeScrollOffset();
		}
	}
}

