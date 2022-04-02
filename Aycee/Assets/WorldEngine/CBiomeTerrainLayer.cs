using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Assets.WorldEngine
{
    public class CBiomeTerrainLayer:CXMLReadWrite
    {
        //###########################################################
        // INTERNAL VARIABLES
        int _maxHeight;
        int _maxDepth;
        double _probability;
        string _materialKey;
        string _stampKey;
        int _stampCount;


        //###########################################################
        // PROPERTIES
        public int maxHeight
        {
            get { return _maxHeight; }
        }
        public int maxDepth
        {
            get { return _maxDepth; }
        }
        public double probability
        {
            get { return _probability; }
        }
        public string materialKey
        {
            get { return _materialKey; }
        }
        public string stampKey
        {
            get { return _stampKey; }
        }
        public int stampCount
        {
            get { return _stampCount; }
        }

        //###########################################################
        // CONSTRUCTORS
        public CBiomeTerrainLayer()
        {
            _maxHeight = 0;
            _maxDepth = 0;
            _probability = 1;
            _materialKey = "";
            _stampKey = "";
            _stampCount = 0;
        }

        //###########################################################
        // METHODS
        public void ApplyToTerrain(CBiomeTerrain terrain)
        {
            CBiome myBiome = terrain.biome;
            CWorldBlockInfo info = new CWorldBlockInfo();
            CWorld.CWorldTerrainStamp stamp = myBiome.world.GetStamp(_stampKey);

            if(stamp==null)
            {
                return;
            }

            Thread.Sleep(2);
            Random rand = new Random(myBiome.world.randomWorldSeed);
            for(int n=0;n<_stampCount;n++)
            {
                int xStart = rand.Next(myBiome.blockStart, myBiome.blockEnd);
                //int xStart = rand.Next(0, 50);
                int yStart = rand.Next(_maxDepth, _maxHeight);

                for(int y = 0; y < stamp.height; y++)
                {
                    for (int x = 0; x < stamp.width; x++)
                    {
                        char stampValue = stamp.data[x, y];
                        switch(stampValue)
                        {
                            case '#': //Material setzen
                                info.materialKey = _materialKey;
                                myBiome.world.SetWorldBlock(xStart + x, yStart + y, info);
                                break;
                            case '0': //Material entfernen (Höhle)
                                info.deleted = true;
                                myBiome.world.SetWorldBlock(xStart + x, yStart + y, info);
                                break;
                        }
                    }
                }
            }
        }

        public override void Read(XmlNode myNode)
        {
            base.ReadIntAttribute(myNode, "maxHeight", out _maxHeight);
            base.ReadIntAttribute(myNode, "maxDepth", out _maxDepth);
            base.ReadDoubleAttribute(myNode, "probability", out _probability);
            base.ReadStringAttribute(myNode, "material", out _materialKey);
            base.ReadStringAttribute(myNode, "stamp", out _stampKey);
            base.ReadIntAttribute(myNode, "count", out _stampCount);
        }
        public override void Write(XmlNode myNode)
        {
            base.WriteIntAttribute(myNode, "maxHeight", _maxHeight);
            base.WriteIntAttribute(myNode, "maxDepth", _maxDepth);
            base.WriteDoubleAttribute(myNode, "probability", _probability);
            base.WriteStringAttribute(myNode, "material", _materialKey);
            base.WriteStringAttribute(myNode, "stamp", _stampKey);
            base.WriteIntAttribute(myNode, "count", _stampCount);
        }
    }
}
