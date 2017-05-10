using UnityEngine;
using System.Collections;

public class TestWithGetComponent : MonoBehaviour {

    private int testCount = 1000000;//定义测试的次数

    void Start () {

        Transform transformTest;

        using (new YiWatch("GetComponent<>", testCount))
        {
            for(int i = 0; i < testCount; i++)
            {
                GetComponent<TestComp>();
            }
        }

        using (new YiWatch("GetComponent(typeof(T))", testCount))
        {
            for(int i = 0; i < testCount; i++)
            {
                GetComponent(typeof(TestComp));
            }
        }

        using (new YiWatch("GetComponent(string)", testCount))
        {
            for(int i = 0; i < testCount; i++)
            {
                GetComponent("TestComp");
            }
        }

        using (new YiWatch("transform", testCount))
        {
            for(int i = 0; i < testCount; i++)
            {
                transformTest = this.transform;
            }
        }
    }

    void Update()
    {
        //for(int i = 0; i < testCount; i++)
        //{
        //    GetComponent<TestComp>();
        //}
    }
	
}
