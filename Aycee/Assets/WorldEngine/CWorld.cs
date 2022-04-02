using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Drawing;
using UnityEngine;

namespace Assets.WorldEngine
{
    public class CWorld : CXMLReadWrite
    {
        public class CWorldTerrainStamp
        {
            int _width;
            int _height;
            char[,] _data;

            public int width { get { return _width; } }
            public int height { get { return _height; } }
            public Char[,] data { get { return _data; } }

            public CWorldTerrainStamp(string[] textLines)
            {
                _width = 0;
                _height = 0;

                foreach (string line in textLines)
                {
                    //if(!string.IsNullOrEmpty(line))
                    {
                        _width = line.Length;
                        _height++;
                    }
                }
                _data = new char[_width, _height];
               
                int y= _height-1;

                foreach (string line in textLines)
                {
                    for(int x=0;x<line.Length;x++)
                    {
                        _data[x, y] = line[x];
                    }
                    y--;
                }
            }
        }

        //###########################################################
        // INTERNAL VARIABLES
        Dictionary<string, CChemicalEntity> _chemicalEntities;
        Dictionary<string, CChemicalReactionRule> _chemicalReactionRules;
        List<CBiome> _biomeList;
        Dictionary<int, CWorldTopologyStrip>  _topologyStrips;
        Dictionary<string, CWorldTerrainStamp> _stampData;

        int _maxHeight; //höchster Punkt in Metern
        int _maxDepth;  //tiefster Punkt in Metern
        double _blockExtent; //in Metern
        int _blocksPerDegree; //Anzahl Blöcke pro Longitude-Winkel
        int _randomWorldSeed; // seed für die Generierung der Welt (muss immer > 0 sein!)
        System.Random _worldRandomGenerator;

        //###########################################################
        // PROPERTIES
        public int randomWorldSeed { get { return _randomWorldSeed; } }

        public int maxHeight
        {
            get { return _maxHeight; }
        }
        public int maxDepth
        {
            get { return _maxDepth; }
        }
        public double blockExtent
        {
            get { return _blockExtent; }
        }
        public int blocksPerDegree
        {
            get { return _blocksPerDegree; }
        }
        public int horizontalExtent //horizontale Weltausdehnung in Metern
        {
            get { return (int)(360.0 * (double)blocksPerDegree * blockExtent); }
        }
        public int horizontalExtentStart
        {
            get 
            {
                int extStart = (int)(180.0 * (double)blocksPerDegree);

                foreach(CBiome biome in _biomeList)
                {
                    if(biome.lonStart*blocksPerDegree < extStart)
                    {
                        extStart = biome.lonStart * blocksPerDegree;
                    }
                }
                return extStart; 
            }
        }
        public int horizontalExtentEnd
        {
            get
            {
                int extEnd = (int)(-180.0 * (double)blocksPerDegree);

                foreach (CBiome biome in _biomeList)
                {
                    if (biome.lonEnd * blocksPerDegree > extEnd)
                    {
                        extEnd = biome.lonEnd * blocksPerDegree;
                    }
                }
                return extEnd;
            }
        }
        public int verticalExtent
        {
            get { return maxHeight - maxDepth; }
        }
        public int activeBlocksCount
        {
            get
            {
                int N = 0;

                foreach(CWorldTopologyStrip strip in _topologyStrips.Values)
                {
                    N += strip.activeBlocksCount;
                }
                return N;
            }
        }

        public List<CBiome> biomeList
        {
            get { return _biomeList; }
        }

        //###########################################################
        // CONSTRUCTORS
        public CWorld()
        {
            _chemicalEntities = new Dictionary<string, CChemicalEntity>();
            _chemicalReactionRules = new Dictionary<string, CChemicalReactionRule>();
            _biomeList = new List<CBiome>();
            _topologyStrips = new Dictionary<int, CWorldTopologyStrip>();
            _stampData = new Dictionary<string, CWorldTerrainStamp>();

            _maxHeight = 500;
            _maxDepth = -500;
            _blockExtent = 0.5;
            _blocksPerDegree = 100;
            //World's random generator:
            _randomWorldSeed = 19630302;
            _worldRandomGenerator = new System.Random(_randomWorldSeed);
        }

