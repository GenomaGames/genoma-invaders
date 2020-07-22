# Genoma Invaders - Getting Started in Unity 2D Game Development

This post will cover the basics to start making 2D games in Unity.

## Requirements

To follow this Unity tutorial you will need to:
- [Install UnityHub](https://unity3d.com/es/get-unity/download)
- [Install Unity](../_posts/2020-07-02-how-to-install-unity-2020.md)
- [Create a new 2D Game Unity Project](../_posts/2020-07-16-how-to-create-a-unity-2d-project.md)
- [Have basic knowledge about Unity's interface](https://docs.unity3d.com/2020.1/Documentation/Manual/UsingTheEditor.html)

## Genoma Invaders Game

The game we will be developing during this tutorial series is **Genoma Invaders**.

> Genoma Invaders is a [Fixed shooter](https://en.wikipedia.org/wiki/Category:Fixed_shooters) from the [Shoot 'em up](https://en.wikipedia.org/wiki/Shoot_%27em_up#Fixed_shooters) game genre where you are a microscopic submarine killing bacteria, virus, and other microorganisms inside a human body.

To define this project and its scope a very simple [Game Design Document](../docs/one-page-design-document.md) has been created.

<small>**Note:** Don't use this project as an example for game design as there is not much design process behind this project due to its educational purpose</small>

### Project File Structure

It is not mandatory, but a way to keep things tidy is to create a new directory inside `/Assets` with the name of your game, in this case, `/Assets/Genoma Invaders`, and move the `Scenes` directory inside of it. This way, all the game-related assets are contained inside this directory so they do not get mixed with 3rd party assets or plugins.

### Building the First Game Scene

Let's start with the [Scene](https://docs.unity3d.com/2020.1/Documentation/Manual/CreatingScenes.html) Unity creates whenever you create a new project by renaming it from `SampleScene` to `Game`. Here is where all the game action will happen.

![01-Unity_2020.1.0b13_(Beta)_blank_2D_project](/assets/2020-07-18-genoma-invaders-getting-started-with-unity-2d-game-development/01-Unity_2020.1.0b13_(Beta)_blank_2D_project.png)

Every game in Unity is contained within one or multiple scenes. For this project, we will be using them to store one level in each one and the main menu in another one but for now we only need one to start our game development.

#### Game Object

The next item smaller than a Unity scene is a [Game Object](https://docs.unity3d.com/2020.1/Documentation/Manual/GameObjects.html). They comprehend the bricks to build a game, graphics, sounds and logic. A Game Object can represent multiple things in a Unity game, players, enemies, lights, the ground, trees, cameras. This objects can also represent things that the player can not see, like the camera itself or an object in charge of reviving the player whenever it dies.

Unity starts a project always by placing a [Camera](https://docs.unity3d.com/2020.1/Documentation/Manual/class-Camera.html) Game Object inside the scene (depending on the project template it could place more stuff). A Camera is essential since it represents the eyes for the player as they will see what the camera sees. Do not mind it too much yet since we will not be handling it yet.

Player will be represented by a new Game Object. To create Game Objects in the Scene, click on the menu `GameObject > 2D Object > Sprite`.

![02-Unity_GameObject_2D_Object_Sprite_menu](/assets/2020-07-18-genoma-invaders-getting-started-with-unity-2d-game-development/02-Unity_GameObject_2D_Object_Sprite_menu.png)

It will create a new Game Object in the Scene called `New Sprite`. Rename it to `Player` and ensure its position is in `X=0 Y=0 Z=0` in the **Inspector Panel**.

![03-Unity_Inspector_Player_Game_Object](/assets/2020-07-18-genoma-invaders-getting-started-with-unity-2d-game-development/03-Unity_Inspector_Player_Game_Object.png)

This Game Object the we renamed `Player` should show a **Sprite Renderer** in the Inspector panel too.

Each section in the Inspector, divided by a line and a header, represents a [**Component**](https://docs.unity3d.com/2020.1/Documentation/Manual/Components.html) which is the next piece smaller than a Game Object. Game Objects contains Components which provides different features to the Game Object, the most common Component you can found in a Game Object is the [**Transform**](https://docs.unity3d.com/2020.1/Documentation/Manual/class-Transform.html) Component. It gives the Game Object the hability to be positioned, rotated or scaled inside the Scene so if the Game Object renders something it will be placed based on that values.