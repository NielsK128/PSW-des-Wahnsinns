using System;
using System.Collections.Generic;

namespace Assets.WorldEngine
{
    public class CWorldBlockInfo
    {
        public string colorHTML;
        public float xPos;
        public float yPos;
        public bool isAir;
        public bool deleted;
        public string materialKey;

        public CWorldBlockInfo()
        {
            isAir = false;
            deleted = false;
            colorHTML = "#ffffff"; //white is default
            materialKey = "";
            xPos = 0f;
            yPos = 0f;
        }
        public void CopyFrom(CWorldBlockInfo other)
        {
            colorHTML = other.colorHTML;
            xPos = other.xPos;
            yPos = other.yPos;
            isAir = other.isAir;
            deleted = other.deleted;
            materialKey = other.materialKey;
        }
    }
}