        //###########################################################
        // METHODS
        public void InitWorld()
        {
            _topologyStrips.Clear();
            foreach (CBiome biome in _biomeList)
            {
                int blockStart = biome.lonStart * blocksPerDegree;
                int blockEnd = biome.lonEnd * blocksPerDegree;
                for (int blockHPosition = blockStart; blockHPosition <= blockEnd; blockHPosition++)
                {
                    if (!_topologyStrips.ContainsKey(blockHPosition))
                    {
                        int stripRandomSeed;

                        stripRandomSeed = _worldRandomGenerator.Next(10000, 999999);
                        CWorldTopologyStrip strip = new CWorldTopologyStrip(blockHPosition, stripRandomSeed);
                        biome.GenerateTerrain(blockHPosition, strip);
                        _topologyStrips.Add(blockHPosition, strip);
                    }
                }
            }
            //Apply all terrain features per biome
            foreach (CBiome biome in _biomeList)
            {
                biome.ApplyTerrainLayers();
            }
            /*  foreach(int hpos in _topologyStrips.Keys)
              {
                  Debug.Log(hpos);
              }*/
        }


        public void SetWorldBlock(int x,int y,CWorldBlockInfo info)
        {
            int xPos = x;

            if(x<horizontalExtentStart) //left wrapping
            {
                return;
            }
            if(x>horizontalExtentEnd) //right wrapping
            {
                return;
            }

            int yPos = Math.Max(Math.Min(y, _maxHeight), _maxDepth); //clamp height values to world vertical extents

            CWorldTopologyStrip strip = _topologyStrips[xPos];

            info.xPos = xPos;
            info.yPos = yPos;
            strip.SetBlockInfo(yPos, info);
            //Debug.Log(info.materialKey);
        }
        public bool GetWorldBlock(int i, int j, CWorldBlockInfo info)
        {
            int hpos = i;

            if(i > horizontalExtentEnd) //Wrapping right side
            {
                hpos = horizontalExtentStart + (i - horizontalExtentEnd);
            }
            else if (i  <  horizontalExtentStart) //wrapping left side
            {
                hpos = horizontalExtentEnd - (horizontalExtentStart - i);
            }

            if (!_topologyStrips.ContainsKey(hpos)) //darf eigentlich nicht passieren :-)
            {
                info.colorHTML = "#ff0000"; 
                info.isAir = false;
                info.xPos = i;
                info.yPos = j;
               // Debug.Log("HPOS="+hpos+" i="+i+" end="+horizontalExtentEnd);
                return false;
            }

            CWorldTopologyStrip strip;

            strip = _topologyStrips[hpos];
            strip.GetBlockInfo(j, info);
            info.xPos = i;
            info.yPos = j;

            return true;
        }
        public void DestroyBlockAt(float  x,float  y)
        {
            int stripIndex = (int) Mathf.FloorToInt(x);
            int blockHeight = (int)Mathf.FloorToInt(y);

            CWorldTopologyStrip strip;

            strip = _topologyStrips[stripIndex];
            strip.DestroyBlockAt(blockHeight);
        }
        public int  GetHighestTerrainPosition(int HPOS)
        {
            CWorldTopologyStrip strip;

            strip = _topologyStrips[HPOS];
            return strip.maxHeight;
        }

        //#################################################################
        //Load/Save WORD-CONFIG
        public void LoadWorldConfig(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            XmlElement nodeWorld = doc.DocumentElement;

            if (nodeWorld.Name != "WORLD")
            {
                return; //Fehler! Dateiformat stimmt nicht!
            }

            //#######################################################
            //Read world attributes:
            base.ReadIntAttribute(nodeWorld, "maxHeight", out _maxHeight);
            base.ReadIntAttribute(nodeWorld, "maxDepth", out _maxDepth);
            base.ReadIntAttribute(nodeWorld, "blocksPerDegree", out _blocksPerDegree);
            base.ReadDoubleAttribute(nodeWorld, "blockExtent", out _blockExtent);


            XmlNodeList nodeList;

            nodeList = nodeWorld.SelectNodes("BIOME");
            _biomeList.Clear();
            foreach (XmlNode node in nodeList)
            {
                CBiome biome = new CBiome(this);
                biome.Read(node);
                _biomeList.Add(biome);
            }
        }
        public void SaveWorldConfig(string filename)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode nodeWorld = doc.CreateElement("WORLD");
            doc.AppendChild(nodeWorld);

