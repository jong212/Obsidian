
| Git          | https://github.com/jong212/MVVM.git                                                |
| ------------ | ---------------------------------------------------------------------------------- |
| BranchName   | 완성 브랜치명 : SkillButtonClick 체크아웃<br>                                                |
| RefFile      | https://drive.google.com/file/d/1v1DqIdTd3YwKuXaJer716cylbCKrg8Tz/view?usp=sharing |
| Memo         |                                                                                    |
| MOC_Category | [[DesignPattern - MOC]]                                                            |
## MVVM 패턴
MVVM(Model-View-ViewModel) 패턴은 UI와 비즈니스 로직을 분리하는 구조로, 유연한 데이터 바인딩을 가능하게 합니다.
* View (뷰): 사용자가 보는 화면으로, 데이터를 직접 처리하지 않고 단순히 표시하는 역할을 합니다. 데이터를 ViewModel로부터 전달받아 UI에 반영합니다. View는 자신의 역할인 데이터 표현에 충실하며, 처리 로직을 담당하지 않습니다.
* ViewModel (뷰모델): View와 Model 사이의 중재자 역할을 하며, View에 필요한 데이터를 제공하고 변경 사항을 알려줍니다. 주로 INotifyPropertyChanged 인터페이스를 구현하여 데이터가 변경되면 View에 알림을 보냅니다. ViewModel은 View를 직접 참조하지 않고, 데이터를 수정하거나 전달하는 역할만 합니다.
* Model (모델): 실제 비즈니스 로직이나 데이터 구조를 담고 있는 부분입니다. ViewModel을 통해 데이터를 제공받으며, 데이터 변경에 따른 비즈니스 로직을 처리합니다.

### MVVM 로직 처리 순서 요약
>[!Tip]- 펼처보기
> ![[MVVM.png]]

## View, ViewModel, Model
###  View는 데이터를 처리하는 것이 아니라 전달받아 반영하는 역할이다
* MVVM 패턴에서 View는 View와 관련된 요소만을 들고 있어야 하기 때문에 직접 데이터를 처리해선 안 된다
* View는 단순히 데이터를 표시하는 역할을 하며, 실제 데이터 처리는 아래와 같은 순서로 처리 된다

GameLogicManager 
특히, 게임 로직에서 데이터를 처리할 때는 `GameLogicManager`를 통해 `ViewModelExtension`에서 데이터가 처리되고, 이 과정에서 `Setter`가 호출되어 값이 변경됩니다. 결국 View는 이 변경된 데이터를 전달받아 화면에 반영하는 역할을 한다

즉, **View는 데이터를 처리하는 것이 아니라, 전달받아 표시하는 역할을 맡고 있다.**  
이 때문에 저는 View가 자신의 운명을 스스로 결정짓는다고 제목을 정한 것이다.

> [!tip]+ View에 대해 자세히 알아보자
> View는 데이터를 처리하는 것이 아니라 전달받아 반영하는 역할이다
> 
> 다시 한 번 강조하지만, View는 데이터를 직접 처리하는 곳이 아니며, 단지 데이터를 전달받아 반영하는 곳입니다. 
> 이를 위해서는 먼저 View가 자신의 역할과 운명을 스스로 결정해야 합니다. 따라서 여러 선작업이 필요하죠. 
> 예를 들어, ViewModel에 접근하여 PropertyChanged 이벤트에 OnPropertyChanged 메서드를 등록하는 작업이 그 중 하나입니다.
>
>이렇게 해두면 앞서 말한 것처럼 게임 로직이 GameLogicManager를 통해 처리되고, 그 결과 Extension에서 값이 변경됩니다. 
>이때 Setter가 호출되면서 값이 변경되면 OnPropertyChanged 메서드가 호출되면서 invoke 되고 구독한 changeds함수가 실행 되면서. 결국 View는 데이터를 전달받는 역할을 하게 되는 것이죠.
>
>같은 내용을 반복하는 게 이상할 수 있지만, 이 어려운 구조를 이해하기 위해서는 여러 번 강조하는 것도 도움이 될 것 같다!!
>
>[뷰에서 선작업 하는 이미지]
   ![[Pasted image 20240816013553.png]]
> 
> 아래 이벤트에 메서드를 등록하는것임
> 그리고 뷰모델들은 아래 ViewModelBase를 상속받음
   ![[Pasted image 20240816015853.png]]
   >[위 이벤트가 Invoke 되면 구독 된 함수들 모두 실행 됨 뷰에서는 아래 메서드를 등록했기 때문에 아래 메서드가 실행 되면서 프로퍼티값을 받아 뷰에 반영함]
   ![[Pasted image 20240816013413.png]]
>아래와 같이 뷰는 여러개가 될 수 있고 뷰의 요소만 들고 있는다 초기화 로직으로 자신의 운명을 결정짓기 위해 뷰모델을 들고 있고 이벤트를 등록한다
![[Pasted image 20240816013928.png]]



