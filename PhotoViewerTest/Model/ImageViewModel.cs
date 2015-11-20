using System;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class ImageViewModel : BaseViewModel
    {
        private string _imageName;
        public string ImageName 
        {
            get 
            {
                return _imageName;
            }
            set 
            {
                SetObservableProperty(ref _imageName, value);
            }
        }
    }
}