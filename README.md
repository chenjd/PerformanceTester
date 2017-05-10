# PerformanceTester
Unity相关操作的性能测试

运行的结果如图（单位ms）：
![QQ截图20170506163532.png](http://upload-images.jianshu.io/upload_images/1372105-3c297bd775b845f2.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/800)
我们可以发现在Unity 5.x版本中，泛型版本的GetComponent<>的性能最好，而GetComponent(string)的性能最差。

做成柱状图可能更加直观：

![QQ截图20170506163819.png](http://upload-images.jianshu.io/upload_images/1372105-ffb5c692cbc57d31.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/500)

接下来，我们来测试一下我们感兴趣的堆内存分配吧。为了更好的观察，我们把测试代码放在Update中执行。

    void Update()
    {
        for(int i = 0; i < testCount; i++)
        {
            GetComponent<TestComp>();
        }
    }

同样每帧执行1000000次的GetComponent<T>方法。打开profiler来观察一下堆内存分配吧：

![QQ截图20170506204741.png](http://upload-images.jianshu.io/upload_images/1372105-8c57570a77b856c6.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/800)
我们可以发现，虽然**频繁调用GetComponent<T>时会造成CPU的开销很大，但是堆内存分配却是0B**。

但是，我和朋友聊天偶尔聊到这个话题时，朋友说有时候会发现每次调用GetComponent<T>时，在profiler中都会增加0.5kb的堆内存分配。不知道各位读者是否有遇到过这个问题，那么是不是说GetComponent方法有时的确会造成GC呢？

答案是否定的。

这是因为朋友是在**Editor中运行**，并且**GetComponent<T>返回Null**的情况下，才会出现堆内存分配的问题。
我们还可以继续我们的测试，这次把TestComp组件从场景中去除，同时把测试次数改为100000。我们在Editor运行测试，可以看到结果如下：
![QQ图片20170506210207.png](http://upload-images.jianshu.io/upload_images/1372105-b49a039abae2ebfa.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1000)
10000次调用GetComponent方法，并且返回为Null时，观察Editor的Profiler，可以发现每一帧都分配了5.6MB的堆内存。

那么如果在移动平台上调用GetComponent方法，并且返回为Null时，是否会造成堆内存分配呢？

这次我们让这个测试跑在一个小米4的手机上，连接profiler观察堆内存分配，结果如图：

![QQ图片20170506212045.png](http://upload-images.jianshu.io/upload_images/1372105-9d3ead73a8a8d24d.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

可以发现，在手机上并不会产生堆内存的分配。

#### Null Check造成的困惑
那么这是为什么呢？其实这种情况只会发生在运行在Editor的情况下，因为Editor会做更多的检测来保证正常运行。而这些堆内存的分配也是这种检测的结果，它会在找不到对应组件时在内部生成警告的字符串，从而造成了堆内存的分配。
> We do this in the editor only. This is why when you call GetComponent() to query for a component that doesn’t exist, that you see a C# memory allocation happening, because we are generating this custom warning string inside the newly allocated fake null object. 

所以各位不必担心使用GetComponent会造成额外的堆内存分配了。同时也可以发现只要不频繁的调用GetComponent方法，CPU的开销还是可以接受的。但是频繁的调用GetComponent会造成显著的CPU的开销的情况下，各位还是对组件进行缓存的好。
