using System.CommandLine;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Industrious.Starter
{
	internal static class Program
	{
		// You'll want to change these if you aren't me
		private const String CompanyName = "Industrious One";

		// Name of file used to store last-run configuration about the solution
		private const String ConfigFileName = "Starter.config.json";

		// All the steps to make an Industrious-worthy solution
		private static readonly Action<Configuration>[] Versions = new Action<Configuration>[] {
			CreateSupportFiles,
			CreateSolution,
			CreateMacOsProject
		};

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Program entry point.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		public static Int32 Main(String[] args)
		{
			// Command arguments; reused across commands
			var slnNameArgument = new Argument<String>("sln_name", "name of the solution to be generated");

			// Command options; reused across commands
			var titleOption =
				new Option<String?>(new[] { "-t", "--title" }, "application title; defaults to solution name");

			// New solution command
			var newCommand = new Command("new", "Generate a new solution") {
				slnNameArgument,
				titleOption
			};

			newCommand.SetHandler(Program.OnNew, slnNameArgument, titleOption);

			// Put it all together; make it go brr
			var rootCommand = new RootCommand("Generate an Industrious standard .NET solution");

			rootCommand.AddCommand(newCommand);

			return rootCommand.Invoke(args);
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Generate a new solution.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void OnNew(String name, String? title)
		{
			var cfg = new Configuration(name, title);
			ApplyUpdates(cfg);
			cfg.Save(ConfigFileName);
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Run the update steps, starting from a specific version.
		/// </summary>
		/// <param name="cfg">
		///  The current solution's configuration, which includes its current version.
		/// </param>
		////////////////////////////////////////////////////////////////////////////////
		private static void ApplyUpdates(Configuration cfg)
		{
			for (var i = cfg.Version; i < Versions.Length; ++i)
				Versions[i].Invoke(cfg);
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create the supporting configuration files required by all projects.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateSupportFiles(Configuration cfg)
		{
			Console.WriteLine("Creating support files");
			File.WriteAllText(".editorconfig", ReadResource(".editorconfig"));
			File.WriteAllText(".gitattributes", ReadResource(".gitattributes"));
			File.WriteAllText(".gitignore", ReadResource(".gitignore"));
			File.WriteAllText("LICENSE.txt", ReadResource("LICENSE.txt")
				.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString())
				.Replace("{CompanyName}", CompanyName));
			File.WriteAllText("README.md", ReadResource("README.md")
				.Replace("{Name}", cfg.Name));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create an empty Visual Studio solution.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateSolution(Configuration cfg)
		{
			Console.WriteLine("Creating solution");
			RunCommand("dotnet", $"new sln --name {cfg.Name} --force");

			var contents = File.ReadAllText($"{cfg.Name}.sln");
			contents = Regex.Replace(contents,
				"^Global",
				String.Join(
					"\r\n",
					"Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Support Files\", \"Support Files\", \"{6B0FD701-B9D2-44C1-BD08-C9E8AE7318DE}\"",
					"\tProjectSection(SolutionItems) = preProject",
					"\t\t.editorconfig = .editorconfig",
					"\t\t.gitattributes = .gitattributes",
					"\t\t.gitignore = .gitignore",
					"\t\tLICENSE.txt = LICENSE.txt",
					"\t\tREADME.md = README.md",
					"\tEndProjectSection",
					"EndProject",
					"Global"),
				RegexOptions.Multiline);

			File.WriteAllText($"{cfg.Name}.sln", contents);
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create the macOS project and add it to the solution.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateMacOsProject(Configuration cfg)
		{
			RunCommand("dotnet", $"new macos --output {cfg.Name}.macOS --force");
			RunCommand("dotnet", $"sln add {cfg.Name}.macOS/{cfg.Name}.macOS.csproj");
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Read text file embedded resource.
		/// </summary>
		/// <param name="resourceName">
		///	 The name of a resource located in this assembly's `Resources` folder.
		/// </param>
		/// <returns>
		///  The contents of the specified resource.
		/// </returns>
		/// <exception cref="InvalidProgramException">
		///  The specified resource could not be found.
		/// </exception>
		////////////////////////////////////////////////////////////////////////////////
		private static String ReadResource(String resourceName)
		{
			using var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("Industrious.Starter.Resources." + resourceName);

			if (stream == null)
				throw new InvalidProgramException($"Missing resource '{resourceName}'");

			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Run an external command.
		/// </summary>
		/// <remarks>
		///  If the command succeeded, this method returns normally. If the command
		///  fails, the contents of `stderr` are echoed to the console, the application
		///  exits, returning the failed error code.
		/// </remarks>
		////////////////////////////////////////////////////////////////////////////////
		private static void RunCommand(String program, String arguments)
		{
			var process = new Process();

			process.StartInfo = new ProcessStartInfo() {
				FileName = program,
				Arguments = arguments,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			process.Start();
			process.WaitForExit();

			if (process.ExitCode == 0) return;

			Console.WriteLine(process.StandardError.ReadToEnd());
			Environment.Exit(process.ExitCode);
		}
	}
}
