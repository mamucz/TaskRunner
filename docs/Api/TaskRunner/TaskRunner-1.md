---
id: TaskRunner-1
title: TaskRunner<T>
---

# Class TaskRunner&lt;T&gt;



<div class="inheritance">

##### Inheritance

<div class="level" style={{"--data-index": 0}}>
System.Object
</div>
</div>

##### Inherited Members

<details>
<summary>Show</summary>
</details>

##### Syntax

```cs
public sealed class TaskRunner<T> : ITaskRunner<T> where T : IEquatable<T>
```

##### Type Parameters
| Name | Description |
| ---- | ---- |
| T |  |

### Constructors

#### TaskRunner(ISender&lt;T&gt;, ISettings)



##### Declaration

```cs
public TaskRunner(ISender<T> sender, ISettings settings)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| TaskRunner.ISender&lt;T&gt; | sender |  |
| TaskRunner.ISettings | settings |  |

### Properties

#### RunningCount

##### Declaration

```cs
public int RunningCount { get; }
```
### Methods
#### Add(T)



##### Declaration

```cs
public void Add(T key)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | key |  |


#### Remove(T)



##### Declaration

```cs
public void Remove(T key)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | key |  |


