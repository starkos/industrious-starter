namespace Industrious.Starter.Commands;

public class RenameCommand
{
	private readonly Configuration _configuration;
	private readonly String _oldName;
	private readonly String _newName;


	public RenameCommand (Configuration configuration, String newName)
	{
		_configuration = configuration;
		_oldName = configuration.Name;
		_newName = newName;
	}


	public void Apply ()
	{
		RenameSolution ();
		RenameProject ("Common");
		RenameProject ("Common.Tests");
		RenameProject ("Console");
		RenameProject ("iOS");
		RenameProject ("macOS");
	}


	private void RenameSolution ()
	{
		var oldFileName = $"{_oldName}.sln";
		var newFileName = $"{_newName}.sln";
		Console.WriteLine ($"{oldFileName} -> {newFileName}");

		var contents = File.ReadAllText (oldFileName)
			.Replace ($"\"{_oldName}.", $"\"{_newName}.")
			.Replace ($"\\{_oldName}.", $"\\{_newName}.");

		File.WriteAllText (newFileName, contents);
		File.Delete (oldFileName);
	}


	private void RenameProject (String suffix)
	{
		var oldBaseName = $"{_oldName}.{suffix}";
		var newBaseName = $"{_newName}.{suffix}";

		if (!Directory.Exists ($"Code/{oldBaseName}"))
			return;

		var oldFileName = $"Code/{oldBaseName}/{oldBaseName}.csproj";
		var newFileName = $"Code/{oldBaseName}/{newBaseName}.csproj";
		Console.WriteLine ($"{oldBaseName}.csproj -> {newBaseName}.csproj");

		var contents = File.ReadAllText (oldFileName)
			.Replace ($"<RootNamespace>{_oldName}</RootNamespace>", $"<RootNamespace>{_newName}</RootNamespace>")
			.Replace ($"<Product>{_oldName}</RootNamespace>", $"<RootNamespace>{_newName}</Product>")
			.Replace ($"<ProjectReference Include=\"..\\{_oldName}.Common\\{_oldName}.", $"<ProjectReference Include=\"..\\{_newName}.Common\\{_newName}.");

		File.WriteAllText (newFileName, contents);
		File.Delete (oldFileName);

		ProcessSourceFiles (oldBaseName);

		Console.WriteLine ($"Code/{oldBaseName} -> Code/{newBaseName}");
		Directory.Move ($"Code/{oldBaseName}", $"Code/{newBaseName}");
	}


	private void ProcessSourceFiles (String baseName)
	{
		var files = new DirectoryInfo ($"Code/{baseName}")
			.GetFiles ("*.*")
			.Where (file => !file.FullName.Contains ("/obj/") && !file.FullName.Contains ("/bin/"));

		foreach (var file in files)
		{
			var fileName = $"Code/{baseName}/{file.Name}";
			switch (file.Extension)
			{
			case ".cs":
				ProcessCsFile (fileName);
				break;

			case ".plist":
				ProcessPlistFile (fileName);
				break;
			}
		}
	}


	private void ProcessCsFile (String fileName)
	{
		Console.WriteLine (fileName);
		var contents = File.ReadAllText (fileName)
			.Replace ($"namespace {_oldName}", $"namespace {_newName}")
			.Replace ($"using {_oldName}", $"using {_newName}");
		File.WriteAllText (fileName, contents);
	}


	private void ProcessPlistFile (String fileName)
	{
		Console.WriteLine (fileName);
		var contents = File.ReadAllText (fileName)
			.Replace ($"<string>{_configuration.Identifier}.{_oldName}</string>", $"<string>{_configuration.Identifier}.{_newName}</string>");
		File.WriteAllText (fileName, contents);
	}
}
