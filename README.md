# UnityLocalization
Unity3d Localization plugin

# Installation
Use Localization.unitypackage to automaticaly import as asset, or just import "Localization" folder to your project.

# How to use
1.You need add localization script on some object on scene.

2.Add some language by clicking on "Add language".

![dbc00ba041b82fbf7649529b2a2d9c50](https://user-images.githubusercontent.com/32653296/46013087-d345ce80-c0d3-11e8-9b39-3a9079cdf0c2.png)

3.Create new List with string by clicking on "Add new list".

![baa93efde62d88971c9fdebda8e77b8f](https://user-images.githubusercontent.com/32653296/46013282-80b8e200-c0d4-11e8-8ed9-6151609285a9.png)

4.Add new string to a new list by clicking on "Add new key" and fill in the fields.

![94d3df598a0805c4cbc829d7a0b56c21](https://user-images.githubusercontent.com/32653296/46013732-c4f8b200-c0d5-11e8-8a15-1cb3ca01207e.png)

5.Now to get this string just use method Localization.GetString("list's key", "string's key") from namespace "SlizzLoc"(SlizzLoc.Localization.GetString("list's key", "string's key"))

To change language use method SlizzLoc.Localization.Instance.ChangeLanguage(language's index)

# How to connect UIText and TextMesh with localization

![4b2fbd3f2da4ea883875c63303302024](https://user-images.githubusercontent.com/32653296/46070571-ccc45f00-c186-11e8-9fef-0b11df0cf0d1.png)

1.Press "Get all string". All text from UIText add to list with key "UI" and all text from TextMesh add to list with key "TextMesh".
2.Now all your UITexts and TextMeshs will get it text from localization.

![8d1f80d67abe73d8d5c989a1ece675a3](https://user-images.githubusercontent.com/32653296/46071508-d8188a00-c188-11e8-9e44-60dc15065c58.png)

# To use it on NUI(UnityUI)
Add script UGUIText to object with Text component and choose which string it must get. 
