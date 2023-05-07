---
id: MockSender-1
title: MockSender<T>
---

# Class MockSender&lt;T&gt;



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
public sealed class MockSender<T> : ISender<T>
```

##### Type Parameters
| Name | Description |
| ---- | ---- |
| T |  |


### Properties

#### OnSendAsyncCalled

##### Declaration

```cs
public EventHandler<T>? OnSendAsyncCalled { get; set; }
```
#### SendAsyncReturn

##### Declaration

```cs
public Task? SendAsyncReturn { get; set; }
```
### Methods
#### SendAsync(T)



##### Declaration

```cs
public Task SendAsync(T item)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | item |  |

##### Returns
| Type | Description |
| ---- | ---- |
| Task |  |

