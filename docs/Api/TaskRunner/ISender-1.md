---
id: ISender-1
title: ISender<T>
---

# Interface ISender&lt;T&gt;


Interface for sending request




##### Syntax

```cs
public interface ISender<in T>
```

##### Type Parameters
| Name | Description |
| ---- | ---- |
| T | Obejct to send |

### Methods
#### SendAsync(T)


Send the object async



##### Declaration

```cs
Task SendAsync(T item)
```
##### Parameters
| Type | Name | Description |
| ---- | ---- | ---- |
| T | item | object to send |

##### Returns
| Type | Description |
| ---- | ---- |
| Task | return task |

