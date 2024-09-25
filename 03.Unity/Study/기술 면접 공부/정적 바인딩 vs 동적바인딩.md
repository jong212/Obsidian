 

### **왜 참조 타입이 중요할까?**
- 코드에서 `Animal animal = new Dog();`라고 했을 때, `animal`은 `Animal` 타입이기 때문에 **컴파일 타임**에는 `Animal` 클래스에서 정의된 메서드만 호출할 수 있습니다.
  
  예를 들어, 아래와 같이 `Animal`과 `Dog` 클래스를 정의했다고 가정해 봅시다:

  ```csharp
  class Animal
  {
      public virtual void Speak()
      {
          Debug.Log("Animal speaks.");
      }

      public void Eat()
      {
          Debug.Log("Animal eats.");
      }
  }

  class Dog : Animal
  {
      public override void Speak()
      {
          Debug.Log("Dog barks.");
      }

      public void WagTail()
      {
          Debug.Log("Dog wags its tail.");
      }
  }
  ```

  여기서 `Dog` 클래스는 `Animal`을 상속받으며, `Speak()` 메서드를 재정의하고 `WagTail()`이라는 새로운 메서드를 추가했습니다.

### **실제로 사용하는 예시**

- `Animal animal = new Dog();`라고 선언하고 나서 사용할 수 있는 메서드는 `Animal`에 정의된 메서드들입니다. 즉, `animal.Speak();`와 `animal.Eat();`는 호출할 수 있지만, `animal.WagTail();`은 호출할 수 없습니다.

- **실제 호출 예**:
  ```csharp
  Animal animal = new Dog();
  animal.Speak();  // Output: "Dog barks." (동적 바인딩으로 Dog의 메서드가 호출됨)
  animal.Eat();    // Output: "Animal eats." (Animal의 메서드 호출)
  
  // animal.WagTail(); // Error: Animal 타입에는 WagTail()이 정의되어 있지 않음
  ```

### **핵심 요점**
- **컴파일 시점**에 `animal`은 `Animal` 타입이기 때문에 `Animal`에 정의된 메서드만 호출할 수 있습니다.
- 하지만 **런타임 시점**에는 `animal`이 실제로 `Dog` 객체를 가리키고 있기 때문에, `Dog` 클래스에서 재정의된 `Speak()` 메서드가 호출됩니다.

이해를 돕기 위해 비유하자면, `animal`은 `Animal`이라는 규칙을 따르는 역할을 맡았지만, 실제로는 `Dog`라는 특별한 기능을 가진 존재라고 생각하면 됩니다. 그래서 `Animal`의 규칙에 맞는 행동만 할 수 있지만, 그 행동의 실제 구현은 `Dog`의 것이 되는 거죠.



### **왜 `animal.Eat()`은 `Animal`의 메서드를 호출할까?**

1. **정적 바인딩과 동적 바인딩의 구분**:
   - 동적 바인딩은 `virtual`과 `override` 키워드로 지정된 메서드에서만 적용됩니다. 즉, 런타임에 실제 객체의 타입에 따라 메서드가 결정됩니다.
   - 정적 바인딩은 컴파일 시점에 메서드 호출이 결정되는 경우로, 메서드에 `virtual`과 `override` 키워드가 없으면 정적 바인딩이 일어나며, 부모 클래스의 메서드가 호출됩니다.

2. **메서드 바인딩의 과정**:
   - `animal.Speak()`는 `Speak()` 메서드가 `virtual`로 선언되어 있고, `Dog` 클래스에서 `override`로 재정의되었기 때문에 동적 바인딩에 의해 `Dog`의 메서드가 호출됩니다.
   - 반면, `animal.Eat()`은 `Animal` 클래스에만 정의된 메서드로 `virtual`이 없고, `Dog` 클래스에서 재정의되지 않았기 때문에 정적 바인딩이 적용됩니다. 따라서 컴파일 시점에 `Animal`의 `Eat()` 메서드로 결정되고, 그대로 호출됩니다.

### **코드로 다시 살펴보기**

```csharp
class Animal
{
    public virtual void Speak() // virtual로 정의되어 동적 바인딩 대상
    {
        Debug.Log("Animal speaks.");
    }

    public void Eat() // virtual이 없으므로 정적 바인딩
    {
        Debug.Log("Animal eats.");
    }
}

class Dog : Animal
{
    public override void Speak() // 재정의된 메서드
    {
        Debug.Log("Dog barks.");
    }
}

Animal animal = new Dog();
animal.Speak(); // Output: "Dog barks." (동적 바인딩, Dog의 메서드 호출)
animal.Eat();   // Output: "Animal eats." (정적 바인딩, Animal의 메서드 호출)
```

### **핵심 요약**
- `Speak()`는 `virtual`과 `override`로 정의되어 동적 바인딩이 일어나고, 런타임에 실제 객체인 `Dog`의 메서드가 호출됩니다.
- `Eat()`는 `virtual` 키워드가 없으므로 정적 바인딩이 일어나, `Animal` 클래스에 정의된 메서드가 호출됩니다.

이로 인해 참조 변수 `animal`이 `Dog` 객체를 가리키고 있더라도, `Eat()` 메서드는 `Animal`의 메서드를 그대로 호출하게 되는 것입니다.