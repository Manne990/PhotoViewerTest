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

        #region Enums

        public enum AutoScaleModes
        {
            AutoWidth,
            AutoHeight
        }

        #endregion

        // ---------------------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<PhotoView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                CleanUpRenderer();
            }

            if (e.NewElement != null)
            {
                InitializeRenderer(e.NewElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsActive")
            {
                // Reset zoom scale when image gets active (visible)
                _scrollView.SetZoomScale(_scrollView.MinimumZoomScale, false);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            // Update the scroll view
            _scrollView.ContentSize = new CGSize(_scrollView.Frame.Width, _scrollView.Frame.Height);

            UpdateMinimumMaximumZoom();

            _scrollView.SetZoomScale(_scrollView.MinimumZoomScale, false);

            // Render the control
            var imageView = (ViewRenderer<Image,UIImageView>)_childRenderer.NativeView;

            _childRenderer.Element.Layout(new Rectangle(0, 0, imageView.Control.Image.Size.Width, imageView.Control.Image.Size.Height));

            // Center the image view
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

        #region Public Properties

        public AutoScaleModes AutoScaleMode { get; set; }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private void InitializeRenderer(PhotoView view)
        {
            // Init
            this.AutoScaleMode = AutoScaleModes.AutoHeight;

            // Create the scroll view
            _scrollView = new UIScrollView
                {
                    PagingEnabled = false,
                    ShowsHorizontalScrollIndicator = false,
                    ShowsVerticalScrollIndicator = false,
                    ScrollsToTop = false
                };

            // Load the image
            view.ImageView.Source = view.ImageName;

            // Add the child renderer to the scroll view
            _childRenderer = Platform.CreateRenderer(view.ImageView);
            _scrollView.AddSubview(_childRenderer.NativeView);

            // Handle events
            _scrollView.DidZoom += (object sender, EventArgs ee) => { CenterImageInScrollView(); };
            _scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return _childRenderer.NativeView; };

            // Set the scroll view as the native control
            this.SetNativeControl(_scrollView);

            // Add gesture recognizers
            this.Control.AddGestureRecognizer(new UITapGestureRecognizer(OnDoubleTap) { NumberOfTapsRequired = 2 });
        }

        private void CleanUpRenderer()
        {
            _scrollView.Dispose();

            _childRenderer.NativeView.RemoveFromSuperview();
            _childRenderer.NativeView.Dispose();
            _childRenderer.Dispose();
        }

        private void UpdateMinimumMaximumZoom()
        {
            var imageView = (ViewRenderer<Image,UIImageView>)_childRenderer.NativeView;
            nfloat zoomScale = GetZoomScaleThatFits(this.Bounds.Size, imageView.Bounds.Size);

            _scrollView.MinimumZoomScale = zoomScale * 0.99f;   
            _scrollView.MaximumZoomScale = _scrollView.MinimumZoomScale * 6;
        }

        private nfloat GetZoomScaleThatFits(CGSize target, CGSize source)
        {
            nfloat wScale = target.Width / source.Width;
            nfloat hScale = target.Height / source.Height;

            return AutoScaleMode == AutoScaleModes.AutoWidth ? (wScale < hScale ? hScale : wScale) : (wScale < hScale ? wScale : hScale);
        }

        private void CenterImageInScrollView()
        {
            // Calculate the left/right paddings
            nfloat left = 0;
            if (_scrollView.ContentSize.Width < _scrollView.Bounds.Size.Width)
            {
                left = (_scrollView.Bounds.Size.Width - _scrollView.ContentSize.Width) * 0.5f;
            }

            // Calculate the top/bottom paddings
            nfloat top = 0;
            if (_scrollView.ContentSize.Height < _scrollView.Bounds.Size.Height)
            {
                top = (_scrollView.Bounds.Size.Height - _scrollView.ContentSize.Height) * 0.5f;
            }

            // Apply the paddings/insets
            _scrollView.ContentInset = new UIEdgeInsets(top, left, top, left);
        }

        private void ZoomToPoint(CGPoint zoomPoint, nfloat scale, bool animated)
        {
            // Normalize current content size back to content scale of 1.0f
            var contentSize = new CGSize(_scrollView.ContentSize.Width / _scrollView.ZoomScale, _scrollView.ContentSize.Height / _scrollView.ZoomScale);

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