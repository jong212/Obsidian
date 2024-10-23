![[Diagram 2.svg]]

각 클래스마다 어떤 기능을 해야 하는지 구체적으로 설명드릴게요. 여기서는 **각 클래스의 역할**과 **어떤 기능을 구현해야 하는지**를 더 명확하게 정의하겠습니다.

---

### **1. `Entity` 클래스**
`Entity` 클래스는 **모든 캐릭터**(적, 플레이어 등)에 공통적으로 적용될 **기본 기능**을 정의하는 클래스입니다. **공통적인 동작**을 여러 캐릭터 간에 재사용할 수 있도록 하기 위해 만들었습니다.

#### **`Entity` 클래스에서 해야 할 일:**
1. **캐릭터 공통 속성 정의**:
   - **애니메이션(`Animator`)**, **물리적 속성(`Rigidbody2D`)**, **충돌 처리(`Collider`)** 등의 기본 컴포넌트를 정의.
   - 모든 캐릭터가 공통적으로 가질 수 있는 속성을 정의.

2. **공통 동작 구현**:
   - **이동, 속도 제어, 넉백 처리, 충돌 처리, 죽음 처리**와 같은 캐릭터의 기본 동작을 구현.
   - **메서드 예시**:
     - `SetVelocity()`: 캐릭터의 이동 속도를 설정.
     - `Flip()`: 캐릭터의 방향을 바꿔주는 함수.
     - `Die()`: 캐릭터가 죽었을 때 호출되는 함수.

3. **가상 메서드 제공**:
   - 특정 캐릭터(적 또는 플레이어)에 맞게 재정의할 수 있는 가상 메서드(`virtual`)를 제공.
   - 예를 들어, **플레이어와 적의 죽음 처리**가 다를 수 있으므로 `Die()` 메서드를 **가상 메서드로 정의**하고, 적과 플레이어에서 각각 다르게 구현할 수 있도록 함.

#### **구현 예시:**
```csharp
public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    public virtual void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }

    public virtual void Die()
    {
        // 캐릭터가 죽었을 때 처리
    }
}
```

---

### **2. `Enemy` 클래스**
`Enemy` 클래스는 **적**에 특화된 기능을 정의하는 클래스입니다. **`Entity`를 상속받아 적의 특화된 기능을 추가**합니다.

#### **`Enemy` 클래스에서 해야 할 일:**
1. **적 특화 속성 추가**:
   - **스턴**, **전투 거리** 등 **적에게만 해당되는 속성**을 정의.
   - 예를 들어, `CanBeStunned()` 같은 함수는 적에게만 적용될 수 있습니다.

2. **상태 패턴(State Machine) 추가**:
   - 적의 행동을 상태로 관리할 수 있도록 **상태 머신**을 구현.
   - 예: **Idle(대기)**, **Move(이동)**, **Attack(공격)**, **Stunned(스턴)**, **Dead(죽음)** 상태로 나누어 각 상태에서 적의 행동을 제어.

3. **상태 전환 관리**:
   - 적이 특정 상태(예: 대기, 이동, 공격)에서 **다른 상태로 전환**하는 로직을 관리.

#### **구현 예시:**
```csharp
public class Enemy : Entity
{
    public float moveSpeed = 2f;
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update(); // 현재 상태를 업데이트
    }

    public virtual void CanBeStunned()
    {
        // 스턴 가능 여부를 확인하는 로직
    }

    public override void Die()
    {
        base.Die();
        // 적의 구체적인 죽음 처리
    }
}
```

---

### **3. `Enemy_Skeleton` 클래스**
`Enemy_Skeleton` 클래스는 **구체적인 스켈레톤 적**의 행동과 상태를 정의하는 클래스입니다. **`Enemy` 클래스를 상속받아 스켈레톤 적의 상태와 동작**을 구현합니다.

#### **`Enemy_Skeleton` 클래스에서 해야 할 일:**
1. **구체적인 상태 정의**:
   - 스켈레톤의 **상태(State)**를 구체적으로 정의.
   - 예: **`IdleState`, `MoveState`, `AttackState`, `StunnedState`, `DeadState`** 등.

2. **상태 전환 로직**:
   - 스켈레톤의 **상태 전환**을 구현.
   - 예: 스켈레톤이 **대기 상태에서 플레이어를 감지하면** **공격 상태로 전환**.

3. **구체적인 스켈레톤 특화 동작**:
   - **스켈레톤만의 고유한 동작**을 추가.
   - 예: **스켈레톤이 화살을 발사**하는 동작, **스켈레톤이 특정 상황에서 점프**하는 동작 등.

#### **구현 예시:**
```csharp
public class Enemy_Skeleton : Enemy
{
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); // 초기 상태를 Idle로 설정
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(attackState); // 죽을 때의 상태 전환
    }
}
```

---

### **정리**

