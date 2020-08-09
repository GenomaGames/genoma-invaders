---
lesson: 3
name: Unity Physics 2D Using Collider2D and Kinematic RigidBody2D
excerpt: Learn how to handle 2D game objects collisions, while making a space invaders 2D game, using the Unity physics 2D engine with help of 2D colliders and kinematic 2D rigid bodies.
date: 2020-08-09
published: false
# tagline: TAGLINE
# header:
#   overlay_image: /path/to/image.png
#   overlay_filter: rgba(0, 0, 0,.8)
#   caption: "[see full image](../path/to/image.png)"
---

# Genoma Invaders - Unity Physics 2D Using Collider2D and Kinematic RigidBody2D

Learn how to handle 2D game objects collisions, while making a space invaders 2D game, using the Unity physics 2D engine with help of 2D colliders and kinematic 2D rigid bodies.


## Requirements

- Complete ["**How to Move 2D Objects in Unity**"](../_tutorials/02-how-to-move-2d-objects-in-unity.md) or checkout the code from [Genoma Invaders' Github repository, branch `tutorial/02`](https://github.com/GenomaGames/genoma-invaders/tree/tutorial/02)
- [**Unity 2020.1**](https://store.unity.com/download?ref=personal)
- [**Visual Studio Editor**](https://visualstudio.microsoft.com/es/vs/)


## Intro

In the last tutorial, we walked through **How to Move 2D Objects in Unity** using Unity's Scripting API ending up with a Scene containing a Player Game Object that moves when pressing directional input buttons and shooting bullets that move upwards in a constant manner when a `Fire1` related input is pressed.

Our next steps will be to:
- Make Bullets collide with the Enemies
- Avoid Player to get out of screen
- Organize our code


## Unity Physics 2D with Collider2D and RigidBody2D

--


## Unity's Kinematic RigidBody2D, Moving Objects with Physics

--


## Unity's Collider2D, Detecting Collisions

--


## Unity's Static RigidBody2D

--


## Conclusion

--


Happy Game Dev! :space_invader:






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
