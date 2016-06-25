using NUnit.Framework;
using System;

namespace Cake.SimpleHTTPServer.Tests {
	[TestFixture()]
	public class Test {
		[Test()]
		public void TestCase() {
			var server = new SimpleHTTPServer("./", 8000);
			server.Start();
		}
	}
}

