## 구현내용 파악하기

CustomTile - 커스텀 스크립트인데 이동 불가 타일에 대한 로직을 작성했음 / 어떤식으로 동작하는지 확인


> [!NOTE]- Astar 알고리즘 코드 분석
> 
> **`Pathfinder.cs` 코드 주석 및 상세 설명**
> 
> ```csharp 
> using System.Collections;
> using System.Collections.Generic;
> using UnityEngine;
> using UnityEngine.Tilemaps;
> 
> public class Pathfinder : MonoBehaviour
> {
>     // 경로 탐색에 사용할 타일맵 (유니티 에디터에서 할당)
>     public Tilemap tilemap;
> 
>     // A* 알고리즘을 사용하여 경로를 찾는 함수
>     public void FindPath(Vector3 startPos, Vector3 targetPos)
>     {
>         // 시작 위치와 목표 위치를 타일맵의 셀 좌표로 변환
>         Vector3Int startCell = tilemap.WorldToCell(startPos);
>         Vector3Int targetCell = tilemap.WorldToCell(targetPos);
> 
>         // Z축 좌표를 0으로 설정하여 2D 평면에서 계산되도록 함
>         startCell.z = 0;
>         targetCell.z = 0;
> 
>         // 시작 셀과 목표 셀 정보를 로그로 출력
>         Debug.Log($"Start Cell: {startCell}, Target Cell: {targetCell}");
> 
>         // 목표 셀에 타일이 없거나, 이동 불가능한 타일이면 경로 탐색 중지
>         if (!tilemap.HasTile(targetCell) || !IsTilePassable(tilemap.GetTile(targetCell)))
>         {
>             Debug.Log("No tile found at target cell or tile is impassable, stopping pathfinding.");
>             return;
>         }
> 
>          //* 알고리즘에 필요한 데이터 구조 초기화
>         List< Vector3Int> openList = new List< Vector3Int>(); // 탐색할 셀 목록
>         HashSet< Vector3Int> closedList = new HashSet< Vector3Int>(); // 이미 탐색한 셀 목록
>         Dictionary< Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>(); // 각 셀에 도달하기 직전의 셀
>         Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>(); // 시작 지점부터 해당 셀까지의 실제 비용
>         Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>(); // gScore + 휴리스틱 추정 비용
> 
>         // 시작 셀을 openList에 추가하고, gScore와 fScore 초기화
>         openList.Add(startCell);
>         gScore[startCell] = 0; // 시작 지점의 gScore는 0
>         fScore[startCell] = HeuristicCostEstimate(startCell, targetCell); // fScore는 휴리스틱 추정값으로 초기화
> 
>         // openList에 탐색할 셀이 남아있는 동안 반복
>         while (openList.Count > 0)
>         {
>             // fScore가 가장 낮은 셀을 선택하여 현재 셀로 설정
>             Vector3Int current = GetLowestFScore(openList, fScore);
> 
>             // 현재 셀이 목표 셀과 동일한지 확인
>             if (current.x == targetCell.x && current.y == targetCell.y)
>             {
>                 Debug.Log("Retracing path to move to target.");
>                 // 경로를 복원하고 이동 시작
>                 RetracePath(cameFrom, current);
>                 return;
>             }
> 
>             // 현재 셀을 openList에서 제거하고 closedList에 추가
>             openList.Remove(current);
>             closedList.Add(current);
> 
>             // 현재 셀의 이웃 셀들을 검사
>             foreach (Vector3Int neighbor in GetNeighbors(current))
>             {
>                 // 이미 탐색한 셀은 무시
>                 if (closedList.Contains(neighbor))
>                     continue;
> 
>                 // 이웃 셀의 타일 가져오기
>                 TileBase tile = tilemap.GetTile(neighbor);
> 
>                 // 타일이 없거나, 이동 불가능한 타일이면 무시
>                 if (tile == null || !IsTilePassable(tile))
>                 {
>                     Debug.Log($"Skipping neighbor: {neighbor}, tile is impassable.");
>                     continue;
>                 }
> 
>                 // 현재 셀을 통해 이웃 셀로 가는 비용 계산 (여기서는 가중치를 모두 1로 설정)
>                 float tentativeGScore = gScore[current] + 1;
> 
>                 // 이웃 셀이 openList에 없거나, 더 짧은 경로를 발견한 경우
>                 if (!openList.Contains(neighbor) || tentativeGScore < gScore.GetValueOrDefault(neighbor, Mathf.Infinity))
>                 {
>                     // 이웃 셀로 가는 최단 경로 갱신
>                     cameFrom[neighbor] = current;
>                     gScore[neighbor] = tentativeGScore;
>                     fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, targetCell);
> 
>                     // openList에 없으면 추가
>                     if (!openList.Contains(neighbor))
>                         openList.Add(neighbor);
>                 }
>             }
>         }
> 
>         // 경로를 찾지 못한 경우
>         Debug.Log("No valid path found.");
>     }
> 
>     // 휴리스틱 함수: 현재 셀에서 목표 셀까지의 추정 비용 계산 (맨해튼 거리 사용)
>     private float HeuristicCostEstimate(Vector3Int start, Vector3Int goal)
>     {
>         return Mathf.Abs(goal.x - start.x) + Mathf.Abs(goal.y - start.y);
>     }
> 
>     // fScore가 가장 낮은 셀을 openList에서 선택하는 함수
>     private Vector3Int GetLowestFScore(List< Vector3Int> openList, Dictionary<Vector3Int, float> fScore)
>     {
>         Vector3Int lowest = openList[0];
>         float lowestScore = fScore[lowest];
> 
>         // openList의 각 셀에 대해 fScore를 비교하여 최소값 찾기
>         foreach (Vector3Int cell in openList)
>         {
>             if (fScore.ContainsKey(cell) && fScore[cell] < lowestScore)
>             {
>                 lowest = cell;
>                 lowestScore = fScore[cell];
>             }
>         }
> 
>         return lowest;
>     }
> 
>     // 현재 셀의 인접한 이웃 셀들의 리스트를 반환하는 함수
>     private List< Vector3Int> GetNeighbors(Vector3Int cell)
>     {
>         List< Vector3Int> neighbors = new List< Vector3Int>();
> 
>         // 상하좌우 이웃 추가
>         neighbors.Add(new Vector3Int(cell.x + 1, cell.y, cell.z)); // 오른쪽
>         neighbors.Add(new Vector3Int(cell.x - 1, cell.y, cell.z)); // 왼쪽
>         neighbors.Add(new Vector3Int(cell.x, cell.y + 1, cell.z)); // 위쪽
>         neighbors.Add(new Vector3Int(cell.x, cell.y - 1, cell.z)); // 아래쪽
> 
>         // 대각선 이웃 추가 (필요한 경우)
>         neighbors.Add(new Vector3Int(cell.x + 1, cell.y + 1, cell.z)); // 오른쪽 위
>         neighbors.Add(new Vector3Int(cell.x - 1, cell.y + 1, cell.z)); // 왼쪽 위
>         neighbors.Add(new Vector3Int(cell.x + 1, cell.y - 1, cell.z)); // 오른쪽 아래
>         neighbors.Add(new Vector3Int(cell.x - 1, cell.y - 1, cell.z)); // 왼쪽 아래
> 
>         return neighbors;
>     }
> 
>     // 타일의 이동 가능 여부를 확인하는 함수
>     private bool IsTilePassable(TileBase tile)
>     {
>         // 타일을 CustomTile로 캐스팅하여 isPassable 속성 확인
>         CustomTile customTile = tile as CustomTile;
>         if (customTile != null)
>         {
>             return customTile.isPassable; // 이동 가능 여부 반환
>         }
>         else
>         {
>             // CustomTile이 아닌 경우 기본적으로 이동 가능하다고 간주
>             return true;
>         }
>     }
> 
>     // 경로를 복원하고, 플레이어를 이동시키는 함수
>     private void RetracePath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
>     {
>         List< Vector3Int> path = new List< Vector3Int>();
> 
>         // 목표 지점부터 시작 지점까지 역추적하여 경로 생성
>         while (cameFrom.ContainsKey(current))
>         {
>             path.Add(current);
>             current = cameFrom[current];
>         }
> 
>         // 경로를 반전시켜 시작 지점부터 목표 지점 순서로 변경
>         path.Reverse();
> 
>         // 경로의 길이를 출력
>         Debug.Log($"Path found with {path.Count} steps.");
> 
>         // 경로가 존재하면 이동 코루틴 실행
>         if (path.Count > 0)
>         {
>             StartCoroutine(MoveAlongPath(path));
>         }
>     }
> 
>     // 플레이어가 경로를 따라 이동하는 코루틴
>     private IEnumerator MoveAlongPath(List< Vector3Int> path)
>     {
>         foreach (Vector3Int cell in path)
>         {
>             // 셀의 중심 월드 좌표를 목표 지점으로 설정
>             Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
> 
>             // 현재 위치에서 목표 지점까지 이동
>             while (Vector3.Distance(transform.position, worldPos) > 0.1f)
>             {
>                 // 이동 속도에 따라 위치를 보간하여 이동
>                 transform.position = Vector3.MoveTowards(transform.position, worldPos, 5f * Time.deltaTime);
>                 yield return null; // 다음 프레임까지 대기
>             }
>         }
>     }
> }
> ```
> 
> ---
> 
> ### **코드 상세 설명**
> 
> #### **1. `FindPath` 함수**
> 
> - **기능**: A* 알고리즘을 사용하여 시작 위치(`startPos`)에서 목표 위치(`targetPos`)까지의 최단 경로를 찾습니다.
> 
> - **주요 단계**:
> 
>   1. **시작 셀과 목표 셀 설정**:
> 
>      ```csharp
>      Vector3Int startCell = tilemap.WorldToCell(startPos);
>      Vector3Int targetCell = tilemap.WorldToCell(targetPos);
>      ```
> 
>      - 월드 좌표를 타일맵의 셀 좌표로 변환합니다.
> 
>   2. **Z축 좌표 설정**:
> 
>      ```csharp
>      startCell.z = 0;
>      targetCell.z = 0;
>      ```
> 
>      - 2D 타일맵이므로 Z축을 0으로 설정합니다.
> 
>   3. **목표 셀의 유효성 검사**:
> 
>      ```csharp
>      if (!tilemap.HasTile(targetCell) || !IsTilePassable(tilemap.GetTile(targetCell)))
>      {
>          Debug.Log("No tile found at target cell or tile is impassable, stopping pathfinding.");
>          return;
>      }
>      ```
> 
>      - 목표 셀에 타일이 없거나 이동 불가능한 타일이면 경로 탐색을 중지합니다.
> 
>   4. **A* 알고리즘 초기화**:
> 
>      ```csharp
>      List< Vector3Int> openList = new List< Vector3Int>(); // 탐색할 셀 목록
>      HashSet< Vector3Int> closedList = new HashSet< Vector3Int>(); // 이미 탐색한 셀 목록
>      Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>(); // 경로 추적용
>      Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>(); // 시작 지점부터 해당 셀까지의 실제 비용
>      Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>(); // gScore + 휴리스틱 추정 비용
>      ```
> 
>   5. **시작 셀 설정**:
> 
>      ```csharp
>      openList.Add(startCell);
>      gScore[startCell] = 0;
>      fScore[startCell] = HeuristicCostEstimate(startCell, targetCell);
>      ```
> 
>      - 시작 셀의 gScore를 0으로 설정하고, fScore는 휴리스틱 추정값으로 초기화합니다.
> 
>   6. **메인 루프**:
> 
>      ```csharp
>      while (openList.Count > 0)
>      {
>          // ...
>      }
>      ```
> 
>      - openList에 셀이 남아있는 동안 반복합니다.
> 
>   7. **현재 셀 선택**:
> 
>      ```csharp
>      Vector3Int current = GetLowestFScore(openList, fScore);
>      ```
> 
>      - fScore가 가장 낮은 셀을 선택합니다.
> 
>   8. **목표 도달 여부 확인**:
> 
>      ```csharp
>      if (current.x == targetCell.x && current.y == targetCell.y)
>      {
>          // 경로 복원 및 이동 시작
>          RetracePath(cameFrom, current);
>          return;
>      }
>      ```
> 
>      - 현재 셀이 목표 셀과 동일하면 경로를 복원하고 함수 종료.
> 
>   9. **현재 셀 업데이트**:
> 
>      ```csharp
>      openList.Remove(current);
>      closedList.Add(current);
>      ```
> 
>      - 현재 셀을 openList에서 제거하고 closedList에 추가합니다.
> 
>   10. **이웃 셀 탐색**:
> 
>       ```csharp
>       foreach (Vector3Int neighbor in GetNeighbors(current))
>       {
>           // ...
>       }
>       ```
> 
>       - 현재 셀의 인접한 이웃 셀들을 검사합니다.
> 
>   11. **이웃 셀 처리**:
> 
>       - **이미 탐색한 셀 무시**:
> 
>         ```csharp
>         if (closedList.Contains(neighbor))
>             continue;
>         ```
> 
>       - **타일 존재 및 이동 가능 여부 확인**:
> 
>         ```csharp
>         TileBase tile = tilemap.GetTile(neighbor);
> 
>         if (tile == null || !IsTilePassable(tile))
>         {
>             Debug.Log($"Skipping neighbor: {neighbor}, tile is impassable.");
>             continue;
>         }
>         ```
> 
>       - **gScore 및 fScore 계산**:
> 
>         ```csharp
>         float tentativeGScore = gScore[current] + 1;
> 
>         if (!openList.Contains(neighbor) || tentativeGScore < gScore.GetValueOrDefault(neighbor, Mathf.Infinity))
>         {
>             cameFrom[neighbor] = current;
>             gScore[neighbor] = tentativeGScore;
>             fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, targetCell);
> 
>             if (!openList.Contains(neighbor))
>                 openList.Add(neighbor);
>         }
>         ```
> 
>         - 현재 셀을 통해 이웃 셀로 가는 비용이 더 낮으면 경로 갱신.
> 
>   12. **경로 탐색 실패 처리**:
> 
>       ```csharp
>       Debug.Log("No valid path found.");
>       ```
> 
>       - openList가 비었는데 목표 셀에 도달하지 못하면 경로가 없다는 메시지 출력.
> 
> #### **2. `HeuristicCostEstimate` 함수**
> 
> - **기능**: 현재 셀에서 목표 셀까지의 예상 비용을 계산합니다.
> - **방법**: 맨해튼 거리(절대값 거리)를 사용하여 계산합니다.
> 
> #### **3. `GetLowestFScore` 함수**
> 
> - **기능**: openList에서 fScore가 가장 낮은 셀을 선택합니다.
> - **방법**: openList를 순회하며 fScore를 비교하여 최소값을 찾습니다.
> 
> #### **4. `GetNeighbors` 함수**
> 
> - **기능**: 현재 셀의 인접한 이웃 셀들의 리스트를 반환합니다.
> - **방법**: 상하좌우 및 대각선 방향의 이웃을 추가합니다.
> 
> #### **5. `IsTilePassable` 함수**
> 
> - **기능**: 타일의 이동 가능 여부를 확인합니다.
> - **방법**: 타일을 `CustomTile`로 캐스팅하여 `isPassable` 속성을 확인합니다.
> 
> #### **6. `RetracePath` 함수**
> 
> - **기능**: 목표 지점부터 시작 지점까지 경로를 역추적하여 경로를 복원합니다.
> - **방법**: `cameFrom` 딕셔너리를 사용하여 역추적하고, 경로를 반전시켜 올바른 순서로 만듭니다.
> 
> #### **7. `MoveAlongPath` 코루틴**
> 
> - **기능**: 플레이어를 경로를 따라 이동시킵니다.
> - **방법**: 경로상의 각 셀에 대해 순서대로 이동하며, `Vector3.MoveTowards`를 사용하여 부드럽게 이동합니다.
> 
> ---
> 
> ### **A* 알고리즘 개요**
> 
> A* 알고리즘은 시작 지점에서 목표 지점까지의 최단 경로를 찾는 경로 탐색 알고리즘입니다. 휴리스틱 함수를 사용하여 예상 비용을 계산하고, 실제 비용과 합산하여 가장 효율적인 경로를 선택합니다.
> 
> - **gScore**: 시작 지점에서 현재 셀까지의 실제 이동 비용.
> - **hScore**: 현재 셀에서 목표 지점까지의 추정 비용 (휴리스틱).
> - **fScore = gScore + hScore**: 총 예상 비용.
> 
> A* 알고리즘은 다음과 같이 작동합니다:
> 
> 1. **초기화**: 시작 셀을 설정하고 openList에 추가합니다.
> 2. **반복**:
>    - openList에서 fScore가 가장 낮은 셀(current)을 선택합니다.
>    - current가 목표 셀이면 경로를 복원하고 종료합니다.
>    - current의 이웃 셀들을 검사하여 이동 가능하고 더 나은 경로가 있는지 확인합니다.
> 3. **경로 복원**: 목표 셀에 도달하면 `cameFrom`을 사용하여 경로를 역추적합니다.
> 
> ---
> 
> **`TilemapPathFinder.cs` 코드 주석 및 상세 설명**
> 
> ```csharp
> using UnityEngine;
> using UnityEngine.Tilemaps;
> using System.Collections.Generic;
> 
> public class TilemapPathFinder : MonoBehaviour
> {
>     // 경로 탐색에 사용할 타일맵 (유니티 에디터에서 할당)
>     public Tilemap tilemap;
>     // Pathfinder 스크립트 참조 (유니티 에디터에서 할당)
>     public Pathfinder pathfinder;
> 
>     void Update()
>     {
>         // 마우스 왼쪽 버튼이 클릭되었을 때 실행
>         if (Input.GetMouseButtonDown(0))
>         {
>             // 마우스 클릭 위치를 월드 좌표로 변환
>             Vector3 mousePos = Input.mousePosition;
>             mousePos.z = -Camera.main.transform.position.z; // 카메라가 -Z 위치에 있을 경우 양수로 변환
> 
>             Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
>             worldPos.z = 0; // Z축을 0으로 설정하여 2D 평면에서만 계산
> 
>             // 월드 좌표를 타일맵의 셀 좌표로 변환
>             Vector3Int cellPos = tilemap.WorldToCell(worldPos);
> 
>             // 클릭한 셀의 좌표를 출력
>             Debug.Log($"Clicked Cell Position: {cellPos}");
> 
>             // 클릭한 셀에 타일이 있는지 확인
>             if (tilemap.HasTile(cellPos))
>             {
>                 Debug.Log("Tile exists at the clicked position.");
> 
>                 // 플레이어의 현재 위치와 목표 위치 설정
>                 Vector3 startPos = pathfinder.transform.position;
>                 Vector3 targetPos = tilemap.GetCellCenterWorld(cellPos); // 타일의 중심 좌표 사용
> 
>                 // 경로 탐색 및 이동 실행
>                 pathfinder.FindPath(startPos, targetPos);
>             }
>             else
>             {
>                 Debug.Log("No tile found at the clicked position.");
>             }
>         }
>     }
> }
> ```
> 
> ---
> 
> ### **코드 상세 설명**
> 
> - **기능**: 사용자의 마우스 클릭을 감지하고, 클릭한 위치로의 경로 탐색을 시작합니다.
> 
> - **주요 단계**:
> 
>   1. **마우스 입력 감지**:
> 
>      ```csharp
>      if (Input.GetMouseButtonDown(0))
>      {
>          // ...
>      }
>      ```
> 
>      - 마우스 왼쪽 버튼 클릭을 감지합니다.
> 
>   2. **마우스 클릭 위치 변환**:
> 
>      ```csharp
>      Vector3 mousePos = Input.mousePosition;
>      mousePos.z = -Camera.main.transform.position.z;
> 
>      Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
>      worldPos.z = 0;
>      ```
> 
>      - 스크린 좌표를 월드 좌표로 변환합니다.
>      - Z축을 0으로 설정하여 2D 평면에서 계산합니다.
> 
>   3. **클릭한 셀 좌표 획득**:
> 
>      ```csharp
>      Vector3Int cellPos = tilemap.WorldToCell(worldPos);
>      ```
> 
>      - 월드 좌표를 타일맵의 셀 좌표로 변환합니다.
> 
>   4. **타일 존재 여부 확인 및 경로 탐색 실행**:
> 
>      ```csharp
>      if (tilemap.HasTile(cellPos))
>      {
>          // ...
>      }
>      else
>      {
>          Debug.Log("No tile found at the clicked position.");
>      }
>      ```
> 
>      - 클릭한 위치에 타일이 있으면 `FindPath` 함수를 호출하여 경로 탐색을 시작합니다.
> 
> ---
> 
> ### **전반적인 동작 흐름**
> 
> 1. **사용자 입력**: 플레이어가 마우스로 타일을 클릭합니다.
> 2. **위치 변환**: 클릭한 위치를 월드 좌표 및 타일맵 셀 좌표로 변환합니다.
> 3. **타일 확인**: 해당 위치에 타일이 있는지 확인합니다.
> 4. **경로 탐색**: 타일이 있으면 `Pathfinder` 스크립트의 `FindPath` 함수를 호출하여 경로 탐색을 시작합니다.
> 5. **경로 복원 및 이동**: 경로가 발견되면 플레이어는 해당 경로를 따라 목표 지점으로 이동합니다.
> 
> ---
> 
> **이렇게 코드에 주석을 추가하고 상세한 설명을 제공해 드렸습니다.**
> 
> 이해를 돕기 위해 A* 알고리즘의 핵심 개념과 코드의 각 부분이 어떤 역할을 하는지 설명했습니다. 이를 통해 코드의 작동 원리와 A* 알고리즘에 대한 이해를 높이실 수 있을 것입니다.
> 
> 추가로 궁금하신 점이나 도움이 필요하시면 언제든지 말씀해주세요!

