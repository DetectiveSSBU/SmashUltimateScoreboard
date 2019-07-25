using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUStreamManager
{
    public class Player
    {
        string playerTag;
        string lastCharacter;
        int lastAlt;
        string twitterHandle;

        public Player()
        {
            playerTag = "";
            lastCharacter = "Bayonetta";
            lastAlt = 0;
            twitterHandle = "@TwitterHandle";
        }
        public Player(string tag)
        {
            playerTag = tag;
            lastCharacter = "Bayonetta";
            lastAlt = 0;
        }

        public Player(string tag, string character, int alt)
        {
            playerTag = tag;
            lastCharacter = character;
            lastAlt = alt;
        }


        //getters and setters
        public string GetTag()
        {
            return playerTag;
        }
        public string GetCharacter()
        {
            return lastCharacter;
        }
        public int GetAlt()
        {
            return lastAlt;
        }

        public void SetTag(string tag)
        {
            playerTag = tag;
        }
        public void SetCharacter(string character)
        {
            lastCharacter = character;
        }
        public void SetAlt(int alt)
        {
            lastAlt = alt;
        }

        public string GetTwitter()
        {
            return twitterHandle;
        }

        public void setTwitter(string twitter)
        {
            twitterHandle = twitter;
        }
    }
}
