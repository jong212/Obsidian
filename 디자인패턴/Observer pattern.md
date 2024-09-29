
| 카테고리       | [[DesignPattern - MOC]]                                                                                                                                         |
| ---------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 공부목적       |                                                                                                                                                                 |
| 느낀점        |                                                                                                                                                                 |
| 참고사이트      | https://www.youtube.com/watch?v=j4kVuJQUMtw&list=PL412Ym60h6ut9NcbAIfzVgyy5F4O22oSq&index=6<br>[[Level_up_your_code_with_Game_Programming_Pattern-3-ko_kr.pdf]] |
| 깃주소 및 브랜치명 | https://github.com/jong212/designpatterndemo<br>main                                                                                                            |
## 옵저버 패턴(Observer Pattern)

 -한 객체의 상태가 바뀌면 그 책체에 의존하는 다른 객체한테 연락이 가고 자동으로 내용이 갱신되는 방식으로
  일대다(one-to-many) 의존성을 정의한다.
 -한 객체의 상태가 변경되면 그 객체에 의존하는 모든 객체에 연락을 한다.
### 이벤트를  활용한 옵저버 패턴 간단하게 이해하기

만약 
A라는 버튼을 클릭 했을 때 
B오브젝트에서 소리가 나오면서 
C 오브젝트에서 파티클 효과가 나타나오게 하고 싶다면?

1. A 오브젝트에서는 Action 이벤트를 만들어 두고 
2. B,C 오브젝트에서는 A에 접근해서 콜백함수를 미리 등록만 해두면 된다.

간략히 설명하면 위 구조가 옵저버 패턴의 한 예시 이다.

### 코드를 통해 이해하기

> [!NOTE]- 코드 전문 펼쳐보기
> // A 오브젝트는 주체이기 때문에 이벤트 핸들러(Clicked) 생성
> ``` csharp
> using System.Collections;
> using System.Collections.Generic;
> using UnityEngine;
> using System;
> 
> namespace DesignPatterns.Observer
> {
>     [RequireComponent(typeof(Collider))]
>     public class ButtonSubject: MonoBehaviour
>     {
>         public event Action Clicked;
> 
>         private Collider m_Collider;
> 
>         void Start()
>         {
>             m_Collider = GetComponent< Collider>();
>         }
> 
>         public void ClickButton()
>         {
>             Clicked?.Invoke();
>         }
> 
>         void Update()
>         {
>             CheckCollider();
>         }
> 
>         private void CheckCollider()
>         {
>             // Check if the mouse left button is pressed over the collider
>             if (Input.GetMouseButtonDown(0))
>             {
>                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
>                 RaycastHit hitInfo;
> 
>                 if (Physics.Raycast(ray, out hitInfo, 100f))
>                 {
>                     if (hitInfo.collider == this.m_Collider)
>                     {
>                         ClickButton();
>                     }
>                 }
>             }
>         }
>     }
> }
> 
> ```
> ------
> 주체 접근해서 콜백 함수를 등록 해놓는다
> ``` csharp
> using System.Collections;
> using System.Collections.Generic;
> using UnityEngine;
> 
> namespace DesignPatterns.Observer
> {
>     [RequireComponent(typeof(AudioSource))]
>     public class AudioObserver : MonoBehaviour
>     {
>         // dependency to observe
>         [SerializeField] ButtonSubject subjectToObserve;
>         [SerializeField] float delay = 0f;
>         private AudioSource source;
> 
>         private void Awake()
>         {
>             source = GetComponent< AudioSource>();
> 
>             if (subjectToObserve != null)
>             {
>                 subjectToObserve.Clicked += OnThingHappened;
>             }
>         }
> 
>         public void OnThingHappened()
>         {
>             StartCoroutine(PlayWithDelay());
>         }
> 
>         IEnumerator PlayWithDelay()
>         {
>             yield return new WaitForSeconds(delay);
>             source.Stop();
>             source.Play();
>         }
> 
>         private void OnDestroy()
>         {
>             // unsubscribe/deregister from the event if we destroy the object
>             if (subjectToObserve != null)
>             {
>                 subjectToObserve.Clicked -= OnThingHappened;
>             }
>         }
>     }
> }
> ```
> 
> --------
> 마찬가지로 주체 접근해서 콜백 함수를 등록 해놓는다
> ``` csharp
> using System.Collections;
> using System.Collections.Generic;
> using UnityEngine;
> 
> namespace DesignPatterns.Observer
> {
>     public class AnimObserver : MonoBehaviour
>     {
>         [SerializeField] Animation animClip;
>         [SerializeField] ButtonSubject subjectToObserve;
>         void Start()
>         {
>             if (subjectToObserve != null)
>             {
>                 subjectToObserve.Clicked += OnThingHappened;
>             }
>         }
> 
>         private void OnThingHappened()
>         {
>             if (animClip != null)
>             {
>                 animClip.Stop();
>                 animClip.Play();
>             }
>         }
>     }
> }
> 
> ```

