
### 코드 작성 이유
몬스터가 플레이어를 감지하기 위해 (정찰 하다가 발견하면 BattleState등으로 변경하기 위함)

### 코드분석
1. 플레이어 방향으로 백터값 계산 플레이어 위치 - 몬스터 위치 빼고 normalized를 통해 정규화를 한다.
2. ray 변수에 어디에서 어느방향으로 쏠건지에 대한 몬스터 위치와 방향벡터값 저장
3. Raycast를 통해 어느정도 쏠건지 길이값 전달
``` csharp
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player;       // 플레이어의 위치
    public float rayDistance = 10f; // 레이 길이

    void Update()
    {
        // 1. 플레이어의 방향으로 벡터를 계산
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // 2. 몬스터의 위치에서 플레이어 방향으로 레이 발사
        Ray ray = new Ray(transform.position, directionToPlayer);

        // 3. 레이캐스트 수행 (길이 제한)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // 레이가 물체에 맞았을 때 (플레이어나 다른 물체에 충돌)
            Debug.DrawRay(transform.position, directionToPlayer * rayDistance, Color.red);
            Debug.Log("Ray hit: " + hit.collider.gameObject.name); // 히트된 오브젝트 이름 출력
        }
        else
        {
            // 레이가 아무것도 맞추지 않았을 때
            Debug.DrawRay(transform.position, directionToPlayer * rayDistance, Color.green);
        }
    }
}

```