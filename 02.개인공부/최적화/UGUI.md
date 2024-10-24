
| 카테고리     | [[최적화 - MOC]]                                                                                                                                   |
| -------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| 글 작성 이유  | UGUI 는 그냥 UI 구성할 때 쓰는 버튼, 텍스트와 같은 컴포넌트들 아닌가? 했는데 유니티 코리아에서 최적화 기법이 있다고 하여 정리해 봄                                                                 |
| 공부 후 느낀점 |                                                                                                                                                 |
| 참고사이트    | [유니티 코리아 UGUI 의 성능 최적화 유튜브](https://www.youtube.com/watch?v=1e2mSCS7o1A&t=626s)<br>[같은 강의를 들은 다른 블로거의 정리 본](https://dev-junwoo.tistory.com/141) |
## <center>유니티 UGUI 최적화 기법 알아보기</center>
### UGUI 의미
게임 및 응용 프로그램의 런타임 UI를 개발하는 데 사용할 수 있는 오래된 GameObject 기반 UI 시스템
### UI도 최적화가 필요한가
Description
### UI 구성 요소
#### UI는 메쉬로 구성 된다
* UI는 평면에 있어서 UI를 처리하기 위해 다른게 있을까 했지만 UI도 메쉬로 구성이 되어있다.
* 많은 사각형 평면들의 조합.
* 3D 그래픽에서 신경써야하는 병목도 UI에서 똑같이 적용된다고 보면 된다.

결론은 UI는 메쉬로 구성 되어있기 때문에 메쉬가 어떤식으로 만들어 지는지 알아야한다

#### GPU에게 메쉬란
* 메쉬는 데이터의 조합이다
* 점-> 점이 모여 선이 되고 -> 선이 모여 삼각혀이 되고 -> 삼각형이 모여 메쉬가 된다 
![[Pasted image 20241024163908.png||300]]

근데 메쉬라는 게 GPU 입장에서는 위와 같이 입체적인 형태가 아닌 데이터의 조합으로 인식을 하고 있다 이와 관련한 메쉬 생성 과정을 아라보자

#### [[버텍스]] 처리과정
* 버텍스 버퍼  
	* 버텍스 버퍼에서 버텍스들의 정보를 가지고있고(데이터 공간)
		* 점들의 위치를 저장한 것이라고 생각하면 된다  ( 0, 0, 0 ) ( 1, 1, 0 ) 같은 버텍스를 가지고 있는 주머니인 것.
* 인덱스 버퍼
	* 인덱스 버퍼에서 어떤 순서로 선을 이을지에 대한 정보를 가지고 있다.
		* 012230 보면 012 삼각형 하나 230삼각형 하나 이런식으로 삼각형을만듬(실제로 만들지는 않음)
* 렌더 스테이트 : 
	* 그리고 렌더 상태에서는 이 선을 시계방향으로 돌릴것인지 정하고 정보를 리스트에 담는다
		* Counter Clockwise : (GPU에게)삼각형을 ==시계방향으로 만들==건지 반시계방향으로 만들건지를 알려줘야해서 그 방식을 정한다
		* Trinagle List : 2. 삼각형 정보를 List에 담는다
* Vertex Decl(버텍스 정의) 
	* 에서는 위치, 컬러, UV 등을 담고 있다.

#### 버텍스 처리 과정 결론
* GPU는 위의 버텍스 처리 과정을 통해 삼각형을 만들어 낸다
* ★그래서 이 삼각형으로 3D 오브젝트나 UI를 만드는 것이라고 한다★
* UI는 평면 모양의 메쉬 라고 함
![[Pasted image 20241024164040.png||400]]

#### CPU 병목현상
>[!Tip]+ 메쉬가 어떻게 만들어 지는지 과정은 이해했다 근데 메모리 관점에서는?

여기서 이 모든 정보가 합쳐져서 CPU에서 연산을 하여 GPU 메모리로 넘겨주고, 렌더링을 하라는 Draw Call을 내린다.
![[Pasted image 20241024213555.png||500]]
 보통 일반적인 3D 오브젝트들 ex)책상 탁상 건물 등등 로딩타임에 CPU가 연산을 끝내고 GPU에게 넘겨주어 GPU가 계속 그 데이터를 가지고 렌더링을 하면 끝이라고 한다. (스키닝처럼 변하는것 제외)

