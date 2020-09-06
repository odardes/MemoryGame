using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {
        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

    }
}
