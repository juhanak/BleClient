using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace BleGame.Controls
{
    public sealed partial class SpaceShipControl : GameObjectControlBase
    {
        private const string DefaultImageUri = "ms-appx:///Assets/NCC1701.png";
        private const double BounceCoefficient = 0.75d;

        public event EventHandler FireEvent;

        /// <summary>
        /// The URI of the image source of the space ship.
        /// </summary>
        public Uri ImageUri
        {
            get
            {
                return (Uri)GetValue(ImageUriProperty);
            }
            set
            {
                SetValue(ImageUriProperty, value);
            }
        }
        public static readonly DependencyProperty ImageUriProperty =
            DependencyProperty.Register("ImageUri", typeof(Uri), typeof(SpaceShipControl),
                new PropertyMetadata(new Uri(DefaultImageUri)));

        /// <summary>
        /// Constructor.
        /// </summary>
        public SpaceShipControl()
            : base()
        {
            this.InitializeComponent();
        }

        public void Fire()
        {
            if (FireEvent != null)
            {
                FireEvent(this, null);
            }
        }

        public void DoBounce(double bounceCoefficient)
        {
            VelocityX = -VelocityX * bounceCoefficient;
            VelocityY = -VelocityY * bounceCoefficient;
        }

        public bool Collides(SpaceShipControl other)
        {
            double thisWidth = spaceShipImage.ActualWidth;
            double thisHeight = spaceShipImage.ActualHeight;
            double thisWidthHalved = thisWidth / 2;
            double thisHeightHalved = thisHeight / 2;

            Rect thisBoundingBox =
                new Rect(
                    X - thisWidthHalved,
                    Y - thisHeightHalved,
                    thisWidth,
                    thisHeight);

            double otherWidth = other.spaceShipImage.ActualWidth;
            double otherHeight = other.spaceShipImage.ActualHeight;
            double otherWidthHalved = otherWidth / 2;
            double otherHeightHalved = otherHeight / 2;

            Rect otherBoundingBox =
                new Rect(
                    other.X - otherWidthHalved,
                    other.Y - otherHeightHalved,
                    otherWidth,
                    otherHeight);

            thisBoundingBox.Intersect(otherBoundingBox);
            return (thisBoundingBox.Width > 0 && thisBoundingBox.Height > 0);
        }

        public Size Size()
        {
            return new Size(spaceShipImage.ActualWidth, spaceShipImage.ActualHeight);
        }

        protected override void OnXPropertyChanged(double newX)
        {
            if (newX < Limits.Left)
            {
                if (Bounce)
                {
                    X = Limits.Left;
                    VelocityX = -VelocityX * BounceCoefficient;
                }
                else
                {
                    X = Limits.Right;
                }
            }
            else if (newX > Limits.Right)
            {
                if (Bounce)
                {
                    X = Limits.Right;
                    VelocityX = -VelocityX * BounceCoefficient;
                }
                else
                {
                    X = Limits.Left;
                }
            }
        }

        protected override void OnYPropertyChanged(double newY)
        {
            if (newY < Limits.Top)
            {
                if (Bounce)
                {
                    Y = Limits.Top;
                    VelocityY = -VelocityY * BounceCoefficient;
                }
                else
                {
                    Y = Limits.Bottom;
                }
            }
            else if (newY > Limits.Bottom)
            {
                if (Bounce)
                {
                    Y = Limits.Bottom;
                    VelocityY = -VelocityY * BounceCoefficient;
                }
                else
                {
                    Y = Limits.Top;
                }
            }
        }
    }
}
