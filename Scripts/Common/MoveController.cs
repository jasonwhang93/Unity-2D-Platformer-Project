
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveController : MonoBehaviour
{
    [SerializeField] private LayerMask GroundLayer;

    [Header("Rope Interaction")]
    public Tilemap ropeTilemap; // Rope Ÿ�ϸ��� ����
    public LayerMask RopeLayer; // Rope Ÿ�� ���̾� ����ũ
    public LayerMask UpGroundLayer; // Rope Ÿ�� ���̾� ����ũ
    private bool isOnRope = false; // ���� Rope Ÿ�ϰ� ��ȣ�ۿ� ������ ����
    private bool isRope = false;
    private Vector3 ropeTileCenter;
    private Collider2D ignoredCollider = null; // ���õ� collider�� ������ ����

    [Header("Ladder Interaction")]
    public Tilemap ladderTilemap; // Ladder Ÿ�ϸ��� ����
    public LayerMask LadderLayer; // Ladder Ÿ�� ���̾� ����ũ
    private bool isOnLadder = false; // ���� Ladder Ÿ�ϰ� ��ȣ�ۿ� ������ ����
    private bool isLadder = false;
    private Vector3 ladderTileCenter;

    public float moveSpeed = 2f;
    public float jumpPower = 5f;

    Rigidbody2D rigidBody2D;
    Animator anim;
    Collider2D col2D;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    bool isJumpKeyPressed = false;
    bool isJumping = false;
    bool isGrounded = false;

    bool isProneKeyPreesed = false;
    bool isProne = false;

    float clickTime = 0f; // ������ Ŭ�� �ð�
    float clickDelay = 0.5f; // ���� Ŭ������ ������ �ִ� �ð� ����

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameData.isPause)
        {
            UserKeySetting();
            CheckIsGrounded();
            CheckRopeInteraction();
            CheckLadderInteraction();
            CheckAnimCondition();
            InputMoveValue();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InputMoveValue()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        // ���� ���³� ��ٸ� ���¿����� ���� Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Space) && !isRope && !isLadder)
        {
            isJumpKeyPressed = true;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isProneKeyPreesed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isProneKeyPreesed = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetTrigger("Attack");
        }

        SetMoveDir();
    }

    private void MovePlayer()
    {
        // Rope Interaction
        if (isOnRope && verticalMove != 0)
        {
            isRope = true;
            isJumping = false;  // Ensure the player is not in jumping state
            rigidBody2D.velocity = new Vector2(0, verticalMove * moveSpeed);
            transform.position = new Vector3(ropeTileCenter.x, transform.position.y, transform.position.z);
        }
        else if (isRope && !isGrounded && verticalMove == 0)
        {
            rigidBody2D.gravityScale = 0;
            rigidBody2D.velocity = Vector2.zero;
            isRope = true;
        }
        else
        {
            isRope = false;
        }

        if (isRope && verticalMove < 0) // ���ٿ��� �Ʒ��� �������� �� ��
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, UpGroundLayer);
            if (hit.collider != null && hit.collider.CompareTag("UpGround"))
            {
                Physics2D.IgnoreCollision(col2D, hit.collider, true);
                ignoredCollider = hit.collider; // ���õ� collider ����
            }
        }

        // Ladder Interaction
        if (isOnLadder && verticalMove != 0)
        {
            isLadder = true;
            isJumping = false;  // Ensure the player is not in jumping state
            rigidBody2D.velocity = new Vector2(0, verticalMove * moveSpeed);
            transform.position = new Vector3(ladderTileCenter.x, transform.position.y, transform.position.z);
        }
        else if (isLadder && !isGrounded && verticalMove == 0)
        {
            rigidBody2D.gravityScale = 0;
            rigidBody2D.velocity = Vector2.zero;
            isLadder = true;
        }
        else
        {
            isLadder = false;
        }

        // ��ٸ����� �Ʒ��� �������� �� ��
        if (isLadder && verticalMove < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, UpGroundLayer);
            if (hit.collider != null && hit.collider.CompareTag("UpGround"))
            {
                Physics2D.IgnoreCollision(col2D, hit.collider, true);
                ignoredCollider = hit.collider; // ���õ� collider ����
            }
        }

        // Handle ignored colliders for rope and ladder
        if (ignoredCollider != null)
        {
            if (col2D.bounds.max.y < ignoredCollider.bounds.min.y)
            {
                Physics2D.IgnoreCollision(col2D, ignoredCollider, false);
                ignoredCollider = null; // ���õ� collider �ʱ�ȭ
            }
        }

        // Normal Movement
        if (!isRope && !isLadder)
        {
            rigidBody2D.gravityScale = 1;
            rigidBody2D.velocity = new Vector2(horizontalMove * moveSpeed, rigidBody2D.velocity.y);

            if (isJumpKeyPressed && isGrounded)
            {
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpPower);
                isJumpKeyPressed = false;
                isJumping = true;
            }

            if (isProneKeyPreesed == true && isGrounded == true)
            {
                isProne = true;
            }
            if (isProneKeyPreesed == false) isProne = false;
        }
    }



    private void SetMoveDir()
    {
        if (horizontalMove < 0) transform.localScale = new Vector3(1, 1, 1);
        if (horizontalMove > 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void CheckIsGrounded()
    {
        int combinedLayerMask = GroundLayer | UpGroundLayer;
        float extraHeight = 0.05f;
        RaycastHit2D hit = Physics2D.Raycast(col2D.bounds.center + new Vector3(0, -col2D.bounds.extents.y, 0), Vector2.down, extraHeight, combinedLayerMask);

        bool wasGrounded = isGrounded;
        isGrounded = (hit.collider != null);

        if (isGrounded) isJumping = false;

        // ���ٿ��� ������ ���� ���� ���
        if (isRope && wasGrounded == false && isGrounded == true && rigidBody2D.velocity.y <= 0.1f)
        {
            isRope = false;
        }

        // ��ٸ����� ������ ���� ���� ���
        if (isLadder && wasGrounded == false && isGrounded == true && rigidBody2D.velocity.y <= 0.1f)
        {
            isLadder = false; // ��ٸ� ���� ����
            isOnLadder = false; // ��ٸ� ��ȣ�ۿ� ���� ����
        }

        // ���� ������ �����ϰ� �Ʒ� ����Ű�� ��� ������ �ִ� ���
        if (isRope && isGrounded && verticalMove < 0)
        {
            isRope = false;
        }

        // ��ٸ����� ���� ������ �����ϰ� �Ʒ� ����Ű�� ��� ������ �ִ� ���
        if (isLadder && isGrounded && verticalMove < 0)
        {
            isLadder = false;
            isOnLadder = false;
        }
    }


    private void CheckRopeInteraction()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.up, 0.02f, RopeLayer);

        if (hit.collider != null)
        {
            isOnRope = true;

            // Ÿ���� ���� �߽� ��ġ ���
            var tilemap = hit.collider.GetComponent<Tilemap>();
            Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);

            // ������ �߽� ��ġ ���� (Ÿ���� �߽ɿ��� -0.24f��ŭ ������)
            ropeTileCenter = tileCenter + new Vector3(-0.2f, 0, 0);
        }
        else
        {
            isOnRope = false;
            isRope = false;
        }
    }

    private void CheckLadderInteraction()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.up, 0.02f, LadderLayer);

        if (hit.collider != null)
        {
            isOnLadder = true;

            var tilemap = hit.collider.GetComponent<Tilemap>();
            Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);
            ladderTileCenter = tileCenter; // Ladder�� Tile�� �߾ӿ� ��ġ�ϱ� ������ �߰� ������ ����
        }
        else
        {
            isOnLadder = false;
        }
    }

    private void CheckAnimCondition()
    {
        anim.SetBool("isWalk", horizontalMove != 0);
        anim.SetBool("isJump", isJumping);
        anim.SetBool("isProne", isProne);
        anim.SetBool("isRope", isRope);
        anim.SetBool("isLadder", isLadder);
    }

    private void UserKeySetting()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            EventManager.instance.SendEvent("Inventory :: UIToggle");
        }

        if (Input.GetMouseButtonDown(0)) // ���� ���콺 ��ư�� Ŭ���ߴ��� Ȯ��
        {
            if (Time.time - clickTime < clickDelay)
            {
                DoubleClickEvent("ShopNPC", "Shop :: UIToggle");
                DoubleClickEvent("QuestNPC", "Quest :: UIToggle");
            }
            clickTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerData.PrintData();
        }
    }

    private void DoubleClickEvent(string tag, string eventName)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag(tag))
        {
            // ShopNPC ������Ʈ�� ���� Ŭ�� ���� ���� ������ ���⿡ �ۼ��ϼ���.
            EventManager.instance.SendEvent(eventName, hit.collider.gameObject); // Send NPC GameObject as a parameter
        }
    }
}
