using System.CommandLine;

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
			Program.CreateEditorConfig,
			Program.CreateGitAttributes,
			Program.CreateGitIgnore,
			Program.CreateLicense,
			Program.CreateReadme
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
			File.WriteAllText(".editorconfig", String.Join(
				Environment.NewLine,
				"root = true",
				"",
				"[*]",
				"charset = utf-8",
				"end_of_line = lf",
				"indent_size = 4",
				"indent_style = tab",
				"insert_final_newline = true",
				"trim_trailing_whitespace = true",
				"",
				"[*.cs]",
				"# imports",
				"dotnet_sort_system_directives_first = true",
				"dotnet_separate_import_directive_groups = true",
				"",
				"# indentation",
				"csharp_indent_switch_labels = false",
				"csharp_new_line_before_open_brace = methods, properties, control_blocks, types",
				"",
				"# framework type aliasing",
				"dotnet_style_predefined_type_for_locals_parameters_members = false",
				"dotnet_style_predefined_type_for_member_access = false",
				""));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a .gitattributes file, with LFS entries for common binary types.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateGitAttributes(Configuration _)
		{
			Console.WriteLine("Creating .gitattributes");
			File.WriteAllText(".gitattributes", String.Join(
				Environment.NewLine,
				"* text=auto",
				"",
				"# Visual Studio files",
				"*.csproj  text eol=crlf",
				"*.sln     text eol=crlf",
				"",
				"# Binary files",
				"*.png filter=lfs diff=lfs merge=lfs -text",
				""));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a .gitignore file, set up for .NET projects.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateGitIgnore(Configuration _)
		{
			Console.WriteLine("Creating .gitignore");
			File.WriteAllText(".gitignore", String.Join(
				Environment.NewLine,
				"# Build files",
				"[Bb]in/",
				"[Oo]bj/",
				"",
				"# macOS files",
				".DS_Store",
				""));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Add an MIT-style license.
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateLicense(Configuration _)
		{
			Console.WriteLine("Creating LICENSE.txt");
			File.WriteAllText("LICENSE.txt", String.Join(
				Environment.NewLine,
				$"Copyright {DateTime.Now.Year} {CompanyName}",
				"",
				"Permission is hereby granted, free of charge, to any person obtaining a copy",
				"of this software and associated documentation files (the \"Software\"), to deal",
				"in the Software without restriction, including without limitation the rights",
				"to use, copy, modify, merge, publish, distribute, sublicense, and/or sell",
				"copies of the Software, and to permit persons to whom the Software is furnished",
				"to do so, subject to the following conditions:",
				"",
				"The above copyright notice and this permission notice shall be included in all",
				"copies or substantial portions of the Software.",
				"",
				"THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR",
				"IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS",
				"FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR",
				"COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN",
				"AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION",
				"WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.",
				""));
		}

		////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a placeholder README.md
		/// </summary>
		////////////////////////////////////////////////////////////////////////////////
		private static void CreateReadme(Configuration cfg)
		{
			Console.WriteLine("Creating README.md");
			File.WriteAllText("README.md", String.Join(
				Environment.NewLine,
				$"# {cfg.Name}",
				""));
		}
	}
}
