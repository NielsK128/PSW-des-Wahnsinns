using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.WorldEngine;

public class UWorldRenderer : MonoBehaviour
{
    int  _lookAheadRange,_updateRange;
    public GameObject renderBlockPrefab;
    Camera _camera;

    List<GameObject> _renderBlockGameObjects;
    Dictionary<string,Sprite> _blockSprites;

    void Awake()
    {
        _renderBlockGameObjects = new List<GameObject>();
        _blockSprites = new Dictionary<string,Sprite>();
        _camera = GetComponent<Camera>();
        _lookAheadRange = 4;
        _updateRange = 1;
        LoadAllBlockSprites();
    }
    void LoadAllBlockSprites()
    {
        object[] allSprites = Resources.LoadAll("Textures/Blocks", typeof(Sprite));

        _blockSprites.Clear();
        for (int x = 0; x < allSprites.Length; x++)
        {
            Sprite mySprite;

            mySprite = (Sprite)allSprites[x];
            _blockSprites.Add(mySprite.name, mySprite);
        }
        Debug.Log("Loaded " + _blockSprites.Count + " block sprites.");
    }
    public Sprite GetBlockSprite(string name)
    {
        if (_blockSprites.ContainsKey(name))
            return _blockSprites[name];

        return null;
    }

    public void UpdateRenderBlocks(Rect viewrect,UPlayerController player)
    {
        CWorld world=UWorldManager.world;

        Rect renderRect = new Rect(viewrect.xMin - _updateRange, viewrect.yMin - _updateRange, viewrect.width + 2 * _updateRange, viewrect.height + 2 * _updateRange);
        int iMin, iMax;
        int jMin, jMax;
        int k=0;
        
        iMin = (int)renderRect.xMin;
        iMax = (int)renderRect.xMax;

        jMin = Math.Max(world.maxDepth, (int)renderRect.yMin);
        jMax = (int)renderRect.yMax;
        
        foreach (GameObject go in _renderBlockGameObjects)
        {
            go.SetActive(false);
        }

        for (int j = jMin; j <= jMax; j++)
        {
            for (int i = iMin; i <= iMax; i++)
            {
                CWorldBlockInfo blockInfo = new CWorldBlockInfo();

                world.GetWorldBlock(i, j, blockInfo);
                if(blockInfo.isAir == false)
                {
                    URenderBlock renderBlock;
                    GameObject goRenderBlock;

                    if (_renderBlockGameObjects.Count <= k)
                    {
                        goRenderBlock = Instantiate(renderBlockPrefab, new Vector3((float)i, (float)j, 0), Quaternion.identity);
                        _renderBlockGameObjects.Add(goRenderBlock);
                    }
                    else
                    {
                        goRenderBlock = _renderBlockGameObjects[k];
                    }
                    renderBlock = goRenderBlock.GetComponent<URenderBlock>();
                    renderBlock.player = player;
                    renderBlock.SetBlockInfo(blockInfo);
                    renderBlock.Render(this);
                }
                k++;
            }
        }
    }
}
