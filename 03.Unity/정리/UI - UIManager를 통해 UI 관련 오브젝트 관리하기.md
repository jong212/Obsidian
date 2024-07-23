---
dg-publish: true
---

| Git          | https://github.com/jong212/UIManager                |
| ------------ | --------------------------------------------------- |
| BranchName   | UI_Button_Character_Popup                           |
| RefFile      |                                                     |
| Memo         | UI_Button_Character_Popup 체크아웃 후 “전투” 버튼 클릭 시 확인 가능 |
| MOC_Category | [[UI - MOC]]                                        |
## UI Manager 특징
* 싱글턴으로 구성되어 있어서 오브젝트간 참조가 안 되어 있어도 UIManager.instance.SimplePopup() 이런식으로 호출이 가능
	* 이게 왜 장점이냐면 일단 UIManager에서는 아래와 같은 로직들이 구성되어 있기 때문에 UI를 체계적으로 관리할 수 있음
	  1. 하이어라키에 오브젝트가 있는지 체크하는 함수 (Dictionary)
	  2. 있는데 비활성화면 활성화 하는 함수 (DIctionary)
	  3. 없으면 Resource 폴더에서 프리팹을 가져와서 하이어라키에 추가해주는 로직
	  4. ![[Pasted image 20240724013116.png|250]]

## UI Manager 구조를 정리한 이미지


![[Pasted image 20240724013542.png|2000]]

> [!NOTE]+ Test.............Open
> TestOut..................................