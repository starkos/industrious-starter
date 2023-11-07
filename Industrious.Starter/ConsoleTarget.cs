namespace Industrious.Starter;

public class ConsoleTarget
{
	public ConsoleTarget (String name)
	{
		Project = new TextFile ($"{name}.Console/{name}.Console.csproj");
		Program = new TextFile ($"{name}.Console/Program.cs");
	}


	public TextFile Project { get; }
	public TextFile Program { get; }


	public void Save ()
	{
		Project.Save ();
		Program.Save ();
	}
}
