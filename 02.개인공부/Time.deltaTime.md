
| 카테고리       | [[기초 - MOC]]                                                     |
| ---------- | ---------------------------------------------------------------- |
| 공부목적       | 개발 하면서 플레이어 이동과 같은 것을 할 때 자주 사용했지만 왜 사용해야 하는지 몰라서 학습겸 공부하기 위해 정리 |
| 느낀점        | Time.deltaTime을 사용하지 않으면 PC마다 동작 횟수가 달라질 수 있음                    |
| 참고사이트      |                                                                  |
| 깃주소 및 브랜치명 |                                                                  |
## Time.deltaTime
Time.deltaTime는 시간 개념으로 각 프레임을 렌더링하는데 걸리는 시간을 계산하는 Unity 함수이다.  
프레임이 바뀔때마다의 시간을 측정한 값이 Time.deltaTime으로 나타난다.
### 사용 이유
만약 게임 속 캐릭터가 초당 5미터를 이동해야 한다고 가정하고 deltaTime을 사용하지 않으면? 컴퓨터마다 초당 프레임이 달라서 이동 거리가 달라질 것
- **문제**: 각 프레임마다 고정된 거리만큼 이동하게 만들면, 프레임이 빠른 컴퓨터에서는 캐릭터가 더 빠르게 이동하고, 프레임이 느린 컴퓨터에서는 캐릭터가 더 느리게 이동하게 됩니다. 즉, 컴퓨터 성능에 따라 게임 속 캐릭터의 움직임이 달라지게 되는 거죠.
- **해결책**: `Time.deltaTime`을 곱해주면 각 프레임이 그려지는 시간이 얼마나 걸렸는지 알 수 있으니, 프레임이 빠르든 느리든 캐릭터가 일정한 속도로 움직이게 만들 수 있다.
### 프레임?
- 게임은 여러 개의 **프레임**을 계속해서 그리는 방식으로 움직임을 표현. 이때 각 프레임이 하나의 "장면"이라고 생각하면 된다.
- FPS (Frames Per Second)는 초당 몇 개의 프레임이 그려지는지를 말하는데, 예를 들어 FPS가 60이면 1초 동안 60번의 장면을 그린다는 뜻이다 따라서 FPS가 높을수록 더 부드러운 화면이 만들어진다.
### PC마다 프레임이 다른 이유
각 프레임이 그려지는 데는 약간의 시간이 걸리며 이 시간이 컴퓨터 성능에 따라 달라질 수 있다.예를 들어, 성능이 좋은 컴퓨터에서는 프레임이 빠르게 그려지지만, 성능이 낮은 컴퓨터에서는 프레임이 더 천천히 그려진다. Time.deltaTime은 이전 프레임이 그려진 시점과 현재 프레임이 그려진 **시점 사이** 의 시간 차이를 의미합니다. 즉, 프레임 간의 **시간 간격**이다.
### 프레임 실제로 이해하기
- 초당 5미터 이동하는 캐릭터를 만들어 보자고 가정.
- FPS가 60이라면, 1초에 60개의 프레임이 그려진다. 각 프레임이 그려지는 데 걸리는 시간(Time.deltaTime)은 약 1/60초, 즉 0.0167초이다.
- 그럼 이때 이동 거리는 5미터 * 0.0167초 = 약 0.0835미터가 된다. 즉, 캐릭터는 1프레임마다 0.0835미터씩 이동하게 된다.
- FPS가 30이라면, 1초에 30개의 프레임이 그려진다. 각 프레임이 그려지는 데 걸리는 시간은 1/30초, 즉 0.0333초이다.
- 그럼 이때 이동 거리는 5미터 * 0.0333초 = 약 0.167미터가 된다. 즉, 캐릭터는 1프레임마다 0.167미터씩 이동하게 된다.
### 정 이해가 가지 않는다면
 Time.deltaTime이 들어간 계산은 프레임마다 무언갈 처리하는 방식이라고 이해하면 된다
