using System;

namespace Cake.SimpleHTTPServer.Console {
	class MainClass {
		public static void Main(string[] args) {
			var path = "/Users/wk/Source/project/practika/sale-tracking-admin";
			var settings = new HTTPServerSettings {
				Port = 8000,
				Path = path
			};
			var server = new SimpleHTTPServer(settings.Path, settings.Port);
			server.Start();

			while (System.Console.ReadLine() != "q") {}
		}
	}
}
