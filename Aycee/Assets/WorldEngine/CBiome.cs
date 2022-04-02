using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Assets.WorldEngine
{
    public class CBiome : CXMLReadWrite
    {
        //###########################################################
        // INTERNAL VARIABLES
        CWorld _world;
        string _key;
        string _name;
        int _lonStart;
        int _lonEnd;
        CBiomeTerrain _terrain;

        //###########################################################
        // PROPERTIES
        public string key
        {
            get { return _key; }
        }
        public string name
        {
            get { return _name; }
        }
        public int lonStart //in Winkelgrad
        {
            get { return _lonStart; }
        }
        public int lonEnd //in Winkelgrad
        {
            get { return _lonEnd; }
        }
        public int blockStart //in Blockposition
        {
            get { return _lonStart * _world.blocksPerDegree; }
        }
        public int blockEnd //in Blockposition
        {
            get { return _lonEnd * _world.blocksPerDegree; }
        }
        public CWorld world
        {
            get { return _world; }
        }

        //###########################################################
        // CONSTRUCTORS
        public CBiome(CWorld world)
        {
            _world = world;
            _terrain=new CBiomeTerrain(this);
            _lonStart = -180;
            _lonEnd = 180;
        }

        //###########################################################
        // METHODS
        public bool ContainsLongitude(int lonDegree)
        {
            if (lonDegree < _lonStart)
            {
                return false;
            }
            if(lonDegree > _lonEnd)
            {
                return false;
            }
            return true;
        }

        public void GenerateTerrain(int hPosition, CWorldTopologyStrip strip)
        {
            _terrain.GenerateTerrain(hPosition, strip);
        }
        public void ApplyTerrainLayers()
        {
            _terrain.ApplyTerrainLayers();
        }

        //##########################################################
        //XML FILE I/O
        public override void Read(XmlNode myNode)
        {
            base.ReadStringAttribute(myNode, "key", out _key);
            base.ReadStringAttribute(myNode, "name", out _name);
            base.ReadIntAttribute(myNode, "lonStart",out  _lonStart);
            base.ReadIntAttribute(myNode, "lonEnd", out _lonEnd);

            //READ TOPOLOGY INFOS
            XmlNodeList nodeList;

            nodeList = myNode.SelectNodes("TERRAIN");
            foreach (XmlNode node in nodeList)
            {
                _terrain.Read(node);
            }
        }
        public override void Write(XmlNode myNode)
        {
            base.WriteStringAttribute(myNode, "key", _key);
            base.WriteStringAttribute(myNode, "name", _name);
            base.WriteIntAttribute(myNode, "lonStart", _lonStart);
            base.WriteIntAttribute(myNode, "lonEnd", _lonEnd);

            //WRITE TOPOLOGY INFOS
            XmlNode node = base.CreateSection(myNode, "TERRAIN");
            _terrain.Write(node);
        }
    }
}