- **`Entity` 클래스**는 모든 캐릭터(적, 플레이어 등)에 적용되는 **기본 동작**(이동, 속도, 충돌 처리 등)을 정의합니다.
- **`Enemy` 클래스**는 **적 캐릭터에 특화된 동작**(스턴, 상태 패턴 등)을 추가로 정의합니다.
- **`Enemy_Skeleton` 클래스**는 **스켈레톤 적의 구체적인 행동과 상태**를 관리하며, **스켈레톤에 특화된 동작**(공격, 죽음 등)을 구현합니다.

이렇게 클래스별로 **역할과 책임을 분리**하면 유지보수와 확장이 용이해지며, 다른 적 캐릭터도 쉽게 추가할 수 있게 됩니다. 각 클래스는 자신의 역할에 맞는 기능만을 담당하게 되므로, 코드가 명확해집니다.

이해가 되셨나요? 추가 질문이 있으면 언제든지 물어보세요! 😊

## 프로젝트 메모
* 241017
	* StaticManager > UIManager > Alert 방향으로 설계한 이유와 게터세터 부분 파악해서 정리하기
	* OnClickCreateNicknameButton > BaseUI, protected, ShowAlertUI() > StaticManager > UIManager > AlertUI > AlertUI의 OpenWarningUI 실행 메커니즘 이해
 ## 기획
## 구현 리스트

### 게임시작

**1. 데이터 세팅 (어드레서블)**

**사용이유**
어드레서블을 사용한 이유는 모바일 출시를 계획하고 있기 때문에 변경사항에 대해 손쉽게 패치하기 위해서

**어드레서블 세팅**
https://www.youtube.com/watch?v=Z84GCeod_BM  (로컬 세팅)
https://www.youtube.com/watch?v=uTSxPPaW2-k (서버세팅)
로컬이랑 서버 세팅이라는데 일단 로컬 보면서 세팅하고 서버세팅 영상도 보면서 세팅함

영상에서 공부한 내용 아래 기록하였음 (로컬용이긴한데 어차피 서버 영상도 이어져서 일단 따라하였음)

Cache Clear Behavior : ClearWhenNewVersionLoaded 옵션을 선택했다 그 이유는 서버에 최산화 된 번들이 있을 때 기존 번들에 매치되는 캐시 파일을 자동으로 정리하는 방식으로 작동하기 때문이다 

Unique Bundle  IDS : 옵션을 체크하였다 안 푸는게 좋다고 한다 이걸 체크 해제하면 a가 있는데 빌드하고 b가 추가 됐을 뿐인데 a로 또 받는다고 한다 어쩃든 체크!
Send profiler events : 체크한다 메모리 로드 언로드를 확인할 수 있다고 한다



**2. PlayFab (로그인)**
* 로그인 완료 시 
	* 플레이어 데이터 세팅
	* 코어 데이터 세팅
	* 인벤토리데이터 세팅








