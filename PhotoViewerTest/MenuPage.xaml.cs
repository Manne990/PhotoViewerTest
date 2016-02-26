using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PhotoViewerTest
{
  public partial class MenuPage : ContentPage
  {
    public MenuPage()
    {
      InitializeComponent();
    }
    async void OnButtonShowPageSingleImage(object sender, EventArgs args)
    {
      var page = new SingleImagePage();
      await Navigation.PushAsync(page, true);
    }
    async void OnButtonShowPagePhotoCarousel(object sender, EventArgs args)
    {
      var page = new PhotoCarouselPage();
      await Navigation.PushAsync(page, true);
    }
  }

}
