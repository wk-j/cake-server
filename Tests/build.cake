#addin "nuget:?package=Cake.SimpleHTTPServer"

var target = Argument("target", "default");

Task("default")
    .Does(() => {
            Console.WriteLine("use './build.sh --target server'");
    });

Task("server")
    .Does(() => {
        var settings = new HTTPServerSettings {
            Path = "./",
            Port = 8080
        };
        HTTPServer(settings);
    });

RunTarget(target);