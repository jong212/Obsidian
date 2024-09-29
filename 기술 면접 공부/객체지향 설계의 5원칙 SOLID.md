| 카테고리  | [[기초 - MOC]]                                                                                |
| ----- | ------------------------------------------------------------------------------------------- |
| 공부목적  |                                                                                             |
| 느낀점   |                                                                                             |
| 참고사이트 | https://www.youtube.com/watch?v=iyeRmq24HVk&list=PL412Ym60h6ut9NcbAIfzVgyy5F4O22oSq&index=7 |
## SOLID 원칙이란
객체지향 설계에서 지켜줘야 할 5개의 소프트웨어 개발 원칙( **S**RP, **O**CP, **L**SP, **I**SP, **D**IP )을 말한다.
- **S**RP(Single Responsibility Principle): 단일 책임 원칙
- **O**CP(Open Closed Priciple): 개방 폐쇄 원칙
- **L**SP(Listov Substitution Priciple): 리스코프 치환 원칙
- **I**SP(Interface Segregation Principle): 인터페이스 분리 원칙
- **D**IP(Dependency Inversion Principle): 의존 역전 원칙

### 1. 단일 책임원칙 
![[Pasted image 20240818211343.png|500]]
예를들어 Audio Source는 오디오와 관련 된 기능만 담당 한다던가 MeshRenderer 는 메시와 관련 된 기능을 한다던가 이런식으로 모든 클래스는 하나의 책임만을 가져야 한다는 게 단일 책임 원칙이다 메시렌더러인데 오디오를 출력한다던가 이런 게 단일 책임원칙을 위배하는 것이다
> [!NOTE]- 단일 책임 원칙을 위배 하는 예시
> 단일 책임 원칙을 지키지 않는 경우, 클래스가 여러 가지 일을 동시에 하려고 합니다. 예를 들어, 플레이어의 이동과 데이터를 저장하는 기능을 하나의 클래스가 모두 처리하는 경우입니다.
> ```csharp 
> using UnityEngine;
> 
> public class PlayerController : MonoBehaviour
> {
>     public float speed = 5f;
>     
>     // Update 함수에서 이동 및 데이터 저장 모두 수행
>     void Update()
>     {
>         // 플레이어 이동 처리
>         float moveHorizontal = Input.GetAxis("Horizontal");
>         float moveVertical = Input.GetAxis("Vertical");
> 
>         Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
>         transform.Translate(movement * speed * Time.deltaTime);
> 
>         // 데이터 저장 처리
>         if (Input.GetKeyDown(KeyCode.S))
>         {
>             SavePlayerData();
>         }
>     }
> 
>     // 데이터를 저장하는 함수
>     void SavePlayerData()
>     {
>         PlayerPrefs.SetFloat("PlayerX", transform.position.x);
>         PlayerPrefs.SetFloat("PlayerY", transform.position.y);
>         PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
>         Debug.Log("Player Data Saved");
>     }
> }
> 
> ```

> [!NOTE]- 단일 책임 원칙을 잘 지킨 예시
> 클래스는 하나의 책임만 가집니다. 이동과 데이터 저장이 분리되어 있으므로, 각각의 클래스가 독립적으로 변경될 수 있고, 유지보수와 확장이 용이해집니다. 예를 들어, 데이터를 저장하는 방식(예: 클라우드 저장으로 변경)이 달라지더라도 `PlayerMovement` 클래스는 변경할 필요가 없습니다.
> ``` csharp
> using UnityEngine;
> 
> public class PlayerMovement : MonoBehaviour
> {
>     public float speed = 5f;
> 
>     // 이동만 처리
>     void Update()
>     {
>         MovePlayer();
>     }
> 
>     void MovePlayer()
>     {
>         float moveHorizontal = Input.GetAxis("Horizontal");
>         float moveVertical = Input.GetAxis("Vertical");
> 
>         Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
>         transform.Translate(movement * speed * Time.deltaTime);
>     }
> }
> 
> public class PlayerDataSaver : MonoBehaviour
> {
>     public Transform playerTransform;
> 
>     // 데이터 저장만 처리
>     void Update()
>     {
>         if (Input.GetKeyDown(KeyCode.S))
>         {
>             SavePlayerData();
>         }
>     }
> 
>     void SavePlayerData()
>     {
>         PlayerPrefs.SetFloat("PlayerX", playerTransform.position.x);
>         PlayerPrefs.SetFloat("PlayerY", playerTransform.position.y);
>         PlayerPrefs.SetFloat("PlayerZ", playerTransform.position.z);
>         Debug.Log("Player Data Saved");
>     }
> }
> 
> ```

### 2. 개방폐쇄원칙
개방-폐쇄 원칙은 SOLID 원칙 중 하나로, "확장에는 열려 있고, 수정에는 닫혀 있어야 한다"는 원칙입니다. 

