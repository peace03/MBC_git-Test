using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("상태")]
    [SerializeField] private bool pressedRunKey;        // 달리기 키 누름 여부

    [Header("스탯")]
    [SerializeField] private float moveSpeed;           // 움직이는 속도
    [SerializeField] private float runSpeed;            // 달리는 속도
    [SerializeField] private float rollDistance;        // 구르는 거리
    [SerializeField] private float maxStamina;          // 최대 스테미나
    [SerializeField] private float curStamina;          // 현재 스테미나

    private CharacterController cc;                     // 캐릭터 컨트롤러

    private Vector2 moveInput;                          // 입력 값

    private void Awake()
    {
        // 초기화
        curStamina = maxStamina;
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // 이동 이벤트 구독
        EventBus<MoveEvent>.OnEvent += SetMoveInput;
        // 달리기 이벤트 구독
        EventBus<RunEvent>.OnEvent += SetPressedRunKey;
    }

    private void Update()
    {
        // 스테미나 설정
        HandleStamina();
    }

    private void FixedUpdate()
    {
        // 입력 값이 있다면
        if (moveInput.magnitude > 0)
            // 이동
            HandleMove();
    }

    private void OnDisable()
    {
        // 이동 이벤트 구독 해제
        EventBus<MoveEvent>.OnEvent -= SetMoveInput;
        // 달리기 이벤트 구독 해제
        EventBus<RunEvent>.OnEvent -= SetPressedRunKey;
    }

    // 이동 입력 값 설정 함수
    private void SetMoveInput(MoveEvent data) => moveInput = data.moveInput;

    // 달리기 키 입력 값 설정 함수
    private void SetPressedRunKey(RunEvent data) => pressedRunKey = data.isPressed;

    // 이동 관리 함수
    private void HandleMove()
    {
        // 입력 값에 따른 방향 정하기
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        // 달리기 조건(키 눌림, 스테미나 있음)에 해당되면 달리기 속도, 아니라면 움직이는 속도
        float curSpeed = (pressedRunKey && curStamina > 0) ? runSpeed : moveSpeed;
        // 캐릭터 컨트롤러는 중력이 없으니 y축 눌러주기
        float yVelocity = 0f;

        // 땅에 있다면
        if (cc.isGrounded)
            // 고정적으로 눌러주기
            yVelocity = -2f;

        // 방향에 값 반영
        dir.y = yVelocity;
        // 이동(방향 * 속도 * 시간)
        cc.Move(dir * curSpeed * Time.fixedDeltaTime);

    }

    // 스테미나 관리 함수
    private void HandleStamina()
    {
        // 달리기 조건(키 눌림, 방향키 입력 중)에 해당되면 스테미나 감소, 아니라면 스테미나 증가
        curStamina += (pressedRunKey && moveInput.magnitude > 0) ? -Time.deltaTime : Time.deltaTime;
        // 스테미나 최소값, 최대값에 맞춰서 반영하기
        curStamina = Mathf.Clamp(curStamina, 0, maxStamina);
    }
}