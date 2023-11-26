namespace Industrious.Starter;

public class ClassLibTarget
{
	public ClassLibTarget (String name)
	{
		var projectFolder = $"Code/{name}";
		ProjectPath = $"{projectFolder}/{name}.csproj";

		Project = new TextFile (ProjectPath);
	}


	public String ProjectPath { get; }

	public TextFile Project { get; }


	public void Save ()
	{
		Project.Save ();
	}
}
