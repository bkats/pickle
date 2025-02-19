package net.razorvine.pickle.objects;

import java.util.HashMap;
import java.util.Map;
import java.util.ArrayList;

/**
 * A dictionary containing just the fields of the class.
 */
public class ClassDict extends HashMap<String, Object>
{
	private static final long serialVersionUID = 6157715596627049511L;
	private final String classname;
	private Object[] constructorArgs;
	private ArrayList<String> slotNames;

	public ClassDict(String modulename, String classname)
	{
		if(modulename==null)
			this.classname = classname;
		else
			this.classname = modulename+"."+classname;

		this.put("__class__", this.classname);
	}

	/**
	 * for the unpickler to restore state
	 */
	public void __setstate__(HashMap<String, Object> values) {
		this.clear();
		this.put("__class__", this.classname);
        this.putAll(values);
	}

	public void __setstate__(Object[] state) {
		this.clear();
		this.put("__class__", this.classname);
		if (state.length == 2 && state[1] instanceof HashMap<?, ?>) {
			HashMap<String, Object> slotsValues = (HashMap<String, Object>)state[1];
			ArrayList<String> slots = new ArrayList<String>();
			for(String key : slotsValues.keySet()) {
				this.put(key, slotsValues.get(key));
				slots.add(key);
			}
			slotNames = slots;

			if (state[0] instanceof HashMap<?, ?>) {
				this.putAll((HashMap<String, Object>)state[0]);
			}
		} else {
        	this.put("__state__", state);
		}
	}

	/**
	 * retrieve the (python) class name of the object that was pickled.
	 */
	public String getClassName() {
		return this.classname;
	}

	public Object[] getConstructorArguments() {
		return this.constructorArgs;
	}

	public void setConstructorArguments(Object[] args) {
		this.constructorArgs = args;
	}

	public ArrayList<String> SlotNames() {
		return slotNames;
	}
}