            //#######################################################
            //Write world attributes:
            base.WriteIntAttribute(nodeWorld, "maxHeight", maxHeight);
            base.WriteIntAttribute(nodeWorld, "maxDepth", maxDepth);
            base.WriteIntAttribute(nodeWorld, "blocksPerDegree", blocksPerDegree);
            base.WriteDoubleAttribute(nodeWorld, "blockExtent", blockExtent);

            //#######################################################
            //Write world biomes:
            foreach (CBiome biome in _biomeList)
            {
                XmlNode node = base.CreateSection(nodeWorld, "BIOME");
                biome.Write(node);
            }
            doc.Save(filename);
        }
        public void LoadWorldData(string pathGameData)
        {
            //###################################################################
            //LOAD all stamp data:
            string pathStampData=pathGameData +"Stamps/";
            string[] stampFiles = Directory.GetFiles(pathStampData, "*.txt");

            CWorldTerrainStamp stamp;

            _stampData.Clear();
            foreach (string stampFilename in stampFiles)
            {
                string[] fileLines = File.ReadAllLines(stampFilename);
                string stampKey = Path.GetFileNameWithoutExtension(stampFilename);
                stamp = new CWorldTerrainStamp(fileLines);
                _stampData.Add(stampKey, stamp);
            }
        }
        public CWorldTerrainStamp GetStamp(string key)
        {
            if (_stampData.ContainsKey(key))
            {
                return _stampData[key];
            }
            return null;
        }

        //#################################################################
        //Load/Save chemical entities
        public void LoadChemicalEntities(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            XmlElement nodeRoot = doc.DocumentElement;

            if (nodeRoot.Name != "AC_CHEMICAL_ENTITIES")
            {
                return; //Fehler! Dateiformat stimmt nicht!
            }
            XmlNodeList nodeList;
            
            nodeList = nodeRoot.SelectNodes("CHEMICAL_ENTITY");

            _chemicalEntities.Clear();
            foreach (XmlNode node in nodeList)
            {
                CChemicalEntity chem = new CChemicalEntity();
                chem.Read(node);
                _chemicalEntities.Add(chem.codeName, chem);
            }
        }
        public void SaveChemicalEntities(string filename)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode nodeRoot = doc.CreateElement("AC_CHEMICAL_ENTITIES");
            doc.AppendChild(nodeRoot);

            foreach(CChemicalEntity chem in _chemicalEntities.Values)
            {
                XmlNode node = nodeRoot.OwnerDocument.CreateElement("CHEMICAL_ENTITY");
                nodeRoot.AppendChild(node);
                chem.Write(node);
            }
            doc.Save(filename);
        }
        //#################################################################
        //Load/Save chemical reaction rules
        public void LoadChemicalReactionRules(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            XmlElement nodeRoot = doc.DocumentElement;

            if (nodeRoot.Name != "AC_CHEMICAL_REACTION_RULES")
            {
                return; //Fehler! Dateiformat stimmt nicht!
            }

            XmlNodeList nodeList;

            nodeList = nodeRoot.SelectNodes("CHEMICAL_RULE");
            _chemicalReactionRules.Clear();
            foreach (XmlNode node in nodeList)
            {
                CChemicalReactionRule rule = new CChemicalReactionRule();
                rule.Read(node);
                _chemicalReactionRules.Add(rule.input, rule);
            }
        }
        public void SaveChemicalReactionRules(string filename)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode nodeRoot = doc.CreateElement("AC_CHEMICAL_REACTION_RULES");
            doc.AppendChild(nodeRoot);
            foreach (CChemicalReactionRule rule in _chemicalReactionRules.Values)
            {
                XmlNode node = nodeRoot.OwnerDocument.CreateElement("CHEMICAL_RULE");
                nodeRoot.AppendChild(node);
                rule.Write(node);
            }
            doc.Save(filename);
        }

    }
}
