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

namespace SUStreamManager
{
    /// <summary>
    /// Interaction logic for ModifyText.xaml
    /// </summary>
    public partial class ModifyText : Window
    {

        List<string> outputContents;

        public ModifyText(List<string> contents)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            outputContents = new List<string>();

            rtbInput.Document.Blocks.Clear();

            foreach(string item in contents)
            {
                rtbInput.Document.Blocks.Add(new Paragraph(new Run(item)));
            }

        }

        public List<string> GetOutputContents()
        {
            return outputContents;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //get contents
            //outputContents = tbInput.Text;
            this.DialogResult = true;
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