이 원칙은 객체 지향 설계에서 매우 중요한 개념으로, 기존의 코드를 수정하지 않고 새로운 기능을 추가할 수 있도록 설계해야 한다는 뜻입니다.
![[Pasted image 20240818215023.png|500]]
> [!NOTE]- 개방폐쇄 원칙을 치키지 않은  경우
> 아래 예시는 개방-폐쇄 원칙을 지키지 않은 코드입니다. 새로운 무기 유형을 추가할 때마다 기존 코드를 수정해야 합니다.
> ``` csharp
> using UnityEngine;
> 
> public class Player : MonoBehaviour
> {
>     public void Attack(string weaponType)
>     {
>         if (weaponType == "Sword")
>         {
>             Debug.Log("Attack with Sword!");
>         }
>         else if (weaponType == "Bow")
>         {
>             Debug.Log("Attack with Bow!");
>         }
>         else if (weaponType == "Magic")
>         {
>             Debug.Log("Attack with Magic!");
>         }
>     }
> }
> ```

> [!NOTE]- 개방폐쇄 원칙을 지킨경우
> 
**확장에는 열려 있음**: 새로운 무기 유형을 추가하고 싶다면, 단순히 새로운 무기 클래스를 작성하고 `IWeapon` 인터페이스를 구현하면 됩니다. 기존의 `Player` 클래스는 수정할 필요가 없습니다.
예시로 `Gun` 무기를 추가할 때는 다음과 같이 구현할 수 있습니다:
> ``` csharp
> using UnityEngine;
> 
> // 무기 인터페이스 정의
> public interface IWeapon
> {
>     void Attack();
> }
> 
> // 각각의 무기 클래스가 IWeapon 인터페이스를 구현
> public class Sword : IWeapon
> {
>     public void Attack()
>     {
>         Debug.Log("Attack with Sword!");
>     }
> }
> 
> public class Bow : IWeapon
> {
>     public void Attack()
>     {
>         Debug.Log("Attack with Bow!");
>     }
> }
> 
> public class Magic : IWeapon
> {
>     public void Attack()
>     {
>         Debug.Log("Attack with Magic!");
>     }
> }
> ```
> ### 객체를 저장하는 IWeapon 변수
> ``` csharp
> // 플레이어 클래스는 IWeapon을 사용하여 공격
> public class Player : MonoBehaviour
> {
>     private IWeapon currentWeapon;
> 
>     // 무기를 설정하는 함수
>     public void SetWeapon(IWeapon weapon)
>     {
>         currentWeapon = weapon;
>     }
> 
>     public void Attack()
>     {
>         currentWeapon.Attack();
>     }
> }
> 
> ```
> ### 아래와 같이 확장 가능
> ```csharp
> public class Gun : IWeapon
> {
>     public void Attack()
>     {
>         Debug.Log("Attack with Gun!");
>     }
> }
> ```
> ### 무기 사용
> ``` csharp
> public class GameManager : MonoBehaviour
> {
>     public Player player; // 유니티에서 플레이어 오브젝트를 할당
> 
>     void Start()
>     {
>         // 플레이어에게 무기를 설정
>         IWeapon sword = new Sword();
>         IWeapon bow = new Bow();
>         IWeapon magic = new Magic();
> 
>         // 검을 무기로 설정
>         player.SetWeapon(sword);
> 
>         // 플레이어가 공격
>         player.Attack();
> 
>         // 활로 무기 변경
>         player.SetWeapon(bow);
>         player.Attack();
> 
>         // 마법으로 무기 변경
>         player.SetWeapon(magic);
>         player.Attack();
>     }
> }
> ```
> 

