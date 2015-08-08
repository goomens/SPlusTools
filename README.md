# SPlusTools
Tools for interfacing with Syllabus+ from .NET.

Main ingredients:
- UvA.SPlusTools.Data.Entities: a set of strongly-typed classes for interacting directory with a Syllabus+ image via COM
- UvA.SPlusTools.Data.QueryTool: a tool for retrieving data via OLEDB

Examples:
- UvA.SPlusTools.Data.Tasks: a set of specific tasks that use the COM interface to do batch actions against Syllabus+
- UvA.SPlusTools.Data.Booking: a tool to get room availability using OLEDB queries
