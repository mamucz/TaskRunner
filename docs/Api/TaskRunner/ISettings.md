---
id: ISettings
title: ISettings
---

# Interface ISettings




##### Syntax

```cs
public interface ISettings
```


### Properties

#### Interval

Safety interval of each task. Cannot rerun more often.


##### Declaration

```cs
TimeSpan Interval { get; }
```
#### ConcurrentCount

Maximal count of simultaneously running tasks.


##### Declaration

```cs
int ConcurrentCount { get; }
```