Isometric x as y 타일맵 셀 이동 시 좌표 변화 체크
![[Pasted image 20240920161036.png|500]]
![[Diagram 2.svg]]

각 클래스마다 어떤 기능을 해야 하는지 구체적으로 설명드릴게요. 여기서는 **각 클래스의 역할**과 **어떤 기능을 구현해야 하는지**를 더 명확하게 정의하겠습니다.

---

### **1. `Entity` 클래스**
`Entity` 클래스는 **모든 캐릭터**(적, 플레이어 등)에 공통적으로 적용될 **기본 기능**을 정의하는 클래스입니다. **공통적인 동작**을 여러 캐릭터 간에 재사용할 수 있도록 하기 위해 만들었습니다.

#### **`Entity` 클래스에서 해야 할 일:**
1. **캐릭터 공통 속성 정의**:
   - **애니메이션(`Animator`)**, **물리적 속성(`Rigidbody2D`)**, **충돌 처리(`Collider`)** 등의 기본 컴포넌트를 정의.
   - 모든 캐릭터가 공통적으로 가질 수 있는 속성을 정의.

2. **공통 동작 구현**:
   - **이동, 속도 제어, 넉백 처리, 충돌 처리, 죽음 처리**와 같은 캐릭터의 기본 동작을 구현.
   - **메서드 예시**:
     - `SetVelocity()`: 캐릭터의 이동 속도를 설정.
     - `Flip()`: 캐릭터의 방향을 바꿔주는 함수.
     - `Die()`: 캐릭터가 죽었을 때 호출되는 함수.

