# Animal Sports Day — playable Unity prototype

This repository includes a small, playable Unity game prototype. It creates its menus, track, racers, obstacles, and HUD at runtime using Unity primitives; no external art assets are needed.

## Requirements

- Unity 2021.3 LTS or later
- Unity UI (UGUI) package
- Active Input Handling set to **Input Manager (Old)** or **Both** (the prototype reads Horizontal and Left Shift/Space)

## Create the demo scenes

1. Open the repository as a Unity project and wait for scripts to compile.
2. Select **Tools > Pet Game > Create Demo Scenes**.
3. Open `Assets/Scenes/MainMenu.unity` and press Play.
4. If UI input does not respond, ensure an EventSystem exists and the UI package is enabled.

The editor utility creates `MainMenu` and `AnimalRaceScene`, adds both to Build Settings, and places a `GameBootstrap` in each. The bootstrap assembles the actual game content when the scene starts.

## Play

- Main menu: cycle through unlocked animals, switch between Sprint and Obstacle, train speed, unlock animals, or start a race.
- Race: click START, steer with A/D or arrow keys, jump with Space, and use the animal skill with Left Shift.
- Finish the 60 m track to get a placement. First place earns 120 coins, second 80, and third 40. Progress is saved in PlayerPrefs.
- The Rabbit has a sprint, Turtle recovers stamina, and Monkey gets a jump boost. AI runners use their skills and attempt to jump over obstacles.

## Scope

This is a complete, runnable **small MVP**, not a finished commercial game. Sprint and obstacle race modes are playable. Tug-of-war and relay are listed as future event types but are not selectable/playable in this MVP. Character art, animation, audio, multiplayer, and full relay mechanics are not included.
