
| Git          |                                                                                                                                                  |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| BranchName   |                                                                                                                                                  |
| RefFile      | https://ongveloper.tistory.com/403<br>https://velog.io/@soonsoo3595/Unity-%EC%9C%A0%EB%8B%88%ED%8B%B0C-%EC%9E%90%EB%A3%8C%EA%B5%AC%EC%A1%B0-List |
| Memo         | 240808 - 무슨 차이가 있는지 몰라서 공부해 보려 함 아직 못했음<br>240829 - RefFile 참조 링크 추가 velog...                                                                    |
| MOC_Category | [[자료구조 - MOC]]                                                                                                                                   |

# Array
> [!Tip] 자료구조 관점에서 Array란?
> **Array**는 고정된 크기의 자료를 연속적으로 저장하는 정적 배열로, 빠른 인덱스 접근이 가능하지만 크기 변경이 불가능하여 유연성이 떨어지는 자료구조입니다.

## 1. 배열(Array)의 개념
- 배열이란 가장 일반적인 **선형 자료구조**의 일종입니다.
- 복수의 데이터들을 **연결**시켜 관리하는 자료구조입니다.
- 메모리 관리 방법의 차이로 **정적 배열(Static Array)** 과 **동적 배열(Dynamic Array)** 로 나뉩니다.
> [!NOTE]- 정적배열이란?
> - 정적 배열(Static Array) 라고도 하고 Array List 라고도 합니다.  
>     대부분 후자로 부르나 저는 이번 글에서 메모리의 차이를 집중적으로 다루고자 Static Array라고 부르겠습니다.
> - 아래 이미지와 같이 **같은 타입**의 데이터를 **연속적 메모리 공간**에 저장하고 **인덱스**로 접근하는 자료구조입니다.
> ![[Pasted image 20240829173216.png]]
> 
> **같은 타입** 
>* 하나의 같은 자료형으로만 선언이 가능하고 하나의 배열 내에 int, char등의 여러 자료형이 나올 수 없다.
>      
> **연속적 메모리 공간**
>* 메모리 공간에 연속적으로 저장됩니다. 때문에 프로그래밍 과정에서 정해진 크기에서 변동되는것이 불가 하기에 정적(Static)이라고 합니다.
>      
> **인덱스**
>* 정적 배열은 시작 메모리 주소만을 저장하고 그 외의 주소는 따로 저장 하지 않으며 그것들에 접근 하기 위해 Offset이라는 개념을 사용하여 그것들에 접근합니다. Offset이라 함은 첫번째 원소로 부터 얼마나 떨어진지를 말하며 때문에 index가 0부터 시작 합니다.
>* array[n]의 주소 = base address + n
> ``` csharp
> using UnityEngine;
> 
> public class StaticArrayExample : MonoBehaviour
> {
>     // 정적 배열 선언: 5개의 정수를 저장할 수 있는 배열
>     private int[] scores = new int[5];
> 
>     void Start()
>     {
>         // 배열 초기화
>         scores[0] = 10;
>         scores[1] = 20;
>         scores[2] = 30;
>         scores[3] = 40;
>         scores[4] = 50;
> 
>         // 배열 요소 출력
>         PrintArrayElements();
>         
>         // 배열의 특정 인덱스에 접근하여 값 수정
>         scores[2] = 100;
>         
>         // 수정된 배열 요소 출력
>         PrintArrayElements();
>     }
> 
>     // 배열 요소를 출력하는 함수
>     void PrintArrayElements()
>     {
>         for (int i = 0; i < scores.Length; i++)
>         {
>             Debug.Log("Index " + i + ": " + scores[i]);
>         }
>     }
> }
> 
> /*
>     출력 결과:
>     배열 요소 초기화 후:
>     Index 0: 10
>     Index 1: 20
>     Index 2: 30
>     Index 3: 40
>     Index 4: 50
> 
>     세 번째 요소 값을 수정한 후:
>     Index 0: 10
>     Index 1: 20
>     Index 2: 100
>     Index 3: 40
>     Index 4: 50
> */
> 




# List
# ArrayList
# LinkedList
# Array vs List vs ArrayList vs LinkedList
