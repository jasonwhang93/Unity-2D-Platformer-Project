using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Map2MoveController : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // �̵� �ӵ�
    public float jumpForce = 7.0f;  // ���� ��
    private bool isJumping = false;  // ���� ������ ����

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 characterScale;  // ĳ������ �ʱ� ũ�� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        characterScale = transform.localScale;
    }

    private void Update()
    {
        Move();
        Jump();

        // Animator ���� ������Ʈ
        animator.SetBool("isWalk", rb.velocity.x != 0 && !isJumping);
        animator.SetBool("isJump", isJumping);
    }

    private void Move()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        // ĳ���� ���� ����
        if (horizontalMove < 0)
            transform.localScale = characterScale;
        else if (horizontalMove > 0)
            transform.localScale = new Vector3(-characterScale.x, characterScale.y, characterScale.z);

        Vector2 moveDirection = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);
        rb.velocity = moveDirection;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾ ���� ������ �ٽ� ���� ���� ���·� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
