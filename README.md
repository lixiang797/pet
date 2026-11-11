# 动物运动会 Unity 原型

这是一个用于 Unity 的主菜单与赛场切换原型，包含动物选择、赛项选择、比赛场景、简单的玩家/AI 赛跑和结果面板。

## 要求

- Unity 2021.3 或更新版本
- TextMeshPro（Unity 首次提示时导入 TMP Essentials）
- 输入使用 Unity 旧版 Input Manager：`Horizontal` / `Vertical`。若项目使用 New Input System，请在 Player Settings 中将 Active Input Handling 设为 `Both` 或 `Input Manager (Old)`。

## 文件

- `Assets/Scripts/Core/GameFlowManager.cs`：跨场景保存菜单选择并切换场景。
- `Assets/Scripts/Animals/AnimalData.cs`：动物 ScriptableObject 数据。
- `Assets/Scripts/Animals/AnimalController.cs`：简单赛跑控制器。
- `Assets/Scripts/Race/RaceManager.cs`：比赛开始、完赛顺序与结束判断。
- `Assets/Scripts/UI/MainMenuController.cs`：主菜单、动物和赛项选择。
- `Assets/Scripts/UI/RaceSceneUIController.cs`：比赛 HUD、结果、重赛和返回菜单。

## 创建场景

### 1. 建立场景并加入构建列表

创建并保存：

- `Assets/Scenes/MainMenu.unity`
- `Assets/Scenes/AnimalRaceScene.unity`

在 **File > Build Profiles**（旧版 Unity 为 **File > Build Settings**）中，将两个场景加入 Scenes In Build，确保 `MainMenu` 在列表中。

### 2. 配置 MainMenu

1. 创建 Canvas。确保有 `GraphicRaycaster`；Unity 通常会自动添加 `EventSystem`。
2. 在 Canvas 下创建 `MainPanel`、`AnimalPanel`、`ProjectPanel` 三个 Panel。
3. MainPanel 放动物选择、赛项选择、开始比赛、退出按钮，以及当前选择文字。
4. AnimalPanel 放 Rabbit、Turtle、Monkey 按钮和返回按钮。
5. ProjectPanel 放 Sprint、Obstacle 按钮和返回按钮。
6. 在场景根节点创建空物体 `GameFlowManager`，挂载同名脚本。
7. 在 Canvas 挂载 `MainMenuController`，在 Inspector 中逐一绑定脚本字段。
8. 初始只启用 MainPanel；另外两个 Panel 取消激活。

### 3. 创建动物数据

在 Project 窗口右键选择 **Create > Pet Games > Animal Data**，创建 Rabbit、Turtle、Monkey 三个数据资源。`AnimalName` 分别填写 `Rabbit`、`Turtle`、`Monkey`。名称区分大小写，必须与菜单按钮的名称一致。

### 4. 配置 AnimalRaceScene

1. 创建地面 Cube，命名 `Ground`，设置 Tag 为 `Ground`。若没有该 Tag，先在 Inspector 的 Tag 下拉菜单中添加。
2. 地面沿 Z 轴延伸，动物面向世界坐标的 +Z 方向。
3. 创建至少两个动物对象，每个对象添加 Rigidbody、Collider、AnimalController，并将对应 AnimalData 拖入字段。
4. 将动物摆在不同的 X 位置、相同的起跑 Z 位置。确保 Rigidbody 不使用 Is Kinematic。
5. 创建空物体 `RaceManager` 并挂载 RaceManager 脚本，把所有参赛动物拖入 `racers` 列表。
6. 创建 Canvas，添加比赛状态、排名、开始按钮、返回菜单按钮和结果 Panel。结果 Panel 内放结果文字、重赛按钮和菜单按钮。
7. 将 `RaceSceneUIController` 挂在 Canvas 上，绑定所有 UI 字段。结果 Panel 初始可以启用，脚本运行时会将它隐藏。

### 5. 操作和测试

1. 从 MainMenu 启动 Play Mode。
2. 选择动物和赛项，点击开始比赛。
3. 使用 A/D 或方向键调整横向位置；WASD 输入映射由 Unity Input Manager 提供；空格跳跃。
4. AI 选手会自动向 +Z 方向跑。先到终点线的动物进入完赛排名。
5. 全部动物完赛后显示结果面板，可重赛或回主菜单。

## 当前原型的范围

Sprint 和 Obstacle 目前都使用基础赛跑流程；这个仓库版本尚未实现障碍判定、技能、接力、拔河、训练、商店或存档。它们应在基础场景切换和比赛闭环验证后再逐项添加。当前菜单里的赛项选择会传递到比赛场景，但不会自动生成障碍或拔河玩法。

## 常见问题

- 场景加载失败：检查场景名与 `GameFlowManager` 中的名称一致，并确认场景加入 Scenes In Build。
- 玩家动物没选中：检查该动物已加入 `RaceManager.racers`，且 AnimalData 名称与菜单选择一致。
- 角色不动：检查 Rigidbody、Collider、地面 Tag，并确认项目启用了旧版 Input Manager 或 Active Input Handling 为 Both。
- UI 按钮没响应：检查 Canvas 有 GraphicRaycaster、场景有 EventSystem，并在 Inspector 绑定了 Button 引用。
