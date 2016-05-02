using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("CoinFlipTests")]
    internal class CoinFlipTests
    {
        [Test]
        public void setExpectedTests()
        {
            CoinFlip CF = new CoinFlip();
            
            //Check origonal Value
            Assert.AreEqual(1, CF.expected);

            //Change Expected to 1 and check
            CF.setExpected(1);
            Assert.AreEqual(1, CF.expected);

            //Change Expected to 0 and Check
            CF.setExpected(0);
            Assert.AreEqual(0, CF.expected);

            //CHeck that numbers greater than 1 don't work
            CF.setExpected(2);
            Assert.AreNotEqual(2, CF.expected);
        }
    }
}
