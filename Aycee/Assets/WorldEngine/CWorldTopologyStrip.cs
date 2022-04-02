using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.WorldEngine
{
    public class CWorldTopologyStrip
    {
        //###########################################################
        // INTERNAL VARIABLES
        int _horizontalPosition;
        int _maxHeight;
        string _defaultMaterial;
        Dictionary<int, CWorldBlockInfo> _activeBlocks;
        Dictionary<int, bool> _deletedVirtualBlocks;
        System.Random _randy;
        int _randomSeed;

        //###########################################################
        // PROPERTIES
        public int horizontalPosition
        {
            get { return _horizontalPosition; }
        }
        public int maxHeight
        {
            get { return _maxHeight; }
            set { _maxHeight = value; }
        }
        public int activeBlocksCount
        {
            get { return _activeBlocks.Count; }
        }
        public string defaultMaterial
        {
            get { return _defaultMaterial; }
            set { _defaultMaterial = value; }
        }


        //###########################################################
        // CONSTRUCTORS
        public CWorldTopologyStrip(int horizontalPos, int seed)
        {
            _randomSeed = seed;
            _horizontalPosition = horizontalPos;
            _maxHeight = 0;
            _activeBlocks = new Dictionary<int, CWorldBlockInfo>();
            _deletedVirtualBlocks = new Dictionary<int, bool>();
            _randy = new System.Random(_randomSeed);
            _defaultMaterial = "";
        }

        //###########################################################
        // METHODS
        /*
        public void EvaluateBiomeLayer(CBiomeLayer layer,CBiomeTerrain terrain)
        {
            int minHeight = Math.Max(terrain.maxDepth, layer.maxDepth);
            int maxHeight = Math.Min(terrain.maxHeight, layer.maxHeight);

            for (int h=minHeight;h<maxHeight;h++)
            {
                double Randale = _randy.NextDouble();

                if(Randale < layer.probability)
                {
                    CWorldBlockInfo blockInfo;

                    if(_activeBlocks.ContainsKey(h))
                    {
                        //Ist unklar, was passiert, wenn mehrere Blöcke einen Platz besetzen. :-)
                    }
                    else
                    {
                        blockInfo = new CWorldBlockInfo();
                        blockInfo.materialKey = layer.materialKey;
                        blockInfo.isAir = false;
                        blockInfo.xPos = _horizontalPosition;
                        blockInfo.yPos = h;
                        _activeBlocks.Add(h, blockInfo);
                    }
                }
            }
        }
        */
        public void ApplyLayer(CBiomeTerrainLayer layer, CBiomeTerrain terrain)
        {
           // int minHeight = Math.Max(terrain.maxDepth, layer.maxDepth);
            //int maxHeight = Math.Min(terrain.maxHeight, layer.maxHeight);

            for (int h = terrain.maxDepth; h < terrain.maxHeight; h++)
            {
                double Randale = _randy.NextDouble();

                if(h>=layer.maxDepth && h < layer.maxHeight)
                {
                    if (Randale < layer.probability)
                    {
                        CWorldBlockInfo blockInfo;

                        if (_activeBlocks.ContainsKey(h))
                        {
                            //Ist unklar, was passiert, wenn mehrere Blöcke einen Platz besetzen. :-)
                        }
                        else
                        {
                            blockInfo = new CWorldBlockInfo();
                            blockInfo.materialKey = layer.materialKey;
                            blockInfo.isAir = false;
                            blockInfo.xPos = _horizontalPosition;
                            blockInfo.yPos = h;
                            _activeBlocks.Add(h, blockInfo);
                        }
                    }
                }
            }
        }
        public bool SetBlockInfo(int yPos, CWorldBlockInfo blockInfo)
        {
            if (_activeBlocks.ContainsKey(yPos))
            {
                if(blockInfo.deleted)
                {
                    _activeBlocks.Remove(yPos);
                    if (!_deletedVirtualBlocks.ContainsKey(yPos))
                        _deletedVirtualBlocks.Add(yPos, true);
                }
                else
                {
                    CWorldBlockInfo activeBlock = _activeBlocks[yPos];
                    activeBlock.CopyFrom(blockInfo);
                }
            }
            else
            {
                if(blockInfo.deleted)
                {
                    if (!_deletedVirtualBlocks.ContainsKey(yPos))
                    {
                        _deletedVirtualBlocks.Add(yPos, true);
                    }
                }
                else
                {
                    CWorldBlockInfo activeBlock = new CWorldBlockInfo();

                    activeBlock.CopyFrom(blockInfo);
                    _activeBlocks.Add(yPos, activeBlock);
                }
            }
            return true;
        }

        public bool GetBlockInfo(int height, CWorldBlockInfo blockInfo)
        {
            if (height > _maxHeight)
            {
                blockInfo.materialKey = "air";
                blockInfo.isAir = true;
                return true;
            }

            if (_activeBlocks.ContainsKey(height))
            {
                blockInfo.CopyFrom(_activeBlocks[height]);
                return true;
            }
            //Use Default Block:
            blockInfo.materialKey = _defaultMaterial;
            blockInfo.isAir = false;
            blockInfo.colorHTML = "#ffffff";
            if (_deletedVirtualBlocks.ContainsKey(height)) //virtueller block wurde gelöschert, ja, gelöschert!
            {
                blockInfo.colorHTML = "#404040";
                blockInfo.deleted = true;
            }
            return true;
        }
        public void DestroyBlockAt(int height)
        {
            if (_activeBlocks.ContainsKey(height))
            {
                _activeBlocks.Remove(height);
            }
            //virtual block was hit!
            if(!_deletedVirtualBlocks.ContainsKey(height))
            {
                _deletedVirtualBlocks.Add(height, true);
            }
        }
    }
}
