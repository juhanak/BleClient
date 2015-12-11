using BleClient;
using BleGame.Controls;
using BleGame.Model;
using BLESDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace BleGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public BleClientManager SdkManager
        {
            get
            {
                return BleClientManager.Instance;
            }
        }
        public BleGameModel AppModel
        {
            get
            {
                return BleGameModel.Instance;
            }
        }


        private const int UpdateViewIntervalInMilliseconds = 17;
        private const double LimitsMargin = 50d;
        private const double PhotonTorpedoWidth = 20d;
        private const double LaserProjectileWidth = 5d;
        private const double ProjectileDiffVelocity = 7.5d;

        private AiPlayer _aiPlayer;
        private List<ProjectileControl> _projectiles;
        private Timer _updateViewTimer;
        private double _width;
        private double _height;
        private bool _leftXwingCannonIsCurrent;
        private int _counter;
        private bool _blControl = false;

        public MainPage()
        {
            this.InitializeComponent();

            _projectiles = new List<ProjectileControl>();

            Loaded += OnPageLoaded;
            SizeChanged += OnPageSizeChanged;
        }

        override protected void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        private void UpdateLimits()
        {
            double widthHalved = _width / 2;
            double heightHalved = _height / 2;

            Rect limits = new Rect(
                new Point(-widthHalved + LimitsMargin, -heightHalved + LimitsMargin),
                new Point(widthHalved - LimitsMargin, heightHalved - LimitsMargin));

            ncc1701.Limits = limits;
            xwing.Limits = limits;

            limits = new Rect(
                new Point(-widthHalved - LimitsMargin, -heightHalved - LimitsMargin),
                new Point(widthHalved + LimitsMargin, heightHalved + LimitsMargin));

            foreach (ProjectileControl projectile in _projectiles)
            {
                projectile.Limits = limits;
            }
        }

        private ProjectileControl CreateProjectile(double x, double y, double angle, double shipVelocity, double width)
        {
            ProjectileControl projectile = new ProjectileControl();
            projectile.Width = width;
            projectile.X = x;
            projectile.Y = y;
            projectile.Angle = angle;
            projectile.SetVelocityBasedOnAngle(shipVelocity + ProjectileDiffVelocity);

            projectile.OutOfLimits += OnProjectileOutOfLimits;

            double widthHalved = _width / 2;
            double heightHalved = _height / 2;

            Rect limits = new Rect(
                new Point(-widthHalved - LimitsMargin, -heightHalved - LimitsMargin),
                new Point(widthHalved + LimitsMargin, heightHalved + LimitsMargin));

            projectile.Limits = limits;

            _projectiles.Add(projectile);
            gameArea.Children.Add(projectile);

            return projectile;
        }

        private void DeleteProjectile(ProjectileControl projectile)
        {
            gameArea.Children.Remove(projectile);
            _projectiles.Remove(projectile);
        }

        private void OnXwingFireEvent(object sender, EventArgs e)
        {
            double angle = xwing.Angle;
            double offset = _leftXwingCannonIsCurrent ? -15d : 15d;
            _leftXwingCannonIsCurrent = !_leftXwingCannonIsCurrent;
            double x = xwing.X + offset * Math.Cos(angle * Math.PI / 180d) + 10d * Math.Sin(angle * Math.PI / 180d);
            double y = xwing.Y + offset * Math.Sin(angle * Math.PI / 180d) - 10d * Math.Cos(angle * Math.PI / 180d);

            ProjectileControl projectile = CreateProjectile(x, y, angle, xwing.TotalVelocity(), LaserProjectileWidth);
            projectile.ImageUri = new Uri("ms-appx:///Assets/LaserProjectile.png");
        }

        private void OnNcc1701FireEvent(object sender, EventArgs e)
        {
            double angle = ncc1701.Angle;
            double x = ncc1701.X + 70d * Math.Sin(angle * Math.PI / 180d);
            double y = ncc1701.Y - 70d * Math.Cos(angle * Math.PI / 180d);

            ProjectileControl projectile = CreateProjectile(x, y, angle, ncc1701.TotalVelocity(), PhotonTorpedoWidth);
            projectile.ImageUri = new Uri("ms-appx:///Assets/PhotonTorpedo.png");
        }

        private void OnProjectileOutOfLimits(object sender, EventArgs e)
        {
            ProjectileControl projectile = sender as ProjectileControl;

            if (projectile != null)
            {
                DeleteProjectile(projectile);
            }
        }

        private async void OnUpdateViewAsync(object state)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _aiPlayer.Update(3d, 0.1d);

                    ncc1701.Update();
                    xwing.Update();

                    if (_blControl)
                    {
                        if (_counter % 20 == 0)
                        {
                            ncc1701.Fire();
                        }

                        if (_counter % 8 == 0)
                        {
                            // TODO: Remove
                            xwing.Fire();
                        }
                    }

                    _counter++;

                    for (int i = 0; i < _projectiles.Count; ++i)
                    {
                        bool collides = false;

                        if (_projectiles[i].Collides(ncc1701))
                        {
                            if (!nccLifesControl.Kill())
                            {
                                nccLifesControl.OneUp(3);
                            }

                            collides = true;
                        }
                        else if (_projectiles[i].Collides(xwing))
                        {
                            if (!xwingLifesControl.Kill())
                            {
                                xwingLifesControl.OneUp(3);
                            }

                            collides = true;
                        }

                        if (collides)
                        {
                            DeleteProjectile(_projectiles[i]);
                            i--;
                        }
                        else
                        {
                            _projectiles[i].Update();
                        }
                    }

                    // When there is no ble controller.  
                    if (!_blControl)
                    {
                        xwing.Angle += 1d;
                        xwing.ApplyThrustBasedOnAngle(0.1d);
                    }

                });
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            ncc1701.X = -200;
            xwing.X = 200;
            ncc1701.Bounce = true;
            xwing.Bounce = true;

            if ((_width == 0 || _height == 0) && Width > 0 && Height > 0)
            {
                _width = Width;
                _height = Height;
                UpdateLimits();
            }

            ncc1701.FireEvent += OnNcc1701FireEvent;
            xwing.FireEvent += OnXwingFireEvent;

            _aiPlayer = new AiPlayer(ncc1701, xwing);

            _updateViewTimer = new Timer(OnUpdateViewAsync, null,
                UpdateViewIntervalInMilliseconds, UpdateViewIntervalInMilliseconds);
            SdkManager.DiscoverServicesAsync(AppModel);

            AppModel.Movement.MovementChanged += OnMovementChanged;
        }

        private void OnMovementChanged(object sender, BLESDK.Model.MovementValue e)
        {
            _blControl = true;

            //System.Diagnostics.Debug.WriteLine("X" + e.AccX + "Y" + e.AccY + "Z" + e.AccZ);
            double angeChange = 50 * e.AccX + e.GyroY / 10;

            if (angeChange < -15)
                angeChange = -15;
            else if (angeChange > 15)
                angeChange = 15;

            double velocityChange = -e.AccY * 3; //+ e.GyroY / 8;

            if (velocityChange < 0)
                velocityChange = 0;

            xwing.Angle += angeChange;

            if (xwing.PotentialTotalVelocityIfApplied(velocityChange) <= 20d)
            {
                xwing.ApplyThrustBasedOnAngle(velocityChange);
            }


        }

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                _width = e.NewSize.Width;
                _height = e.NewSize.Height;
                UpdateLimits();
            }
        }
    }
}
