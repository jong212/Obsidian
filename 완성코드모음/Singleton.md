## 제네릭 싱글톤 스크립트 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();  
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            // 첫 번째 인스턴스이므로, 이 인스턴스를 사용하고 파괴되지 않게 한다.
            instance = this as T;
            if (transform.parent != null && transform.root != null)
            {
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            // 이미 인스턴스가 존재하므로, 이 인스턴스를 파괴한다.
            Destroy(gameObject);
        }
    }

}
```