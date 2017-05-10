using System;
using System.Diagnostics;

/// <summary>
/// 简易的计时类
/// </summary>
public class YiWatch : IDisposable
{
    #region 字段

    private string testName;
    private int testCount;
    private Stopwatch watch;

    #endregion


    #region 构造函数

    public YiWatch(string name, int count)
    {
        this.testName = name;

        this.testCount = count > 0 ? count : 1;

        this.watch = Stopwatch.StartNew();
    }

    #endregion


    #region 方法
    public void Dispose()
    {
        this.watch.Stop();

        float totalTime = this.watch.ElapsedMilliseconds;

        UnityEngine.Debug.Log(string.Format("测试名称：{0}   总耗时：{1}   单次耗时：{2}    测试数量：{3}",
            this.testName, totalTime, totalTime / this.testCount, this.testCount));
    }

    #endregion

}
