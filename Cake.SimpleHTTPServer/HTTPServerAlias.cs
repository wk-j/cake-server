using System;
using Cake.Core;
using Cake.Core.Annotations;
using System.IO;

namespace Cake.SimpleHTTPServer {

	/// <summary>
	/// Contains functionality to start static server.
	/// </summary>
	[CakeAliasCategory("HTTPServer")]
	public static class HTTPServerAlias {

		/// <summary>
		/// Start server that is specified by port.
		/// </summary>
		/// <example>
		/// HTTPServer(9000);
		/// </example>
		/// <param name="context"></param>
		/// <param name="port"></param>
		[CakeMethodAlias]
		public static void HTTPServer(this ICakeContext context, int port) {
			var current = new DirectoryInfo("./").FullName;
			HTTPServer(context, new HTTPServerSettings { Path = current, Port = port });
		}


		/// <summary>
		/// Start server that is specified by settings.
		/// </summary>
		/// <example>
		/// var settings = new HTTPServerSettings { Path = "./Web", Port = 9000 };
		/// HTTPServer(settings);
		/// </example>
		/// <param name="context"></param>
		/// <param name="settings"></param>
		[CakeMethodAlias]
		public static void HTTPServer(this ICakeContext context, HTTPServerSettings settings) {
			var server = new SimpleHTTPServer(settings.Path, settings.Port);
			server.Start();

			//var server = new MultiThreadHttpServer(30);
			//server.Start(settings.Path, settings.Port);
		}

	}
}

