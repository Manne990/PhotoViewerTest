using System;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class ImageTemplate : ContentView, ICarouselLayoutChildDelegate
    {
        private PhotoView _photoView;

        public ImageTemplate()
        {
            _photoView = new PhotoView() { BackgroundColor = Color.Gray };

            //_photoView.SetBinding(PhotoView.ImageNameProperty, "ImageName");

            Content = _photoView;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _photoView.ImageName = ((ImageViewModel)this.BindingContext).ImageName;
        }

        #region ICarouselLayoutChildDelegate implementation

        public void GotActive()
        {
            _photoView.IsActive = true;
        }

        public void GotInactive()
        {
            _photoView.IsActive = false;
        }

        #endregion
    }
}