### 3. 리스코프 치환 원칙
> [!NOTE]- 리스코프 치환  원칙을 준수한 케이스 
> 
> 
> 리스코프 치환 원칙(Liskov Substitution Principle, LSP)은 객체 지향 프로그래밍의 SOLID 원칙 중 하나로, "서브 클래스는 반드시 자신의 기반 클래스에서 정의된 기능을 수행할 수 있어야 한다"는 원칙입니다. 이를 준수하려면 서브 클래스가 기반 클래스의 행동을 변경하거나 깨지 않도록 해야 합니다.
> 
> 유니티에서는 리스코프 치환 원칙을 따르는 스크립트를 작성할 때, 상속을 사용하되, 기반 클래스의 계약을 준수하도록 코드를 작성해야 합니다. 아래는 LSP를 준수하는 예시를 각기 다른 스크립트로 정리했습니다.
> 
> ### 1. 기반 클래스 예시: `Enemy.cs`
> 
> ```csharp
> public class Enemy : MonoBehaviour
> {
>     public virtual void TakeDamage(int damage)
>     {
>         Debug.Log("Enemy takes damage: " + damage);
>     }
> }
> ```
> 
> - **설명**: `Enemy`는 기본적으로 데미지를 받는 행동을 구현한 기반 클래스입니다. 이 클래스는 서브 클래스에서 상속받아 확장할 수 있지만, 기본적인 동작(데미지를 받는 것)은 변경하지 않아야 합니다.
> 
> ### 2. 서브 클래스 1 예시: `Zombie.cs`
> 
> ```csharp
> public class Zombie : Enemy
> {
>     public override void TakeDamage(int damage)
>     {
>         Debug.Log("Zombie takes reduced damage: " + (damage / 2));
>     }
> }
> ```
> 
> - **설명**: `Zombie`는 `Enemy`를 상속받아 데미지를 덜 받는 좀비를 구현합니다. 기반 클래스에서 제공하는 데미지를 받는 기능을 확장하여 좀비가 피해를 절반만 받도록 했지만, 여전히 기본적으로 데미지를 받는 동작은 유지됩니다. 이는 리스코프 치환 원칙을 준수한 예입니다.
> 
> ### 3. 서브 클래스 2 예시: `Robot.cs`
> 
> ```csharp
> public class Robot : Enemy
> {
>     public override void TakeDamage(int damage)
>     {
>         Debug.Log("Robot takes no damage because it's armored.");
>     }
> }
> ```
> 
> - **설명**: `Robot`은 `Enemy`의 서브 클래스로, 데미지를 받지 않는 로봇을 구현합니다. 로봇은 데미지를 전혀 받지 않지만, 여전히 `TakeDamage` 메서드를 구현하여 기반 클래스의 계약을 준수합니다. 이 역시 LSP를 지키고 있습니다.
> 
> ### 4. 클라이언트 코드 예시: `Game.cs`
> 
> ```csharp
> public class Game : MonoBehaviour
> {
>     private void Start()
>     {
>         Enemy zombie = new Zombie();
>         Enemy robot = new Robot();
> 
>         zombie.TakeDamage(10);
>         robot.TakeDamage(10);
>     }
> }
> ```
> 
> - **설명**: 클라이언트 코드에서는 `Enemy` 타입의 변수를 사용해 좀비와 로봇을 처리합니다. 두 객체 모두 `Enemy`를 상속받아 처리되고 있으며, 각각의 `TakeDamage` 메서드가 올바르게 호출됩니다. 이는 서브 클래스가 기반 클래스와 상호교환 가능하다는 LSP 원칙을 잘 따르고 있는 예입니다.
> -  Enemy zombie = new Zombie(); 여기서 zombie 변수는 Enemy타입이지만 실제로는 Zmbie() 객체를 참조함 
> - 객체 지향 프로그래밍에서 부모 클래스 타입의 변수가 자식 클래스의 인스턴스를 참조할 수 있는 이유는 **다형성(Polymorphism)** 덕분입니다. 이를 통해 부모 클래스와 자식 클래스 간의 상호 교환이 가능해집니다.
> 
> ### 리스코프 치환 원칙 요약
> 
> 리스코프 치환 원칙을 준수하려면:
> 1. 서브 클래스는 기반 클래스의 행동을 바꾸거나 깨지 말아야 합니다.
> 2. 기반 클래스와 서브 클래스는 상호 교환 가능해야 하며, 클라이언트는 두 클래스 간의 차이를 알 필요가 없어야 합니다.
> 3. 서브 클래스는 기반 클래스의 계약을 존중해야 하며, 메서드를 재정의할 때 기반 클래스에서 요구하는 모든 기능을 유지해야 합니다.
> 
> 이러한 예시를 통해 유니티에서 리스코프 치환 원칙을 준수하는 코드를 작성할 수 있습니다.

