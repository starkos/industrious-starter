namespace Industrious.Starter;

public class ConsoleTarget
{
	public ConsoleTarget (String name)
	{
		var projectFolder = $"Code/{name}.Console";
		ProjectPath = $"{projectFolder}/{name}.Console.csproj";

		Project = new TextFile (ProjectPath);
		Program = new TextFile ($"{projectFolder}/Program.cs");
	}


	public String ProjectPath { get; }

	public TextFile Project { get; }
	public TextFile Program { get; }


	public void Save ()
	{
		Project.Save ();
		Program.Save ();
	}
}
