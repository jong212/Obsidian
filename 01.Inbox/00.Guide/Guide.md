ttttdt
## ==MOC?==

- MOC란 여러곳에 폴더에 흩어져 있는 노트들을 하나로 모아서 보여주는 분류 체계
- MOC 체계를 사용하는 이유는 내가 만약에 디자인패턴에 관한 글을 100개 쓰고 독서에 관한 글을 100개 썼다고 할 때 큰 카테고리를 정하지 않으면 글로벌 그래프 뷰에서 볼 때 많이 헷갈릴 수 있기 때문이다 만약 디자인 패턴 MOC와 독후감 MOC 이런식의 카테고리를 정한다면 구분이 가능할 것이다 쉽게 말해서 카테고리와 같은 역할을 하는것

## MOC 카테고리 생성시 반드시 해야할 것 (태그 설정)
* 규칙 1 : Inbox/MOC 경로에 생성할 것
* 글 이름 뒤에 아래와 같이 -MOC 붙이기
* Properties에 dg-publish 추가 후 체크박스 True 설정 (블로그에도 연동 해주기 위함)
* 그래프 뷰에서 확인할 수 있도록 아래와 같이 태그 설정
  ![[Pasted image 20240723000436.png]]
   

  
## 글 작성 시 해야할 것
![[Pasted image 20240723130905.png]]
* 반드시 작성하는 글의 카테고리가 무엇인지를 생각하고 괄호 참조로 MOC_Category에 넣기
* 위와 같이 하는 이유는 예를들어 내가 오늘 MVC를 배웠다고 하면 아래와 같이 해당 페이지를 참조 시키면
* 아래와 같이 추후에 그래프뷰에서 디자인 패턴에 연결 된 세부 글들의 가시성 높게 볼 수 있기 때문
1. ![[Pasted image 20240721011114.png]]

---

## 콜아웃 코드(블로그 연동)
* 아래꺼 그냥 갖다 쓰면 됨

> [!NOTE]+ Test.............Open
> TestOut..................................