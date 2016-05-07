using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("BallResetTests")]
    internal class BallResetTests
    {

        private BallReset setup()
        {
            GameObject RedSideCenter = new GameObject("RSC");
            RedSideCenter.transform.position = new Vector3(1, 1, 1);
            GameObject RedSideLeft = new GameObject("RSL");
            RedSideLeft.transform.position = new Vector3(2, 2, 2);
            GameObject RedSideRight = new GameObject("RSR");
            RedSideRight.transform.position = new Vector3(3, 3, 3);
            GameObject BlueSideCenter = new GameObject("BSC");
            BlueSideCenter.transform.position = new Vector3(-1, -1, -1);
            GameObject BlueSideLeft = new GameObject("BSL");
            BlueSideLeft.transform.position = new Vector3(-2, -2, -2);
            GameObject BlueSideRight = new GameObject("BSR");
            BlueSideRight.transform.position = new Vector3(-3, -3, -3);

            GameObject Ball = new GameObject("Ball");
            Ball.AddComponent<Possession>();
            Ball.AddComponent<Rigidbody>();
            Ball.transform.position = Vector3.zero;

            BallReset BR = new BallReset();
            BR.Ball = Ball;
            BR.RedSideCenter = RedSideCenter;
            BR.RedSideLeft = RedSideLeft;
            BR.RedSideRight = RedSideRight;
            BR.BlueSideCenter = BlueSideCenter;
            BR.BlueSideLeft = BlueSideLeft;
            BR.BlueSideRight = BlueSideRight;
            return BR;
        }

        [Test]
        public void ballStopsAllMotionTest()
        {
            //Pre Test setup
            BallReset BR = setup();
            Rigidbody ballRB = BR.Ball.GetComponent<Rigidbody>();
            Vector3 testV = new Vector3(10, 10, 10);
            ballRB.velocity = testV;
            ballRB.angularVelocity = testV;
            ballRB.useGravity = false;

            //Pre Stop
            Assert.AreEqual(testV, ballRB.velocity);
            Assert.AreEqual(testV, ballRB.angularVelocity);
            Assert.False(ballRB.useGravity);

            BR.placeBallRSC();

            //PostStop
            Vector3 zero = new Vector3(0, 0, 0);
            Assert.AreEqual(zero, ballRB.velocity);
            Assert.AreEqual(zero, ballRB.angularVelocity);
            Assert.True(ballRB.useGravity);
        }

        [Test]
        public void placeBallRSCTest()
        {
            //Setup
            BallReset BR = setup();
            GameObject ball = BR.Ball;

            Assert.AreEqual(new Vector3(0,0,0), ball.transform.position);

            BR.placeBallRSC();

            Assert.AreEqual(BR.RedSideCenter.transform.position, ball.transform.position);
        }

        [Test]
        public void placeBallRSLTest()
        {
            //Setup
            BallReset BR = setup();
            GameObject ball = BR.Ball;

            Assert.AreEqual(new Vector3(0, 0, 0), ball.transform.position);

            BR.placeBallRSL();

            Assert.AreEqual(BR.RedSideLeft.transform.position, ball.transform.position);
        }

        [Test]
        public void placeBallRSRTest()
        {
            //Setup
            BallReset BR = setup();
            GameObject ball = BR.Ball;

            Assert.AreEqual(new Vector3(0, 0, 0), ball.transform.position);

            BR.placeBallRSR();

            Assert.AreEqual(BR.RedSideRight.transform.position, ball.transform.position);
        }

        [Test]
        public void placeBallBSCTest()
        {
            //Setup
            BallReset BR = setup();
            GameObject ball = BR.Ball;

            Assert.AreEqual(new Vector3(0, 0, 0), ball.transform.position);

            BR.placeBallBSC();

            Assert.AreEqual(BR.BlueSideCenter.transform.position, ball.transform.position);
        }

        [Test]
        public void placeBallBSLTest()
        {
            //Setup
            BallReset BR = setup();
            GameObject ball = BR.Ball;

            Assert.AreEqual(new Vector3(0, 0, 0), ball.transform.position);

            BR.placeBallBSL();

            Assert.AreEqual(BR.BlueSideLeft.transform.position, ball.transform.position);
        }

        [Test]
        public void placeBallBSRTest()
        {
            //Setup
            BallReset BR = setup();
            GameObject ball = BR.Ball;

            Assert.AreEqual(new Vector3(0, 0, 0), ball.transform.position);

            BR.placeBallBSR();

            Assert.AreEqual(BR.BlueSideRight.transform.position, ball.transform.position);
        }
    }
}