### 사용 예시 10가지
> [!Tip]- Example10
> Time.deltaTime을 사용하는 짧은 예시 코드를 10개 입니다. 
> 이 예시들은 모두 프레임 속도에 독립적으로 일정한 동작을 수행하도록 하는 방법들을 보여줍니다.
>#### 1. **캐릭터 이동 속도 제어**
> 캐릭터가 일정한 속도로 앞으로 이동하는 예시입니다.
> ```csharp
> void Update()
> {
>     float speed = 5f;
>     transform.Translate(Vector3.forward * speed * Time.deltaTime);
> }
> ```
>#### 2. **카메라 회전**
> 카메라가 일정한 속도로 회전하는 예시입니다.
> ```csharp
> void Update()
> {
>     float rotationSpeed = 50f;
>     transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
> }
> ```
> #### 3. **점진적인 크기 증가**
> 오브젝트의 크기를 서서히 키우는 예시입니다.
> ```csharp
> void Update()
> {
>     Vector3 scaleIncrease = new Vector3(1, 1, 1);
>     transform.localScale += scaleIncrease * Time.deltaTime;
> }
> ```
> #### 4. **감속 처리**
> 물체의 속도를 서서히 줄이는 예시입니다.
> ```csharp
> float speed = 10f;
> float deceleration = 5f;
> 
> void Update()
> {
>     if (speed > 0)
>     {
>         speed -= deceleration * Time.deltaTime;
>         speed = Mathf.Max(speed, 0);
>         transform.Translate(Vector3.forward * speed * Time.deltaTime);
>     }
> }
> ```
> #### 5. **선형 보간(Lerp)**
> 오브젝트의 위치를 일정한 속도로 목표 위치로 선형 보간하는 예시입니다.
> ```csharp
> Vector3 targetPosition = new Vector3(10, 0, 0);
> void Update()
> {
>     transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f * Time.deltaTime);
> }
> ```
> #### 6. **부드러운 감속**
> 오브젝트의 속도를 선형 보간 방식으로 부드럽게 줄이는 예시입니다.
> ```csharp
> float speed = 10f;
> void Update()
> {
>     speed = Mathf.Lerp(speed, 0f, 0.5f * Time.deltaTime);
>     transform.Translate(Vector3.forward * speed * Time.deltaTime);
> }
> ```
> #### 8. **간단한 애니메이션**
> 오브젝트가 위로 일정한 속도로 움직이는 애니메이션입니다.
> ```csharp
> void Update()
> {
>     float speed = 3f;
>     transform.Translate(Vector3.up * speed * Time.deltaTime);
> }
> ```
> #### 9. **시간에 따른 스케일 감소**
> 오브젝트의 크기를 서서히 줄이는 예시입니다.
> ```csharp
> void Update()
> {
>     float shrinkRate = 0.5f;
>     transform.localScale -= new Vector3(shrinkRate, shrinkRate, shrinkRate) * Time.deltaTime;
> }
> ```
> #### 10. **중력 효과 구현**
> 오브젝트가 중력에 의해 떨어지게 만드는 간단한 중력 효과입니다.
> ```csharp
> float gravity = -9.8f;
> void Update()
> {
>     Vector3 fallVelocity = new Vector3(0, gravity * Time.deltaTime, 0);
>     transform.Translate(fallVelocity);
> }
> ```
> #### 11. **FPS 표시기**
> 현재 프레임 속도를 표시하는 간단한 예시입니다.
> ```csharp
> void OnGUI()
> {
>     float fps = 1.0f / Time.deltaTime;
>     GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + fps);
> }
> ```
> 이와 같은 `Time.deltaTime`을 사용하는 예시들은 **프레임 독립적인 속도, 회전, 크기 변경, 이동 및 기타 애니메이션을 구현**하는 데 유용합니다. 이를 통해 각 프레임에서 경과한 시간에 따라 동작을 제어할 수 있습니다.
