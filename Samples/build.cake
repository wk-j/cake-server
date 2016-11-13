#r "../Cake.SimpleHTTPServer/bin/Debug/Cake.SimpleHTTPServer.dll"

using Cake.SimpleHTTPServer;

Task("Start").Does(() => {
    HTTPServer(9000);
});

Task("Start-Web") .Does(() => {
    var settings = new HTTPServerSettings {
        Path = "./Web",
        Port = 9000 
    };
    HTTPServer(settings);
});

var target = Argument("target", "default");
RunTarget(target);