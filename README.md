blanket.net
===========

A simple API wrapper for .NET

Base on the blanket library for Ruby (https://github.com/inf0rmer/blanket), this library provides a simple wrapper for APIs in .NET.

## Usage

    var wrapper = Wrapper.Wrap("https://api.github.com");
    wrapper.Users("RCBDev").Repos.Get(); // Gets from the url https://api.github.com/Users/RCBDev/Repos

## Features

Currently only fetching of the API result as a string is supported. A library such as JSON.NET can then be used to convert the result into the desired class.

The generated URL can also be retrieved via the `URL` property.

## Future Features

* Support for converting the result of the API call to a given type
* Support for custom extensions to the end of the URL (e.g. if ".json" is required)
* Support for custom headers (both global under the `Wrapper.Wrap` call or locally under the `Get` call)
* Support for posting data to an API
* Support for oData query construction
