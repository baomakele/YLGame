﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class System_Collections_Generic_Dictionary_string_SingleFieldWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Collections.Generic.Dictionary<string,SingleField>), typeof(System.Object), "Dictionary_string_SingleField");
		L.RegFunction("get_Item", get_Item);
		L.RegFunction("set_Item", set_Item);
		L.RegFunction("Add", Add);
		L.RegFunction("Clear", Clear);
		L.RegFunction("ContainsKey", ContainsKey);
		L.RegFunction("ContainsValue", ContainsValue);
		L.RegFunction("GetEnumerator", GetEnumerator);
		L.RegFunction("GetObjectData", GetObjectData);
		L.RegFunction("OnDeserialization", OnDeserialization);
		L.RegFunction("Remove", Remove);
		L.RegFunction("TryGetValue", TryGetValue);
		L.RegFunction("New", _CreateSystem_Collections_Generic_Dictionary_string_SingleField);
		L.RegVar("this", _this, null);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Comparer", get_Comparer, null);
		L.RegVar("Count", get_Count, null);
		L.RegVar("Keys", get_Keys, null);
		L.RegVar("Values", get_Values, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Collections_Generic_Dictionary_string_SingleField(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				System.Collections.Generic.Dictionary<string,SingleField> obj = new System.Collections.Generic.Dictionary<string,SingleField>();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(int)))
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				System.Collections.Generic.Dictionary<string,SingleField> obj = new System.Collections.Generic.Dictionary<string,SingleField>(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(System.Collections.Generic.IEqualityComparer<string>)))
			{
				System.Collections.Generic.IEqualityComparer<string> arg0 = (System.Collections.Generic.IEqualityComparer<string>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.IEqualityComparer<string>));
				System.Collections.Generic.Dictionary<string,SingleField> obj = new System.Collections.Generic.Dictionary<string,SingleField>(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(System.Collections.Generic.IDictionary<string,SingleField>)))
			{
				System.Collections.Generic.IDictionary<string,SingleField> arg0 = (System.Collections.Generic.IDictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.IDictionary<string,SingleField>));
				System.Collections.Generic.Dictionary<string,SingleField> obj = new System.Collections.Generic.Dictionary<string,SingleField>(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(System.Collections.Generic.IEqualityComparer<string>)))
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				System.Collections.Generic.IEqualityComparer<string> arg1 = (System.Collections.Generic.IEqualityComparer<string>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.IEqualityComparer<string>));
				System.Collections.Generic.Dictionary<string,SingleField> obj = new System.Collections.Generic.Dictionary<string,SingleField>(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Collections.Generic.IDictionary<string,SingleField>), typeof(System.Collections.Generic.IEqualityComparer<string>)))
			{
				System.Collections.Generic.IDictionary<string,SingleField> arg0 = (System.Collections.Generic.IDictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.IDictionary<string,SingleField>));
				System.Collections.Generic.IEqualityComparer<string> arg1 = (System.Collections.Generic.IEqualityComparer<string>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.IEqualityComparer<string>));
				System.Collections.Generic.Dictionary<string,SingleField> obj = new System.Collections.Generic.Dictionary<string,SingleField>(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: System.Collections.Generic.Dictionary<string,SingleField>.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _get_this(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			SingleField o = obj[arg0];
			ToLua.PushObject(L, o);
			return 1;

		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _set_this(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			SingleField arg1 = (SingleField)ToLua.CheckObject(L, 3, typeof(SingleField));
			obj[arg0] = arg1;
			return 0;

		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _this(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushvalue(L, 1);
			LuaDLL.tolua_bindthis(L, _get_this, _set_this);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			SingleField o = obj[arg0];
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Item(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			SingleField arg1 = (SingleField)ToLua.CheckObject(L, 3, typeof(SingleField));
			obj[arg0] = arg1;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Add(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			SingleField arg1 = (SingleField)ToLua.CheckObject(L, 3, typeof(SingleField));
			obj.Add(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			obj.Clear();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ContainsKey(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			bool o = obj.ContainsKey(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ContainsValue(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			SingleField arg0 = (SingleField)ToLua.CheckObject(L, 2, typeof(SingleField));
			bool o = obj.ContainsValue(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEnumerator(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			System.Collections.Generic.Dictionary<string,SingleField>.Enumerator o = obj.GetEnumerator();
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetObjectData(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			System.Runtime.Serialization.SerializationInfo arg0 = (System.Runtime.Serialization.SerializationInfo)ToLua.CheckObject(L, 2, typeof(System.Runtime.Serialization.SerializationInfo));
			System.Runtime.Serialization.StreamingContext arg1 = (System.Runtime.Serialization.StreamingContext)ToLua.CheckObject(L, 3, typeof(System.Runtime.Serialization.StreamingContext));
			obj.GetObjectData(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDeserialization(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			object arg0 = ToLua.ToVarObject(L, 2);
			obj.OnDeserialization(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Remove(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			bool o = obj.Remove(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryGetValue(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<string,SingleField>));
			string arg0 = ToLua.CheckString(L, 2);
			SingleField arg1 = null;
			bool o = obj.TryGetValue(arg0, out arg1);
			LuaDLL.lua_pushboolean(L, o);
			ToLua.PushObject(L, arg1);
			return 2;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Comparer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)o;
			System.Collections.Generic.IEqualityComparer<string> ret = obj.Comparer;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Comparer on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Count(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)o;
			int ret = obj.Count;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Count on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Keys(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)o;
			System.Collections.Generic.Dictionary<string,SingleField>.KeyCollection ret = obj.Keys;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Keys on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Values(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			System.Collections.Generic.Dictionary<string,SingleField> obj = (System.Collections.Generic.Dictionary<string,SingleField>)o;
			System.Collections.Generic.Dictionary<string,SingleField>.ValueCollection ret = obj.Values;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Values on a nil value" : e.Message);
		}
	}
}

