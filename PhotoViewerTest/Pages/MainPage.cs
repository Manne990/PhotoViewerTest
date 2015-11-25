using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class MainPage : ContentPage
    {
        #region Private Members

        private static bool _useCarousel = true;
        private PhotoView _photoView;

        #endregion

        // ------------------------------------------------

        #region Constructors

        public MainPage()
        {
            Content = new SpinnerView() { IsBusy = true };
        }

        #endregion

        // ------------------------------------------------

        #region Lifecycle

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (_useCarousel)
            {
                BindingContext = new ImagesViewModel();

                await LoadMultipleImages();

                Content = CreatePhotoViewerCarousel();
            }
            else
            {
                Content = CreateSinglePhotoViewer();

                await LoadSingleImage("bild1.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild1.jpg");
            }
        }

        #endregion

        // ------------------------------------------------

        #region Private Methods

        private async Task LoadSingleImage(string imageName, string imageUrl)
        {
            var fileSystem = DependencyService.Get<IFileSystem>();

            if (fileSystem.FileExists(imageName))
            {
                ShowImage(imageName);
            }
            else
            {
                await DownloadAndSaveFile(imageUrl, imageName);
                ShowImage(imageName);
            }
        }

        private async Task LoadMultipleImages()
        {
            await LoadSingleImage("bild1.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild1.jpg");
            await LoadSingleImage("bild2.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild2.jpg");
            await LoadSingleImage("bild3.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild3.jpg");
            await LoadSingleImage("bild4.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild4.jpg");
            await LoadSingleImage("bild5.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild5.jpg");
            await LoadSingleImage("bild6.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild6.jpg");
            await LoadSingleImage("bild7.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild7.jpg");
        }

        private void ShowImage(string imageName)
        {
            if (_useCarousel)
            {
                ((ImagesViewModel)this.BindingContext).Images.Add(new ImageViewModel() { ImageName = imageName });
            }
            else
            {
                _photoView.ImageName = imageName;
            }
        }

        private View CreateSinglePhotoViewer()
        {
            // Create a signle PhotoView
            _photoView = new PhotoView() { BackgroundColor = Color.Gray };

            // Add the PhotoView to a layout
            var layout = new RelativeLayout();

            layout.Children.Add(_photoView, 
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width;
                    }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height;
                    }));

            return layout;
        }

        private View CreatePhotoViewerCarousel()
        {
            // Create the CarouselLayout
            var carouselLayout = CreatePagesCarousel();

            // Add the PhotoView to a layout
            var layout = new RelativeLayout();

            layout.Children.Add(carouselLayout,
                Constraint.RelativeToParent((parent) => { return parent.X; }),
                Constraint.RelativeToParent((parent) => { return parent.Y; }),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );

            return layout;
        }

        private CarouselLayout CreatePagesCarousel()
        {
            var carousel = new CarouselLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IndicatorStyle = CarouselLayout.IndicatorStyleEnum.None,
                ItemTemplate = new DataTemplate(typeof(ImageTemplate))
            };

            carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Images");
            carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentImage", BindingMode.TwoWay);

            return carousel;
        }

        private async Task DownloadAndSaveFile(string url, string filename)
        {
            var httpClient = new HttpClient();
            var fileSystem = DependencyService.Get<IFileSystem>();

            try
            {
                // Download file
                var streamAsync = await httpClient.GetStreamAsync(url);

                // Save the file
                fileSystem.SaveBinaryFile(filename, streamAsync);
            }
            catch
            {
                await DisplayAlert("Error!", "Download failed!", "Ok");
            }
        }

        #endregion
    }
}