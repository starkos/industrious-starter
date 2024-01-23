using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Industrious.Starter;

public class TextFile : ISaveable
{
	private readonly Encoding _encoding;
	private String? _contents;
	private Boolean _isDirty;


	public TextFile (String path, Encoding? encoding = null)
	{
		Path = path;
		_encoding = encoding ?? new UTF8Encoding ();
		_contents = null;
		_isDirty = false;
	}


	public String Path { get; }


	private String Contents
	{
		get => _contents ?? File.ReadAllText (Path, _encoding);
		set => _contents = value;
	}


	public TextFile InsertBeforeLast (String pattern, String value)
	{
		var matches = Regex.Matches (Contents, pattern, Options (pattern));
		var insertAtIndex = matches.Last ().Groups[0].Index;
		Contents = Contents.Insert (insertAtIndex, value);
		_isDirty = true;
		return this;
	}


	public TextFile LoadFromResource (String resourcePath)
	{
		using var stream = Assembly
			.GetExecutingAssembly ()
			.GetManifestResourceStream ("Industrious.Starter.Resources." + resourcePath.Replace ('/', '.'));

		if (stream == null)
			throw new InvalidProgramException ($"Missing required resource for '{resourcePath}'");

		using var reader = new StreamReader (stream);
		Contents = reader.ReadToEnd ();
		_isDirty = true;
		return this;
	}


	public TextFile Replace (String pattern, String replacement)
	{
		Contents = Regex.Replace (Contents, pattern, replacement, Options (pattern));
		_isDirty = true;
		return this;
	}


	public virtual void Save ()
	{
		if (_isDirty)
		{
			var directory = System.IO.Path.GetDirectoryName (Path);
			if (!String.IsNullOrEmpty (directory))
				Directory.CreateDirectory (directory);

			File.WriteAllText (Path, _contents, _encoding);
			_isDirty = false;
		}
	}


	private static RegexOptions Options (String pattern)
	{
		return pattern.StartsWith ("^")
			? RegexOptions.Multiline
			: RegexOptions.Singleline;
	}
}
