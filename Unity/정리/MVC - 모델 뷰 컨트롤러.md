---
Git: 
BranchName: 
MOC: "[[DesignPattern - MOC]]"
RefFile: "[[MVVM.pdf]]"
---

# MVC 특징 

* **1 : N 구조 :** 아래와 같이 컨트롤러는 뷰를 들고있기 때문에 1:n 구조이며 여러개의 View를 들고(선택) 있는 게 가능함
* **Controller는 View를 업데이트 하지 않음** : 아래와 같이 모델의 정보를 뷰에 전달해서 뷰에서 처리하는 것이기 때문에 직접 업데이트를 하지 않음 + View는 컨트롤러를 알지 못함 Why? 컨트롤러만 뷰를 들고 있으니까
* test
* test

![[Pasted image 20240721014222.png]]


## MVC 동작 순서


![[Pasted image 20240721014031.png]]


### GPT가 설명하는 MVC 특징

![[Pasted image 20240721012845.png]]