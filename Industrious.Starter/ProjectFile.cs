namespace Industrious.Starter;

public class ProjectFile : TextFile
{
	public ProjectFile (String name)
		: base (name)
	{
	}


	public void AddLocalProjectReference (ProjectFile projectToReference)
	{
		var name = System.IO.Path.GetFileNameWithoutExtension (projectToReference.Path);
		InsertBeforeLast ("</Project>", String.Join ("\n",
			"  <ItemGroup>",
			$"    <ProjectReference Include=\"..\\{name}\\{name}.csproj\" />",
			"  </ItemGroup>",
			""));
	}
}
