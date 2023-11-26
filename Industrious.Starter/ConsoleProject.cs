namespace Industrious.Starter;

public class ConsoleProject
{
	private readonly TextFile _project;
	private readonly TextFile _program;


	public ConsoleProject (String name)
	{
		var projectFolder = $"Code/{name}";
		Path = $"{projectFolder}/{name}.csproj";

		_project = new TextFile (Path);
		_program = new TextFile ($"{projectFolder}/Program.cs");
	}


	public String Path { get; }


	public void LoadFromResources (Workspace wks)
	{
		_project.LoadFromResource ("Console/Project")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName);

		_program.LoadFromResource ("Console/Program");
	}


	public void Save ()
	{
		_project.Save ();
		_program.Save ();
	}
}
