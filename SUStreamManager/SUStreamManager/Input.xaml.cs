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
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Window
    {
        public Input()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbInput.Focus();
        }

        List<string> playerNames = new List<string>();

        public List<string> getNames()
        {
            return playerNames;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            for(int i = 0; i < tbInput.LineCount; i++)
            {
                string temp = tbInput.GetLineText(i);
                if (temp.Contains("\n"))
                    temp = temp.Remove(temp.Length - 1);
                if (temp.Contains("\r"))
                    temp = temp.Remove(temp.Length - 1);
                if (temp.Length > 0)
                    playerNames.Add(temp);
            }

            this.DialogResult = true;
            this.Hide();
        }

        
    }
}
