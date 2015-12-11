using BLESDK.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SimpleApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccelometerPage : Page
    {
        public SimpleAppModel AppModel
        {
            get
            {
                return SimpleAppModel.Instance;
            }
        }

        public AccelometerPage()
        {
            this.InitializeComponent();
        }

        override protected void OnNavigatedFrom(NavigationEventArgs e)
        {
            AppModel.Movement.MovementChanged -= Movement_MovementChanged;
        }

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            AppModel.Movement.MovementChanged += Movement_MovementChanged;
        }

        private void Movement_MovementChanged(object sender, MovementValue e)
        {
            System.Diagnostics.Debug.WriteLine("x:" + e.AccX.ToString() + " y:" + e.AccY.ToString() + " z:" + e.AccZ.ToString());
            graphCtrl1.AddValue(e.AccX);
            graphCtrl2.AddValue(e.AccY);
            graphCtrl3.AddValue(e.AccZ);
        }
    }


}
