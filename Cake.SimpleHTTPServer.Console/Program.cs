using System;

namespace Cake.SimpleHTTPServer.Console {
	class MainClass {
		public static void Main(string[] args) {
			var path = "/Users/wk/Source/project/practika/sale-tracking-admin";

			//var server = new SimpleHTTPServer(settings.Path, settings.Port);
			//server.Start();

			path = "C:\\wk\\sale-tracking-admin";

			var server = new CakeHttpServer(path, 8000);
			server.Start();

			while (System.Console.ReadLine() != "q") {}
		}
	}
}
