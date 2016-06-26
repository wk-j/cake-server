using System;
using Cake.Core;
using Cake.Core.Annotations;
using System.IO;

namespace Cake.SimpleHTTPServer {
	public static class HTTPServerAlias {

		[CakeMethodAlias]
		public static void HTTPServer(this ICakeContext context, int port) {
			var current = new DirectoryInfo("./").FullName;
			HTTPServer(context, new HTTPServerSettings { Path = current, Port = port });
		}


		[CakeMethodAlias]
		public static void HTTPServer(this ICakeContext context, HTTPServerSettings settings) {
			/*
			var server = new SimpleHTTPServer(settings.Path, settings.Port);
			server.Start();
			*/

			var server = new MultiThreadHttpServer(30);
			server.Start(settings.Path, settings.Port);
		}

	}
}

