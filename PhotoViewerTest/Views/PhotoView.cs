using System;
using Xamarin.Forms;

namespace PhotoViewerTest
{
    public class PhotoView : ScrollView
    {
        #region Constructors

        public PhotoView()
        {
            ImageView = new Image();

            this.Content = ImageView;
        }

        #endregion

        // ---------------------------------------------------------

        #region Public Properties

        public Image ImageView { get; private set; }

        public static readonly BindableProperty ImageNameProperty = BindableProperty.Create("ImageName", typeof(string), typeof(PhotoView), string.Empty);
        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create("IsActive", typeof(bool), typeof(PhotoView), false);
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        #endregion
    }
}