3. **가상 메서드 제공**:
   - 특정 캐릭터(적 또는 플레이어)에 맞게 재정의할 수 있는 가상 메서드(`virtual`)를 제공.
   - 예를 들어, **플레이어와 적의 죽음 처리**가 다를 수 있으므로 `Die()` 메서드를 **가상 메서드로 정의**하고, 적과 플레이어에서 각각 다르게 구현할 수 있도록 함.

#### **구현 예시:**
```csharp
public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    public virtual void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }

    public virtual void Die()
    {
        // 캐릭터가 죽었을 때 처리
    }
}
```

---

### **2. `Enemy` 클래스**
`Enemy` 클래스는 **적**에 특화된 기능을 정의하는 클래스입니다. **`Entity`를 상속받아 적의 특화된 기능을 추가**합니다.

#### **`Enemy` 클래스에서 해야 할 일:**
1. **적 특화 속성 추가**:
   - **스턴**, **전투 거리** 등 **적에게만 해당되는 속성**을 정의.
   - 예를 들어, `CanBeStunned()` 같은 함수는 적에게만 적용될 수 있습니다.

2. **상태 패턴(State Machine) 추가**:
   - 적의 행동을 상태로 관리할 수 있도록 **상태 머신**을 구현.
   - 예: **Idle(대기)**, **Move(이동)**, **Attack(공격)**, **Stunned(스턴)**, **Dead(죽음)** 상태로 나누어 각 상태에서 적의 행동을 제어.

