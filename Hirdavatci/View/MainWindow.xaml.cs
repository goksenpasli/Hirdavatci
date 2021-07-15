using System;
using System.IO;
using System.Windows;

namespace Hirdavatci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void CameraUserControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (DataContext is MainViewModel data && e.PropertyName == "ResimData")
            {
                string filename = Guid.NewGuid() + ".jpg";
                File.WriteAllBytes($"{Path.GetDirectoryName(ExtensionMethods.xmldatapath)}\\{filename}", (sender as CameraUserControl)?.ResimData);
                data.Malzeme.ResimYolu = filename;
            }
        }
    }
}
