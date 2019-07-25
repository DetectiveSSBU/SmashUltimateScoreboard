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
using Microsoft.WindowsAPICodePack.Dialogs;
namespace SUStreamManager
{
    /// <summary>
    /// Interaction logic for DirectoryChange.xaml
    /// </summary>
    public partial class DirectoryChange : Window
    {
        string charPath;
        string sponsPath;
        string outPath;
        public DirectoryChange(string characterPath,string sponserPath,string outputPath)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbCharDir.Text = characterPath;
            tbSponserIconDir.Text = sponserPath;
            tbOutputDir.Text = outputPath;
            charPath = characterPath;
            sponsPath = sponserPath;
            outPath = outputPath;
        }

        public void SetDirectory(TextBox tb)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = tb.Text;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                tb.Text = dialog.FileName;
            }
            this.Focus();
        }

        public void SavePaths()
        {
            if (tbCharDir.Text != charPath)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Switching character folders will cause all players' character information to be reset\nIs this ok?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    charPath = tbCharDir.Text;
                }
            }

            outPath = tbOutputDir.Text;
            sponsPath = tbSponserIconDir.Text;
            this.DialogResult = true;
            this.Hide();

        }

        private void btnCharDir_Click(object sender, RoutedEventArgs e)
        {
            SetDirectory(tbCharDir);
        }

        private void btnSponserDir_Click(object sender, RoutedEventArgs e)
        {
            SetDirectory(tbSponserIconDir);
        }

        private void btnOutputDir_Click(object sender, RoutedEventArgs e)
        {
            SetDirectory(tbOutputDir);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SavePaths();
        }

        public string GetCharacterPath()
        {
            return charPath;
        }

        public string GetSponserPath()
        {
            return sponsPath;
        }

        public string GetOutputPath()
        {
            return outPath;
        }
    }
}
