using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PhotoViewerTest
{
    public class ImagesViewModel : BaseViewModel
    {
        public ImagesViewModel()
        {
            this.Images = new ObservableCollection<ImageViewModel>();
            /*
            this.Images = new List<ImageViewModel>() { 
                new ImageViewModel() { ImageName = "bild1.jpg"},
                new ImageViewModel() { ImageName = "bild2.jpg"},
                new ImageViewModel() { ImageName = "bild3.jpg"}
            };
            */
        }

        private ObservableCollection<ImageViewModel> _images;
        public ObservableCollection<ImageViewModel> Images 
        {
            get 
            {
                return _images;
            }
            set 
            {
                SetObservableProperty(ref _images, value);
                CurrentImage = Images.FirstOrDefault ();
            }
        }

        private ImageViewModel _currentImage;
        public ImageViewModel CurrentImage 
        {
            get 
            {
                return _currentImage;
            }
            set 
            {
                SetObservableProperty(ref _currentImage, value);
            }
        }
    }
}