# 动物运动会 Unity 原型

这是一个可扩展的 Unity 原型，包含：

- 主菜单与场景切换
- 动物选择
- 赛项选择
- 创建 RaceManager 进行比赛
- 简单 AI / 玩家赛跑
- 结果面板
- 障碍赛基础脚本
- 拔河赛基础脚本
- 训练/商店系统

## 结构

- `Assets/Scripts/Core/GameFlowManager.cs`
- `Assets/Scripts/Animals/AnimalData.cs`
- `Assets/Scripts/Animals/AnimalController.cs`
- `Assets/Scripts/Race/RaceProjectType.cs`
- `Assets/Scripts/Race/RaceManager.cs`
- `Assets/Scripts/Race/Obstacle.cs`
- `Assets/Scripts/Race/ObstacleRaceManager.cs`
- `Assets/Scripts/Race/TugOfWarManager.cs`
- `Assets/Scripts/Progress/PlayerProgress.cs`
- `Assets/Scripts/UI/MainMenuController.cs`
- `Assets/Scripts/UI/RaceSceneUIController.cs`
- `Assets/Scripts/UI/TrainingPanelController.cs`
- `Assets/Scripts/UI/ShopPanelController.cs`

## 使用方式

1. 在 Unity 里创建 `MainMenu` 与 `AnimalRaceScene` 两个场景。
2. 在 `MainMenu` 场景中，放置一个 `GameFlowManager`，挂载 `GameFlowManager.cs`。
3. 创建 `Canvas`，绑定 `MainMenuController`，并添加动物/项目选择按钮。
4. 在 `AnimalRaceScene` 中创建赛道、地面和动物对象，创建 `RaceManager`，填充 racers 列表。
5. 添加 `RaceSceneUIController` 到 Canvas，绑定状态文字、排名、开始按钮、重赛与返回菜单按钮。
6. 为每个动物绑定一个 `AnimalData` ScriptableObject。
7. 给障碍赛物体挂 `Obstacle` 组件，并在 `ObstacleRaceManager` 里配置 obstacles 数组。
8. 在训练/商店 UI 中分别挂 `TrainingPanelController` 和 `ShopPanelController`，绑定按钮与文本。

## 当前进度

已支持：
- 主菜单切换到比赛场景
- 动物选择
- 项目选择
- 玩家/AI 控制赛跑
- 完赛排名与结算
- 障碍赛基础机制
- 拔河赛基础机制
- 训练面板
- 商店面板
- 玩家金币和解锁系统

尚未支持：
- 真正的技能系统（例如冲刺、闪避、跳跃增强）
- 接力交棒
- 完整事件与动画
- 音效和卡通美术

## 运行建议

- 优先在一个简易直线赛道中测试移动和完赛逻辑。
- 再启用障碍赛，验证 `Obstacle` 与 `RaceManager` 的联动。
- 最后补训练和商店界面，确保奖励与解锁流程闭环。
