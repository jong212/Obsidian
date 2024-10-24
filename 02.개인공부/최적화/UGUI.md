
| 카테고리     | [[최적화 - MOC]]                                                                   |
| -------- | ------------------------------------------------------------------------------- |
| 글 작성 이유  | UGUI 는 그냥 UI 구성할 때 쓰는 버튼, 텍스트와 같은 컴포넌트들 아닌가? 했는데 유니티 코리아에서 최적화 기법이 있다고 하여 정리해 봄 |
| 공부 후 느낀점 |                                                                                 |
| 참고사이트    | [유니티 코리아 UGUI 의 성능 최적화 유튜브](https://www.youtube.com/watch?v=1e2mSCS7o1A&t=626s) |
## <center>유니티 UGUI 최적화 기법 알아보기</center>
### UGUI란?
게임 및 응용 프로그램의 런타임 UI를 개발하는 데 사용할 수 있는 오래된 GameObject 기반 UI 시스템
### 최적화 필요성?
Description
### UI는 메시로 구성되어 있다.
* UI는 평면에 있어서 UI를 처리하기 위해 다른게 있을까 했지만 UI도 메시로 구성이 되어있다.
* 많은 사각형 평면들의 조합.
* 3D 그래픽에서 신경써야하는 병목도 UI에서 똑같이 적용된다고 보면 된다.

결론은 UI는 메시로 구성 되어있기 때문에 메시가 어떤식으로 만들어 지는지 알면 좋다.
#### 메시는 어떻게 구성되어 있을까?
==메시는 데이터의 조합이다==

GPU는 메시를 아래와 같이 입체적으로 표현 물체로 인식하지 않고 데==이터의 조합==으로 인식한다. 가장 먼저 [[버텍스]] 처리 과정을 알아 보자
![[Pasted image 20241024163908.png||300]]

#### 버텍스 처리 과정


* 버텍스 버퍼 : 데이터 공간이다.
	* 점들의 위치를 저장한 것이라고 생각하면 된다  ( 0, 0, 0 ) ( 1, 1, 0 ) 같은 버텍스를 가지고 있는 주머니인 것.
* 인덱스 버퍼 : 버텍스를 어떤 순서로 조합해서 삼각형을 만들지 처리하는 부분
	* 012230 보면 012 삼각형 하나 230삼각형 하나 이런식으로 삼각형을만듬(실제로 만들지는 않음)
* 렌더 스테이트 : 이 단계에서는 두 가지 작업이 진행된다 
	1. Counter Clockwise : (GPU에게)삼각형을 ==시계방향으로 만들==건지 반시계방향으로 만들건지를 알려줘야해서 그 방식을 정한다
	2. Trinagle List : 2. 삼각형 정보를 List에 담는다
![[Pasted image 20241024164040.png||400]]

### 그래픽 요소를 담은 Graphic 클래스
Description
### 렌더링이 이루어지는 Canvas (C++)
Description
### 하위 캔버스 및 별도 관리가 가능한Nested Canvas
Description
### 계층 구조 시스템에 활용하는 Dirty Flag
Description
### Transform을 상속받는 RectTransform(c++)
Description
### 레이아웃과 메시를 다시 계산하는 Rebuild
Description
### 레너링 명령을 위한 Batch Building (Canvas)
Description
### Batching의 4가지 기준
Description
### Transparent 오브젝트에 영향을 주는 Rendering order
Description
### UI 에서의 Pixel Perfect
Description
### UI 구조화에 유용한 Layout Components
Description
### 레이아웃 컴포넌트를 런타임 상에서 비활성화 하고 에디터 모드에서 작동하기
Description
### Object Pool 활용하기 
Description
### 항상 변경되는 동적인 UI에만 Animator 사용하기
Description
### 필요한 곳에서만 Raycaster 사용하기
Description
### Full Screen UI 사용 시 3D 오브젝트 렌더링 하지 않기
Description
### 글자를 미리 텍스처로 제작하는 TMP
Description
### Xcode & Instrument로 프로파일링하기
Description


> [!Tip]+ GoodTip
>Good
>``` csharp
>public void Test();
>```

> [!Warning]+ Warning
> Warning
> ```csharp 
> Warning
> ```
## <center>Title H2</center>
### H3
Description
### H3
Description

> [!Tip]+ GoodTip
>Good
>``` csharp
>public void Test();
>```

> [!Warning]+ Warning
> Warning
> ```csharp 
> Warning
> ```
　

버텍스 : 버텍스는 그냥 아래와 같은 점인데 아직 화면에는 아무것도 존재하지 않음 그냥 그래픽 카드 내부에서 값으로만 존재하는 데이터일 뿐, 버텍스는 3차원 공감에서 위치, 색상, 법선 텍스처 좌표 등의 데이터를 포함한 정점을 의미