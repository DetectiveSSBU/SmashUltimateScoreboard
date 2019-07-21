using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace SUStreamManager
{
    public class FileFunctions
    {

        public static void ReadCharacters(ref List<string> c, string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            
            foreach (string line in lines)
            {
                c.Add(line);
            }

                
        }

        public static void ReadIcons(ref List<Image> c, string characterName)
        {
            string curDir = Directory.GetCurrentDirectory();
            curDir = Directory.GetParent(curDir).ToString();
            curDir = Directory.GetParent(curDir).ToString();
            string[] fileNames = Directory.GetFiles(curDir+"\\Resources\\Icons\\"+characterName);
            c.Clear();
            foreach (string file in fileNames)
            {
                Image imgTemp = new Image();
                imgTemp.Height = 32;
                imgTemp.Width = 32;
                imgTemp.Visibility = System.Windows.Visibility.Visible;
                //var uri = new Uri("C:\\Users\\Ben\\Documents\\Visual Studio 2017\\Projects\\SUStreamManager\\SUStreamManager\\Resources\\Icons\\Bayonetta\\chara_2_bayonetta_00.png");
                //var bitmap = new BitmapImage(new Uri(file));
                //imgTemp.Source = bitmap;

                imgTemp.Source = new BitmapImage(new Uri(file));
                c.Add(imgTemp);
            }


        }

        public static string ReadSavedDirectory()
        {
            string curDir = Directory.GetCurrentDirectory();
            curDir = Directory.GetParent(curDir).ToString();
            curDir = Directory.GetParent(curDir).ToString();
            string[] lines = File.ReadAllLines(curDir + "\\Resources\\OutputDirectory.txt");

            return lines[0];
        }

        public static void SaveDirectory(string directory)
        {
            string curDir = Directory.GetCurrentDirectory();
            curDir = Directory.GetParent(curDir).ToString();
            curDir = Directory.GetParent(curDir).ToString();
            string[] temp = { directory }; 
            File.WriteAllLines(curDir + "\\Resources\\OutputDirectory.txt", temp);
        }

        public static void ReadPlayersFromXML(ref SortedDictionary<string,Player> p, string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //player nodes
            foreach (XmlNode playerNode in doc.DocumentElement.ChildNodes)
            {
                Player tempPlayer = new Player();
                //parse through each player node
                foreach (XmlNode node in playerNode.ChildNodes)
                {
                    
                    if (node.Name == "Tag")
                        tempPlayer.SetTag(node.InnerText);
                    if (node.Name == "Character")
                        tempPlayer.SetCharacter(node.InnerText);
                    if (node.Name == "Alt")
                        tempPlayer.SetAlt(Int32.Parse( node.InnerText));

                  
                }
                
                p.Add(tempPlayer.GetTag(),tempPlayer);

            }
        }

        public static void SavePlayersToXML(SortedDictionary<string,Player> players)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode playersNode = doc.CreateElement("Players");
            doc.AppendChild(playersNode);

            foreach (KeyValuePair<string, Player> p in players)
            {

                XmlNode playerNode = doc.CreateElement("Player");

                XmlNode tagNode = doc.CreateElement("Tag");
                tagNode.InnerText = p.Key;
                XmlNode characterNode = doc.CreateElement("Character");
                characterNode.InnerText = p.Value.GetCharacter();
                XmlNode altNode = doc.CreateElement("Alt");
                altNode.InnerText = p.Value.GetAlt().ToString();

                playerNode.AppendChild(tagNode);
                playerNode.AppendChild(characterNode);
                playerNode.AppendChild(altNode);

                playersNode.AppendChild(playerNode);

            }

            doc.Save("..\\..\\Resources\\Players.xml");
        }

        //holy fuck that's a lot of parameters...
        public static void UpdateAllTextOutputFiles(string outputPath ,string player1Name, int player1Score,
                                                    string player2Name, int player2Score, string round,
                                                    int roundNum, string bracket, bool isP1Loser, bool isP2Loser,
                                                    Image p1Icon, Image p2Icon)
        {
            //Let's start this mess
            //p1 name
            try
            {
                if (isP1Loser)
                    player1Name += " [L]";
                File.WriteAllText(outputPath + "\\p1.txt", player1Name);
                //p2 name
                if (isP2Loser)
                    player2Name += " [L]";
                File.WriteAllText(outputPath + "\\p2.txt", player2Name);
                //p1 score
                File.WriteAllText(outputPath + "\\p1score.txt", player1Score.ToString());
                //p2 score
                File.WriteAllText(outputPath + "\\p2score.txt", player2Score.ToString());
                //round
                if (roundNum > 0)
                    round += " " + roundNum.ToString();
                File.WriteAllText(outputPath + "\\round.txt", round);
                //bracket
                File.WriteAllText(outputPath + "\\bracket.txt", bracket);

                //Images
                string uriPath1 = new Uri(p1Icon.Source.ToString()).LocalPath;
                string uriPath2 = new Uri(p2Icon.Source.ToString()).LocalPath;
                if (File.Exists(outputPath + "\\p1Char.png"))
                    File.Delete(outputPath + "\\p1Char.png");
                if (File.Exists(outputPath + "\\p2Char.png"))
                    File.Delete(outputPath + "\\p2Char.png");

                File.Copy(uriPath1, outputPath + "\\p1Char.png");
                File.Copy(uriPath2, outputPath + "\\p2Char.png");
                File.SetLastWriteTime(outputPath + "\\p1Char.png", DateTime.Now);
                File.SetLastWriteTime(outputPath + "\\p2Char.png", DateTime.Now);

            }
            catch(Exception e)
            {
                //display message here?
                //nah
            }
        }
    }
}
