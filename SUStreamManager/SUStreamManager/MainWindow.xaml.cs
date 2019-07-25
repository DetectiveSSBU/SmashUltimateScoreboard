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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

/*
 * Name: Ben Taylor
 * Date: 7/13/2019
 * Program: Smash Ultimate Scoreboard
 * -TEMP DESCRIPTION-
 * 
 */

namespace SUStreamManager
{
 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Initialize();
        }

        //Main Player list
        SortedDictionary<string,Player> players = new SortedDictionary<string, Player>();
        //will hold character
        List<string> characterNames = new List<string>();
        //hold bracket types
        List<string> bracketTypes = new List<string>();
        //hold round types
        List<string> roundTypes = new List<string>();
        int player1score = 0;
        int player2score = 0;
        string outputPath;
        string characterPath;
        string sponserPath;
        bool isDoubles = false;
        PropertyList currentProperties;
        //Initialize all the main components of the program
        //will load data from text files to combo boxes etc...
        public void Initialize()
        {
            

            //get current saved directory (c:/) initially unless saved over
            outputPath = FileFunctions.ReadSavedDirectory();

            //get current character directory, default is ..\\..\\Resources\\Icons
            characterPath = FileFunctions.ReadFromLocation("..\\..\\Resources\\CharacterDirectory.txt");

            //get current sponser directory, default is c:\
            sponserPath = FileFunctions.ReadFromLocation("..\\..\\Resources\\SponserIconPath.txt");
            
            //read the characters into a list
            currentProperties = new PropertyList();
            //load property configuration
            FileFunctions.ReadConfigurationFromXML(ref currentProperties);

            if (!currentProperties.isSponserIcons)
            {
                cbPlayer1TeamIcon.Visibility = Visibility.Collapsed;
                cbPlayer2TeamIcon.Visibility = Visibility.Collapsed;
                lblPlayer1Sponsor.Visibility = Visibility.Collapsed;
                lblPlayer2Sponsor.Visibility = Visibility.Collapsed;
                cbTeam1TeamIcon.Visibility = Visibility.Collapsed;
                cbTeam2TeamIcon.Visibility = Visibility.Collapsed;

                cbPlayer1TeamIcon.Margin = new Thickness(10, 0, 0, 0);
                cbPlayer2TeamIcon.Margin = new Thickness(0, 0, 10, 0);
            }
            else
            {
                if (isDoubles)
                {
                    cbTeam1TeamIcon.Visibility = Visibility.Visible;
                    cbTeam2TeamIcon.Visibility = Visibility.Visible;
                    lblPlayer1Sponsor.Content = "Team1 Sponsors";
                    lblPlayer2Sponsor.Content = "Team2 Sponsors";

                    cbPlayer1TeamIcon.Margin = new Thickness(10, 0, 0, 30);
                    cbPlayer2TeamIcon.Margin = new Thickness(0, 0, 10, 30);
                }
                
                LoadPlayerSponsorIcons();
            }

            if (currentProperties.isTwitterHandle)
            {
                tbPlayer1Twitter.Visibility = Visibility.Visible;
                tbPlayer2Twitter.Visibility = Visibility.Visible;
                cbPlayerName1.Margin = new Thickness(10, 0, 0, 0);
                cbPlayerName2.Margin = new Thickness(0, 0, 10, 0);
                cbPlayerTeam1.Margin = new Thickness(110, 0, 0, 0);
                cbPlayerTeam2.Margin = new Thickness(0, 0, 110, 0);
            }
            else
            {
                tbPlayer1Twitter.Visibility = Visibility.Collapsed;
                tbPlayer2Twitter.Visibility = Visibility.Collapsed;
                cbPlayerName1.Margin = new Thickness(10, 15, 0, 0);
                cbPlayerName2.Margin = new Thickness(0, 15, 10, 0);
                cbPlayerTeam1.Margin = new Thickness(110, 15, 0, 0);
                cbPlayerTeam2.Margin = new Thickness(0, 15, 110, 0);
            }

            if (currentProperties.isCommentatorTags)
            {
                cbCommentator1.Visibility = Visibility.Visible;
                cbCommentator2.Visibility = Visibility.Visible;
                lblComm1.Visibility = Visibility.Visible;
                lblComm2.Visibility = Visibility.Visible;
            }
            else
            {
                cbCommentator1.Visibility = Visibility.Collapsed;
                cbCommentator2.Visibility = Visibility.Collapsed;
                lblComm1.Visibility = Visibility.Collapsed;
                lblComm2.Visibility = Visibility.Collapsed;

            }

            try
            {
                FileFunctions.ReadCharacters(ref characterNames, characterPath, true);
            }
            catch(Exception e)
            {
                MessageBox.Show("Could not load Character Icons...\n Please select a new directory in Options.","Error during loading",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            //populate the comboboxes
            foreach(string name in characterNames)
            {
                cbPlayer1Character.Items.Add(name);
                cbPlayer2Character.Items.Add(name);
            }
            //sets default character to first in the list
            cbPlayer1Character.SelectedIndex = 0;
            cbPlayer2Character.SelectedIndex = 0;

            //Initialize Round Combobox
            FileFunctions.ReadCharacters(ref roundTypes, "..\\..\\Resources\\Rounds.txt");
            //populate the comboboxes
            foreach (string name in roundTypes)
            {
                cbRound.Items.Add(name);
            }
            //sets default round to first in the list
            cbRound.SelectedIndex = 0;

            //Initialize Bracket Combobox
            FileFunctions.ReadCharacters(ref bracketTypes, "..\\..\\Resources\\BracketType.txt");
            //populate the comboboxes
            foreach (string name in bracketTypes)
            {
                cbBracket.Items.Add(name);
            }
            //sets default round to first in the list
            cbBracket.SelectedIndex = 0;

            //Call function to fill icon cb
            RefreshIconComboBox(1);
            RefreshIconComboBox(2);

            // load saved players from xml to dictionary
            FileFunctions.ReadPlayersFromXML(ref players, "..\\..\\Resources\\Players.xml");

            //populate combo boxes
            foreach (KeyValuePair<string, Player> entry in players)
            {
                cbPlayerName1.Items.Add(entry.Key);
                cbPlayerName2.Items.Add(entry.Key);
                cbPlayerTeam1.Items.Add(entry.Key);
                cbPlayerTeam2.Items.Add(entry.Key);
                cbCommentator1.Items.Add(entry.Key);
                cbCommentator2.Items.Add(entry.Key);
            }
        }

        //loads combobox with current character's icons
        public void RefreshIconComboBox(int playerNum)
        {
            List<Image> Icons = new List<Image>();
            
                if (playerNum == 1 && cbPlayer1Character.SelectedItem != null)
                {
                    //get all the icons

                    FileFunctions.ReadIcons(ref Icons, cbPlayer1Character.SelectedItem.ToString(), characterPath, isDoubles);

                    //clear the current icons
                    cbPlayer1Icon.Items.Clear();

                    //populate the new icons
                    foreach (Image img in Icons)
                    {
                        cbPlayer1Icon.Items.Add(img);
                    }
                    cbPlayer1Icon.SelectedIndex = 0;
                }
                else if (playerNum == 2 && cbPlayer2Character.SelectedItem != null)
                {

                    FileFunctions.ReadIcons(ref Icons, cbPlayer2Character.SelectedItem.ToString(), characterPath, isDoubles);

                    cbPlayer2Icon.Items.Clear();
                    foreach (Image img in Icons)
                    {
                        cbPlayer2Icon.Items.Add(img);
                    }
                    cbPlayer2Icon.SelectedIndex = 0;
                }
            
          
        }
        //Used to update the character and icon based on tag selected
        public void RefreshCharacter(int player)
        {
            //make sure there is a selected item
            if(player == 1 && cbPlayerName1.SelectedItem != null)
            {
                //pull information from dictionary using string in combobox as the key
                cbPlayer1Character.SelectedItem = players[cbPlayerName1.SelectedItem.ToString()].GetCharacter();
                cbPlayer1Icon.SelectedIndex = players[cbPlayerName1.SelectedItem.ToString()].GetAlt();
                tbPlayer1Twitter.Text = players[cbPlayerName1.SelectedItem.ToString()].GetTwitter();
            }
            else if (player == 2 && cbPlayerName2.SelectedItem != null)
            {
                cbPlayer2Character.SelectedItem = players[cbPlayerName2.SelectedItem.ToString()].GetCharacter();
                cbPlayer2Icon.SelectedIndex = players[cbPlayerName2.SelectedItem.ToString()].GetAlt();
                tbPlayer2Twitter.Text = players[cbPlayerName2.SelectedItem.ToString()].GetTwitter();
            }
        }

        //used to swap sides of the players
        public void SwapPlayers()
        {
            if (cbPlayerName1.SelectedItem != null && cbPlayerName2.SelectedItem != null)
            {

                //creating a lot of temporary variables and just switching information

                string tempName = cbPlayerName1.SelectedItem.ToString();
                string tempChar = cbPlayer1Character.SelectedItem.ToString();
                int tempAlt = cbPlayer1Icon.SelectedIndex;
                string tempScore = tbScore1.Text;
                int tempIntScore = player1score;
                bool tempInLosers = chkPlayer1Loser.IsChecked.Value;
                int tempLogoIndex = cbPlayer1TeamIcon.SelectedIndex;
                int tempTeamLogoIndex = cbTeam1TeamIcon.SelectedIndex;

                cbPlayerName1.SelectedItem = cbPlayerName2.SelectedItem;
                cbPlayer1Character.SelectedItem = cbPlayer2Character.SelectedItem;
                cbPlayer1Icon.SelectedIndex = cbPlayer2Icon.SelectedIndex;
                tbScore1.Text = tbScore2.Text;
                player1score = player2score;
                chkPlayer1Loser.IsChecked = chkPlayerLoser2.IsChecked.Value;
                cbPlayer1TeamIcon.SelectedIndex = cbPlayer2TeamIcon.SelectedIndex;
                cbTeam1TeamIcon.SelectedIndex = cbTeam2TeamIcon.SelectedIndex;

                cbPlayerName2.SelectedItem = tempName;
                cbPlayer2Character.SelectedItem = tempChar;
                cbPlayer2Icon.SelectedIndex = tempAlt;
                tbScore2.Text = tempScore;
                player2score = tempIntScore;
                chkPlayerLoser2.IsChecked = tempInLosers;
                cbPlayer2TeamIcon.SelectedIndex = tempLogoIndex;
                cbTeam2TeamIcon.SelectedIndex = tempTeamLogoIndex;
                //check to see if swap is needed for doubles
                if (cbPlayerTeam1.SelectedItem != null && cbPlayerTeam2.SelectedItem != null)
                {
                    string tempTeam1 = cbPlayerTeam1.SelectedItem.ToString();
                    cbPlayerTeam1.SelectedItem = cbPlayerTeam2.SelectedItem;
                    cbPlayerTeam2.SelectedItem = tempTeam1;
                }
                }

        }

        //changing directory
        public void ChangeDirectory()
        {
            //opens a new window to prompt for an output path
            TextOutputLocation to = new TextOutputLocation(outputPath);

            if(to.ShowDialog() == true)
            {
                outputPath = to.GetPath();
                FileFunctions.SaveDirectory(outputPath);

              
            }

            to.Close();
        }

        //adding players 
        public void AddPlayer()
        {
            //custom input window
            Input inputWindow = new Input();

          if(inputWindow.ShowDialog() == true)
          {
                //if name is not already in the combobox
                if (!cbPlayerName1.Items.Contains(inputWindow.getName()))
                {
                    //add player to comboboxes and dictionary
                    players.Add(inputWindow.getName(), new Player(inputWindow.getName()));

                    //remembering last selected players
                    var tempName1 = cbPlayerName1.SelectedItem;
                    var tempName2 = cbPlayerName2.SelectedItem;
                    var tempTeam1 = cbPlayerTeam1.SelectedItem;
                    var tempTeam2 = cbPlayerTeam2.SelectedItem;
                    var tempComm1 = cbCommentator1.SelectedItem;
                    var tempComm2 = cbCommentator2.SelectedItem;

                    //clearing the comboxes
                    cbPlayerName1.Items.Clear();
                    cbPlayerName2.Items.Clear();
                    cbPlayerTeam1.Items.Clear();
                    cbPlayerTeam2.Items.Clear();
                    cbCommentator1.Items.Clear();
                    cbCommentator2.Items.Clear();

                    //repopulated the combobxes because this was the easiest way to sort
                    //curtesy of using a sorted dictionary
                    foreach (KeyValuePair<string, Player> entry in players)
                    {
                        cbPlayerName1.Items.Add(entry.Key);
                        cbPlayerName2.Items.Add(entry.Key);
                        cbPlayerTeam1.Items.Add(entry.Key);
                        cbPlayerTeam2.Items.Add(entry.Key);
                        cbCommentator1.Items.Add(entry.Key);
                        cbCommentator2.Items.Add(entry.Key);
                    }

                    //double checking to make sure the comboboxes had values to begin with
                    if (tempName1 != null)
                        cbPlayerName1.SelectedItem = tempName1;
                    if (tempName2 != null)
                        cbPlayerName2.SelectedItem = tempName2;
                    if (tempTeam1 != null)
                        cbPlayerTeam1.SelectedItem = tempTeam1;
                    if (tempTeam2 != null)
                        cbPlayerTeam2.SelectedItem = tempTeam2;
                    if (tempComm1 != null)
                        cbCommentator1.SelectedItem = tempComm1;
                    if (tempComm2 != null)
                        cbCommentator2.SelectedItem = tempComm2;

                }
            }
         
            inputWindow.Close();
        }

        //Removing players
        public void RemovePlayer()
        {
            RemovePlayers remove = new RemovePlayers(players);

            if (remove.ShowDialog() == true)
            {
                //get list of names to remove
                List<string> namesToRemove = remove.GetPlayersToBeRemoved();

                if(namesToRemove.Count > 0)
                {
                    foreach(string name in namesToRemove)
                    {
                        players.Remove(name);
                        if(cbPlayerName1.SelectedItem !=null && cbPlayerName1.SelectedItem.ToString() == name)
                        {
                            cbPlayerName1.SelectedItem = null;
                        }
                        if (cbPlayerName2.SelectedItem != null && cbPlayerName2.SelectedItem.ToString() == name)
                        {
                            cbPlayerName2.SelectedItem = null;
                        }
                        cbPlayerName1.Items.Remove(name);
                        cbPlayerName2.Items.Remove(name);
                        cbPlayerTeam1.Items.Remove(name);
                        cbPlayerTeam2.Items.Remove(name);
                        cbCommentator1.Items.Remove(name);
                        cbCommentator2.Items.Remove(name);
                    }
                }
            }

            remove.Close();
        }

        public void UpdateRoundInformation()
        {
            //making certain options available or unavailable based on current round/bracket etc...
            if (cbRound.SelectedItem.ToString().Contains("Round"))
            {
                tbRoundNum.IsEnabled = true;
                tbRoundNum.Visibility = Visibility.Visible;
            }
            else
            {
                tbRoundNum.IsEnabled = false;
                tbRoundNum.Visibility = Visibility.Collapsed;
            }
            if (cbRound.SelectedItem.ToString().Contains("Grand"))
            {
                chkPlayer1Loser.IsEnabled = true;
                chkPlayerLoser2.IsEnabled = true;
                chkPlayer1Loser.Visibility = Visibility.Visible;
                chkPlayerLoser2.Visibility = Visibility.Visible;
            }
            else
            {
                chkPlayer1Loser.IsEnabled = false;
                chkPlayerLoser2.IsEnabled = false;
                chkPlayer1Loser.Visibility = Visibility.Collapsed;
                chkPlayerLoser2.Visibility = Visibility.Collapsed;
                chkPlayer1Loser.IsChecked = false;
                chkPlayerLoser2.IsChecked = false;
            }
        }

        //Update locally stored player information in the dictionary
        public void UpdatePlayer(string name, string character, int alt, string twitter)
        {
         
                players[name].SetCharacter(character);
                players[name].SetAlt(alt);
                players[name].setTwitter(twitter);
            
        }

        //will update all the text files at the specified directory
        public void UpdateTextFiles()
        {
            int roundNumber = 0;
            string roundString = cbRound.SelectedItem.ToString();
            if (cbRound.SelectedItem.ToString().Contains("Round"))
                roundNumber = Int32.Parse(tbRoundNum.Text);

            string name1 = cbPlayerName1.SelectedItem.ToString();
            string name2 = cbPlayerName2.SelectedItem.ToString();

            //check to see if doubles is checked
            if(isDoubles && cbPlayerTeam1.SelectedItem != null && cbPlayerTeam2.SelectedItem != null)
            {
                name1 += " & " + cbPlayerTeam1.SelectedItem.ToString();
                name2 = cbPlayerTeam2.SelectedItem.ToString() + " & " + name2;
            }

            if (cbBracket.SelectedItem.ToString() == "Money Match")
                roundString = "";

            Image teamIcon1 = (Image)cbPlayer1TeamIcon.SelectedItem;
            Image teamIcon2 = (Image)cbPlayer2TeamIcon.SelectedItem;

            Image teamTeamIcon1 = (Image)cbTeam1TeamIcon.SelectedItem;
            Image teamTeamIcon2 = (Image)cbTeam2TeamIcon.SelectedItem;

            if (!currentProperties.isSponserIcons)
            {
                teamIcon1 = null;
                teamIcon2 = null;
                teamTeamIcon1 = null;
                teamTeamIcon2 = null;
            }
            if (!isDoubles)
            {
                teamTeamIcon1 = null;
                teamTeamIcon2 = null;
            }
            string comm1 = "";
            string comm2 = "";

            if (cbCommentator1.SelectedItem != null)
                comm1 = cbCommentator1.SelectedItem.ToString();
            if (cbCommentator2.SelectedItem != null)
                comm2 = cbCommentator2.SelectedItem.ToString();

            //the big long messy function with too many parameters that writes all the scoreboard information
            FileFunctions.UpdateAllTextOutputFiles(outputPath, name1, Int32.Parse(tbScore1.Text), name2,
                Int32.Parse(tbScore2.Text), roundString, roundNumber, cbBracket.SelectedItem.ToString(), (bool) chkPlayer1Loser.IsChecked, 
                (bool) chkPlayerLoser2.IsChecked, (Image) cbPlayer1Icon.SelectedItem, (Image)cbPlayer2Icon.SelectedItem, teamIcon1, teamIcon2,
                tbPlayer1Twitter.Text,tbPlayer2Twitter.Text, currentProperties.isTwitterHandle,comm1,comm2, currentProperties.isCommentatorTags,
                teamTeamIcon1, teamTeamIcon2);

            //save current player information to an xml file
            FileFunctions.SavePlayersToXML(players);
        }

        //checks to see if doubles has been selected and update the layout accordingly
        public void CheckForDoubles()
        {
            //changes layout of window for doubles usage
            if(cbBracket.SelectedItem.ToString() == "Doubles Bracket")
            {
                isDoubles = true;
                cbPlayerName1.Width = 100;
                cbPlayerTeam1.Visibility = Visibility.Visible;

                cbPlayerName2.Width = 100;
                cbPlayerTeam2.Visibility = Visibility.Visible;

                lblSide1.Content = "Team 1";
                lblSide2.Content = "Team 2";

                //cbPlayer1Character.SelectedItem = "Yoshi";
                //cbPlayer2Character.SelectedItem = "Yoshi";

                if (currentProperties.isSponserIcons)
                {
                    cbTeam1TeamIcon.Visibility = Visibility.Visible;
                    cbTeam2TeamIcon.Visibility = Visibility.Visible;
                    lblPlayer1Sponsor.Content = "Team1 Sponsors";
                    lblPlayer2Sponsor.Content = "Team2 Sponsors";

                    cbPlayer1TeamIcon.Margin = new Thickness(10, 0, 0, 30);
                    cbPlayer2TeamIcon.Margin = new Thickness(0, 0, 10, 30);
                }

                cbPlayer1Icon.SelectedIndex = 0;
                cbPlayer2Icon.SelectedIndex = 0;
                cbPlayer1Character.Visibility = Visibility.Collapsed;
                cbPlayer2Character.Visibility = Visibility.Collapsed;
                RefreshIconComboBox(1);
                RefreshIconComboBox(2);
            }
            else
            {
                isDoubles = false;
                cbPlayerName1.Width = 200;
                cbPlayerTeam1.Visibility = Visibility.Collapsed;

                cbPlayerName2.Width = 200;
                cbPlayerTeam2.Visibility = Visibility.Collapsed;

                lblSide1.Content = "Player 1";
                lblSide2.Content = "Player 2";

                cbPlayer1Character.Visibility = Visibility.Visible;
                cbPlayer2Character.Visibility = Visibility.Visible;

                if (currentProperties.isSponserIcons)
                {
                    cbTeam1TeamIcon.Visibility = Visibility.Collapsed;
                    cbTeam2TeamIcon.Visibility = Visibility.Collapsed;
                    lblPlayer1Sponsor.Content = "P1 Sponsor";
                    lblPlayer2Sponsor.Content = "P2 Sponsor";

                    cbPlayer1TeamIcon.Margin = new Thickness(10, 0, 0, 0);
                    cbPlayer2TeamIcon.Margin = new Thickness(0, 0, 10, 0);
                }

                RefreshCharacter(1);
                RefreshCharacter(2);
                RefreshIconComboBox(1);
                RefreshIconComboBox(2);
                if (cbPlayerName1.SelectedItem != null && cbPlayerName2.SelectedItem != null)
                {
                    cbPlayer1Icon.SelectedIndex = players[cbPlayerName1.SelectedItem.ToString()].GetAlt();
                    cbPlayer2Icon.SelectedIndex = players[cbPlayerName2.SelectedItem.ToString()].GetAlt();
                }
            }
        }

        //this is gonna be a nightmare, oh well...
        public void ViewDirectories()
        {
            DirectoryChange viewDirectories = new DirectoryChange(characterPath, sponserPath,outputPath);

            if (viewDirectories.ShowDialog() == true)
            {
                string returnedPath = viewDirectories.GetCharacterPath();
                //oh boy, now we get to switch all the characters and refresh everything
                if(returnedPath != characterPath)
                {
                    characterPath = returnedPath;
                    //clear original items
                    characterNames.Clear();
                    cbPlayer1Character.Items.Clear();
                    cbPlayer2Character.Items.Clear();
                    //read the characters into a list
                    FileFunctions.ReadCharacters(ref characterNames, characterPath, true);
                    //populate the comboboxes
                    foreach (string name in characterNames)
                    {
                        cbPlayer1Character.Items.Add(name);
                        cbPlayer2Character.Items.Add(name);
                    }
                    //sets default character to first in the list
                    cbPlayer1Character.SelectedIndex = 0;
                    cbPlayer2Character.SelectedIndex = 0;

                    //reset all players' character to a default
                    foreach (KeyValuePair<string, Player> entry in players)
                    {
                        entry.Value.SetCharacter(cbPlayer1Character.Text);
                        entry.Value.SetAlt(0);
                    }

                    RefreshIconComboBox(1);
                    RefreshIconComboBox(2);

                    //save changes to character location text file
                    File.WriteAllText("..//..//Resources//CharacterDirectory.txt", characterPath);

                }
                File.WriteAllText("..//..//Resources//SponserIconPath.txt", viewDirectories.GetSponserPath());
                outputPath = viewDirectories.GetOutputPath();
                sponserPath = viewDirectories.GetSponserPath();
            }
            viewDirectories.Close();

        }

        public void LoadPlayerSponsorIcons()
        {
            try
            {
                List<Image> images = new List<Image>();
                FileFunctions.ReadTeamLogos(ref images, sponserPath);

                cbPlayer1TeamIcon.Items.Clear();
                cbPlayer2TeamIcon.Items.Clear();
                cbTeam1TeamIcon.Items.Clear();
                cbTeam2TeamIcon.Items.Clear();
                //populate the new icons

                foreach (Image img in images)
                {
                    cbPlayer1TeamIcon.Items.Add(img);
                    
                }
                FileFunctions.ReadTeamLogos(ref images, sponserPath);
                
                foreach (Image img in images)
                {
                    cbPlayer2TeamIcon.Items.Add(img);

                }
                FileFunctions.ReadTeamLogos(ref images, sponserPath);

                foreach (Image img in images)
                {
                    cbTeam1TeamIcon.Items.Add(img);

                }
                FileFunctions.ReadTeamLogos(ref images, sponserPath);

                foreach (Image img in images)
                {
                    cbTeam2TeamIcon.Items.Add(img);

                }
                //cbPlayer1TeamIcon.SelectedIndex = 0;
                //cbPlayer2TeamIcon.SelectedIndex = 0;
            }
            catch(Exception e)
            {
                currentProperties.isSponserIcons = false;
                MessageBox.Show("Could not load logos from desired location...\nPlease try again","Error loading logos!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        public void OpenProperties()
        {
            PropertyWindow properties= new PropertyWindow(currentProperties);
            this.Topmost = false;
            if (properties.ShowDialog() == true)
            {
                currentProperties = properties.GetPendingProperties();

                FileFunctions.SaveConfigurationToXML(currentProperties);

                if(!currentProperties.isWindowOnTop)
                    this.Topmost = false;

                if (!currentProperties.isSponserIcons)
                {
                    cbPlayer1TeamIcon.Visibility = Visibility.Collapsed;
                    cbPlayer2TeamIcon.Visibility = Visibility.Collapsed;
                    lblPlayer1Sponsor.Visibility = Visibility.Collapsed;
                    lblPlayer2Sponsor.Visibility = Visibility.Collapsed;
                    cbTeam1TeamIcon.Visibility = Visibility.Collapsed;
                    cbTeam2TeamIcon.Visibility = Visibility.Collapsed;
                    lblPlayer1Sponsor.Content = "P1 Sponsor";
                    lblPlayer2Sponsor.Content = "P2 Sponsor";
                    cbPlayer1TeamIcon.Margin = new Thickness(10, 0, 0, 0);
                    cbPlayer2TeamIcon.Margin = new Thickness(0, 0, 10, 0);
                }
                else
                {
                    cbPlayer1TeamIcon.Visibility = Visibility.Visible;
                    cbPlayer2TeamIcon.Visibility = Visibility.Visible;
                    lblPlayer1Sponsor.Visibility = Visibility.Visible;
                    lblPlayer2Sponsor.Visibility = Visibility.Visible;
                    if (isDoubles)
                    {
                        cbTeam1TeamIcon.Visibility = Visibility.Visible;
                        cbTeam2TeamIcon.Visibility = Visibility.Visible;
                        lblPlayer1Sponsor.Content = "Team1 Sponsors";
                        lblPlayer2Sponsor.Content = "Team2 Sponsors";

                        cbPlayer1TeamIcon.Margin = new Thickness(10, 0, 0, 30);
                        cbPlayer2TeamIcon.Margin = new Thickness(0, 0, 10, 30);
                    }
                    LoadPlayerSponsorIcons();
                }

                if (currentProperties.isTwitterHandle)
                {
                    tbPlayer1Twitter.Visibility = Visibility.Visible;
                    tbPlayer2Twitter.Visibility = Visibility.Visible;
                    cbPlayerName1.Margin = new Thickness(10, 0, 0, 0);
                    cbPlayerName2.Margin = new Thickness(0, 0, 10, 0);
                    cbPlayerTeam1.Margin = new Thickness(110, 0, 0, 0);
                    cbPlayerTeam2.Margin = new Thickness(0, 0, 110, 0);
                }
                else
                {
                    tbPlayer1Twitter.Visibility = Visibility.Collapsed;
                    tbPlayer2Twitter.Visibility = Visibility.Collapsed;
                    cbPlayerName1.Margin = new Thickness(10, 15, 0, 0);
                    cbPlayerName2.Margin = new Thickness(0, 15, 10, 0);

                    cbPlayerTeam1.Margin = new Thickness(110, 15, 0, 0);
                    cbPlayerTeam2.Margin = new Thickness(0, 15, 110, 0);
                }

                if (currentProperties.isCommentatorTags)
                {
                    cbCommentator1.Visibility = Visibility.Visible;
                    cbCommentator2.Visibility = Visibility.Visible;
                    lblComm1.Visibility = Visibility.Visible;
                    lblComm2.Visibility = Visibility.Visible;
                }
                else
                {
                    cbCommentator1.Visibility = Visibility.Collapsed;
                    cbCommentator2.Visibility = Visibility.Collapsed;
                    lblComm1.Visibility = Visibility.Collapsed;
                    lblComm2.Visibility = Visibility.Collapsed;

                }

            }

            properties.Close();
        }


        public void OpenModifyTextWindow(List<string> content)
        {

            
            ModifyText modifyTextWindow = new ModifyText(content);

            if (modifyTextWindow.ShowDialog() == true)
            {

            }

            modifyTextWindow.Close();
            }

        //event handlers

        private void cbPlayer1Character_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!isDoubles)
                RefreshIconComboBox(1);
        }

        private void cbPlayer2Character_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isDoubles)
                RefreshIconComboBox(2);
        }

        //will grey out the Round text box if its not required 
        //also check for Grand Finals [L]
        private void cbRound_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRoundInformation();
        }

        private void cbPlayerName1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshCharacter(1);
        }

        private void cbPlayerName2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshCharacter(2);
        }

        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            SwapPlayers();
        }

        private void btnP1Up_Click(object sender, RoutedEventArgs e)
        {
            player1score++;
            tbScore1.Text = player1score.ToString();
        }

        private void btnP1Down_Click(object sender, RoutedEventArgs e)
        {
            player1score--;
            tbScore1.Text = player1score.ToString();
        }

        private void btnPlayer2Up_Click(object sender, RoutedEventArgs e)
        {
            player2score++;
            tbScore2.Text = player2score.ToString();
        }

        private void btnPlayer2Down_Click(object sender, RoutedEventArgs e)
        {
            player2score--;
            tbScore2.Text = player2score.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPlayer();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            RemovePlayer();
        }

        private void cbPlayer1Icon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void cbPlayer2Icon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //update local player information
            if ( !isDoubles)
            {
                if (cbPlayerName1.SelectedItem != null)
                {
                    UpdatePlayer(cbPlayerName1.SelectedItem.ToString(),
                        cbPlayer1Character.SelectedItem.ToString(),
                        cbPlayer1Icon.SelectedIndex, tbPlayer1Twitter.Text);
                }
                if (cbPlayerName2.SelectedItem != null)
                {
                    UpdatePlayer(cbPlayerName2.SelectedItem.ToString(),
                        cbPlayer2Character.SelectedItem.ToString(),
                        cbPlayer2Icon.SelectedIndex,tbPlayer2Twitter.Text);
                }
            }
            //Update Text Files
            if (cbPlayerName1.SelectedItem != null && cbPlayerName2.SelectedItem != null)
                UpdateTextFiles();
        }

        //save everything to the player xml file as you're closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileFunctions.SavePlayersToXML(players);
        }

        private void btnOutput_Click(object sender, RoutedEventArgs e)
        {
            ChangeDirectory();
        }

        private void cbBracket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckForDoubles();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewDirectories();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenProperties();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (currentProperties.isWindowOnTop)
            {
                this.Topmost = true;
                this.Activate();
            }
            else
            {
                this.Topmost = false;
            }
        }

        private void btnAddRemoveBracket_Click(object sender, RoutedEventArgs e)
        {
            OpenModifyTextWindow(bracketTypes);
        }

        private void btnAddRemoveRound_Click(object sender, RoutedEventArgs e)
        {
            OpenModifyTextWindow(roundTypes);
        }
    }

    
}
