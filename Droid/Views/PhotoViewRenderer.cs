using System;
using Xamarin.Forms;
using PhotoViewerTest;
using PhotoViewerTest.Droid;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Views;

[assembly:ExportRenderer(typeof(PhotoView), typeof(PhotoViewRenderer))]
namespace PhotoViewerTest.Droid
{
    public class PhotoViewRenderer : ViewRenderer<PhotoView, Android.Views.View>
    {

    }
}