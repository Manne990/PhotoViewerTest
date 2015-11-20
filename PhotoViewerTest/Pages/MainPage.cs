using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            //Content = CreateSinglePhotoViewer();
            Content = CreatePhotoViewerCarousel();
        }

        private View CreateSinglePhotoViewer()
        {
            // Create a signle PhotoView
            var photoView = new PhotoView() { BackgroundColor = Color.Gray, ImageName = "bild1.jpg" };

            // Add the PhotoView to a layout
            var layout = new RelativeLayout();

            layout.Children.Add(photoView, 
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) => {return parent.Width;}),
                heightConstraint: Constraint.RelativeToParent ((parent) => {return parent.Height;}));

            return layout;
        }

        private View CreatePhotoViewerCarousel()
        {
            // Create ViewModel
            BindingContext = new ImagesViewModel();

            // Create the CarouselLayout
            var pagesCarousel = CreatePagesCarousel();

            // Add the PhotoView to a layout
            var layout = new RelativeLayout();

            layout.Children.Add(pagesCarousel,
                Constraint.RelativeToParent ((parent) => { return parent.X; }),
                Constraint.RelativeToParent ((parent) => { return parent.Y; }),
                Constraint.RelativeToParent ((parent) => { return parent.Width; }),
                Constraint.RelativeToParent ((parent) => { return parent.Height; })
            );

            return layout;
        }

        private CarouselLayout CreatePagesCarousel()
        {
            var carousel = new CarouselLayout {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IndicatorStyle = CarouselLayout.IndicatorStyleEnum.None,
                ItemTemplate = new DataTemplate(typeof(ImageTemplate))
            };

            carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Images");
            carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentImage", BindingMode.TwoWay);

            return carousel;
        }
    }


    // Template

    public class ImageTemplate : ContentView, ICarouselLayoutChildDelegate
    {
        private PhotoView _photoView;

        public ImageTemplate()
        {
            _photoView = new PhotoView() { BackgroundColor = Color.Gray };

            _photoView.SetBinding(PhotoView.ImageNameProperty, "ImageName");

            Content = _photoView;
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


    // View Models

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        internal virtual Task Initialize (params object[] args)
        {
            return Task.FromResult (0);
        }

        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged == null) return;
            PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
        }

        protected void SetObservableProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged (propertyName);
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    public class ImageViewModel : BaseViewModel
    {
        public string ImageName { get; set; }
    }
        
    public class ImagesViewModel : BaseViewModel
    {
        public ImagesViewModel()
        {
            this.Images = new List<ImageViewModel>() { 
                new ImageViewModel() { ImageName = "bild1.jpg"},
                new ImageViewModel() { ImageName = "bild2.jpg"},
                new ImageViewModel() { ImageName = "bild3.jpg"}
            };
        }

        private List<ImageViewModel> _images;
        public List<ImageViewModel> Images {
            get {
                return _images;
            }
            set {
                SetObservableProperty (ref _images, value);
                CurrentImage = Images.FirstOrDefault ();
            }
        }

        private ImageViewModel _currentImage;
        public ImageViewModel CurrentImage {
            get {
                return _currentImage;
            }
            set {
                SetObservableProperty (ref _currentImage, value);
            }
        }
    }
}