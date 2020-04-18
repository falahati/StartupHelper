using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace StartupHelper
{
    /// <summary>
    ///     A class for managing the startup of the application.
    /// </summary>
    public class StartupManager
    {
        // ReSharper disable once PrivateMembersMustHaveComments
        private static bool? _isFileSystemCaseSensitive;

        /// <summary>
        ///     Simplest form for initializing an instance of the class, with
        ///     providing a <paramref name="name" /> for the program and specifying
        ///     an <paramref name="scope" />.
        /// </summary>
        /// <param name="name">
        ///     A unique name for the rule as an alias for the program
        /// </param>
        /// <param name="scope">
        ///     Scope in which startup rule should be created or managed
        /// </param>
        public StartupManager(string name, RegistrationScope scope)
            : this(name, scope, IsElevated)
        {
        }

        /// <summary>
        ///     Initializing an instance of the class, with providing a
        ///     <paramref name="name" /> for the program, specifying an
        ///     <paramref name="scope" /> and explicitly specifying the dependency on
        ///     administrator privileges.
        /// </summary>
        /// <param name="name">
        ///     A unique name for the rule as an alias for the program
        /// </param>
        /// <param name="scope">
        ///     Scope in which startup rule should be created or managed
        /// </param>
        /// <param name="needsAdminPrivileges">
        ///     Set to True if the program should be executed with administrator's
        ///     rights
        /// </param>
        public StartupManager(string name, RegistrationScope scope, bool needsAdminPrivileges)
            : this(Assembly.GetEntryAssembly()?.Location, name, scope, needsAdminPrivileges)
        {
            FixWorkingDirectory();
        }

        /// <summary>
        ///     Initializing an instance of the class, with providing the filename
        ///     of the starting executable file, a <paramref name="name" /> for the
        ///     program, specifying an <paramref name="scope" /> and explicitly
        ///     specifying the dependency on administrator privileges.
        /// </summary>
        /// <param name="applicationImage">
        ///     The address of the executable file of the application
        /// </param>
        /// <param name="name">
        ///     A unique name for the rule as an alias for the program
        /// </param>
        /// <param name="scope">
        ///     Scope in which startup rule should be created or managed
        /// </param>
        /// <param name="needsAdminPrivileges">
        ///     Set to True if the program should be executed with administrator's
        ///     rights
        /// </param>
        public StartupManager(string applicationImage, string name, RegistrationScope scope, bool needsAdminPrivileges)
            :
                this(applicationImage, name, scope, needsAdminPrivileges, "--startup")
        {
        }


        /// <summary>
        ///     Initializing an instance of the class, with providing the filename
        ///     of the starting executable file, a <paramref name="name" /> for the
        ///     program, specifying an <paramref name="scope" /> and explicitly
        ///     specifying the dependency on administrator privileges.
        /// </summary>
        /// <param name="applicationImage">
        ///     The address of the executable file of the application
        /// </param>
        /// <param name="name">
        ///     A unique name for the rule as an alias for the program
        /// </param>
        /// <param name="scope">
        ///     Scope in which startup rule should be created or managed
        /// </param>
        /// <param name="provider">
        ///     Method that is expected to be used for registering the program
        ///     startup
        /// </param>
        /// <param name="needsAdminPrivileges">
        ///     Set to True if the program should be executed with administrator's
        ///     rights
        /// </param>
        public StartupManager(string applicationImage, string name, RegistrationScope scope, StartupProviders provider,
            bool needsAdminPrivileges)
            :
                this(applicationImage, name, scope, needsAdminPrivileges, provider, "--startup")
        {
        }

        /// <summary>
        ///     Initializing an instance of the class, with providing the filename
        ///     of the starting executable file, a <paramref name="name" /> for the
        ///     program, specifying an <paramref name="scope" /> and explicitly
        ///     specifying the dependency on administrator privileges. Using this
        ///     constructor you can specify a custom string to be used as the
        ///     startup indicator. Make sure that there is no other string similar
        ///     to this string in your expected command line arguments to conflict
        ///     with it.
        /// </summary>
        /// <param name="applicationImage">
        ///     The address of the executable file of the application
        /// </param>
        /// <param name="name">
        ///     A unique name for the rule as an alias for the program
        /// </param>
        /// <param name="scope">
        ///     Scope in which startup rule should be created or managed
        /// </param>
        /// <param name="needsAdminPrivileges">
        ///     Set to True if the program should be executed with administrator's
        ///     rights
        /// </param>
        /// <param name="startupSpecialArgument">
        ///     A special string to send to the program when started and to detect
        ///     the automatic startup
        /// </param>
        /// <exception cref="ArgumentException">Bad argument value.</exception>
        public StartupManager(string applicationImage, string name, RegistrationScope scope, bool needsAdminPrivileges,
            string startupSpecialArgument)
            : this(
                applicationImage, name, scope, needsAdminPrivileges,
                Environment.OSVersion.Version.Major >= 6 &&
                (needsAdminPrivileges || Environment.OSVersion.Version.Minor >= 2)
                    ? StartupProviders.Task
                    : StartupProviders.Registry, startupSpecialArgument)
        {
        }

        /// <summary>
        ///     Initializing an instance of the class, with providing the filename
        ///     of the starting executable file, a <paramref name="name" /> for the
        ///     program, specifying an <paramref name="scope" /> and explicitly
        ///     specifying the dependency on administrator privileges. Using this
        ///     constructor you can specify a custom string to be used as the
        ///     startup indicator. Make sure that there is no other string similar
        ///     to this string in your expected command line arguments to conflict
        ///     with it.
        /// </summary>
        /// <param name="applicationImage">
        ///     The address of the executable file of the application
        /// </param>
        /// <param name="name">
        ///     A unique name for the rule as an alias for the program
        /// </param>
        /// <param name="scope">
        ///     Scope in which startup rule should be created or managed
        /// </param>
        /// <param name="needsAdminPrivileges">
        ///     Set to True if the program should be executed with administrator's
        ///     rights
        /// </param>
        /// <param name="provider">
        ///     Method that is expected to be used for registering the program
        ///     startup
        /// </param>
        /// <param name="startupSpecialArgument">
        ///     A special string to send to the program when started and to detect
        ///     the automatic startup
        /// </param>
        /// <exception cref="ArgumentException">Bad argument value.</exception>
        public StartupManager(string applicationImage, string name, RegistrationScope scope, bool needsAdminPrivileges,
            StartupProviders provider, string startupSpecialArgument)
        {
            if (string.IsNullOrEmpty(applicationImage) || !File.Exists(applicationImage))
            {
                throw new ArgumentException("File doesn't exist.", nameof(applicationImage));
            }
            if (string.IsNullOrEmpty(name.Trim()))
            {
                throw new ArgumentException("Bad name for application.", nameof(name));
            }
            if (string.IsNullOrEmpty(startupSpecialArgument.Trim()) || IsAnyWhitespaceIn(startupSpecialArgument.Trim()))
            {
                throw new ArgumentException("Bad string provided as special argument for startup detection.",
                    nameof(startupSpecialArgument));
            }
            ApplicationImage = applicationImage;
            WorkingDirectory = Path.GetDirectoryName(applicationImage);
            Name = name.Trim();
            NeedsAdministrativePrivileges = needsAdminPrivileges;
            RegistrationScope = scope;
            StartupSpecialArgument = startupSpecialArgument.Trim();
            Provider = provider;
        }

        /// <summary>
        ///     Value of <c>this</c> property shows if the OS's file system is case
        ///     sensitive.
        /// </summary>
        public static bool IsFileSystemCaseSensitive
        {
            get
            {
                if (_isFileSystemCaseSensitive == null)
                {
                    var tempFile = Path.GetTempFileName();
                    _isFileSystemCaseSensitive = !File.Exists(tempFile.ToLower()) || !File.Exists(tempFile.ToUpper());
                    File.Delete(tempFile);
                }
                return _isFileSystemCaseSensitive.Value;
            }
        }

        /// <summary>
        ///     Value of <c>this</c> property shows if the current program executed with
        ///     administrator rights.
        /// </summary>
        public static bool IsElevated
        {
            get
            {
                var currentUser = WindowsIdentity.GetCurrent();
                return new WindowsPrincipal(currentUser).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        ///     A special string to be used as the argument to the program when started to detect auto start sessions
        /// </summary>
        public string StartupSpecialArgument { get; }


        /// <summary>
        ///     Indicates if the registered rule should make sure that program is
        ///     going to be executed with administrator rights
        /// </summary>
        public bool NeedsAdministrativePrivileges { get; }


        /// <summary>
        ///     Indicates the working directory in which the program should be
        ///     executed with
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        ///     Shows the scope in which the rule for auto start is going to be
        ///     registered/removed or modified
        /// </summary>
        public RegistrationScope RegistrationScope { get; }

        /// <summary>
        ///     Address of the executable file
        /// </summary>
        public string ApplicationImage { get; }


        /// <summary>
        ///     A unique name to be used as the rule name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The underlying method that is used by the class to manage the
        ///     startup functionality
        /// </summary>
        public StartupProviders Provider { get; }


        /// <summary>
        ///     Indicates if <c>this</c> session is started as startup
        /// </summary>
        public bool IsStartedUp
            =>
                (Environment.GetCommandLineArgs().Length > 0 &&
                 Environment.GetCommandLineArgs()[0].ToLower().Trim() == StartupSpecialArgument) ||
                (Environment.GetCommandLineArgs().Length > 1 &&
                 Environment.GetCommandLineArgs()[1].ToLower().Trim() == StartupSpecialArgument);

        /// <summary>
        ///     Returns the correct command line arguments used to start <c>this</c>
        ///     session without special startup argument, if presented
        /// </summary>
        public string[] CommandLineArguments
            => Environment.GetCommandLineArgs().Length > 0
                ? SkipFirstElements(Environment.GetCommandLineArgs()
                    , IsStartedUp && Environment.GetCommandLineArgs()[1].ToLower().Trim() == StartupSpecialArgument
                        ? 2
                        : 1)
                : new string[0];

        /// <summary>
        ///     Indicates if there is any active rule by the unique name provided
        /// </summary>
        public bool IsRegistered
        {
            get
            {
                if (Provider == StartupProviders.Task)
                {
                    using (var taskService = new TaskService())
                    {
                        var task = taskService.FindTask(Name, false);
                        if (task != null)
                        {
                            using (task)
                            {
                                return string.IsNullOrEmpty(task.Definition.Principal.UserId.Trim()) ||
                                       (RegistrationScope == RegistrationScope.Local &&
                                        IsCurrentUser(task.Definition.Principal.UserId));
                            }
                        }
                    }
                }
                using (var registryKey = Registry.CurrentUser.OpenSubKey
                    (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (registryKey?.GetValue(Name, null) != null)
                    {
                        if (Provider == StartupProviders.Task)
                        {
                            registryKey.DeleteValue(Name, false);
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                try
                {
                    using (var registryKey = Registry.LocalMachine.OpenSubKey
                        (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", IsElevated))
                    {
                        if (registryKey?.GetValue(Name, null) != null)
                        {
                            if (Provider == StartupProviders.Task && IsElevated)
                            {
                                registryKey.DeleteValue(Name, false);
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (SecurityException)
                {
                    // ignore
                }
                return false;
            }
        }

        /// <summary>
        ///     Removes the rule if exists
        /// </summary>
        /// <returns>
        ///     A value indicating the success of the operation
        /// </returns>
        public bool Unregister()
        {
            if (Provider == StartupProviders.Task)
            {
                using (var taskService = new TaskService())
                {
                    var task = taskService.FindTask(Name, false);
                    if (task != null)
                    {
                        taskService.RootFolder.DeleteTask(Name, false);
                        task.Dispose();
                    }
                }
            }
            if (RegistrationScope == RegistrationScope.Local)
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey
                    (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    registryKey?.DeleteValue(Name, false);
                }
            }
            if (IsElevated)
            {
                using (var registryKey = Registry.LocalMachine.OpenSubKey
                    (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    registryKey?.DeleteValue(Name, false);
                }
            }
            return !IsRegistered;
        }


        /// <summary>
        ///     Creates or replace the existing rule
        /// </summary>
        /// <param name="arguments">
        ///     Special <c>arguments</c> to be sent to the application at startup
        /// </param>
        /// <returns>
        ///     A value indicating the success of the operation
        /// </returns>
        public bool Register(string arguments = null)
        {
            Unregister();
            if (Provider == StartupProviders.Task)
            {
                using (var taskService = new TaskService())
                {
                    using (var newTask = taskService.NewTask())
                    {
                        var taskAction = new ExecAction(ApplicationImage,
                            $"{StartupSpecialArgument} {(arguments ?? string.Empty)}",
                            WorkingDirectory);
                        var taskTrigger = new LogonTrigger();
                        newTask.Settings.DisallowStartIfOnBatteries = false;
                        newTask.Settings.ExecutionTimeLimit = TimeSpan.Zero;
                        if (taskService.HighestSupportedVersion >= new Version(1, 2))
                        {
                            newTask.Settings.AllowDemandStart = false;
                            newTask.Settings.AllowHardTerminate = false;
                            newTask.Settings.Compatibility = TaskCompatibility.V2;
                            newTask.Settings.StartWhenAvailable = true;
                            newTask.Principal.RunLevel = NeedsAdministrativePrivileges
                                ? TaskRunLevel.Highest
                                : TaskRunLevel.LUA;
                            if (RegistrationScope == RegistrationScope.Local)
                            {
                                taskTrigger.UserId = WindowsIdentity.GetCurrent().Name;
                            }
                        }
                        else
                        {
                            newTask.Settings.RunOnlyIfLoggedOn = true;
                        }
                        if (RegistrationScope == RegistrationScope.Global)
                        {
                            newTask.Principal.GroupId =
                                new SecurityIdentifier(
                                    NeedsAdministrativePrivileges
                                        ? WellKnownSidType.BuiltinAdministratorsSid
                                        : WellKnownSidType.BuiltinUsersSid, null).Translate(typeof (NTAccount)).Value;
                        }
                        else
                        {
                            newTask.Principal.LogonType = TaskLogonType.InteractiveToken;
                            newTask.Principal.UserId = WindowsIdentity.GetCurrent().Name;
                        }
                        newTask.Actions.Add(taskAction);
                        newTask.Triggers.Add(taskTrigger);
                        taskService.RootFolder.RegisterTaskDefinition(Name, newTask);
                    }
                }
            }
            else
            {
                using (var registryKey = (RegistrationScope == RegistrationScope.Local
                    ? Registry.CurrentUser.OpenSubKey
                        (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)
                    : Registry.LocalMachine.OpenSubKey
                        (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)))
                {
                    registryKey?.SetValue(Name,
                        $"\"{ApplicationImage}\" {StartupSpecialArgument} {(arguments ?? string.Empty)}");
                }
            }
            return IsRegistered;
        }

        /// <summary>
        ///     Fixes the working directory of the current session if it is not the
        ///     same as the directory in which the executable file resists. Put
        ///     <c>this</c> line in your application's program.cs file. There is no
        ///     need to do so if you use the constructor without specifying the
        ///     address of the executable file.
        /// </summary>
        public void FixWorkingDirectory()
        {
            if (!Path.GetFullPath(Directory.GetCurrentDirectory())
                .Equals(Path.GetFullPath(WorkingDirectory),
                    IsFileSystemCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
            {
                Directory.SetCurrentDirectory(WorkingDirectory);
            }
        }


        // ReSharper disable once PrivateMembersMustHaveComments
        private static bool IsCurrentUser(string username)
        {
            try
            {
                return ((SecurityIdentifier) (new NTAccount(username).Translate(typeof (SecurityIdentifier)))) ==
                       WindowsIdentity.GetCurrent().User;
            }
            catch
            {
                return false;
            }
        }

        // ReSharper disable once PrivateMembersMustHaveComments
        private static T[] SkipFirstElements<T>(T[] array, int count)
        {
            var newArray = new T[array.Length - count];
            Array.Copy(array, count, newArray, 0, newArray.Length);
            return newArray;
        }

        // ReSharper disable once PrivateMembersMustHaveComments
        private static bool IsAnyWhitespaceIn(string str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (char.IsWhiteSpace(str, i))
                    return true;
            }
            return false;
        }
    }
}