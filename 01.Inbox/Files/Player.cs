using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Player.cs             역할: 플레이어 캐릭터에 대한 상태를 관리하는 PlayerStateMachine을 초기화하고, 플레이어의 현재 상태를 업데이트합니다.
// PlayerStateMachine.cs 역할: 플레이어의 현재 상태를 저장하고, 상태를 전환하는 기능을 담당합니다.
// PlayerState.cs        역할: 상태 객체의 기본 클래스입니다. 각 상태는 이 클래스를 상속받아 상태에 따른 행동을 정의합니다.
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
