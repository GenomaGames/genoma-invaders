---
lesson: 2
name: Not defined
excerpt: Not defined
published: false
---

### Enemy Controller

Now to coding, let do the same as we did for the Player component for the first time.

**`Enemy.cs`**
```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Update()
    {
        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Vector3-right.html
        Vector3 right = Vector3.right;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Time-deltaTime.html
        float timeSinceLastFrame = Time.deltaTime;

        Vector3 translation = right * timeSinceLastFrame;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Transform.Translate.html
        transform.Translate(
          translation
        );
    }
}

```

Now our enemies are fleeing to the right further and further. Don't worry bout it now, we'll deal with them later.
