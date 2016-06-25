using NUnit.Framework;
using System;
using System.Threading;

namespace Cake.SimpleHTTPServer.Tests {
	[TestFixture()]
	public class Test {
		[Test()]
		public void TestCase() {
			var path = "/Users/wk/Source/project/practika/sale-tracking-admin";
			path = "./";
			var server = new SimpleHTTPServer(path, 8000);
			server.Start();


		}
	}
}