using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
namespace SUStreamManager
{
    /// <summary>
    /// Interaction logic for TextOutputLocation.xaml
    /// </summary>
    public partial class TextOutputLocation : Window
    {
        public string outputPath;

        public TextOutputLocation()
        {
            InitializeComponent();
        }

        public TextOutputLocation(string path)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbInput.Text = path;
            outputPath = path;
        }

        public string GetPath()
        {
            return outputPath;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            outputPath = tbInput.Text;
            this.DialogResult = true;
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDirectory_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = outputPath;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                tbInput.Text = dialog.FileName;
            }
            this.Focus();
        }
    }
}
