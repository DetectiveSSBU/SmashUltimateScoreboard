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
    /// Interaction logic for RemovePlayers.xaml
    /// </summary>
    public partial class RemovePlayers : Window
    {
        List<string> playersToRemove;
        public RemovePlayers()
        {
            InitializeComponent();
        }

        

        public RemovePlayers(SortedDictionary<string,Player> players)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            playersToRemove = new List<string>();
            foreach (KeyValuePair<string,Player> player in players)
            {
                lbPlayers.Items.Add(player.Key);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
           // playersToRemove = (List<string>) lbPlayers.SelectedItems;
            foreach(var item in lbPlayers.SelectedItems)
            {
                playersToRemove.Add(item.ToString());
            }

            this.DialogResult = true;
            this.Hide();
        }

        public List<string> GetPlayersToBeRemoved()
        {
            return playersToRemove;
        }
    }
}