==하지만 UI는 계속 동적으로 변한다==. (ex. 버튼 클릭, 스크롤 등)

즉, 우리가 육안으로 보이는게 변한다면 매프레임마다 UI도 Mesh이기 때문에 CPU에서 연산을해서 Gpu로 넘겨주어야하는데, 이 과정에서 연산이 많아지면(변하는것이 많아지면) GPU보다는 오히려 ==CPU쪽에 병목이 많이 온다고 한다.== (CPU는 연산을 해서 넘겨줘야 하기 떄문)
### UI의 그래픽 요소를 담당하는 Graphic.cs 클래스
>[!Tip]+ Graphic.cs
>그래픽 클래스는 캔버스에서 랜더링 가능한 모든 Unity UI C#클래스의 기본 클래스 이다.
>
![](https://blog.kakaocdn.net/dn/q6Y84/btsp7JeDtyy/GGI7HApn7ljnqnk6cWGWBk/img.png)

Text,TMP,Image 등 UI로 사용하는 컴포넌트들의 클래스들을 보면 아래와 같이 
MaskableGraphic 클래스를 상속 받고 있다 

Text : MaskableGraphic
![[Pasted image 20241024224140.png]]
TMP : MaskableGraphic
![[Pasted image 20241024224213.png]]
Image : MaskableGraphic
![[Pasted image 20241024224244.png]]

그리고 MaskableGraphic는 또 여러 클래스를 상속받지만 우리가 집중해야할 클래스는 그래픽 클래스 이다 
![[Pasted image 20241024224237.png]]

* UI 관련 컴포넌트들이 그래픽 클래스를 상속받는 이유는 그래픽적인 요소를 가졌기 떄문이다 
* 렌더링의 주체는 캔버스 이지만 그래픽을 다루기 위한 요소들이 그래픽에 담겨있다.

그래픽 클래스에서는 UI의 변경사항이 생기면 아래 함수들을 타고 OnPopulateMesh 함수에서 ==삼각형을 어떻게 구성하는지에== 대한 로직이 실행 되면서 ==버텍스 처리 과정이 다시== 이루어 진다.

Graphic => Rebuild => UpdateGeometric(버텍스) or UpdateMaterial(쉐이더) => DoMeshGeneration => OnPopulateMesh(버텍스 네개 추가,인덱스 구성)

![](https://blog.kakaocdn.net/dn/OVmpb/btsp1gxTPfX/T0qnjkelGb9dUeJbfMmysK/img.png)

 아래 함수가 버텍스 처리 과정의 Vertex Buffer(AddVert)과 IndexBuffer(AddTriangle) 이다.
![](https://blog.kakaocdn.net/dn/dFfIFh/btsp8cnjkY1/3hAfGe9u8nKr7Yz65BSiuk/img.png)

~~앞전에 버텍스 처리 과정에서 설명한 것과 같이~~ 
1. ~~버텍스 구성,~~
2.  ~~인덱스 구성~~
![[Pasted image 20241024231322.png||200]]

~~==결론은 삼각형 두개가 사각형이 되는데 이런 것들이 쫙쫙쫙 여러개 생기는 것이고 UI가 되는 것이다!!==~~

아니였다 아까 OnPopulateMesh는 버텍스 처리 과정에서 인덱스 버퍼까지였고
실제 메쉬에 데이터를 밀어 넣어주는 과정이 FillMesh 였다 이 과정은 또한 
![[Pasted image 20241024232819.png||500]]

버텍스 처리과정에서의 버텍스 디클라 레이션  부분이다!
![[Pasted image 20241024233215.png]]

VU 오타인가? UV 데이터 포지션 컬러 등등..
![[Pasted image 20241024233139.png]]


@@@@@@@@@@@@이어서 영상 보고 작성하기
### 렌더링이 이루어지는 Canvas (C++)
Description
### 하위 캔버스 및 별도 관리가 가능한Nested Canvas
Description
### 계층 구조 시스템에 활용하는 Dirty Flag
Description
### Transform을 상속받는 RectTransform(c++)
Description
### 레이아웃과 메쉬를 다시 계산하는 Rebuild
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