> [!NOTE]- ds
> ![[Diagram 2.svg]]
> 
> 각 클래스마다 어떤 기능을 해야 하는지 구체적으로 설명드릴게요. 여기서는 **각 클래스의 역할**과 **어떤 기능을 구현해야 하는지**를 더 명확하게 정의하겠습니다.
> 
> ---
> 
> ### **1. `Entity` 클래스**
> `Entity` 클래스는 **모든 캐릭터**(적, 플레이어 등)에 공통적으로 적용될 **기본 기능**을 정의하는 클래스입니다. **공통적인 동작**을 여러 캐릭터 간에 재사용할 수 있도록 하기 위해 만들었습니다.
> 
> #### **`Entity` 클래스에서 해야 할 일:**
> 1. **캐릭터 공통 속성 정의**:
>    - **애니메이션(`Animator`)**, **물리적 속성(`Rigidbody2D`)**, **충돌 처리(`Collider`)** 등의 기본 컴포넌트를 정의.
>    - 모든 캐릭터가 공통적으로 가질 수 있는 속성을 정의.
> 
> 2. **공통 동작 구현**:
>    - **이동, 속도 제어, 넉백 처리, 충돌 처리, 죽음 처리**와 같은 캐릭터의 기본 동작을 구현.
>    - **메서드 예시**:
>      - `SetVelocity()`: 캐릭터의 이동 속도를 설정.
>      - `Flip()`: 캐릭터의 방향을 바꿔주는 함수.
>      - `Die()`: 캐릭터가 죽었을 때 호출되는 함수.
> 
> 3. **가상 메서드 제공**:
>    - 특정 캐릭터(적 또는 플레이어)에 맞게 재정의할 수 있는 가상 메서드(`virtual`)를 제공.
>    - 예를 들어, **플레이어와 적의 죽음 처리**가 다를 수 있으므로 `Die()` 메서드를 **가상 메서드로 정의**하고, 적과 플레이어에서 각각 다르게 구현할 수 있도록 함.
> 
> #### **구현 예시:**
> ```csharp
> public class Entity : MonoBehaviour
> {
>     public Animator anim { get; private set; }
>     public Rigidbody2D rb { get; private set; }
>     public CapsuleCollider2D cd { get; private set; }
> 
>     protected virtual void Awake()
>     {
>         anim = GetComponentInChildren< Animator>();
>         rb = GetComponent< Rigidbody2D>();
>         cd = GetComponent< CapsuleCollider2D>();
>     }
> 
>     public virtual void SetVelocity(float x, float y)
>     {
>         rb.velocity = new Vector2(x, y);
>     }
> 
>     public virtual void Die()
>     {
>         // 캐릭터가 죽었을 때 처리
>     }
> }
> ```
> 
> ---
> 
> ### **2. `Enemy` 클래스**
> `Enemy` 클래스는 **적**에 특화된 기능을 정의하는 클래스입니다. **`Entity`를 상속받아 적의 특화된 기능을 추가**합니다.
> 
> #### **`Enemy` 클래스에서 해야 할 일:**
> 1. **적 특화 속성 추가**:
>    - **스턴**, **전투 거리** 등 **적에게만 해당되는 속성**을 정의.
>    - 예를 들어, `CanBeStunned()` 같은 함수는 적에게만 적용될 수 있습니다.
> 
> 2. **상태 패턴(State Machine) 추가**:
>    - 적의 행동을 상태로 관리할 수 있도록 **상태 머신**을 구현.
>    - 예: **Idle(대기)**, **Move(이동)**, **Attack(공격)**, **Stunned(스턴)**, **Dead(죽음)** 상태로 나누어 각 상태에서 적의 행동을 제어.
> 
> 3. **상태 전환 관리**:
>    - 적이 특정 상태(예: 대기, 이동, 공격)에서 **다른 상태로 전환**하는 로직을 관리.
> 
> #### **구현 예시:**
> ```csharp
> public class Enemy : Entity
> {
>     public float moveSpeed = 2f;
>     public EnemyStateMachine stateMachine { get; private set; }
> 
>     protected override void Awake()
>     {
>         base.Awake();
>         stateMachine = new EnemyStateMachine();
>     }
> 
>     protected override void Update()
>     {
>         base.Update();
>         stateMachine.currentState.Update(); // 현재 상태를 업데이트
>     }
> 
>     public virtual void CanBeStunned()
>     {
>         // 스턴 가능 여부를 확인하는 로직
>     }
> 
>     public override void Die()
>     {
>         base.Die();
>         // 적의 구체적인 죽음 처리
>     }
> }
> ```
> 
> ---
> 
> ### **3. `Enemy_Skeleton` 클래스**
> `Enemy_Skeleton` 클래스는 **구체적인 스켈레톤 적**의 행동과 상태를 정의하는 클래스입니다. **`Enemy` 클래스를 상속받아 스켈레톤 적의 상태와 동작**을 구현합니다.
> 
> #### **`Enemy_Skeleton` 클래스에서 해야 할 일:**
> 1. **구체적인 상태 정의**:
>    - 스켈레톤의 **상태(State)**를 구체적으로 정의.
>    - 예: **`IdleState`, `MoveState`, `AttackState`, `StunnedState`, `DeadState`** 등.
> 
> 2. **상태 전환 로직**:
>    - 스켈레톤의 **상태 전환**을 구현.
>    - 예: 스켈레톤이 **대기 상태에서 플레이어를 감지하면** **공격 상태로 전환**.
> 
> 3. **구체적인 스켈레톤 특화 동작**:
>    - **스켈레톤만의 고유한 동작**을 추가.
>    - 예: **스켈레톤이 화살을 발사**하는 동작, **스켈레톤이 특정 상황에서 점프**하는 동작 등.
> 
> #### **구현 예시:**
> ```csharp
> public class Enemy_Skeleton : Enemy
> {
>     public SkeletonIdleState idleState { get; private set; }
>     public SkeletonAttackState attackState { get; private set; }
> 
>     protected override void Awake()
>     {
>         base.Awake();
>         idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
>         attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
>     }
> 
>     protected override void Start()
>     {
>         base.Start();
>         stateMachine.Initialize(idleState); // 초기 상태를 Idle로 설정
>     }
> 
>     public override void Die()
>     {
>         base.Die();
>         stateMachine.ChangeState(attackState); // 죽을 때의 상태 전환
>     }
> }
> ```
> 
> ---
> 
> ### **정리**
> 
> - **`Entity` 클래스**는 모든 캐릭터(적, 플레이어 등)에 적용되는 **기본 동작**(이동, 속도, 충돌 처리 등)을 정의합니다.
> - **`Enemy` 클래스**는 **적 캐릭터에 특화된 동작**(스턴, 상태 패턴 등)을 추가로 정의합니다.
> - **`Enemy_Skeleton` 클래스**는 **스켈레톤 적의 구체적인 행동과 상태**를 관리하며, **스켈레톤에 특화된 동작**(공격, 죽음 등)을 구현합니다.
> 
> 이렇게 클래스별로 **역할과 책임을 분리**하면 유지보수와 확장이 용이해지며, 다른 적 캐릭터도 쉽게 추가할 수 있게 됩니다. 각 클래스는 자신의 역할에 맞는 기능만을 담당하게 되므로, 코드가 명확해집니다.
> 
> 이해가 되셨나요? 추가 질문이 있으면 언제든지 물어보세요! 😊

