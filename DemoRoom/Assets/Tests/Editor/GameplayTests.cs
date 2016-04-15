using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace GameplayTests
{
	[TestFixture]
	[Category("Spawning")]
	internal class SpawnTests
	{
		[Test]
		[Category("Doop")]
		public void ExceptionTest()
		{
			throw new Exception("Exception throwing test");			
		}
	}
}
