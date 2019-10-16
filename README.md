# UnityLocalization
Unity3d Localization plugin

# Installation
Use Localization.unitypackage to automaticaly import as asset, or just import "Localization" folder to your project.

# How to use
1.Open window "Window/Localization Slizz

2.If you don't have "LocalizationHolder" file in any "Resources" folder it ask you to "Init"

![screen1](https://user-images.githubusercontent.com/32653296/66780980-d710e280-eeda-11e9-9cae-018516bb005c.png)

3.Add some languages in "Options" tab

![s2](https://user-images.githubusercontent.com/32653296/66781119-2e16b780-eedb-11e9-8f75-a180083bf360.png)

4.Add new "Group", just enter the name and press "Add"

![dd9421e55ab50bfa9762e9b510d9348e](https://user-images.githubusercontent.com/32653296/66781174-48509580-eedb-11e9-9010-5b2a1fcf5e8e.png)

5.Add "String" to "Group" by enter the string's name and press "Add" and there you can add value for every languages

![s4](https://user-images.githubusercontent.com/32653296/66781373-c2811a00-eedb-11e9-9842-377801e6ac03.png)

You can change language from "Options" and from SlizzLoc.Localization.SetLanguage(index)

# New struct with dinamyc select which dict and string you want to use!

![gif](https://user-images.githubusercontent.com/32653296/66953149-96de6b00-f066-11e9-97cf-dceeac083daf.gif)

# For TextMesh and NUI(UnityUI)

You can add script U3DText for TextMesh and UGUIText for Text(UnityUI). Now you need to enter GroupKey and StringKey. You can turn on "UpdateOnAwake" to get string from localization on Awake or just call method UpdateText()

# Help for using from code

https://github.com/SlizzBen/UnityLocalization/blob/master/Help.md
