using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using PhotoViewerTest;
using PhotoViewerTest.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(PhotoView), typeof(PhotoViewRenderer))]
namespace PhotoViewerTest.Droid
{
    public class PhotoViewRenderer : ViewRenderer<PhotoView, PhotoViewDroid>
    {
        #region Private Members

        private static readonly int MAX_IMAGE_SIZE_WIDTH = 256;
        private static readonly int MAX_IMAGE_SIZE_HEIGHT = 256;

        private PhotoViewDroid _photoView;

        #endregion

        // ---------------------------------------------------------

        #region Overrides

        protected async override void OnElementChanged(ElementChangedEventArgs<PhotoView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                CleanUpRenderer();
            }

            if (e.NewElement != null)
            {
                await InitializeRenderer(e.NewElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsActive")
            {
                //TODO: Reset zoom scale when image gets active (visible)
            }
        }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private async Task InitializeRenderer(PhotoView view)
        {
            // Create the Photo View
            _photoView = new PhotoViewDroid(this.Context);

            // Prepare the image
            var imageResourceName = view.ImageName.Remove(view.ImageName.IndexOf(".")); // Not a good implementation...
            var imageId = this.Resources.GetIdentifier(imageResourceName, "drawable", this.Context.ApplicationInfo.PackageName);

            var options = await GetBitmapOptionsOfImageAsync(Resource.Drawable.bild1);
            var bitmapToDisplay = await LoadScaledDownBitmapForDisplayAsync(Resources, imageId, options, MAX_IMAGE_SIZE_WIDTH, MAX_IMAGE_SIZE_HEIGHT);

            // Set the scroll view as the native control
            this.SetNativeControl(_photoView);

            // Set the image
            this.Control.SetImageBitmap(bitmapToDisplay);
        }

        private void CleanUpRenderer()
        {
            _photoView.Dispose();
        }

        private async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(int id)
        {
            var options = new BitmapFactory.Options {
                InJustDecodeBounds = true
            };

            // The result will be null because InJustDecodeBounds == true.
            await BitmapFactory.DecodeResourceAsync(Resources, id, options);

            //await BitmapFactory.DecodeFileAsync("Path to file", options);

            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            return options;
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }

        private async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, int id, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeResourceAsync(res, id, options);
            //return await BitmapFactory.DecodeFileAsync("Path to file", options);
        }

        #endregion
    }
}