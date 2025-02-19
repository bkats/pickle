/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.Collections;
using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace Razorvine.Pickle.Objects
{

/// <summary>
/// This object constructor creates ClassDicts (for unsupported classes)
/// </summary>
public class ClassDictConstructor : IObjectConstructor {
	public readonly string module;
	public readonly string name;

	public ClassDictConstructor(string module, string name) {
		this.module=module;
		this.name=name;
	}

	public object construct(object[] args) {
		return new ClassDict(module, name) {
			ConstructorArguments = args
		};
	}
}

/// <summary>
/// A dictionary containing just the fields of the class.
/// </summary>
public class ClassDict : Dictionary<string, object>
{
	public ClassDict(string modulename, string classname)
	{
		if(string.IsNullOrEmpty(modulename))
			ClassName = classname;
		else
			ClassName = modulename+"."+classname;

		Add("__class__", ClassName);
	}

	/// <summary>
	/// for the unpickler to restore state
	/// </summary>
	public void __setstate__(Hashtable values) {
		Clear();
		Add("__class__", ClassName);
		foreach(string x in values.Keys)
			Add(x, values[x]);
	}

	/// <summary>
	/// for the unpickler to restore state
	/// </summary>
	public void __setstate__(object[] state)
	{
		Clear();
		Add("__class__", ClassName);
		if (state.Length == 2 && state[1] is Hashtable slots)
		{
			var slotNames = new List<string>();
			foreach (string x in slots.Keys)
			{
				slotNames.Add(x);
				Add(x, slots[x]);
			}
			Slots = slotNames;

			// Check for optional __dict__ values
			if (state[0] is Hashtable values)
			{
				foreach (string x in values.Keys)
				{
					Add(x, values[x]);
				}
			}
		}
		else
		{
			// Probably some custom storage
			Add("__state__", state);
		}
	}

	/// <summary>
	/// retrieve the (python) class name of the object that was pickled.
	/// </summary>
	public string ClassName { get; }

	/// <summary>
	/// retrieve the given arguments to the constructor
	/// </summary>
	public object[] ConstructorArguments { get; internal set; }

	/// <summary>
	/// retrieve the names of slots on this class
	/// note: the values are in the dictionary
	/// </summary>
	public IReadOnlyList<string> Slots { get; protected set; }

}

}
