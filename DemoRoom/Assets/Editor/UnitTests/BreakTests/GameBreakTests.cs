using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("GameBreakTests")]
    internal class GameBreakTests
    {
        private GameObject setUpReferee()
        {
            GameObject referee = new GameObject("Referee");
            referee.AddComponent<Referee>();
            referee.tag = "Referee";
            return referee;
        }

        [Test]
        public void checkOvertimeBreakTest()
        {
            GameObject referee = setUpReferee();
            OvertimeBreak B = new OvertimeBreak();
            Assert.AreEqual(10, B.GetBreakLength());
        }

        [Test]
        public void checkHalftimeBreakTest()
        {
            GameObject referee = setUpReferee();
            HalftimeBreak B = new HalftimeBreak();
            Assert.AreEqual(10, B.GetBreakLength());
        }

        [Test]
        public void checkGoalBreakTest()
        {
            GameObject referee = setUpReferee();
            GoalBreak B = new GoalBreak();
            Assert.AreEqual(2, B.GetBreakLength());
        }

        [Test]
        public void checkGameStartBreakTest()
        {
            GameObject referee = setUpReferee();
            GameStartBreak B = new GameStartBreak();
            Assert.AreEqual(2, B.GetBreakLength());
        }

        [Test]
        public void checkGameEndBreakTest()
        {
            GameObject referee = setUpReferee();
            GameEndBreak B = new GameEndBreak();
            Assert.AreEqual(43, B.GetBreakLength());
        }

        [Test]
        public void checkFoulBreakTest()
        {
            GameObject referee = setUpReferee();
            FoulBreak B = new FoulBreak();
            Assert.AreEqual(2, B.GetBreakLength());
        }
    }
}
