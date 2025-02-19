package net.razorvine.pickle.objects;

import net.razorvine.pickle.IObjectConstructor;

/**
 * This object constructor creates ClassDicts (for unsupported classes).
 *
 * @author Irmen de Jong (irmen@razorvine.net)
 */
public class ClassDictConstructor implements IObjectConstructor {

	final String module;
	final String name;

	public ClassDictConstructor(String module, String name) {
		this.module = module;
		this.name = name;
	}

	public Object construct(Object[] args) {
		ClassDict obj = new ClassDict(module, name);
		obj.setConstructorArguments(args);
		return obj;
	}
}
