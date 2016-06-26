using System;
using System.IO;
using System.Net;
using MimeTypes;

namespace Cake.SimpleHTTPServer {
	public class CakeHttpServer {

		private readonly int _port;
		private readonly string _path;

		private readonly string[] _indexFiles = {
					"index.html",
					"index.htm",
					"default.html",
					"default.htm" };

		public CakeHttpServer(string path = "./", int port = 8000) {
			_port = port;
			_path = path;
		}

		public void Start() {
			var server = new MultiThreadHttpServer(100);

			server.ProcessRequest += ProcessRequest;
			server.Start(_path, _port);

		}

		private void ProcessRequest(HttpListenerContext context) {

			string filename = context.Request.Url.AbsolutePath;
			Console.WriteLine(filename);
			filename = filename.Substring(1);

			if (string.IsNullOrEmpty(filename)) {
				foreach (string indexFile in _indexFiles) {
					if (File.Exists(Path.Combine(_path, indexFile))) {
						filename = indexFile;
						break;
					}
				}
			}

			filename = Path.Combine(_path, filename);

			if (File.Exists(filename)) {
				try {
					var input = new FileStream(filename, FileMode.Open);
					var ext = Path.GetExtension(filename);
					string mime = MimeTypeMap.GetMimeType(ext);

					Console.WriteLine(mime);

					context.Response.ContentType = mime;
					context.Response.ContentLength64 = input.Length;
					context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
					context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

					var buffer = new byte[1024 * 16];
					int nbytes;
					while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
						context.Response.OutputStream.Write(buffer, 0, nbytes);
					input.Close();

					context.Response.StatusCode = (int)HttpStatusCode.OK;
					context.Response.OutputStream.Flush();
				} catch (Exception ex) {
					Console.Error.WriteLine(ex.Message);
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				}
			} else {
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			context.Response.OutputStream.Close();
		}
	}
}

