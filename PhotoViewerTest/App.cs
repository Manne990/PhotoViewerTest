using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoViewerTest
{
  public class App : Application
  {
    public static INavigation Navigation;
    public App()
    {
      // The root page of your application
      var p = new NavigationPage(new PhotoViewerTest.MenuPage());
      App.Navigation = p.Navigation;
      MainPage = p;
      //MainPage = new PhotoViewerTest.SingleImagePage();
    }

    protected override void OnStart()
    {
      // Handle when your app starts
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}

