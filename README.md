# 運動牧場 Sports Ranch
運動牧場利用紀錄來培養運動習慣，透過圖表以及遊戲，幫助使用者保持日常運動。

## 展示 DEMO
<img src="./pic/demo/demo1.gif" width="400"> <img src="./pic/demo/demo2.gif" width="400">
<img src="./pic/demo/demo3.gif" width="400"> <img src="./pic/demo/demo4.gif" width="400">

## 遊玩方法
玩家是牧場的場主，一開始就擁有三隻獅子，牧場內的動物們會隨時間過去而飢餓、心情變差。玩家要藉由登記運動紀錄，來得到糧食，來得到更多的動物。
```
草，用來餵食草食性動物
肉 ，用來餵食肉食性動物
```

## 特點 Features
+ 將每一次的運動記錄起來
+ 使用者可以自訂運動類型
+ 透過月曆上每日不同的顏色深淺一目了然這個月的運動量
+ 一年內、一周間的卡路里表
+ 透過養成遊戲、為了餵食獅子更努力的運動吧!

## 技術/環境
+ 使用 Visual Studio 2015
+ 主要語言為 C#
+ 串接mySQL作為資料庫

## Data Base and API
#### ExerciseType
```c#
bool addExerciseName (string ExerciseName, float Calories, string ExerciseType);
bool delExerciseByName (string exerciseName);
Arraylist getAllExerciseName ();
```

#### AnimalSpecies
```c#
int getSpeciesHungerValue (string species); 
int getSpeciesMood (string species);
string getSpeciesFeedingHabit  (string species);
```
#### ObtainedAnimals
```c#
int getAnimalHungerValue (string animalName);
int getAnimalMood (string animalName);
string getAnimalSpecies (string animalName);
int getAnimalLevel (string animalName);
bool addAnimal (string animalName,  int hungryValue,  int mood,  string species, int animalLevel);
bool delAnimal (string animalName);
ArrayList getAllAnimalName ();
```
#### System
```c#
ArrayList getSystemData ();
void updateSystemDatumById (int userId, Datetime logoutTime, int grass, int meat, 
int money, int animalNumber, string username, float height, float weight, int ranchLevel, int recordNumber);
```
#### Badge 
```c#
ArrayList  getBadgeData ();
```


## 優化UI
### 紀錄下載時機
+ 每一次只吃進幾筆資料
+ 減少使用者等待時間
+ 根據卷軸位置決定吃進資料的時機

### 在標籤中新增運動項目
當找不到想要的項目時可以直接新增
跳出新增頁面

## 作者 Authors
|分工| 負責 |  工作描述 |
|:----:| -------------|--------------|
|前端程式設計| 洪曼容 (Klareh) | 介面設計、按鈕功能實現、功能設計 |
|後端程式設計| 陳楷雯  |資料庫上下載、字串處理、功能設計|

