using System.Collections.Concurrent;
using System.Diagnostics;
using NUnit.Framework;

namespace TaskRunner.Test;

public class TestTaskRunner
{
    [Test]
    public async Task TestAddThenProcess()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);

        var item1 = "item1";

        var sw = Stopwatch.StartNew();
        ConcurrentDictionary<int, (TimeSpan, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, inst.RunningCount));
        };

        inst.Add(item1);
        var count1 = inst.RunningCount;
        await Task.Delay(300);
        var count2 = inst.RunningCount;

        Assert.That(count1 == 1);
        Assert.That(count2 == 0);

        Assert.That(data.Count == 1);
        Assert.That(data[1].Item1 < TimeSpan.FromMilliseconds(10));
    }
    
    [Test]
    public async Task TestAddLongTask()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);

        var item1 = "item1";

        var sw = Stopwatch.StartNew();
        ConcurrentDictionary<int, (TimeSpan, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(1000);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, inst.RunningCount));
        };

        inst.Add(item1);
        var count1 = inst.RunningCount;
        await Task.Delay(300); 
        inst.Add(item1);
        var count2 = inst.RunningCount;
        await Task.Delay(300);
        inst.Add(item1);
        var count3 = inst.RunningCount;
        await Task.Delay(300);
        inst.Add(item1);
        var count4 = inst.RunningCount;
        await Task.Delay(300); 
        var count5 = inst.RunningCount;
        await Task.Delay(300); 
        var count6 = inst.RunningCount;

        Assert.That(count1 == 1);
        Assert.That(count2 == 1);
        Assert.That(count3 == 1);
        Assert.That(count4 == 1);
        Assert.That(count5 == 0);
        Assert.That(count6 == 0);

        Assert.That(data.Count == 2);
        Assert.That(data[1].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(data[2].Item1 < TimeSpan.FromMilliseconds(1000+10));
    }

    [Test]
    public async Task TestAddFourDifferentNamesThenProcess()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);

        var item1 = "item1";
        var item2 = "item2";
        var item3 = "item3";
        var item4 = "item4";

        var sw = Stopwatch.StartNew();
        ConcurrentDictionary<int, (TimeSpan, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, inst.RunningCount));
        };

        inst.Add(item1);
        var count1 = inst.RunningCount;
        await Task.Delay(25);
        inst.Add(item2);
        var count2 = inst.RunningCount;
        await Task.Delay(25);
        inst.Add(item3);
        var count3 = inst.RunningCount;
        await Task.Delay(25);
        inst.Add(item4);
        var count4 = inst.RunningCount;
        await Task.Delay(250);
        var count5 = inst.RunningCount;
        await Task.Delay(200);
        var count6 = inst.RunningCount;

        var diff = data[4].Item1 - data[1].Item1;

        Assert.That(count1 == 1);
        Assert.That(count2 == 2);
        Assert.That(count3 == 3);
        Assert.That(count4 == 3);
        Assert.That(count5 == 1);
        Assert.That(count6 == 0);

        Assert.That(data.Count == 4);
        Assert.That(data[1].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(data[2].Item1 < TimeSpan.FromMilliseconds(10 + 25));
        Assert.That(data[3].Item1 < TimeSpan.FromMilliseconds(10 + 25 + 25));
        Assert.That(diff > settings.Interval - TimeSpan.FromMilliseconds(10));
        Assert.That(diff < settings.Interval + TimeSpan.FromMilliseconds(10));
    }

    [Test]
    public async Task TestAddFourOrdersSameNamesThenProcess()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);

        var item1 = "item1";

        var sw = Stopwatch.StartNew();
        ConcurrentDictionary<int, (TimeSpan, string, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, "sendAsync", inst.RunningCount));
        };

        inst.Add(item1);
        var count1 = inst.RunningCount;
        inst.Add(item1);
        var count2 = inst.RunningCount;
        inst.Add(item1);
        await Task.Delay(25);
        var count3 = inst.RunningCount;
        inst.Add(item1);
        await Task.Delay(500);
        var count4 = inst.RunningCount;

        var diff = data[2].Item1 - data[1].Item1;

        Assert.That(count1 == 1);
        Assert.That(count2 == 1);
        Assert.That(count3 == 1);
        Assert.That(count4 == 0);

        Assert.That(data.Count == 2);
        Assert.That(data[1].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(diff > settings.Interval - TimeSpan.FromMilliseconds(10));
        Assert.That(diff < settings.Interval + TimeSpan.FromMilliseconds(10));
    }

    [Test]
    public async Task TestAddMultipleTimes()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);

        var item1 = "item1";
        var item2 = "item2";

        var sw = Stopwatch.StartNew();
        ConcurrentDictionary<int, (TimeSpan, string, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, "sendAsync", inst.RunningCount));
        };

        inst.Add(item1);
        var count1 = inst.RunningCount;
        inst.Add(item2);
        var count2 = inst.RunningCount;
        inst.Add(item1);
        var count3 = inst.RunningCount;
        inst.Add(item1);
        var count4 = inst.RunningCount;
        await Task.Delay(250);
        var count5 = inst.RunningCount;
        await Task.Delay(250);
        var count6 = inst.RunningCount;

        var diff = data[3].Item1 - data[1].Item1;

        Assert.That(count1 == 1);
        Assert.That(count2 == 2);
        Assert.That(count3 == 2);
        Assert.That(count4 == 2);
        Assert.That(count5 == 1);
        Assert.That(count6 == 0);

        Assert.That(data.Count == 3);
        Assert.That(data[1].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(data[2].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(diff > settings.Interval - TimeSpan.FromMilliseconds(10));
        Assert.That(diff < settings.Interval + TimeSpan.FromMilliseconds(10));
    }

    [Test]
    public async Task TestBiggerFlow()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);

        var item1 = "item1";
        var item2 = "item2";
        var item3 = "item3";
        var item4 = "item4";

        var sw = Stopwatch.StartNew();
        ConcurrentDictionary<int, (TimeSpan, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, inst.RunningCount));
        };

        inst.Add(item1);
        var count1 = inst.RunningCount;
        inst.Add(item2);
        var count2 = inst.RunningCount;
        inst.Add(item1);
        var count3 = inst.RunningCount;
        await Task.Delay(250);
        var count4 = inst.RunningCount;
        inst.Add(item1);
        var count5 = inst.RunningCount;
        await Task.Delay(25);
        inst.Add(item2);
        var count6 = inst.RunningCount;
        await Task.Delay(25);
        inst.Add(item3);
        var count7 = inst.RunningCount;
        await Task.Delay(25);
        inst.Add(item4);
        var count8 = inst.RunningCount;
        await Task.Delay(400);
        var count9 = inst.RunningCount;

        var item1FirstRun = data.First(x => x.Value.Item2 == item1).Value;
        var diff1 = data[3].Item1 - item1FirstRun.Item1;
        var diff2 = data[6].Item1 - data[3].Item1;
        var diff3 = data[7].Item1 - data[4].Item1;


        Assert.That(count1 == 1);
        Assert.That(count2 == 2);
        Assert.That(count3 == 2);
        Assert.That(count4 == 1);
        Assert.That(count5 == 1);
        Assert.That(count6 == 2);
        Assert.That(count7 == 3);
        Assert.That(count8 == 3);
        Assert.That(count9 == 0);

        Assert.That(data.Count == 7);
        Assert.That(data[1].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(data[2].Item1 < TimeSpan.FromMilliseconds(10));
        Assert.That(diff1 > settings.Interval - TimeSpan.FromMilliseconds(10));
        Assert.That(diff1 < settings.Interval + TimeSpan.FromMilliseconds(10));
        Assert.That(data[4].Item1 > TimeSpan.FromMilliseconds(250 + 25));
        Assert.That(data[5].Item1 > TimeSpan.FromMilliseconds(250 + 25 + 25));
        Assert.That(diff2 > settings.Interval - TimeSpan.FromMilliseconds(10));
        Assert.That(diff2 < settings.Interval + TimeSpan.FromMilliseconds(10));
        Assert.That(diff3 > settings.Interval - TimeSpan.FromMilliseconds(10));
        Assert.That(diff3 < settings.Interval + TimeSpan.FromMilliseconds(10));
    }
    
      [Test]
    public async Task TestRunPrioritizeOldest()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);
         
        var item1 = "item1";
        var item2 = "item2";
        var item3 = "item3";
        var item4 = "item4";

        var sw = new Stopwatch();
        ConcurrentDictionary<int, (TimeSpan, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, inst.RunningCount));
        };
 
        inst.Add(item1);
        await Task.Delay(250);
        data.Clear();

        sw.Start();
        inst.Add(item1);
        var count1 = inst.RunningCount;
        inst.Add(item2);
        var count2 = inst.RunningCount;
        inst.Add(item3);
        var count3 = inst.RunningCount;
        inst.Add(item4);
        var count4 = inst.RunningCount; 
        inst.Add(item1);
        var count5 = inst.RunningCount; 
        inst.Add(item2);
        var count6 = inst.RunningCount; 
        inst.Add(item3);
        var count7 = inst.RunningCount; 
        inst.Add(item4);
        var count8 = inst.RunningCount; 
        inst.Add(item1);
        var count9 = inst.RunningCount; 
        inst.Add(item2);
        var count10 = inst.RunningCount; 
        inst.Add(item3);
        var count11 = inst.RunningCount; 
        inst.Add(item4);
        var count12 = inst.RunningCount;
        await Task.Delay(250);
        var count13 = inst.RunningCount;
        await Task.Delay(350);
        var count14 = inst.RunningCount; 

        var firstItem1 = data.First(x => x.Value.Item2 == item1).Value;
        var firstItem2 = data.First(x => x.Value.Item2 == item2).Value;
        var firstItem3 = data.First(x => x.Value.Item2 == item3).Value;
        var firstItem4 = data.First(x => x.Value.Item2 == item4).Value;

        var secondItem1 = data.Where(x => x.Value.Item2 == item1).Skip(1).First().Value;
        var secondItem2 = data.Where(x => x.Value.Item2 == item2).Skip(1).First().Value;
        var secondItem3 = data.Where(x => x.Value.Item2 == item3).Skip(1).First().Value;
        var secondItem4 = data.Where(x => x.Value.Item2 == item4).Skip(1).First().Value;

        var diffs = new TimeSpan[4];
        diffs[0] = secondItem1.Item1 - firstItem1.Item1;
        diffs[1] = secondItem2.Item1 - firstItem2.Item1;
        diffs[2] = secondItem3.Item1 - firstItem3.Item1;
        diffs[3] = secondItem4.Item1 - firstItem4.Item1;

        var diff1Count = 0;
        var diff2Count = 0;
        foreach (var diff in diffs)
        {
            if (diff > settings.Interval - TimeSpan.FromMilliseconds(10) &&
                diff < settings.Interval + TimeSpan.FromMilliseconds(10))
                diff1Count++;
            if (diff > 2*settings.Interval - TimeSpan.FromMilliseconds(10) &&
                diff < 2*settings.Interval + TimeSpan.FromMilliseconds(10))
                diff2Count++;
        }
        
        Assert.That(count1, Is.EqualTo(1));
        Assert.That(count2, Is.EqualTo(2));
        Assert.That(count3, Is.EqualTo(3));
        Assert.That(count4, Is.EqualTo(3));
        Assert.That(count5, Is.EqualTo(3));
        Assert.That(count6, Is.EqualTo(3));
        Assert.That(count7, Is.EqualTo(3));
        Assert.That(count8, Is.EqualTo(3));
        Assert.That(count9, Is.EqualTo(3));
        Assert.That(count10, Is.EqualTo(3));
        Assert.That(count11, Is.EqualTo(3));
        Assert.That(count12, Is.EqualTo(3));
        Assert.That(count13, Is.EqualTo(3));
        Assert.That(count14, Is.EqualTo(0));

        Assert.That(data.Count, Is.EqualTo(8));
        Assert.That(firstItem1.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(firstItem2.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(firstItem3.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(firstItem4.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(diff1Count, Is.GreaterThanOrEqualTo(2));
        Assert.That(diff2Count, Is.EqualTo(1)); 
    }

    [Test]
    public async Task TestRunRemoveAdded()
    {
        var sender = new MockSender<string>();
        var settings = new MockSettings();
        var inst = new TaskRunner<string>(sender, settings);
         
        var item1 = "item1";
        var item2 = "item2";
        var item3 = "item3";
        var item4 = "item4";

        EventHandler<string>? remove = null;
        var sw = new Stopwatch();
        ConcurrentDictionary<int, (TimeSpan, string, int)> data = new();
        int i = 0;

        sender.SendAsyncReturn = Task.Delay(100);
        sender.OnSendAsyncCalled += (o, item) =>
        {
            data.TryAdd(Interlocked.Increment(ref i), (sw.Elapsed, item, inst.RunningCount));
            if (item == item4 || item == item3)
                remove?.Invoke(this, item);
        }; 
        
        remove += (sender, s) => inst.Remove(s); 
        
        inst.Add(item1);
        await Task.Delay(250);
        data.Clear();

        sw.Start();
        inst.Add(item1);
        var count1 = inst.RunningCount;
        inst.Add(item2);
        var count2 = inst.RunningCount;
        inst.Add(item3);
        var count3 = inst.RunningCount;
        inst.Add(item4);
        var count4 = inst.RunningCount; 
        inst.Add(item1);
        var count5 = inst.RunningCount; 
        inst.Add(item2);
        var count6 = inst.RunningCount; 
        inst.Add(item3);
        var count7 = inst.RunningCount; 
        inst.Add(item4);
        var count8 = inst.RunningCount; 
        inst.Add(item1);
        var count9 = inst.RunningCount; 
        inst.Add(item2);
        var count10 = inst.RunningCount; 
        inst.Add(item3);
        var count11 = inst.RunningCount; 
        inst.Add(item4);
        var count12 = inst.RunningCount;
        await Task.Delay(250);
        var count13 = inst.RunningCount;
        await Task.Delay(350);
        var count14 = inst.RunningCount; 

        var firstItem1 = data.First(x => x.Value.Item2 == item1).Value;
        var firstItem2 = data.First(x => x.Value.Item2 == item2).Value;
        var firstItem3 = data.First(x => x.Value.Item2 == item3).Value;
        var firstItem4 = data.First(x => x.Value.Item2 == item4).Value;

        var secondItem1 = data.Where(x => x.Value.Item2 == item1).Skip(1).First().Value;
        var secondItem2 = data.Where(x => x.Value.Item2 == item2).Skip(1).First().Value;

        var diffs = new TimeSpan[2];
        diffs[0] = secondItem1.Item1 - firstItem1.Item1;
        diffs[1] = secondItem2.Item1 - firstItem2.Item1;

        var diff1Count = 0;
        foreach (var diff in diffs)
        {
            if (diff > settings.Interval - TimeSpan.FromMilliseconds(10) &&
                diff < settings.Interval + TimeSpan.FromMilliseconds(10))
                diff1Count++;
        }
        
        Assert.That(count1, Is.EqualTo(1));
        Assert.That(count2, Is.EqualTo(2));
        Assert.That(count3, Is.EqualTo(3));
        Assert.That(count4, Is.EqualTo(3));
        Assert.That(count5, Is.EqualTo(3));
        Assert.That(count6, Is.EqualTo(3));
        Assert.That(count7, Is.EqualTo(3));
        Assert.That(count8, Is.EqualTo(3));
        Assert.That(count9, Is.EqualTo(3));
        Assert.That(count10, Is.EqualTo(3));
        Assert.That(count11, Is.EqualTo(3));
        Assert.That(count12, Is.EqualTo(3));
        Assert.That(count13, Is.EqualTo(3));
        Assert.That(count14, Is.EqualTo(0));

        Assert.That(data.Count, Is.EqualTo(6));
        Assert.That(firstItem1.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(firstItem2.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(firstItem3.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(firstItem4.Item1, Is.LessThan(settings.Interval + TimeSpan.FromMilliseconds(20)));
        Assert.That(diff1Count, Is.GreaterThanOrEqualTo(2));
    }
}
