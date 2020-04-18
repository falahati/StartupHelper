# Startup Helper Class Library
[![](https://img.shields.io/github/license/falahati/StartupHelper.svg?style=flat-square)](https://github.com/falahati/StartupHelper/blob/master/LICENSE)
[![](https://img.shields.io/github/commit-activity/y/falahati/StartupHelper.svg?style=flat-square)](https://github.com/falahati/StartupHelper/commits/master)
[![](https://img.shields.io/github/issues/falahati/StartupHelper.svg?style=flat-square)](https://github.com/falahati/StartupHelper/issues)
A .Net library to add or remove your program to the startup list as well as detecting the startup session. Supporting Windows XP+ with and without administrator rights.

## How to get
[![](https://img.shields.io/nuget/dt/StartupHelper.svg?style=flat-square)](https://www.nuget.org/packages/StartupHelper)
[![](https://img.shields.io/nuget/v/StartupHelper.svg?style=flat-square)](https://www.nuget.org/packages/StartupHelper)

This library is available as a NuGet package at [nuget.org](https://www.nuget.org/packages/StartupHelper/).

## Help me fund my own Death Star

[![](https://img.shields.io/badge/crypto-CoinPayments-8a00a3.svg?style=flat-square)](https://www.coinpayments.net/index.php?cmd=_donate&reset=1&merchant=820707aded07845511b841f9c4c335cd&item_name=Donate&currency=USD&amountf=20.00000000&allow_amount=1&want_shipping=0&allow_extra=1)
[![](https://img.shields.io/badge/shetab-ZarinPal-8a00a3.svg?style=flat-square)](https://zarinp.al/@falahati)
[![](https://img.shields.io/badge/usd-Paypal-8a00a3.svg?style=flat-square)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ramin.graphix@gmail.com&lc=US&item_name=Donate&no_note=0&cn=&curency_code=USD&bn=PP-DonationsBF:btn_donateCC_LG.gif:NonHosted)

**--OR--**

You can always donate your time by contributing to the project or by introducing it to others.

## How to use
First you need to create a `StartupManager` object for your program. I recomment doing so by defining a new public field or property in the `Program` class of your application.
```C#
  public static StartupManager Startup = 
      new StartupManager("SAMPLE PROGRAM", RegistrationScope.Local);
```

You can also specify if your program needs administrative rights, or if you prefer the registry method to register the rule as well as other settings using other constractor overloads.
Then, using this object, you can query your application startup status, register and unregister your application for auto start and detect if the current session started as startup.

Check the 'UACHelper.Sample' project for basic usage of the class.
![Screenshot](/screenshot.jpg?raw=true "Screenshot")

## Documentation
### [Primary Members]
* `StartupManager.IsStartedUp` This property indicates if the current session started automatically
* `StartupManager.Register()` Using this method you can register your program for auto start. You can also specify required arguments to be send to your application.
* `StartupManager.Unregister()` Unregisters the startup rule.

### [Other Useful Members]
* `StartupManager.CommandLineArguments` Returns the arguments used to start the program. Except the auto startup indicator argument, if presented
* `StartupManager.WorkingDirectory` Can be used to set or get the expected working directory to be used for registering the rule
* `StartupManager.IsElevated` Returns a `Boolean` indicating the current elevation status of the application.

## License
The MIT License (MIT)

Copyright (c) 2016-2020 Soroush

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
