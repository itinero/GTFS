GTFS Feed Parser
================

.Net/Mono implementation of a General Transit Feed Specification (GTFS) feed parser. (see https://developers.google.com/transit/gtfs/reference)

<img src="http://build.osmsharp.com:8080/app/rest/builds/buildType:(id:OsmSharp_GitHubOsmSharpGtfs)/statusIcon"/>

The implementation is deliberate kept very flexible and customizable because many GTFS feeds out there all have there specific little perks.

### A short example
```csharp
// create the reader.
var reader = new GTFSReader<GTFSFeed>();

// execute the reader.
var feed = reader.Read(new GTFSDirectorySource(new DirectoryInfo("path/to/feed/directory")));
```

### Install GTFS

    PM> Install-Package GTFS
