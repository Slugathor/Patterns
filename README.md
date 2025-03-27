This repo is for a past course on software architecture and design patterns.

The "game demo" is as minimalistic as possible and the point was to utilise many different programming patterns for the game features.
Thus, in Assets/Scripts you will find example uses of the following patterns

-Command pattern (Command.cs, PlayerController.cs)

-Singleton (GenericSingleton.cs, used by GameManager, Achievement System)

-State (elementary version using enum in PlayerController.cs)

-Observer (AchievementSystem.cs, CoinManager.cs, CoinScript.cs)


Use of Genericity:

AchievementSystem.cs contains some cool methods for creating new achievements.
I tried to make these compatible with any numeric variable types (int, float, double, uint, BigInteger, ...)

(see internal class CountableAchievement<T>  and associated methods CreateCoinCollectingAchievement and CreateDistanceWalkedAchievement)


Here is a demonstration video that shows most of these functionalities in action.

https://youtu.be/JSlaCBQCJiM?si=tLwCCj3yVgkfHrR5

There is a lot going on, but some of the achievements earned as well as states being changed can be seen on the console to the right.
