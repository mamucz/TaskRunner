---
id: TaskRunnerItem-1
title: TaskRunnerItem<T>
---

# Class TaskRunnerItem&lt;T&gt;



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
public sealed class TaskRunnerItem<T>
```

##### Type Parameters
| Name | Description |
| ---- | ---- |
| T |  |

### Constructors

#### TaskRunnerItem(T)



##### Declaration

```cs
public TaskRunnerItem(T key)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | key |  |

### Properties

#### Key

##### Declaration

```cs
public T Key { get; }
```
### Methods
#### IsRegistered()



##### Declaration

```cs
public bool IsRegistered()
```

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

#### IsRunningOrNotRegistered()



##### Declaration

```cs
public bool IsRunningOrNotRegistered()
```

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

#### SetTimeCpuLastStart(Int64)



##### Declaration

```cs
public void SetTimeCpuLastStart(long timeCpuLastStart)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| System.Int64 | timeCpuLastStart |  |


#### TryRun()



##### Declaration

```cs
public bool TryRun()
```

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

#### RemoveRunning()



##### Declaration

```cs
public void RemoveRunning()
```


#### TryRemoveRegisteredDuringRun()



##### Declaration

```cs
public bool TryRemoveRegisteredDuringRun()
```

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

#### TryRemoveRegistered()



##### Declaration

```cs
public bool TryRemoveRegistered()
```

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

#### TryGetLastStart(out Int64)



##### Declaration

```cs
public bool TryGetLastStart(out long timeCpuLastStart)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| System.Int64 | timeCpuLastStart |  |

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

#### TryRegister()



##### Declaration

```cs
public bool TryRegister()
```

##### Returns
| Type | Description |
| ---- | ---- |
| System.Boolean |  |