3. **상태 전환 관리**:
   - 적이 특정 상태(예: 대기, 이동, 공격)에서 **다른 상태로 전환**하는 로직을 관리.

#### **구현 예시:**
```csharp
public class Enemy : Entity
{
    public float moveSpeed = 2f;
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update(); // 현재 상태를 업데이트
    }

    public virtual void CanBeStunned()
    {
        // 스턴 가능 여부를 확인하는 로직
    }

    public override void Die()
    {
        base.Die();
        // 적의 구체적인 죽음 처리
    }
}
```

---

### **3. `Enemy_Skeleton` 클래스**
`Enemy_Skeleton` 클래스는 **구체적인 스켈레톤 적**의 행동과 상태를 정의하는 클래스입니다. **`Enemy` 클래스를 상속받아 스켈레톤 적의 상태와 동작**을 구현합니다.

#### **`Enemy_Skeleton` 클래스에서 해야 할 일:**
1. **구체적인 상태 정의**:
   - 스켈레톤의 **상태(State)**를 구체적으로 정의.
   - 예: **`IdleState`, `MoveState`, `AttackState`, `StunnedState`, `DeadState`** 등.

2. **상태 전환 로직**:
   - 스켈레톤의 **상태 전환**을 구현.
   - 예: 스켈레톤이 **대기 상태에서 플레이어를 감지하면** **공격 상태로 전환**.

