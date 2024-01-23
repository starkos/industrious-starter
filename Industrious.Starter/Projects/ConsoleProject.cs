namespace Industrious.Starter.Projects;

public class ConsoleProject : ISaveable
{
	private readonly TextFile _program;


	public ConsoleProject (String name)
	{
		Project = new ProjectFile ($"Code/{name}/{name}.csproj");
		_program = new TextFile ($"Code/{name}/Program.cs");
	}


	public ProjectFile Project { get; }


	public void Init (Configuration cfg)
	{
		Project.LoadFromResource ("Console/Project")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title)
			.Replace ("{Company}", cfg.Company);

		_program.LoadFromResource ("Console/Program");
	}


	public void Save ()
	{
		Project.Save ();
		_program.Save ();
	}
}
