namespace Industrious.Starter;

public class ConsoleProject
{
	private readonly TextFile _program;


	public ConsoleProject (String name)
	{
		Project = new TextFile ($"Code/{name}/{name}.csproj");
		_program = new TextFile ($"Code/{name}/Program.cs");
	}


	public TextFile Project { get; }


	public void LoadFromResources (Workspace wks)
	{
		Project.LoadFromResource ("Console/Project")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName);

		_program.LoadFromResource ("Console/Program");
	}


	public void Save ()
	{
		Project.Save ();
		_program.Save ();
	}
}
