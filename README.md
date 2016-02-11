GTFS Feed Parser
================

.Net/Mono implementation of a General Transit Feed Specification (GTFS) feed parser. (see https://developers.google.com/transit/gtfs/reference)

<img src="http://build.osmsharp.com/app/rest/builds/buildType:(id:OsmSharp_GtfsDevelop)/statusIcon"/>

The implementation is deliberate kept very flexible and customizable because many GTFS feeds out there all have their specific little perks.

### A short example
```
// create the reader.
var reader = new GTFSReader<GTFSFeed>();

// execute the reader.
using (
  var sources = new GTFSDirectorySource(new DirectoryInfo("path/to/feed/directory")))
{
  var feed = reader.Read(sources);
  ...
}
```

### Install GTFS

    PM> Install-Package GTFS