3. **구체적인 스켈레톤 특화 동작**:
   - **스켈레톤만의 고유한 동작**을 추가.
   - 예: **스켈레톤이 화살을 발사**하는 동작, **스켈레톤이 특정 상황에서 점프**하는 동작 등.

#### **구현 예시:**
```csharp
public class Enemy_Skeleton : Enemy
{
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); // 초기 상태를 Idle로 설정
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(attackState); // 죽을 때의 상태 전환
    }
}
```

---

### **정리**

- **`Entity` 클래스**는 모든 캐릭터(적, 플레이어 등)에 적용되는 **기본 동작**(이동, 속도, 충돌 처리 등)을 정의합니다.
- **`Enemy` 클래스**는 **적 캐릭터에 특화된 동작**(스턴, 상태 패턴 등)을 추가로 정의합니다.
- **`Enemy_Skeleton` 클래스**는 **스켈레톤 적의 구체적인 행동과 상태**를 관리하며, **스켈레톤에 특화된 동작**(공격, 죽음 등)을 구현합니다.

이렇게 클래스별로 **역할과 책임을 분리**하면 유지보수와 확장이 용이해지며, 다른 적 캐릭터도 쉽게 추가할 수 있게 됩니다. 각 클래스는 자신의 역할에 맞는 기능만을 담당하게 되므로, 코드가 명확해집니다.

