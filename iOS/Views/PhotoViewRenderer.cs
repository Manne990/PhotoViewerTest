using System;
using System.Collections.Generic;
using CoreGraphics;
using PhotoViewerTest;
using PhotoViewerTest.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(PhotoView), typeof(PhotoViewRenderer))]
namespace PhotoViewerTest.iOS
{
    public class PhotoViewRenderer : ViewRenderer<PhotoView, UIView>
    {
        #region Private Members

        private UIScrollView _scrollView;
        private IVisualElementRenderer _childRenderer;

        #endregion

        // ---------------------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<PhotoView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _scrollView.Dispose();

                _childRenderer.NativeView.RemoveFromSuperview();
                _childRenderer.NativeView.Dispose();
                _childRenderer.Dispose();
            }

            if (e.NewElement != null)
            {
                _scrollView = new UIScrollView
                {
                    PagingEnabled = false,
                    ShowsHorizontalScrollIndicator = false,
                    ShowsVerticalScrollIndicator = false,
                    ScrollsToTop = false
                };
                
                _childRenderer = Platform.CreateRenderer(e.NewElement.ImageView);
                _scrollView.AddSubview(_childRenderer.NativeView);

                _scrollView.DidZoom += (object sender, EventArgs ee) => 
                    {
                        CenterImageInScrollView();
                    };

                var doubletap = new UITapGestureRecognizer(OnDoubleTap)
                {
                    NumberOfTapsRequired = 2
                };

                var imageView = (ViewRenderer<Image,UIImageView>)_childRenderer.NativeView;

                this.SetNativeControl(_scrollView);

                this.Control.AddGestureRecognizer(doubletap);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsActive")
            {
                _scrollView.SetZoomScale(_scrollView.MinimumZoomScale, false);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _scrollView.ContentSize = new CGSize(_scrollView.Frame.Width, _scrollView.Frame.Height);

            _scrollView.ViewForZoomingInScrollView += (UIScrollView sv) =>
                {
                    return _childRenderer.NativeView;
                };

            var imageView = (ViewRenderer<Image,UIImageView>)_childRenderer.NativeView;
            var imageWidth = imageView.Control.Image.Size.Width;
            var imageHeight = imageView.Control.Image.Size.Height;
            var imageAspect = imageWidth / imageHeight;
            var screenAspect = this.Bounds.Width / this.Bounds.Height;

            if (imageWidth > imageHeight)
            {
                // Wide Image
                if (this.Bounds.Width > this.Bounds.Height)
                {
                    // Landscape
                    _scrollView.MinimumZoomScale = this.Bounds.Width / (imageWidth * (screenAspect / imageAspect));
                }
                else
                {
                    // Portrait
                    _scrollView.MinimumZoomScale = this.Bounds.Width / imageWidth;
                }
            }
            else
            {
                // Tall Image
                if (this.Bounds.Width > this.Bounds.Height)
                {
                    // Landscape
                    _scrollView.MinimumZoomScale = this.Bounds.Height / imageHeight;
                }
                else
                {
                    // Portrait
                    _scrollView.MinimumZoomScale = this.Bounds.Height / (imageHeight * (imageAspect / screenAspect));
                }
            }

            _scrollView.MinimumZoomScale = _scrollView.MinimumZoomScale * 0.99f;
            _scrollView.MaximumZoomScale = _scrollView.MinimumZoomScale * 6;
            _scrollView.SetZoomScale(_scrollView.MinimumZoomScale, false);

            _childRenderer.Element.Layout(new Rectangle(0, 0, imageWidth, imageHeight));

            CenterImageInScrollView();
        }

        #endregion

        // ---------------------------------------------------------

        #region Event Handlers

        private void OnDoubleTap(UIGestureRecognizer gesture)
        {
            var location = gesture.LocationInView(_scrollView);

            if (_scrollView.ZoomScale > _scrollView.MinimumZoomScale)
            {
                ZoomToPoint(location, _scrollView.MinimumZoomScale, true);
            }
            else
            {
                ZoomToPoint(location, _scrollView.MinimumZoomScale * 3, true);
            }
        }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private void CenterImageInScrollView()
        {
            nfloat top = 0;
            nfloat left = 0;

            if (_scrollView.ContentSize.Width < _scrollView.Bounds.Size.Width)
            {
                left = (_scrollView.Bounds.Size.Width - _scrollView.ContentSize.Width) * 0.5f;
            }

            if (_scrollView.ContentSize.Height < _scrollView.Bounds.Size.Height)
            {
                top = (_scrollView.Bounds.Size.Height - _scrollView.ContentSize.Height) * 0.5f;
            }

            _scrollView.ContentInset = new UIEdgeInsets(top, left, top, left);
        }

        private void ZoomToPoint(CGPoint zoomPoint, nfloat scale, bool animated)
        {
            // Normalize current content size back to content scale of 1.0f
            var contentSize = new CGSize(_scrollView.ContentSize.Width / _scrollView.ZoomScale, 
                                  _scrollView.ContentSize.Height / _scrollView.ZoomScale);

            // Translate the zoom point to relative to the content rect
            zoomPoint.X = (zoomPoint.X / this.Bounds.Size.Width) * contentSize.Width;
            zoomPoint.Y = (zoomPoint.Y / this.Bounds.Size.Height) * contentSize.Height;

            // Derive the size of the region to zoom to
            var zoomSize = new CGSize(this.Bounds.Size.Width / scale, this.Bounds.Size.Height / scale);

            var zoomRect = new CGRect(zoomPoint.X - zoomSize.Width / 2.0f, 
                               zoomPoint.Y - zoomSize.Height / 2.0f, 
                               zoomSize.Width, 
                               zoomSize.Height);

            // Apply the resize
            _scrollView.ZoomToRect(zoomRect, animated);
        }

        #endregion
    }
}