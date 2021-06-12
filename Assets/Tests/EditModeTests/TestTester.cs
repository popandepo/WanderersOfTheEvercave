using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestTester
{
    [Test]
    public void TestTesterSimplePasses()
    {
        int one = 1;
        int two = 2;
        int three = 3;
        
        Assert.IsTrue(one+two==three);
    }
}
