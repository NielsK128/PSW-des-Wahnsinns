using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.WorldEngine;

public class URenderBlock : MonoBehaviour
{
    CWorldBlockInfo _blockInfo;
    SpriteRenderer _renderer;
    public UPlayerController player;

    //PUBLIC PROPERTIES
    private void Awake()
    {
        _blockInfo = null;
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void SetBlockInfo(CWorldBlockInfo blockInfo)
    {
        _blockInfo = blockInfo;
    }
    public void Render(UWorldRenderer worldRenderer)
    {
        if(_blockInfo == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (_blockInfo.isAir /*|| _blockInfo.deleted*/)
        {
            gameObject.SetActive(false);
            return;
        }

        float blockZPosition = 0;

        Collider2D collie = GetComponent<Collider2D>();
        collie.isTrigger = false;
        if (_blockInfo.deleted)
        {
            collie.isTrigger = true;
            blockZPosition = 1f;
        }

        Vector3 renderBlockPos = new Vector3();

        renderBlockPos.Set(_blockInfo.xPos, _blockInfo.yPos, blockZPosition);
        transform.position = renderBlockPos;

        Color MaterialColour;

        Sprite blockSprite = worldRenderer.GetBlockSprite(_blockInfo.materialKey);
        if (blockSprite != null)
        {
            _renderer.sprite = blockSprite;
            ColorUtility.TryParseHtmlString(_blockInfo.colorHTML, out MaterialColour);
        }
        else
        {
            MaterialColour = Color.red;
        }
        _renderer.material.color = MaterialColour;
        gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            Vector3 hitWorldPosition = player.playerCamera.ScreenToWorldPoint(Input.mousePosition);

            UWorldManager.world.DestroyBlockAt(hitWorldPosition.x, hitWorldPosition.y);
            //Debug.Log("Block delete at " + hitWorldPosition.x + "/" + hitWorldPosition.y);
            player.UpdateBlockStates();
        }
    }

}
