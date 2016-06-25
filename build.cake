
var target = Argument("target", "default");
var npi = EnvironmentVariable("npi");

Task("push")
    .IsDependentOn("pack")
    .Description("Push nuget")
    .Does(() => {
        var nupkg = new DirectoryInfo("./nuget").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
    });

Task("build")
    .Description("Build")
    .Does(() => {
        DotNetBuild("./Cake.SimpleHTTPServer.sln", settings =>
            settings.SetConfiguration("Release")
            //.SetVerbosity(Core.Diagnostics.Verbosity.Minimal)
            .WithTarget("Build")
            .WithProperty("TreatWarningsAsErrors","true"));
    });

Task("pack")
    .Description("Create pack")
    .IsDependentOn("build")
    .Does(() => {
        Func<string,string> getFullName = (file) => {
            var fullName = new DirectoryInfo("./Cake.SimpleHTTPServer/bin/Release")
                .GetFiles()
                .Where(x => x.Name == file)
                .First().FullName;
            return fullName;
        };

        CleanDirectory("./nuget");
        var full = getFullName("Cake.SimpleHTTPServer.dll");
        //var mime = getFullName("MimeTypeMap.dll");
        var version = ParseAssemblyInfo("./Cake.SimpleHTTPServer/Properties/AssemblyInfo.cs").AssemblyVersion;
        var settings   = new NuGetPackSettings {
                        //ToolPath                = "./tools/nuget.exe",
                        Id                      = "Cake.SimpleHTTPServer",
                        Version                 = version,
                        Title                   = "Cake.SimpleHTTPServer",
                        Authors                 = new[] {"wk"},
                        Owners                  = new[] {"wk"},
                        Description             = "Cake.SimpleHTTPServer",
                        //NoDefaultExcludes       = true,
                        Summary                 = "SimpleHTTPServer file change",
                        ProjectUrl              = new Uri("https://github.com/wk-j/cake-server"),
                        IconUrl                 = new Uri("https://github.com/wk-j/cake-server"),
                        LicenseUrl              = new Uri("https://github.com/wk-j/cake-server"),
                        Copyright               = "MIT",
                        ReleaseNotes            = new [] { "New version"},
                        Tags                    = new [] {"Cake", "SimpleHTTPServer" },
                        RequireLicenseAcceptance= false,
                        Symbols                 = false,
                        NoPackageAnalysis       = true,
                        Files                   = new [] {
                                                             new NuSpecContent { Source = full, Target = "bin/net45" },
                                                             //new NuSpecContent { Source = mime, Target = "bin/net45" },
                                                          },
                        BasePath                = "./",
                        OutputDirectory         = "./nuget"
                    };
        NuGetPack(settings);
    });

RunTarget(target);
