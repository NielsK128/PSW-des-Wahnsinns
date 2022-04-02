using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Assets.WorldEngine
{
    public class CBiomeTerrain:CXMLReadWrite
    {
        protected class CTerrainFunction
        {
            public double intensity;
            public double frequency;
            public double offset;
        }

        //###########################################################
        // INTERNAL VARIABLES
        int _maxHeight;
        int _maxDepth;
        string _defaultMaterial;
        CBiome _biome;
        List<CTerrainFunction> _terrainFunctions;
        List<CBiomeTerrainLayer> _terrainLayers;

        //###########################################################
        // PROPERTIES
        public int  maxHeight
        {
            get { return _maxHeight; }
        }
        public int  maxDepth
        {
            get { return _maxDepth; }
        }
        public string defaultMaterial
        {
            get { return _defaultMaterial; }
        }
        public CBiome biome
        {
            get { return _biome; }
        }

        //###########################################################
        // CONSTRUCTORS
        public CBiomeTerrain(CBiome biome)
        {
            _biome = biome;
            _terrainFunctions = new List<CTerrainFunction>();
            _terrainLayers = new List<CBiomeTerrainLayer>();
            _maxHeight = 0;
            _maxDepth = 0;
            _defaultMaterial = "";
        }

        //###########################################################
        // METHODS
        public void GenerateTerrain(int hPosition, CWorldTopologyStrip strip)
        {
            if(_terrainFunctions.Count == 0)
            {
                return;
            }

            double  terrainHeight;
            double result = 0,numFunctions=0;
            double x,norm;

            norm = Math.Max(1,_biome.blockEnd - _biome.blockStart);
            x = (hPosition ) / norm *2.0*Math.PI;
            foreach (CTerrainFunction genfunc in _terrainFunctions)
            {
                result += (genfunc.intensity * Math.Sin(genfunc.offset + x * genfunc.frequency));
                numFunctions++;
            }
            result = result / numFunctions;

            if(result>=0)
            {
                terrainHeight = result * (double)_maxHeight;
            }
            else
            {
                terrainHeight = result * (double)-_maxDepth;
            }
            strip.maxHeight = (int) terrainHeight;
            strip.defaultMaterial = _defaultMaterial;
        }
        public void ApplyTerrainLayers()
        {
            //hier muss der strip die Blöcke von den Layern reingegeben bekommen haben sollen sein.
            foreach (CBiomeTerrainLayer layer in _terrainLayers)
            {
                layer.ApplyToTerrain(this);
            }
        }
        public override void Read(XmlNode myNode)
        {
            base.ReadIntAttribute(myNode, "maxHeight", out _maxHeight);
            base.ReadIntAttribute(myNode, "maxDepth", out _maxDepth);
            base.ReadStringAttribute(myNode, "material", out _defaultMaterial);
            
            //Read generator functions
            _terrainFunctions.Clear();
            XmlNodeList nodeList;

            nodeList = myNode.SelectNodes("TERRAIN_FUNCTION");
            foreach (XmlNode node in nodeList)
            {
                CTerrainFunction genfunc = new CTerrainFunction();
                base.ReadDoubleAttribute(node, "intensity", out genfunc.intensity);
                base.ReadDoubleAttribute(node, "frequency", out genfunc.frequency);
                base.ReadDoubleAttribute(node, "offset", out genfunc.offset);
                _terrainFunctions.Add(genfunc);
            }
            //READ GROUND LAYER INFOS
            _terrainLayers.Clear();
            nodeList = myNode.SelectNodes("TERRAIN_LAYER");
            foreach (XmlNode node in nodeList)
            {
                CBiomeTerrainLayer layer = new CBiomeTerrainLayer();
                layer.Read(node);
                _terrainLayers.Add(layer);
            }

        }
        public override void Write(XmlNode myNode)
        {
            base.WriteIntAttribute(myNode, "maxHeight", _maxHeight);
            base.WriteIntAttribute(myNode, "maxDepth", _maxDepth);
            base.WriteStringAttribute(myNode, "material", _defaultMaterial);

            //Write generator functions
            foreach (CTerrainFunction genfunc in _terrainFunctions)
            {
                XmlNode node = base.CreateSection(myNode, "TERRAIN_FUNCTION");
                base.WriteDoubleAttribute(node, "intensity", genfunc.intensity);
                base.WriteDoubleAttribute(node, "frequency", genfunc.frequency);
                base.WriteDoubleAttribute(node, "offset", genfunc.offset);
            }
            //WRITE GROUND LAYER INFOS
            foreach (CBiomeTerrainLayer layer in _terrainLayers)
            {
                XmlNode nodeLayer = base.CreateSection(myNode, "TERRAIN_LAYER");
                layer.Write(nodeLayer);
            }
        }
    }
}
