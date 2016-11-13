## Cake.SimpleHTTPServer

https://gist.github.com/aksakalli/9191056

## Notes

- This addin is not working under MONO.

## Install

```
#addin "nuget:?package=Cake.SimpleHTTPServer"
```

## Start server in current directory

```csharp
Task("Start").Does(() => {
    HTTPServer(9000);
});
```

## Specify directory 

```csharp
Task("Start-Web")
    .Does(() => {
        var settings = new HTTPServerSettings {
            Path = "./Web",
            Port = 9000
        };
        HTTPServer(settings);
    });
```