이제 아래 주체자의 아래 이벤트만 실행되면 등록 해놓았던 콜백함수가 실행 된다
![[Pasted image 20240821225545.png]]
### 옵저버 패턴 장단점
![[Pasted image 20240821225736.png||500]]

이벤트를 구현하면 약간의 추가 작업이 필요하지만 이점이 있습니다:

**관찰자 패턴은 오브젝트를 분리하는 데 도움이 됩니다:** 이벤트 게시자는 이벤트 구독자에 대해 아무것도 알 필요가 없습니다. 한 클래스와 다른 클래스 사이에 직접적인 의존성을 만드는 대신, 피험자와 관찰자는 어느 정도의 분리(느슨한 결합)를 유지하면서 소통합니다.

**직접 구축할 필요는 없습니다:** C#에는 확립된 이벤트 시스템이 포함되어 있으며, 자체 델리게이트를 정의하는 대신 [System.Action](https://web.archive.org/web/20240302045045/https://docs.microsoft.com/en-us/dotnet/api/system.action?view=net-6.0) 델리게이트를 사용할 수 있습니다. 또는 Unity에는 [UnityEvents](https://web.archive.org/web/20240302045045/https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html) 및 [UnityActions도](https://web.archive.org/web/20240302045045/https://docs.unity3d.com/ScriptReference/Events.UnityAction.html) 포함되어 있습니다.  

**각 옵저버는 자체 이벤트 처리 로직을 구현합니다:** 이러한 방식으로 각 관찰 대상은 응답하는 데 필요한 로직을 유지합니다. 이렇게 하면 디버깅과 단위 테스트가 더 쉬워집니다.  

**사용자 인터페이스에 적합합니다:** 핵심 게임플레이 코드는 UI 로직과 별개로 존재할 수 있습니다. 그러면 UI 요소가 특정 게임 이벤트나 조건을 수신하고 적절하게 반응합니다. MVP 및 MVC 패턴은 이러한 목적으로 옵저버 패턴을 사용합니다.

하지만 다음과 같은 주의 사항도 숙지해야 합니다:

**이는 복잡성을 더합니다:** 다른 패턴과 마찬가지로 이벤트 중심 아키텍처를 만들려면 처음에 더 많은 설정이 필요합니다. 또한 피사체 또는 관찰자를 삭제할 때는 주의하세요. 옵저버가 더 이상 필요하지 않을 때 메모리 참조가 제대로 해제될 수 있도록 OnDestroy에서 옵저버 등록을 해제해야 합니다.  

**관찰자는 이벤트를 정의하는 클래스에 대한 참조가 필요합니다:** 옵저버는 여전히 이벤트를 게시하는 클래스에 대한 종속성을 가집니다. 모든 이벤트를 처리하는 정적 이벤트 관리자(다음 섹션 참조)를 사용하면 오브젝트를 서로 분리하는 데 도움이 될 수 있습니다.  

**성능이 문제가 될 수 있습니다:** 이벤트 중심 아키텍처는 오버헤드를 추가합니다. 큰 씬과 많은 게임 오브젝트는 성능을 저해할 수 있습니다.
