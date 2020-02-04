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

            tbInput.Clear();

            foreach(string item in contents)
            {
                
                tbInput.Text += item+"\n";
            }

        }

        public List<string> GetOutputContents()
        {
            return outputContents;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //get contents

            for(int i = 0; i < tbInput.LineCount; i++)
            {
                
                string temp = tbInput.GetLineText(i);
                if (temp.Contains("\n"))
                    temp = temp.Remove(temp.Length - 1 );
                if(temp.Length > 0)
                outputContents.Add(temp);
            }
            
            this.DialogResult = true;
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
