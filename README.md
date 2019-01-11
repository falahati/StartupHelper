# Startup Helper Class Library
A .Net library to add or remove your program to the startup list as well as detecting the startup session. Supporting Windows XP+ with and without administrator rights.

## WHERE TO FIND
This library is available as a NuGet package at [nuget.org](https://www.nuget.org/packages/StartupHelper/).

## Donation
[<img width="24" height="24" src="http://icons.iconarchive.com/icons/sonya/swarm/256/Coffee-icon.png"/>**Every coffee counts! :)**](https://www.coinpayments.net/index.php?cmd=_donate&reset=1&merchant=820707aded07845511b841f9c4c335cd&item_name=Donate&currency=USD&amountf=10.00000000&allow_amount=1&want_shipping=0&allow_extra=1)

## HOW TO USE
First you need to create a `StartupManager` object for your program. I recomment doing so by defining a new public field or property in the `Program` class of your application.
```C#
  public static StartupManager Startup = 
      new StartupManager("SAMPLE PROGRAM", RegistrationScope.Local);
```

You can also specify if your program needs administrative rights, or if you prefer the registry method to register the rule as well as other settings using other constractor overloads.
Then, using this object, you can query your application startup status, register and unregister your application for auto start and detect if the current session started as startup.

Check the 'UACHelper.Sample' project for basic usage of the class.
![Screenshot](/screenshot.jpg?raw=true "Screenshot")

## MEMBERS
### [Primary Members]
* `StartupManager.IsStartedUp` This property indicates if the current session started automatically
* `StartupManager.Register()` Using this method you can register your program for auto start. You can also specify required arguments to be send to your application.
* `StartupManager.Unregister()` Unregisters the startup rule.

### [Other Useful Members]
* `StartupManager.CommandLineArguments` Returns the arguments used to start the program. Except the auto startup indicator argument, if presented
* `StartupManager.WorkingDirectory` Can be used to set or get the expected working directory to be used for registering the rule
* `StartupManager.IsElevated` Returns a `Boolean` indicating the current elevation status of the application.

## LICENSE
The MIT License (MIT)

Copyright (c) 2016 Soroush

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
