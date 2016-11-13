using System;
namespace Cake.SimpleHTTPServer {
	/// <summary>
	/// Specifies a set of values that are used to start server. 
	/// </summary>
	public class HTTPServerSettings {

		/// <summary>
		/// Gets or sets port number.
		/// </summary>
		public int Port { set; get; }

		/// <summary>
		/// Gets or sets root directory of server.
		/// </summary>
		public string Path { set; get; }
	}
}

