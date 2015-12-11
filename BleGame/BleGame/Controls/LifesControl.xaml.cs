using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BleGame.Controls
{
    public sealed partial class LifesControl : UserControl
    {
        private int _lifeCount;

        /// <summary>
        ///
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
            DependencyProperty.Register("ImageUri", typeof(Uri), typeof(LifesControl), null);

        public bool DecreaseFromRightToLeft
        {
            get
            {
                return (bool)GetValue(DecreaseFromRightToLeftProperty);
            }
            set
            {
                SetValue(DecreaseFromRightToLeftProperty, value);
            }
        }
        public static readonly DependencyProperty DecreaseFromRightToLeftProperty =
            DependencyProperty.Register("DecreaseFromRightToLeft", typeof(bool), typeof(LifesControl), null);

        public LifesControl()
        {
            this.InitializeComponent();
            _lifeCount = 3;
        }

        public int Lifes()
        {
            return _lifeCount;
        }

        public bool Kill()
        {
            bool wasKilled = false;

            switch (_lifeCount)
            {
                case 3:
                    if (DecreaseFromRightToLeft)
                    {
                        lifeRight.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        lifeLeft.Visibility = Visibility.Collapsed;
                    }

                    break;
                case 2:
                    lifeCenter.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    if (DecreaseFromRightToLeft)
                    {
                        lifeLeft.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        lifeRight.Visibility = Visibility.Collapsed;
                    }

                    break;
            }

            if (_lifeCount > 0)
            {
                _lifeCount--;
                wasKilled = true;
            }

            return wasKilled;
        }

        public void OneUp(int howManyTimes = 1)
        {
            for (int i = 0; i < howManyTimes; ++i)
            {
                if (_lifeCount < 3)
                {
                    _lifeCount++;
                }

                switch (_lifeCount)
                {
                    case 3:
                        if (DecreaseFromRightToLeft)
                        {
                            lifeRight.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lifeLeft.Visibility = Visibility.Visible;
                        }

                        break;
                    case 2:
                        lifeCenter.Visibility = Visibility.Visible;
                        break;
                    case 1:
                        if (DecreaseFromRightToLeft)
                        {
                            lifeLeft.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lifeRight.Visibility = Visibility.Visible;
                        }

                        break;
                }
            }
        }
    }
}
