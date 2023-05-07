---
id: TaskRunner
title: TaskRunner
---

# TaskRunner

## Classes

#### [TaskRunner&lt;T&gt;](./TaskRunner-1)
#### [TaskRunnerItem&lt;T&gt;](./TaskRunnerItem-1)
## Interfaces

#### [ISender&lt;T&gt;](./ISender-1)
> 
Interface for sending request

#### [ISettings](./ISettings)
#### [ITaskRunner&lt;T&gt;](./ITaskRunner-1)
> 
Run up to ISettings.ConcurrentCount tasks that awaits ISender.SendAsync method.
Equal items can be running only in one task.
Allows rerun of the equal items only after ISettings.Interval from task start. 
When multiple equal items are added during task processing and
processing ends or ISettings.Interval passes, task will run only once again. 