## [ViewModel 동작 순서]
>[!NOTE] TopLeftUIViewModel.cs 
>[한 줄 요약 및 핵심]
>* 뷰모델은 뷰에 관한 데이터를 받고 Onpropertychanged()함수를 통해  데이터를 뷰한테 쏴주는 역할을 한다
>* 뷰모델은 **데이터 바인딩**에 대한 역할만 하고 있다 **OnPropertyChanged(nameof(IconName));**
>* 뷰모델은 데이터에 관한 부분들로 구성 됨 프로퍼티명을 보면
>   public string Name... 
>   public int Level...... 
>   public string IconName.... 데이터에 관한 것임을 알 수 있다
>* 모노비를 상속받지 않는다.
>* MVVM은 뷰 모델을 통해 데이터 바인딩을 한다 ! 위에서 말했듯 OnPropertyChanged 함수를 통해 하고 이건 C# 함수이다 유니티 함수가 아님

### 뷰모델의 게터세터는 무슨 열할을 할까?
자 어쩃든 UI 내용이 변경되려면 뷰에 있는 Text 같은 값이 변경이 되어야 한다 위 내용에서는 뷰는 통보를 받기만 한다고 했는데 결과적으로 설명하면 뷰모델의 게터세터중 세터의 값이 수정되면 OnPropertChanged 함수가 동작하면서 Viewmodelbase를 통해 Invoke가 되어 최종적으로는 뷰는 통보를 받음과 동시에 데이터를 전달 받아 UI에 반영하는 것이다

아래는 뷰모델의 게터세터 이다  
![[Pasted image 20240816022519.png]]
그럼 게터세터는 언제 수정되는지가 궁금할텐데 이것은 다음 ViewModelExtention 챕터에서 알아보도록 하고 게터세터를 사용하는 이유와 뷰모델 작성 요령을 좀더 파악해 보자
###  VieweModel은 View를 직접 참조하거나 직접 수정하지 않는다
* MVVM에서 VieweModel은 뷰를 직접 참조하거나 뷰의 데이터를 직접 수정하지 않는다
  뷰 모델은 게터세터로 데이터만 받아 보고 값이 수정 된다면 OnPropertyChanged() 함수만 실행한다.
* 아래 사진을 보면 뷰 모델에서 뷰를 직접 들고 있는 것은 없다 자세히 보면 뷰의 변수명과 같게 자료형 변수를 뷰모델에서 만든것을 볼 수 있는데 이는 직접 들고있는 게 아니라 데이터 바인딩을 위해 뷰에서 변경될 데이터들의 자료형을 뷰모델에서 변수화 시켜놓은 것이다. 
  ![[Pasted image 20240815220232.png]]
### 뷰모델은 데이터가 들어오는 곳?
그렇다 뷰모델로 데이터가 들어온다는 의미는 누군가 게터세터로 데이터를 준다는 의미이다 
그리고 모든 데이터를 받는 족족 반영하는 게 아니라 아래와 같이 들어온 값이 기존 값과 같다면 굳이 반영할 필요가 없기 때문에 return시킨다 

쉽게 설명하면 데이터가 들어오면 "그 데이터 반영해줄게!" 에 대한 처리를 하기 전에 if문에서 언더바 네임 == value 가 같다면 return 시키는 처리를 한다
> ![[Pasted image 20240815220525.png]]

### 뷰에 데이터 반영을 위해 게터세터에 OnPropertyChanged() 라인 추가
데이터를 뷰에 반영하기 위해 OnPropertyChanged(nameof()name); 함수를 추가했다.
쉽게 설명하면 이름이 변경되면 알아서 반영해 주세요~와 같음
![[Pasted image 20240815222906.png]]

### 뷰에 노출 되지 않는 데이터라면?
아래와 같이 if( __ name == name) 부분을 그냥 제거해 주면 된다.
![[Pasted image 20240815224404.png]]
### 뷰 모델에서 가장 중요한거 ! 데이터 바인딩을 위해 사용하는 함수가 있는데!
데이터가 변경되었음을 뷰에게 알려주기 위한 OnPropertyChanged 함수가 있는데 이건 뷰모델에서 함수선언 해도 상관은 없지만아래와 같이 뷰모델베이스 클래스로 따로 빼는게 좋다
![[Pasted image 20240815231657.png]]
왜냐하면 결국은 여러 뷰모델에서 코드를 남발하기 보단 클래스로 빼서 상속을 시키는게 더 낫기 때문이다
아래와 같이 상속받은 곳에서는  아래와 같이 그냥 함수 호출 하면 된다
![[Pasted image 20240815231759.png]]
 아래와 같이 Viewmodelbase라는 걸 만들어서 
