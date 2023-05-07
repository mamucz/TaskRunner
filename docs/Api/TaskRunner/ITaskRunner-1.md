---
id: ITaskRunner-1
title: ITaskRunner<T>
---

# Interface ITaskRunner&lt;T&gt;


Run up to ISettings.ConcurrentCount tasks that awaits ISender.SendAsync method.
Equal items can be running only in one task.
Allows rerun of the equal items only after ISettings.Interval from task start. 
When multiple equal items are added during task processing and
processing ends or ISettings.Interval passes, task will run only once again. 




##### Syntax

```cs
public interface ITaskRunner<in T>
    where T : IEquatable<T>
```

##### Type Parameters
| Name | Description |
| ---- | ---- |
| T |  |


### Properties

#### RunningCount

Currently running tasks count.


##### Declaration

```cs
int RunningCount { get; }
```
### Methods
#### Add(T)


Adds an item for processing.



##### Declaration

```cs
void Add(T item)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | item |  |


#### Remove(T)


Removes an item from future processing.



##### Declaration

```cs
void Remove(T key)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | key |  |


