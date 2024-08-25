using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Player.cs             ����: �÷��̾� ĳ���Ϳ� ���� ���¸� �����ϴ� PlayerStateMachine�� �ʱ�ȭ�ϰ�, �÷��̾��� ���� ���¸� ������Ʈ�մϴ�.
// PlayerStateMachine.cs ����: �÷��̾��� ���� ���¸� �����ϰ�, ���¸� ��ȯ�ϴ� ����� ����մϴ�.
// PlayerState.cs        ����: ���� ��ü�� �⺻ Ŭ�����Դϴ�. �� ���´� �� Ŭ������ ��ӹ޾� ���¿� ���� �ൿ�� �����մϴ�.
public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this,stateMachine,"Move");
    }
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