이해가 되셨나요? 추가 질문이 있으면 언제든지 물어보세요! 😊

## 프로젝트 메모
* 241017
	* StaticManager > UIManager > Alert 방향으로 설계한 이유와 게터세터 부분 파악해서 정리하기
	* OnClickCreateNicknameButton > BaseUI, protected, ShowAlertUI() > StaticManager > UIManager > AlertUI > AlertUI의 OpenWarningUI 실행 메커니즘 이해
 ## 기획
## 구현 리스트

### 게임시작

**1. 데이터 세팅 (어드레서블)**

**사용이유**
어드레서블을 사용한 이유는 모바일 출시를 계획하고 있기 때문에 변경사항에 대해 손쉽게 패치하기 위해서

**어드레서블 세팅**
https://www.youtube.com/watch?v=Z84GCeod_BM  (로컬 세팅)
https://www.youtube.com/watch?v=uTSxPPaW2-k (서버세팅)
로컬이랑 서버 세팅이라는데 일단 로컬 보면서 세팅하고 서버세팅 영상도 보면서 세팅함

영상에서 공부한 내용 아래 기록하였음 (로컬용이긴한데 어차피 서버 영상도 이어져서 일단 따라하였음)

Cache Clear Behavior : ClearWhenNewVersionLoaded 옵션을 선택했다 그 이유는 서버에 최산화 된 번들이 있을 때 기존 번들에 매치되는 캐시 파일을 자동으로 정리하는 방식으로 작동하기 때문이다 

