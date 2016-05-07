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
        public void CheckCoinflipExceptedStartsAsOneTest()
        {
            CoinFlip CF = new CoinFlip();
            
            //Check origonal Value
            Assert.AreEqual(1, CF.getExpected());
       }

        [Test]
        public void setExpectedToOneTest()
        {
            CoinFlip CF = new CoinFlip();

            CF.setExpected(1);
            Assert.AreEqual(1, CF.getExpected());
        }

        [Test]
        public void setExpectedToZeroTest()
        {
            CoinFlip CF = new CoinFlip();

            CF.setExpected(0);
            Assert.AreEqual(0, CF.getExpected());
        }

        [Test]
        public void setExpectedToWrongValuesTest()
        {
            CoinFlip CF = new CoinFlip();

            //If the coin flip is set to a number other than 1 or 0 it will do nothing
            CF.setExpected(2);
            Assert.AreNotEqual(2, CF.getExpected());
            Assert.AreEqual(1, CF.getExpected());

            CF.setExpected(-1);
            Assert.AreNotEqual(-1, CF.getExpected());
            Assert.AreEqual(1, CF.getExpected());
        }

    }
}
