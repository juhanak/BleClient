using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace BleGame.Controls
{
    public sealed partial class ProjectileControl : GameObjectControlBase
    {
        public event EventHandler OutOfLimits;
        private int _collisionDetectionCounter;

        /// <summary>
        /// The URI of the image source of the projectile.
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
            DependencyProperty.Register("ImageUri", typeof(Uri), typeof(ProjectileControl), null);

        public ProjectileControl()
            : base()
        {
            this.InitializeComponent();
        }

        public bool Collides(SpaceShipControl spaceShip)
        {
            if (_collisionDetectionCounter < 5)
            {
                _collisionDetectionCounter++;
                return false;
            }

            double thisWidthHalved = projectileImage.ActualWidth / 2;
            double thisHeightHalved = projectileImage.ActualHeight / 2;

            Rect thisBoundingBox =
                new Rect(X - thisWidthHalved,
                    Y - thisHeightHalved,
                    projectileImage.ActualWidth,
                    projectileImage.ActualHeight);

            Size spaceShipSize = spaceShip.Size();
            double spaceShipWidthHalved = spaceShipSize.Width / 2;
            double spaceShipHeightHalved = spaceShipSize.Height / 2;

            Rect spaceShipBoundingBox =
                new Rect(spaceShip.X - spaceShipWidthHalved,
                    spaceShip.Y - spaceShipHeightHalved,
                    spaceShipSize.Width,
                    spaceShipSize.Height);

            thisBoundingBox.Intersect(spaceShipBoundingBox);
            return (thisBoundingBox.Width > 0 && thisBoundingBox.Height > 0);
        }

        protected override void OnXPropertyChanged(double newX)
        {
            if ((newX < Limits.Left || newX > Limits.Right) && OutOfLimits != null)
            {
                OutOfLimits(this, null);
            }
        }

        protected override void OnYPropertyChanged(double newY)
        {
            if ((newY < Limits.Top || newY > Limits.Bottom) && OutOfLimits != null)
            {
                OutOfLimits(this, null);
            }
        }
    }
}
