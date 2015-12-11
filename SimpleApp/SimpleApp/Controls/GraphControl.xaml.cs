using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace SimpleApp.Controls
{
    public sealed partial class GraphControl : UserControl, INotifyPropertyChanged
    {
        private const int GraphWidthInRespectToTimeInSeconds = 5;
        private IList<DateTimeOffset> _timestamps;
        private IList<double> _values;
        private double _width;
        private double _height;
        private double _widthOfMillisecond;
        private int _latestIndex;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        private string _title;

        public double Min
        {
            get
            {
                return (double)GetValue(MinProperty);
            }
            set
            {
                SetValue(MinProperty, value);
            }
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(GraphControl),
                new PropertyMetadata(double.MaxValue, OnMinMaxPropertyChanged));

        private static void OnMinMaxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GraphControl control = d as GraphControl;

            if (control != null)
            {
                control.Rescale();
            }
        }

        public double Max
        {
            get
            {
                return (double)GetValue(MaxProperty);
            }
            set
            {
                SetValue(MaxProperty, value);
            }
        }
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(GraphControl),
                new PropertyMetadata(double.MinValue, OnMinMaxPropertyChanged));

        public GraphControl()
        {
            InitializeComponent();

            Loaded += OnGraphControlLoaded;
            SizeChanged += OnGraphControlSizeChanged;

            _timestamps = new List<DateTimeOffset>();
            _values = new List<double>();
        }

        private void OnGraphControlLoaded(object sender, RoutedEventArgs e)
        {
            double graphContainerWidth = graphContainerGrid.ActualWidth;
            double graphContainerHeight = graphContainerGrid.ActualHeight;

            if ((_width == 0 || _widthOfMillisecond == 0) && graphContainerWidth > 0)
            {

                _width = graphContainerWidth;
                _widthOfMillisecond = graphContainerWidth / (GraphWidthInRespectToTimeInSeconds * 1000);
            }

            if (graphContainerHeight > 0)
            {
                _height = graphContainerHeight;
            }
        }

        private void OnGraphControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                double graphContainerWidth = graphContainerGrid.ActualWidth;
                double graphContainerHeight = graphContainerGrid.ActualHeight;
                System.Diagnostics.Debug.WriteLine("OnGraphControlSizeChanged: " + graphContainerWidth + "x" + graphContainerHeight);

                _width = graphContainerWidth;
                _height = graphContainerHeight;
                _widthOfMillisecond = graphContainerWidth / (GraphWidthInRespectToTimeInSeconds * 1000);
                Rescale();
            }
        }

        public void AddValue(double value)
        {
            DateTimeOffset timestamp = DateTimeOffset.Now;
            double pointX = 0d;

            if (_latestIndex > 0)
            {
                long totalMillisecondsPreviously = _timestamps[_latestIndex - 1].Ticks / TimeSpan.TicksPerMillisecond;
                long totalMillisecondsNow = timestamp.Ticks / TimeSpan.TicksPerMillisecond;
                double deltaX = (totalMillisecondsNow - totalMillisecondsPreviously) * _widthOfMillisecond;
                pointX = polyline.Points[_latestIndex - 1].X + deltaX;
                //System.Diagnostics.Debug.WriteLine("New value is " + value + ", delta X is " + deltaX + ", new X is " + pointX);
            }

            _timestamps.Add(timestamp);
            _values.Add(value);

            AddNewPoint(pointX, value);

            _latestIndex++;
        }

        private void AddNewPoint(double pointX, double value)
        {
            if (_width > 0 && pointX > _width && _values.Count > 1)
            {
                // Delete the first point
                double secondPointX = polyline.Points[1].X;
                pointX -= secondPointX;
                Point point;

                for (int i = 1; i < polyline.Points.Count; ++i)
                {
                    point = polyline.Points[i];
                    point.X -= secondPointX;
                    polyline.Points[i] = point;
                }

                _timestamps.RemoveAt(0);
                _values.RemoveAt(0);
                polyline.Points.RemoveAt(0);

                _latestIndex--;
            }

            polyline.Points.Add(new Point(pointX, _height - value));
            Rescale();
        }

        private void Rescale()
        {
            if (_widthOfMillisecond > 0 && _height > 0)
            {
                int pointCount = polyline.Points.Count;
                bool okToScale = false;
                double min = 0d;
                double max = 0d;

                if (Min != double.MaxValue && Max != double.MinValue && Min < Max)
                {
                    min = Min;
                    max = Max;
                    okToScale = true;
                }
                else if (pointCount > 1)
                {
                    GetMinMaxValues(ref min, ref max);
                    okToScale = true;
                }

                if (okToScale)
                {
                    double scaleCoefficient = _height / (max - min);
                    PointCollection points = polyline.Points;
                    Point point;

                    for (int i = 0; i < pointCount; ++i)
                    {
                        point = points[i];
                        point.Y = _height - (_values[i] - min) * scaleCoefficient;
                        points[i] = point;
                    }

                    maxValueTextBlock.Text = Math.Round(max, 2).ToString();
                    minValueTextBlock.Text = Math.Round(min, 2).ToString();
                }
            }
        }

        private void GetMinMaxValues(ref double min, ref double max)
        {
            min = double.MaxValue;
            max = double.MinValue;

            foreach (double value in _values)
            {
                if (value < min)
                {
                    min = value;
                }

                if (value > max)
                {
                    max = value;
                }
            }

            if (min > max)
            {
                min = 0d;
                max = 0d;
            }
        }
    }
}
