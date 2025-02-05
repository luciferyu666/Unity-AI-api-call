using UnityEngine;

// 確保該物件有 CharacterController
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = 9.81f;

    private Vector3 _moveDirection = Vector3.zero;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_characterController.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal"); // A, D 或 左右方向鍵
            float vertical = Input.GetAxis("Vertical");     // W, S 或 上下方向鍵

            // 計算前後左右的移動向量（相對於玩家自身的 Transform.forward / Transform.right)
            Vector3 forward = transform.forward * vertical;
            Vector3 right = transform.right * horizontal;

            _moveDirection = (forward + right) * moveSpeed;

            // （選用）按空白鍵跳躍
            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = jumpSpeed;
            }
        }

        // 重力影響
        _moveDirection.y -= gravity * Time.deltaTime;

        // 使用 CharacterController 移動
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
}
