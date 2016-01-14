# StartupHelper
A .Net library to add or remove your program to the startup list as well as detecting the startup session. Supporting Windows XP+ with and without administrator rights.

## Where to Find
This library is available as a NuGet package at [nuget.org](https://www.nuget.org/packages/StartupHelper/).


## How to Use
First you need to create a `StartupManager` object for your program. I recomment doing so by defining a new public field or property in the `Program` class of your application.
```C#
  public static StartupManager Startup = 
      new StartupManager("SAMPLE PROGRAM", RegistrationScope.Local);
```

You can also specify if your program needs administrative rights, or if you prefer the registry method to register the rule as well as other settings using other constractor overloads.
Then, using this object, you can query your application startup status, register and unregister your application for auto start and detect if the current session started as startup.

## Primary Members
* `StartupManager.IsStartedUp` This property indicates if the current session started automatically
* `StartupManager.Register()` Using this method you can register your program for auto start. You can also specify required arguments to be send to your application.
* `StartupManager.Unregister()` Unregisters the startup rule.

## Other Members
* `StartupManager.CommandLineArguments` Returns the arguments used to start the program. Except the auto startup indicator argument, if presented
* `StartupManager.WorkingDirectory` Can be used to set or get the expected working directory to be used for registering the rule
