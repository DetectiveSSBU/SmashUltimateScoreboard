using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUStreamManager
{
    public class PropertyList
    {

       public bool isFlags;
       public bool isCommentatorTags;
       public bool isTwitterHandle;
       public bool isSponserIcons;
       public bool isWindowOnTop;

        public PropertyList()
        {
            isFlags = true;
            isCommentatorTags = false;
            isTwitterHandle = false;
            isSponserIcons = false;
            isWindowOnTop = false;
        }

    }
}
