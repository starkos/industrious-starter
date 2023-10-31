using System.CommandLine;
using System.Reflection;

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
			CreateEditorConfig,
			CreateGitAttributes,
			CreateGitIgnore,
			CreateLicense,
			CreateReadme,
			CreateSolution
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
		///  Create an .editorconfig to apply my preferred settings.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateEditorConfig(Configuration _)
		{
			Console.WriteLine("Creating .editorconfig");
			File.WriteAllText(".editorconfig", ReadResource("EditorConfig.txt"));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a .gitattributes file, with LFS entries for common binary types.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateGitAttributes(Configuration _)
		{
			Console.WriteLine("Creating .gitattributes");
			File.WriteAllText(".gitattributes", ReadResource("GitAttributes.txt"));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a .gitignore file, set up for .NET projects.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateGitIgnore(Configuration _)
		{
			Console.WriteLine("Creating .gitignore");
			File.WriteAllText(".gitignore", ReadResource("GitIgnore.txt"));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Add an MIT-style license.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateLicense(Configuration _)
		{
			Console.WriteLine("Creating LICENSE.txt");
			File.WriteAllText("LICENSE.txt", ReadResource("LICENSE.txt")
				.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString())
				.Replace("{CompanyName}", CompanyName));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a placeholder README.md
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateReadme(Configuration cfg)
		{
			Console.WriteLine("Creating README.md");
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
			Console.WriteLine("Creating empty solution");
			File.WriteAllText(cfg.Name + ".sln", ReadResource("Empty.sln"));
		}


		private static String ReadResource(String resourceName)
		{
			using var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("Industrious.Starter.Resources." + resourceName)!;
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
	}
}
