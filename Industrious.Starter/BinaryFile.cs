using System.Reflection;

namespace Industrious.Starter;

public class BinaryFile
{
	public BinaryFile (String path)
	{
		Path = path;
	}


	public String Path { get; }


	public void CopyFromResource (String resourcePath)
	{
		using var stream = Assembly
			.GetExecutingAssembly ()
			.GetManifestResourceStream ("Industrious.Starter.Resources." + resourcePath.Replace ('/', '.'));

		if (stream == null)
			throw new InvalidProgramException ($"Missing required resource for '{resourcePath}'");

		var directory = System.IO.Path.GetDirectoryName (Path);
		if (!String.IsNullOrEmpty (directory))
			Directory.CreateDirectory (directory);

		using var writer = File.Create(Path);
		stream.CopyTo (writer);
	}
}