> [!NOTE]- LSP를 준수하지 않는 예시 
> 리스코프 치환 원칙(LSP)을 준수하지 않는 예시에서는 서브 클래스가 기반 클래스의 동작을 변경하거나 깨트리는 경우가 발생합니다. 이러한 경우, 서브 클래스는 기반 클래스와 교체될 수 없으며, 예상치 못한 행동이 발생하게 됩니다.
> #### 1. 기반 클래스 예시: `Enemy.cs`
> 
> ```csharp
> public class Enemy : MonoBehaviour
> {
>     public virtual void TakeDamage(int damage)
>     {
>         Debug.Log("Enemy takes damage: " + damage);
>     }
> }
> ```
> 
> #### 2. 서브 클래스 1 예시: `Zombie.cs`
> 
> ```csharp
> public class Zombie : Enemy
> {
>     public override void TakeDamage(int damage)
>     {
>         if (damage > 0)
>         {
>             Debug.Log("Zombie takes damage: " + damage);
>         }
>         else
>         {
>             throw new ArgumentException("Damage cannot be negative");
>         }
>     }
> }
> ```
> 
> - **설명**: 이 예시에서는 `Zombie` 클래스가 `TakeDamage` 메서드를 재정의하면서 추가적인 제약을 추가했습니다. 여기서 `damage`가 0 이하인 경우 예외를 던지도록 구현되었습니다. 그러나 `Enemy` 클래스는 그러한 제한을 두지 않기 때문에 클라이언트는 `Enemy` 타입으로 처리되는 객체가 항상 데미지를 받을 수 있을 것이라 기대합니다. `Zombie`가 그 기대를 깨면서 리스코프 치환 원칙을 위반하게 됩니다.
> 
> #### 3. 서브 클래스 2 예시: `Ghost.cs`
> 
> ```csharp
> public class Ghost : Enemy
> {
>     public override void TakeDamage(int damage)
>     {
>         // Ghosts are immune to damage, but instead we cause an error
>         throw new NotImplementedException("Ghosts cannot take damage!");
>     }
> }
> ```
> 
> - **설명**: `Ghost` 클래스는 `Enemy`를 상속받지만 `TakeDamage` 메서드를 아예 구현하지 않고 예외를 던집니다. 기반 클래스는 데미지를 처리할 수 있다고 기대하지만, 이 서브 클래스는 그 기능을 아예 지원하지 않으므로 리스코프 치환 원칙을 위반하게 됩니다. 클라이언트는 `Enemy` 타입을 기대하고 `TakeDamage` 메서드를 호출했으나 예외가 발생하여 프로그램이 중단될 수 있습니다.
> 
> #### 4. 클라이언트 코드 예시: `Game.cs`
> 
> ```csharp
> public class Game : MonoBehaviour
> {
>     private void Start()
>     {
>         Enemy zombie = new Zombie();
>         Enemy ghost = new Ghost();
> 
>         zombie.TakeDamage(10);  // 정상적으로 작동
>         ghost.TakeDamage(10);   // 예외 발생으로 인해 프로그램 중단
>     }
> }
> ```
> 
> - **설명**: `Game` 클래스에서 `Enemy` 타입을 사용하여 `zombie`와 `ghost`를 처리합니다. `Zombie`의 경우, 데미지가 0 이하인 경우 예외가 발생할 수 있고, `Ghost`는 아예 데미지를 받을 수 없어 예외가 발생합니다. 이와 같은 동작은 기반 클래스의 예상된 동작을 깨트리기 때문에 리스코프 치환 원칙을 위반하는 예입니다.
> 
> ### 리스코프 치환 원칙을 위반하는 문제점 요약
> 
> 1. **기반 클래스의 계약 위반**: 서브 클래스가 기반 클래스에서 기대하는 계약(예: `TakeDamage` 메서드가 정상적으로 동작)을 지키지 않으면 문제가 발생합니다.
> 2. **클라이언트 코드의 신뢰도 저하**: 클라이언트는 기반 클래스의 행동을 신뢰할 수 없게 되며, 예상치 못한 예외나 오류로 인해 프로그램이 중단될 수 있습니다.
> 3. **예외 발생**: 서브 클래스가 기반 클래스의 기대와 다른 동작을 하여 예외를 던지면, 안전한 객체 치환이 불가능해집니다.
> 
> 이러한 방식으로 리스코프 치환 원칙을 준수하지 않는 경우, 코드가 불안정해지며 유지보수와 확장이 어려워질 수 있습니다.

**리스코프 치환 원칙을 잘 준수하는 방법**
예를들어 자동차, 기차 클래스가 각각 있다고 했을 때 둘의 공통점은 탈 것이다 
그리하여 기반 클래스를 Vehicle로 하고 메드를 아래와 같이 선언 한다면 어떻게 될까?
![[Pasted image 20240819010257.png|500]]
자동차 클래스에서는 기반 클래스에서 상속받은 앞 뒤 좌 우 메서드를 다 사용할 수 있겠지만 
기차 클래스에서는 앞으로 전진만 가능하기 때문에 아래와 같이 불 필요한 메서드가 생기기 마련이다
기반 클래스의 모든 메서드를 사용하지 않는(무력화) 상황은 리스코프치환원칙에 위반 된다
![[Pasted image 20240819010430.png|500]]
이럴 땐 ==인터페이스를== 사용해야한다 
앞,뒤 를 묶는 인터페이스와 좌,우를 묶는 인터페이스 이렇게 나누고 필요한 인터페이스만 아래와 같이 적용하면 된다.
인터페이스는 여러개 상속 가능한 장점이 있다.
![[Pasted image 20240819010553.png|500]]
### 4.인터페이스 분리 원칙
유닛을 만든다고 했을 때 아래와 같이 인터페이스 하나에 기능을 몰빵하면 안 됨
![[Pasted image 20240819011505.png|500]]
아래와 같이 큰 카테고리 별로 인터페이스를 나누는 게 좋음
![[Pasted image 20240819010834.png|800]]

