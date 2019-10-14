#There are main controller from script SlizzLoc.Localization

to get string on current language
```
GetString(string groupKey,string stringKey) 
```

#You can add new string by this method
```
AddString(string groupKey, string key, params string[] strings)

Example:
 AddString("UI", "Play Game", new string[]{"Play"});
```

#To create new group
```
AddGroup(string groupKey)
```

#To get whole group
```
GetGroup(string key)
```
#To get index of current language
```
int GetLanguage()
```

#To set language
```
SetLanguage(int index)
```

#To add language
```
AddLanguage(string lang)
```
