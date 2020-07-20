# ATD Ver-0.2

**游戏简介**：使用Unity2018开发的一款RPG元素+塔防元素结合的3D游戏。

**Unity版本**: ~~Unity 2018.3.7f1 (64-bit)~~ 已更新到 2019.3.7f1

**可运行平台**：windows

# 游戏介绍

**游戏类型**：塔防+RPG的3D游戏

**游戏要素**：3D 塔防 英雄 建筑树 搭配

**主体玩法**：游戏里将会有一波波怪物进攻基地。玩家可以建造塔来防御敌人，同时也可以控制单独的个体英雄角色来攻击敌人。

**游戏模式**：

- **第三人称视角的RPG模式**

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(1).png)


- **上帝视角的建造模式**

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(2).png)


**控制方式**：在游戏中使用Tab按键，切换这两种操作模式：
- RPG模式下：WASD控制移动，Space跳跃，鼠标左键普通攻击。
- 建造模式下：鼠标左键建造，E销毁已建造的建筑。
- 数字键1,2,3,4,5,6控制物品栏,对应英雄技能或者建筑安放。

**胜利条件**：消灭所有敌人 或者 坚持到时间结束  
**失败条件**：基地生命值为0 或者 英雄死亡


# 游戏截图

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(1).png)

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(2).png)

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(5).png)

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(3).png)

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/screenshot%20(4).png)


# 架构设计

---

详见博客：

[Unity《ATD》塔防RPG类3D游戏架构设计（一） - KillerAery - 博客园](https://www.cnblogs.com/KillerAery/p/11191222.html)

[Unity《ATD》塔防RPG类3D游戏架构设计（二） - KillerAery - 博客园](https://www.cnblogs.com/KillerAery/p/11197175.html)

# 项目配置注意事项

## 安装package

由于项目配置了LWRP作为基本渲染管线，需要安装如下Package:
- LightWeight RP

![](https://github.com/ima-games/ATD/blob/master/MarkDown_Image/notice.png)

## 分辨率

项目目前基于固定的 ***1920\*1080分辨率*** 开发，没有对其他分辨率做适配。
可以在Editor时使用 ***1920\*1080分辨率*** ，而不要用FreeAspect。
