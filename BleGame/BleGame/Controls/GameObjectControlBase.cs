using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BleGame.Controls
{
    public class GameObjectControlBase : UserControl
    {
        private const double DefaultMaxTotalVelocity = 20d;

        /// <summary>
        /// The X coordinate of the game object.
        /// </summary>
        public double X
        {
            get
            {
                return (double)GetValue(xProperty);
            }
            set
            {
                SetValue(xProperty, value);
            }
        }
        public static readonly DependencyProperty xProperty =
            DependencyProperty.Register("X", typeof(double), typeof(GameObjectControlBase),
                new PropertyMetadata(0d, OnXPropertyChanged));

        protected static void OnXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameObjectControlBase control = d as GameObjectControlBase;

            if (control != null)
            {
                control.OnXPropertyChanged((double)e.NewValue);
            }
        }

        /// <summary>
        /// The Y coordinate of the game object.
        /// </summary>
        public double Y
        {
            get
            {
                return (double)GetValue(yProperty);
            }
            set
            {
                SetValue(yProperty, value);
            }
        }
        public static readonly DependencyProperty yProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(GameObjectControlBase),
                new PropertyMetadata(0d, OnYPropertyChanged));

        protected static void OnYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameObjectControlBase control = d as GameObjectControlBase;

            if (control != null)
            {
                control.OnYPropertyChanged((double)e.NewValue);
            }
        }

        /// <summary>
        /// The angle of the game object.
        /// </summary>
        public double Angle
        {
            get
            {
                return (double)GetValue(AngleProperty);
            }
            set
            {
                SetValue(AngleProperty, value);
            }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(GameObjectControlBase),
                new PropertyMetadata(0d, OnAnglePropertyChanged));

        protected static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameObjectControlBase control = d as GameObjectControlBase;

            if (control != null)
            {
                double newAngle = (double)e.NewValue;

                if (newAngle < 0)
                {
                    control.Angle = 360 + newAngle;
                }
                else if (newAngle > 360)
                {
                    control.Angle = newAngle - 360;
                }
            }
        }

        public bool Bounce
        {
            get
            {
                return (bool)GetValue(BounceProperty);
            }
            set
            {
                SetValue(BounceProperty, value);
            }
        }
        public static readonly DependencyProperty BounceProperty =
            DependencyProperty.Register("Bounce", typeof(bool), typeof(GameObjectControlBase), null);

        /// <summary>
        /// The limits (X and Y) for the game object.
        /// </summary>
        public Rect Limits
        {
            get;
            set;
        }

        /// <summary>
        /// The velocity of the game object on X axis.
        /// </summary>
        public double VelocityX
        {
            get;
            set;
        }

        /// <summary>
        /// The velocity of the game object on Y axis.
        /// </summary>
        public double VelocityY
        {
            get;
            set;
        }

        public double MaxVelocity
        {
            get;
            set;
        }

        public GameObjectControlBase()
        {
            MaxVelocity = DefaultMaxTotalVelocity;
        }

        public virtual void Update()
        {
            X += VelocityX;
            Y += VelocityY;
        }

        public virtual void SetVelocityBasedOnAngle(double velocity)
        {
            double angle = Angle - 90d;
            VelocityX = velocity * Math.Cos(angle * Math.PI / 180d);
            VelocityY = velocity * Math.Sin(angle * Math.PI / 180d);
        }

        public double TotalVelocity()
        {
            double angle = Angle - 90d;
            return Math.Abs(VelocityX * Math.Cos(angle * Math.PI / 180d))
                + Math.Abs(VelocityY * Math.Sin(angle * Math.PI / 180d));
        }

        public virtual void ApplyThrustBasedOnAngle(double magnitude)
        {
            double angle = Angle - 90d;
            VelocityX += magnitude * Math.Cos(angle * Math.PI / 180d);
            VelocityY += magnitude * Math.Sin(angle * Math.PI / 180d);
        }

        public double PotentialTotalVelocityIfApplied(double magnitude)
        {
            double angle = Angle - 90d;
            double newVelocityX = VelocityX + magnitude * Math.Cos(angle * Math.PI / 180d);
            double newVelocityY = VelocityY + magnitude * Math.Sin(angle * Math.PI / 180d);

            return Math.Abs(newVelocityX * Math.Cos(angle * Math.PI / 180d))
                + Math.Abs(newVelocityY * Math.Sin(angle * Math.PI / 180d));
        }

        public virtual bool Collides(GameObjectControlBase other)
        {
            return false;
        }

        protected virtual void OnXPropertyChanged(double newX)
        {
        }

        protected virtual void OnYPropertyChanged(double newY)
        {
        }
    }
}
