blanket.net
===========

A simple API wrapper for .NET

Base on the blanket library for Ruby (https://github.com/inf0rmer/blanket), this library provides a simple wrapper for APIs in .NET.

## Usage

    var wrapper = Wrapper.Wrap("https://api.github.com");
    wrapper.Users("RCBDev").Repos.Get(); // Gets from the url https://api.github.com/Users/RCBDev/Repos
