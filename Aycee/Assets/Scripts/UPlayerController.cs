using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using Assets.WorldEngine;

public class UPlayerController : NetworkBehaviour
{
    Vector3 lastPosition;
    Rigidbody2D playerBody;
    BoxCollider2D boxCollider2D;
    public LayerMask groundMask;
    public Camera playerCameraPrefab;
    public float playerCameraZPos;
    Camera _playerCamera;
    float _speedWalking,_speedRunning;

    public Camera playerCamera
    {
        get { return _playerCamera; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _speedWalking = 5f;
        _speedRunning = 10f;

        playerBody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        _playerCamera = Instantiate(playerCameraPrefab, new Vector3((float)0, (float)0, playerCameraZPos), Quaternion.identity);

        if (IsLocalPlayer)
        {
            /*
            Transform playerSprite = transform.Find("PlayerSprite");
            if (playerSprite)
            {
                SpriteRenderer sr = playerSprite.gameObject.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.material.color = Color.red;
                }
            }*/

            CWorld world = UWorldManager.world;

            int highestPosition = world.GetHighestTerrainPosition((int)transform.position.x);

            transform.position = new Vector3(transform.position.x,(float)highestPosition + 5f,transform.position.z);
        }
    }


    Rect GetCameraWorldRect()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = playerCamera.orthographicSize * 2f;
        Rect viewrect = new Rect();

        viewrect.x = transform.position.x - playerCamera.orthographicSize * screenAspect;
        viewrect.y = transform.position.y - playerCamera.orthographicSize;
        viewrect.height = cameraHeight;
        viewrect.width = cameraHeight * screenAspect;
        return viewrect;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer)
        {
            playerCamera.enabled = false;
            return;
        }

        float dxMove = Input.GetAxis("Horizontal");
        bool doJump = Input.GetKeyDown(KeyCode.Space);

        Vector3 deltaMove = new Vector3(dxMove, 0f, 0f);
        Vector3 newPosition = transform.position + deltaMove * _speedWalking * Time.deltaTime;

        //PLAYER WRAPPING
        /*
        int currentHPOS = (int)newPosition.x;

        if(currentHPOS >= UWorldManager.world.horizontalExtentEnd)
        {
            currentHPOS = UWorldManager.world.horizontalExtentStart + (UWorldManager.world.horizontalExtentEnd - currentHPOS);
            newPosition.x = currentHPOS;
        }
        else if (currentHPOS <= UWorldManager.world.horizontalExtentStart)
        {
            currentHPOS = UWorldManager.world.horizontalExtentEnd - (UWorldManager.world.horizontalExtentStart - currentHPOS);
            newPosition.x = currentHPOS;
        }*/

         if (newPosition != lastPosition)
         {
             transform.position = newPosition;
             lastPosition = newPosition;
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, playerCameraZPos);
            Rect viewrect = GetCameraWorldRect();
            playerCamera.GetComponent<UWorldRenderer>().UpdateRenderBlocks(viewrect,this);
        }


        if (doJump /*&& IsOnGround()*/)
        {
            playerBody.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
        }

/*        if (playerCamera)
        {
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, playerCameraZPos);
            if (HasChangedPosition())
            {
                Rect viewrect = GetCameraWorldRect();
                playerCamera.GetComponent<UWorldRenderer>().UpdateRenderBlocks(viewrect);
            }
        }*/
    }

    public void UpdateBlockStates()
    {
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, playerCameraZPos);
        Rect viewrect = GetCameraWorldRect();
        playerCamera.GetComponent<UWorldRenderer>().UpdateRenderBlocks(viewrect, this);

    }
    bool HasChangedPosition()
    {
        return (transform.position != lastPosition);
    }

    bool IsOnGround()
    {
        //RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 5f);
        Vector2 rayOrigin = new Vector2(boxCollider2D.bounds.min.x, boxCollider2D.bounds.min.y - 1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 100f);
        if (hit)
        {
            //Debug.Log(hit.distance);
            if (hit.distance < 0.05f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Box")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
