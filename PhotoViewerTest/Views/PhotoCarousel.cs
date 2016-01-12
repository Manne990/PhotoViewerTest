using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class PhotoCarousel : ContentView
    {
        #region Private Members

        private CarouselLayout _carouselLayout;

        #endregion

        // ---------------------------------------------------------

        #region Constructors

        public PhotoCarousel()
        {
            Content = CreatePagesCarousel();
        }

        #endregion

        // ---------------------------------------------------------

        #region Public Methods

        public void LoadPhotos(List<ImageViewModel> images, int selectedIndex)
        {
            _carouselLayout.ItemsSource = images;
            _carouselLayout.SelectedIndex = selectedIndex;
        }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private CarouselLayout CreatePagesCarousel()
        {
            _carouselLayout = new CarouselLayout {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IndicatorStyle = CarouselLayout.IndicatorStyleEnum.None,
                ItemTemplate = new DataTemplate(typeof(ImageTemplate))
            };

            return _carouselLayout;
        }

        #endregion
    }
}