using System;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class ImageTemplate : ContentView, ICarouselLayoutChildDelegate
    {
        #region Private Members

        private PhotoView _photoView;

        #endregion

        // ------------------------------------------------

        #region Constructors

        public ImageTemplate()
        {
            _photoView = new PhotoView() { BackgroundColor = Color.Gray };

            Content = _photoView;
        }

        #endregion

        // ------------------------------------------------

        #region Overrides

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (string.IsNullOrWhiteSpace(((ImageViewModel)this.BindingContext).ImageName) == false)
            {
                _photoView.ImageName = ((ImageViewModel)this.BindingContext).ImageName;
            }
        }

        #endregion

        // ------------------------------------------------

        #region ICarouselLayoutChildDelegate implementation

        public void WillBeActive()
        {
            
        }

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