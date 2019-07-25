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

        public static void ReadCharacters(ref List<string> c, string filePath,bool isDirectories = false)
        {

            if (isDirectories)
            {
                string[] directories = Directory.GetDirectories(filePath);
              
                foreach (string line in directories)
                {
                    string temp = new DirectoryInfo(line).Name;
                    c.Add(temp);
                }
            }
            else
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    c.Add(line);
                }
            }
                
        }

        public static void ReadTeamLogos(ref List<Image> c, string newPath)
        {
            if(newPath == "..\\..\\Resources\\Logos")
            {
                string curDir = Directory.GetCurrentDirectory();
                curDir = Directory.GetParent(curDir).ToString();
                curDir = Directory.GetParent(curDir).ToString();
                newPath = curDir + "\\Resources\\Logos";
            }

            string[] fileNames = Directory.GetFiles(newPath);
            c.Clear();
            foreach (string file in fileNames)
            {
                Image imgTemp = new Image();
                imgTemp.Height = 32;
                imgTemp.Width = 32;
                imgTemp.Visibility = System.Windows.Visibility.Visible;
               
                imgTemp.Source = new BitmapImage(new Uri(file));
                c.Add(imgTemp);
            }

        }

        public static void ReadIcons(ref List<Image> c, string characterName, string newPath, bool isDubs = false)
        {
            

            string fullPath;
            if (!isDubs)
            {
                if (newPath == "..\\..\\Resources\\Icons")
                {
                    string curDir = Directory.GetCurrentDirectory();
                    curDir = Directory.GetParent(curDir).ToString();
                    curDir = Directory.GetParent(curDir).ToString();
                    fullPath = curDir + "\\Resources\\Icons\\" + characterName;
                }
                else
                {
                    fullPath = newPath + "\\" + characterName;
                }
            }
            else
            {
                string curDir = Directory.GetCurrentDirectory();
                curDir = Directory.GetParent(curDir).ToString();
                curDir = Directory.GetParent(curDir).ToString();
                fullPath = curDir + "\\Resources\\TeamIcons";
            }

            string[] fileNames = Directory.GetFiles(fullPath);
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

        public static string ReadFromLocation(string path)
        {
            string[] lines = File.ReadAllLines(path);

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
                    if (node.Name == "Twitter")
                        tempPlayer.setTwitter(node.InnerText);


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
                XmlNode twitterHandleNode = doc.CreateElement("Twitter");
                twitterHandleNode.InnerText = p.Value.GetTwitter();


                playerNode.AppendChild(tagNode);
                playerNode.AppendChild(characterNode);
                playerNode.AppendChild(altNode);
                playerNode.AppendChild(twitterHandleNode);
                playersNode.AppendChild(playerNode);

            }

            doc.Save("..\\..\\Resources\\Players.xml");
        }

        public static void ReadConfigurationFromXML(ref PropertyList properties)
        {
            string curDir = Directory.GetCurrentDirectory();
            curDir = Directory.GetParent(curDir).ToString();
            curDir = Directory.GetParent(curDir).ToString();

            XmlDocument doc = new XmlDocument();
            doc.Load(curDir + "\\Resources\\config.xml");
            
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {

                    if (node.Name == "TeamFlagsEnabled")
                        properties.isFlags = Convert.ToBoolean(node.InnerText);
                    if (node.Name == "CommentatorTagsEnabled")
                        properties.isCommentatorTags = Convert.ToBoolean(node.InnerText);
                    if (node.Name == "TwitterHandleEnabled")
                        properties.isTwitterHandle = Convert.ToBoolean(node.InnerText);
                    if (node.Name == "SponserIconsEnabled")
                        properties.isSponserIcons = Convert.ToBoolean(node.InnerText);
                    if (node.Name == "KeepWindowOnTopEnabled")
                        properties.isWindowOnTop = Convert.ToBoolean(node.InnerText);


                }
            
        }

        public static void SaveConfigurationToXML(PropertyList properties)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode propertiesNode = doc.CreateElement("Properties");

            XmlNode flagsNode = doc.CreateElement("TeamFlagsEnabled");
            flagsNode.InnerText = properties.isFlags.ToString();

            XmlNode commentatorNode = doc.CreateElement("CommentatorTagsEnabled");
            commentatorNode.InnerText = properties.isCommentatorTags.ToString();

            XmlNode twitterNode = doc.CreateElement("TwitterHandleEnabled");
            twitterNode.InnerText = properties.isTwitterHandle.ToString();

            XmlNode sponsorNode = doc.CreateElement("SponserIconsEnabled");
            sponsorNode.InnerText = properties.isSponserIcons.ToString();

            XmlNode windowNode = doc.CreateElement("KeepWindowOnTopEnabled");
            windowNode.InnerText = properties.isWindowOnTop.ToString();

            propertiesNode.AppendChild(flagsNode);
            propertiesNode.AppendChild(commentatorNode);
            propertiesNode.AppendChild(twitterNode);
            propertiesNode.AppendChild(sponsorNode);
            propertiesNode.AppendChild(windowNode);

            doc.AppendChild(propertiesNode);

         

            doc.Save("..\\..\\Resources\\config.xml");
        }

        //holy fuck that's a lot of parameters...
        public static void UpdateAllTextOutputFiles(string outputPath ,string player1Name, int player1Score,
                                                    string player2Name, int player2Score, string round,
                                                    int roundNum, string bracket, bool isP1Loser, bool isP2Loser,
                                                    Image p1Icon, Image p2Icon, Image p1Logo, Image p2Logo,
                                                    string twitterHandle1, string twitterHandle2, bool isUsingTwitter,
                                                    string commentator1, string commentator2, bool isUsingComms,
                                                    Image p3Logo, Image p4Logo)
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

                if(bracket.Contains("Money") || bracket.Contains("Friendlies"))
                {
                    File.Delete(outputPath + "\\round.txt");
                }

                if (isUsingTwitter)
                {
                    File.WriteAllText(outputPath + "\\twitterHandle1.txt", twitterHandle1);
                    File.WriteAllText(outputPath + "\\twitterHandle2.txt", twitterHandle2);
                }
                else
                {
                    if (File.Exists(outputPath + "\\twitterHandle1.txt"))
                        File.Delete(outputPath + "\\twitterHandle1.txt");
                    if (File.Exists(outputPath + "\\twitterHandle2.txt"))
                        File.Delete(outputPath + "\\twitterHandle2.txt");
                }
                if (isUsingComms)
                {
                    File.WriteAllText(outputPath + "\\commentator1.txt", commentator1);
                    File.WriteAllText(outputPath + "\\commentator2.txt", commentator2);
                }
                else
                {
                    if (File.Exists(outputPath + "\\commentator1.txt"))
                        File.Delete(outputPath + "\\commentator1.txt");
                    if (File.Exists(outputPath + "\\commentator2.txt"))
                        File.Delete(outputPath + "\\commentator2.txt");
                }

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

                if (p1Logo != null)
                {
                    string uriPath3 = new Uri(p1Logo.Source.ToString()).LocalPath;
                    if (File.Exists(outputPath + "\\p1Logo.png"))
                        File.Delete(outputPath + "\\p1Logo.png");
                    File.Copy(uriPath3, outputPath + "\\p1Logo.png");
                    File.SetLastWriteTime(outputPath + "\\p1Logo.png", DateTime.Now);
                }
                else
                {
                    if (File.Exists(outputPath + "\\p1Logo.png"))
                        File.Delete(outputPath + "\\p1Logo.png");
                }

                if (p2Logo != null)
                {
                    string uriPath3 = new Uri(p2Logo.Source.ToString()).LocalPath;
                    if (File.Exists(outputPath + "\\p2Logo.png"))
                        File.Delete(outputPath + "\\p2Logo.png");
                    File.Copy(uriPath3, outputPath + "\\p2Logo.png");
                    File.SetLastWriteTime(outputPath + "\\p2Logo.png", DateTime.Now);
                }
                else
                {
                    if (File.Exists(outputPath + "\\p2Logo.png"))
                        File.Delete(outputPath + "\\p2Logo.png");
                }

                if (p3Logo != null)
                {
                    string uriPath3 = new Uri(p3Logo.Source.ToString()).LocalPath;
                    if (File.Exists(outputPath + "\\p3Logo.png"))
                        File.Delete(outputPath + "\\p3Logo.png");
                    File.Copy(uriPath3, outputPath + "\\p3Logo.png");
                    File.SetLastWriteTime(outputPath + "\\p3Logo.png", DateTime.Now);
                }
                else
                {
                    if (File.Exists(outputPath + "\\p3Logo.png"))
                        File.Delete(outputPath + "\\p3Logo.png");
                }
                if (p4Logo != null)
                {
                    string uriPath3 = new Uri(p4Logo.Source.ToString()).LocalPath;
                    if (File.Exists(outputPath + "\\p4Logo.png"))
                        File.Delete(outputPath + "\\p4Logo.png");
                    File.Copy(uriPath3, outputPath + "\\p4Logo.png");
                    File.SetLastWriteTime(outputPath + "\\p4Logo.png", DateTime.Now);
                }
                else
                {
                    if (File.Exists(outputPath + "\\p4Logo.png"))
                        File.Delete(outputPath + "\\p4Logo.png");
                }
            }
            catch(Exception e)
            {
                //display message here?
                //nah
            }
        }
    }
}