![[Pasted image 20240815231613.png]]
## [VIewModelExtension 동작 순서]
>[!Note] TopLeftViewModelExtension.cs
>[한 줄 요약]
>우선 Extention 이란 확장 메서드를 말하는데 확장 메서드는 C#에서 제공하는 기능으로, **기존 클래스에 새로운 메서드를 추가할 수 있게 해준다** 이를 통해 기존 코드에 변경을 가하지 않고도 메서드를 추가하는 것이 가능하다.
>
>**그럼 MVVM에서 확장 메서드를 왜 쓰는데?** ViewModelExtension 이라고 쓰인 걸 보니 뷰모델과 관련이 있을 것으로 예상을 하면 되고 뷰모델은 지금 데이터를 받아서 바인딩을 하고 뷰에 쏴주는 데이터 바인딩 역할에 대한 코드만 있기 때문에 그 외 로직들을 확장메서드로 뺏다고 이해하면 될 것 같다 즉, void Lalala... 이런 메서드들을 뷰모델에서  쓰지 않고 확장메서드에서 쓰려는  것이다 사실 확장메서드가 없어도 상관은 없다 뷰모델에서 함수를 작성하면 그만이긴 하지만 바인딩 로직과 다른 로직을 구분하기 위해  만드는 것이다 
>* 모노비 없음
>* 문법정리
>![[Pasted image 20240815232734.png]]
>
### ViewModelExtention 는 뭘까?
아까 위에서 ViewModelExtention 챕터는 이따 알아보자고 했었고 게터세터의 값이 바뀌는 이유가 ViewModelExtention와 관련이 있다고 했다

좀 복잡하긴 하지만 뷰모델 익스텐션은 내가 봤을 때 중간 관리자 역할을 한다
게임 시작 시 플레이어의 닉네임을 받아오기 위해 MVVM을 사용한다면 과정을 요약하면 아래와 같다

1. 뷰는 데이터 변경에 대해 통보를 받아야 하는 입장이 되어야 함 그래서
	1. 뷰모델을 들고 있는다
	2. 통보를 받기 위해 Invoke가 일어날 때 실행 될 메서드를 등록한다
	3. 처음 초기화 단계에서는 뷰모델에게 요청을 한 번 하긴 한다 (이벤트를 등록해 두면 요청을 안 해도 되는데 필요에 따라 아래와 같이 RefreshViewModel 이런 함수를 호출하기도 함)
		   ![[Pasted image 20240816025425.png]]
2. 1-3 내용의 뷰모델에게 요청을 했기 때문에 뷰모델확장메서드에서는 게임로직 매니저에게 내 플레이어 정보를 달라고 요청함 
   ![[Pasted image 20240816030349.png]]
3. 근데 콜백 함수가 있네? 게임로직 매니저에서는 아래와 같이 플레이어 정보를 요청했으니 반환을 하겠지? 근데 콜백함수의 매게변수로 반환이 됨
   ![[Pasted image 20240816030513.png]]
4. 콜백 함수 실행 되면서 UserId나 Name Level의 값이 변경되는데 이건 TopLevelViewModel 즉 뷰모델의 게터세터 값임
   ![[Pasted image 20240816030550.png]]
5.  게터세터의 값이 수정되면서 OnPropertyChanged 함수가 실행되는 것
6. 뷰는 통보를 받고 데이터를 UI에 반영한다.

자 그럼 확장메서드의 역할에 대해 알아봤으니 좀 더 자세하게 알아보자
### 확장메서드(Extention)에서 this 는 뭘까?
확장 메서드의 첫 번째 매개변수에는 `this` 키워드를 사용하여, 어떤 클래스의 인스턴스를 확장할 것인지를 지정한다. 이를 통해 해당 클래스의 인스턴스에서 직접 이 메서드를 호출할 수 있습니다.

![[Pasted image 20240815234559.png]]
`(this TopLeftViewModel vs)`: 이 부분이 핵심인데, `this` 키워드는 이 메서드가 `TopLeftViewModel` 클래스의 인스턴스에 대한 확장 메서드임을 나타냅니다. 즉, `TopLeftViewModel` 클래스의 인스턴스에서 직접 이 메서드를 호출할 수 있게 됩니다.

### MVVM 에서 뷰모델익스텐션이 하는 역할

>[!Tip] 요청하고 받는 부분이 있다
## [GameLogicManager 동작 순서 (보완필요)]
## 스크립트 파일 분석
### GameLogicManager.cs
아래 그림에서 SubProfileUI 오브젝트는 딱 텍스트 컴포넌트만 들고 있는게 특징이다 

게임로직매니저를 들고 있지가 않는데 어떻게 이름과 레벨이 반영될까?
![[Pasted image 20240815185249.png]]

#### GameLogicManager.cs 의 특징은 아래와 같다
1. 비싼 모노비헤이비어를 상속받지 않는다 
2. 모노 없기 때문에 하이어라키에 추가 되지 않는다
3. 데이터에 관한 로직으로 구성된다
4. using.unity.engine 없음
5. 싱글턴으로 구성 된다
6. C# 그 자체
7. 근데 유니티에 관한 요소가 들어가는 경우가 생긴다면? 잘 생각해보면 필요가 없다고 함

#### GameLogicManager.cs의 역할

강화 버튼을 누르면 요청은 가고 게임 로직 매니저가 처리 하지만 그거에 대한 데이터 변경과 반영은 모델과 뷰모델을 통해서 이루어지고 반영 그 자체는 따로 요청 하지도 않는다 알아서 반영 된다
