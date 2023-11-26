namespace Industrious.Starter;

public class ClassLibraryProject
{
	private readonly TextFile _project;


	public ClassLibraryProject (String name)
	{
		Path = $"Code/{name}/{name}.csproj";

		_project = new TextFile (Path);
	}


	public String Path { get; }


	public void LoadFromResources ()
	{
		_project.LoadFromResource ("Core/Project");
	}


	public void Save ()
	{
		_project.Save ();
	}
}
