using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.IO;
using MimeTypes;

namespace Cake.SimpleHTTPServer {



	public class SimpleHTTPServer {
		private readonly string[] _indexFiles = {
		"index.html",
		"index.htm",
		"default.html",
		"default.htm" };

		private Thread _serverThread;
		private string _rootDirectory;
		private HttpListener _listener;
		private int _port;

		public int Port {
			get { return _port; }
		}

		public SimpleHTTPServer(string path, int port) {
			this._rootDirectory = path;
			this._port = port;
		}

		public void Start() {
			this.Initialize(_rootDirectory, _port);
		}

		public void Stop() {
			_serverThread.Abort();
			_listener.Stop();
		}

		private void Listen() {
			_listener = new HttpListener();
			_listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
			_listener.Start();
			while (true) {
				try {
					var context = _listener.GetContext();
					Process(context);
					//ThreadPool.QueueUserWorkItem(Process, _listener.GetContext());
				} catch(Exception ex)  {
					Console.Error.WriteLine(ex.Message);
				}
			}
		}

		private void Process(object obj) {
			var context = obj as HttpListenerContext;
			string filename = context.Request.Url.AbsolutePath;
			Console.WriteLine(filename);
			filename = filename.Substring(1);

			if (string.IsNullOrEmpty(filename)) {
				foreach (string indexFile in _indexFiles) {
					if (File.Exists(Path.Combine(_rootDirectory, indexFile))) {
						filename = indexFile;
						break;
					}
				}
			}

			filename = Path.Combine(_rootDirectory, filename);

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
				} catch(Exception ex) {
					Console.Error.WriteLine(ex.Message);
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				}
			} else {
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			context.Response.OutputStream.Close();
		}

		private void Initialize(string path, int port) {
			_serverThread = new Thread(this.Listen);
			_serverThread.Start();

			//while (Console.ReadLine() != "q") { }
		}
	}
}

