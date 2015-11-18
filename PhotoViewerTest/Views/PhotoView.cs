using System;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class PhotoView : ScrollView
    {
        public Image ImageView;

        public PhotoView()
        {
            ImageView = new Image();

            this.Content = ImageView;
        }

        public ImageSource PhotoSource
        {
            get
            { 
                return ImageView.Source;
            }
            set
            { 
                ImageView.Source = value;
            }
        }

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create("IsActive", typeof(bool), typeof(PhotoView), false);

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
    }
}