Unique Bundle  IDS : 옵션을 체크하였다 안 푸는게 좋다고 한다 이걸 체크 해제하면 a가 있는데 빌드하고 b가 추가 됐을 뿐인데 a로 또 받는다고 한다 어쩃든 체크!
Send profiler events : 체크한다 메모리 로드 언로드를 확인할 수 있다고 한다



**2. PlayFab (로그인)**
* 로그인 완료 시 
	* 플레이어 데이터 세팅
	* 코어 데이터 세팅
	* 인벤토리데이터 세팅

> [!NOTE]- ds
> ![[Diagram 2.svg]]
> 
> 각 클래스마다 어떤 기능을 해야 하는지 구체적으로 설명드릴게요. 여기서는 **각 클래스의 역할**과 **어떤 기능을 구현해야 하는지**를 더 명확하게 정의하겠습니다.
> 
> ---
> 
> ### **1. `Entity` 클래스**
> `Entity` 클래스는 **모든 캐릭터**(적, 플레이어 등)에 공통적으로 적용될 **기본 기능**을 정의하는 클래스입니다. **공통적인 동작**을 여러 캐릭터 간에 재사용할 수 있도록 하기 위해 만들었습니다.
> 
> #### **`Entity` 클래스에서 해야 할 일:**
> 1. **캐릭터 공통 속성 정의**:
>    - **애니메이션(`Animator`)**, **물리적 속성(`Rigidbody2D`)**, **충돌 처리(`Collider`)** 등의 기본 컴포넌트를 정의.
>    - 모든 캐릭터가 공통적으로 가질 수 있는 속성을 정의.
> 
> 2. **공통 동작 구현**:
>    - **이동, 속도 제어, 넉백 처리, 충돌 처리, 죽음 처리**와 같은 캐릭터의 기본 동작을 구현.
>    - **메서드 예시**:
>      - `SetVelocity()`: 캐릭터의 이동 속도를 설정.
>      - `Flip()`: 캐릭터의 방향을 바꿔주는 함수.
>      - `Die()`: 캐릭터가 죽었을 때 호출되는 함수.
> 
> 3. **가상 메서드 제공**:
>    - 특정 캐릭터(적 또는 플레이어)에 맞게 재정의할 수 있는 가상 메서드(`virtual`)를 제공.
>    - 예를 들어, **플레이어와 적의 죽음 처리**가 다를 수 있으므로 `Die()` 메서드를 **가상 메서드로 정의**하고, 적과 플레이어에서 각각 다르게 구현할 수 있도록 함.
> 
> #### **구현 예시:**
> ```csharp
> public class Entity : MonoBehaviour
> {
>     public Animator anim { get; private set; }
>     public Rigidbody2D rb { get; private set; }
>     public CapsuleCollider2D cd { get; private set; }
> 
>     protected virtual void Awake()
>     {
>         anim = GetComponentInChildren< Animator>();
>         rb = GetComponent< Rigidbody2D>();
>         cd = GetComponent< CapsuleCollider2D>();
>     }
> 
>     public virtual void SetVelocity(float x, float y)
>     {
>         rb.velocity = new Vector2(x, y);
>     }
> 
>     public virtual void Die()
>     {
>         // 캐릭터가 죽었을 때 처리
>     }
> }
> ```
> 
> ---
> 
> ### **2. `Enemy` 클래스**
> `Enemy` 클래스는 **적**에 특화된 기능을 정의하는 클래스입니다. **`Entity`를 상속받아 적의 특화된 기능을 추가**합니다.
> 
> #### **`Enemy` 클래스에서 해야 할 일:**
> 1. **적 특화 속성 추가**:
>    - **스턴**, **전투 거리** 등 **적에게만 해당되는 속성**을 정의.
>    - 예를 들어, `CanBeStunned()` 같은 함수는 적에게만 적용될 수 있습니다.
> 
> 2. **상태 패턴(State Machine) 추가**:
>    - 적의 행동을 상태로 관리할 수 있도록 **상태 머신**을 구현.
>    - 예: **Idle(대기)**, **Move(이동)**, **Attack(공격)**, **Stunned(스턴)**, **Dead(죽음)** 상태로 나누어 각 상태에서 적의 행동을 제어.
> 
> 3. **상태 전환 관리**:
>    - 적이 특정 상태(예: 대기, 이동, 공격)에서 **다른 상태로 전환**하는 로직을 관리.
> 
> #### **구현 예시:**
> ```csharp
> public class Enemy : Entity
> {
>     public float moveSpeed = 2f;
>     public EnemyStateMachine stateMachine { get; private set; }
> 
>     protected override void Awake()
>     {
>         base.Awake();
>         stateMachine = new EnemyStateMachine();
>     }
> 
>     protected override void Update()
>     {
>         base.Update();
>         stateMachine.currentState.Update(); // 현재 상태를 업데이트
>     }
> 
>     public virtual void CanBeStunned()
>     {
>         // 스턴 가능 여부를 확인하는 로직
>     }
> 
>     public override void Die()
>     {
>         base.Die();
>         // 적의 구체적인 죽음 처리
>     }
> }
> ```
> 
> ---
> 
> ### **3. `Enemy_Skeleton` 클래스**
> `Enemy_Skeleton` 클래스는 **구체적인 스켈레톤 적**의 행동과 상태를 정의하는 클래스입니다. **`Enemy` 클래스를 상속받아 스켈레톤 적의 상태와 동작**을 구현합니다.
> 
> #### **`Enemy_Skeleton` 클래스에서 해야 할 일:**
> 1. **구체적인 상태 정의**:
>    - 스켈레톤의 **상태(State)**를 구체적으로 정의.
>    - 예: **`IdleState`, `MoveState`, `AttackState`, `StunnedState`, `DeadState`** 등.
> 
> 2. **상태 전환 로직**:
>    - 스켈레톤의 **상태 전환**을 구현.
>    - 예: 스켈레톤이 **대기 상태에서 플레이어를 감지하면** **공격 상태로 전환**.
> 
> 3. **구체적인 스켈레톤 특화 동작**:
>    - **스켈레톤만의 고유한 동작**을 추가.
>    - 예: **스켈레톤이 화살을 발사**하는 동작, **스켈레톤이 특정 상황에서 점프**하는 동작 등.
> 
> #### **구현 예시:**
> ```csharp
> public class Enemy_Skeleton : Enemy
> {
>     public SkeletonIdleState idleState { get; private set; }
>     public SkeletonAttackState attackState { get; private set; }
> 
>     protected override void Awake()
>     {
>         base.Awake();
>         idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
>         attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
>     }
> 
>     protected override void Start()
>     {
>         base.Start();
>         stateMachine.Initialize(idleState); // 초기 상태를 Idle로 설정
>     }
> 
>     public override void Die()
>     {
>         base.Die();
>         stateMachine.ChangeState(attackState); // 죽을 때의 상태 전환
>     }
> }
> ```
> 
> ---
> 
> ### **정리**
> 
> - **`Entity` 클래스**는 모든 캐릭터(적, 플레이어 등)에 적용되는 **기본 동작**(이동, 속도, 충돌 처리 등)을 정의합니다.
> - **`Enemy` 클래스**는 **적 캐릭터에 특화된 동작**(스턴, 상태 패턴 등)을 추가로 정의합니다.
> - **`Enemy_Skeleton` 클래스**는 **스켈레톤 적의 구체적인 행동과 상태**를 관리하며, **스켈레톤에 특화된 동작**(공격, 죽음 등)을 구현합니다.
> 
> 이렇게 클래스별로 **역할과 책임을 분리**하면 유지보수와 확장이 용이해지며, 다른 적 캐릭터도 쉽게 추가할 수 있게 됩니다. 각 클래스는 자신의 역할에 맞는 기능만을 담당하게 되므로, 코드가 명확해집니다.
> 
> 이해가 되셨나요? 추가 질문이 있으면 언제든지 물어보세요! 😊

## 문제 해결
* 문제 : 런타임에서 어드레서블로 플레이어 로드할때 머테리얼 핑크 현상
	* 원인 : 모바일 빌드인데 PC에디터로 렌더링해서
		* 해결 : https://velog.io/@9oo9leljh/Unity-Addressable-%EC%97%90%EC%85%8B-%EB%A1%9C%EB%93%9C-%EC%8B%9C-%EC%89%90%EC%9D%B4%EB%8D%94-%EA%B9%A8%EC%A7%90-%ED%95%91%ED%81%AC%EC%83%89-%EB%B3%B4%EC%9E%84 (원복해야하나)

## 구현 과정 설명

### 클릭 한 위치에 적 있는지 체크하는 로직
게임뷰에서 플레이어 머리를 클릭한 후에 기즈모를 통해 확인했더니 정상적으로 카메라에서 레이를 발사하여 플레이어의 머리를 정확히 관통하였다 이로서 내가 클릭한 위치의 오브젝트를 식별할 수 있게 되었다. 
![[Pasted image 20241103183151.png||500]]

카메라를 Orthographic 으로 하면 카메라의 절두체가 아래와 같이 잘리게 되는데 
![[Pasted image 20241103183829.png || 500]]