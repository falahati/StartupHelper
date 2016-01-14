						StartupHelper
		A simple class to manage your program startup

===============================================================

USAGE
	Simply create an instance of the StartupManager and keeps
	it as a static property/field of the Program.cs file for
	easier accessing. Then use the StartupManager.IsRegistered
	property and StartupManager.Register() and 
	StartupManager.Unregister() methods to add or remove your
	program from the startup list.

CODE SAMPLE:
	internal class Program
	{
		public static StartupManager StartupController = 
			new StartupManager("SAMPLE PROGRAM", 
								RegistrationScope.Local);