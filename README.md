GTFS Feed Parser
================

.NET standard implementation of a General Transit Feed Specification (GTFS) feed parser. (see https://developers.google.com/transit/gtfs/reference)

![Build status](http://build.osmsharp.com/app/rest/builds/buildType:(id:Itinero_Gtfs)/statusIcon)
[![Visit our website](https://img.shields.io/badge/website-itinero.tech-020031.svg) ](http://www.itinero.tech/)
[![MIT licensed](https://img.shields.io/badge/license-GPLv2-blue.svg)](https://github.com/itinero/GTFS/blob/develop/LICENSE)

[![NuGet](https://img.shields.io/nuget/v/GTFS.svg?style=flat)](http://www.nuget.org/packages/GTFS)  

### Getting started

```
// create the reader.
var reader = new GTFSReader<GTFSFeed>();
var feed = reader.Read("/path/to/feed");

// read archive.
feed = reader.Read("/path/to/archive.zip");

// write feed to folder.
var writer = new GTFSWriter<GTFSFeed>();
writer.Write(feed, "/path/to/output");
```

### Install GTFS

    PM> Install-Package GTFS
