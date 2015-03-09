# DateRanger
Date and Date Range handling for .NET - Written in C#

## Quick Start

Install the package from Nuget:

```
PM> Install-Package DateRanger
```

### Usage
```c#
// Constructor
var dr = new DateRange(DateTime.MinValue, DateTime.MaxValue);

// Properties
var sd = dr.StartDate;
// 1/1/0001 12:00:00 AM
var ed = dr.EndDate;
// 12/31/9999 11:59:59 PM
var ts = dr.TimeSpan;
//3652058.23:59:59.9999999

// Given a date, get the day as a DateRange
var day = new DateTime(2015, 3, 9, 3, 30, 0);

var dayRange = DateRange.Day(day);
//StartDate: 3/9/2015 12:00:00 AM 
//EndDate: 3/9/2015 11:59:59 PM 

// Get the month that this day is in.
var monthRange = DateRange.Month(day);
// StartDate: 3/1/2015 12:00:00 AM 
// EndDate: 3/31/2015 11:59:59 PM 

// Get the year that this day is in.
var yearRange = DateRange.Year(day);
// StartDate: 1/1/2015 12:00:00 AM 
// EndDate: 12/31/2015 11:59:59 PM 

// Get the quarter that this day is in.
var quarterRange = DateRange.Quarter(day);
// StartDate: 1/1/2015 12:00:00 AM 
// EndDate: 3/31/2015 11:59:59 PM 

// step through the hours of the day
var enumStartDay = new DateTime(2015, 3, 1);
var dayRange = DateRange.Day(enumStartDay);

var hours = dayRange.EnumerateDateTimes(TimeUnit.Hour);
// 3/1/2015 12:00:00 AM 
// 3/1/2015 1:00:00 AM 
// 3/1/2015 2:00:00 AM 
// 3/1/2015 3:00:00 AM 
// 3/1/2015 4:00:00 AM 
// ...
// 3/1/2015 10:00:00 PM 
// 3/1/2015 11:00:00 PM 

// Get each month between two dates, I find this useful for making dropdown boxes for reports.
var startDate = new DateTime(2014, 4, 1);
var endDate = new DateTime(2015, 3, 1);
var dr = new DateRange(startDate, endDate);
var months = dr.EnumerateDateTimes(TimeUnit.Month);

// 4/1/2014 12:00:00 AM 
// 5/1/2014 12:00:00 AM 
// 6/1/2014 12:00:00 AM 
// ...
// 2/1/2015 12:00:00 AM 
// 3/1/2015 12:00:00 AM 

// Check if a DateRange contains a date:
var startDate = new DateTime(2014, 4, 1);
var endDate = new DateTime(2015, 3, 1);
var dr = new DateRange(startDate, endDate);

dr.Contains(new DateTime(2014, 12, 1));
// True

// Intersection
var maxDateRange = new DateRange(DateTime.MinValue, DateTime.MaxValue);

var startDate = new DateTime(2014, 4, 1);
var endDate = new DateTime(2015, 3, 1);
var dr = new DateRange(startDate, endDate);

maxDateRange.Intersects(dr);
// True
dr.Intersects(maxDateRange);
// True

var startDate0 = new DateTime(2013, 4, 1);
var endDate0 = new DateTime(2014, 3, 1);
var dr0 = new DateRange(startDate0, endDate0);

dr.Intersects(dr0);
// False
dr0.Intersects(dr);
// False
```
