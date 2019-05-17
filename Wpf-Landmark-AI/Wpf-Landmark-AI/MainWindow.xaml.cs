using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_Landmark_AI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png; *jgp)|*.png;*jpg;*jpeg|All Files (*.*)|*.*";

            if (dialog.ShowDialog().Value)
            {
                string fileName = dialog.FileName;

                selectedImage.Source = new BitmapImage(new Uri(fileName));

                MakePreditionAsync(fileName);
            }
        }

        private async void MakePreditionAsync(string fileName)
        {
            #region Set URL and Keys
            string url = "";
            string predictionKey = "";
            string contentType = "application/octet-stream";

            try
            {
                url = File.ReadAllText(@"C:\Users\cornie\Documents\Microsoft-CustomVision-AI\Image-URL.txt");
                predictionKey = File.ReadAllText(@"C:\Users\cornie\Documents\Microsoft-CustomVision-AI\Prediction-Key.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
            #endregion

            var file = File.ReadAllBytes(fileName);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                using (var content = new ByteArrayContent(file))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var response = await client.PostAsync(url, content);
                }
            }
        }
    }
}
