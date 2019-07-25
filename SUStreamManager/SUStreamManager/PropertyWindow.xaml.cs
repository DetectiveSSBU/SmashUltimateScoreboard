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
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    public partial class PropertyWindow : Window
    {
        PropertyList pendingProperties;

        public PropertyWindow(PropertyList currentProperties)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            chkCommentator.IsChecked = currentProperties.isCommentatorTags;
            chkSponserIcons.IsChecked = currentProperties.isSponserIcons;
            chkTwitterHandles.IsChecked = currentProperties.isTwitterHandle;
            chkWindowOnTop.IsChecked = currentProperties.isWindowOnTop;
            //rdbFlags.IsChecked = currentProperties.isFlags;
            //rdbStocks.IsChecked = !currentProperties.isFlags;
            pendingProperties = currentProperties;
        }


        public void UpdateProperties()
        {
            pendingProperties.isCommentatorTags = chkCommentator.IsChecked.Value;
            pendingProperties.isFlags = true;
            pendingProperties.isSponserIcons = chkSponserIcons.IsChecked.Value;
            pendingProperties.isTwitterHandle = chkTwitterHandles.IsChecked.Value;
            pendingProperties.isWindowOnTop = chkWindowOnTop.IsChecked.Value;
        }

        public PropertyList GetPendingProperties()
        {
            return pendingProperties;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            UpdateProperties();
            this.DialogResult = true;
            this.Hide();
        }
    }
}
