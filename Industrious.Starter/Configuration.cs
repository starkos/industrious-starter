using System.Text.Json;

namespace Industrious.Starter;

///////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
///  Workspace settings which are serialized and persisted between runs.
/// </summary>
/// <remarks>
///  The name, title, etc. which are specified at the time the solution is created are
///  persisted between runs so they don't need to be re-entered or detected. The last
///  applied starter kit change version is also stored in this file; only version which
///  have been added to the kit since the last update are applied.
/// </remarks>
///////////////////////////////////////////////////////////////////////////////////////////
public class Configuration
{
	public Configuration (String name, String title, String company, String identifier, Int32 version = 0)
	{
		Name = name;
		Title = title;
		Company = company;
		Identifier = identifier;
		Version = version;
	}

	public String Name { get; }
	public String Title { get; }
	public String Company { get; }
	public String Identifier { get; }
	public Int32 Version { get; }


	public static Configuration? Load (String fileName)
	{
		try
		{
			var json = File.ReadAllText (fileName);
			return JsonSerializer.Deserialize<Configuration> (json);
		}
		catch (FileNotFoundException)
		{
			return null;
		}
	}


	public void Save (String fileName)
	{
		var options = new JsonSerializerOptions { WriteIndented = true };
		var json = JsonSerializer.Serialize (this, options);
		File.WriteAllText (fileName, json);
	}


	public Configuration WithVersion (Int32 version)
	{
		return new Configuration (Name, Title, Company, Identifier, version);
	}
}
