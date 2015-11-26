using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PhotoViewerTest
{
    public class ImagesViewModel : BaseViewModel
    {
        #region Private Members

        private ObservableCollection<ImageViewModel> _images;
        private ImageViewModel _currentImage;

        #endregion

        // ------------------------------------------------

        #region Constructors

        public ImagesViewModel()
        {
            this.Images = new ObservableCollection<ImageViewModel>();
        }

        public ImagesViewModel(int numberOfImages)
        {
            this.Images = new ObservableCollection<ImageViewModel>();

            for (int i = 0; i < numberOfImages; i++)
            {
                this.Images.Add(new ImageViewModel());
            }
        }

        #endregion

        // ------------------------------------------------

        #region Public Properties

        public ObservableCollection<ImageViewModel> Images
        {
            get
            {
                return _images;
            }
            set
            {
                SetObservableProperty(ref _images, value);
                CurrentImage = Images.FirstOrDefault();
            }
        }

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

        #endregion
    }
}