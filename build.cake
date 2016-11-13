
var target = Argument("target", "default");
var npi = EnvironmentVariable("npi");

Task("Publish-Nuget")
    .IsDependentOn("Pack-Nuget")
    .Does(() => {
        var nupkg = new DirectoryInfo("./nuget").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
    });

Task("Build")
    .Does(() => {
        DotNetBuild("./Cake.SimpleHTTPServer.sln", settings =>
            settings.SetConfiguration("Release")
            //.SetVerbosity(Core.Diagnostics.Verbosity.Minimal)
            .WithTarget("Build")
            .WithProperty("TreatWarningsAsErrors","true"));
    });

Task("Pack-Nuget")
    .IsDependentOn("Build")
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
                        ProjectUrl              = new Uri("https://github.com/cake-addin/cake-server"),
                        IconUrl                 = new Uri("https://github.com/cake-addin/cake-server"),
                        LicenseUrl              = new Uri("https://github.com/cake-addin/cake-server"),
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
