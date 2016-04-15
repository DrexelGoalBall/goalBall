using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace UITests
{
	[TestFixture]
	[Category("MenuLogic")]
	internal class MenuLogic
	{
		[Datapoint]
		public double zero = 0;

		[Test]
		[Category("Doop")]
		public void ExceptionTest()
		{
			throw new Exception("Exception throwing test");			
		}
	}

	public class FakeMenuLogic
	{



	}
}
