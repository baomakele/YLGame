//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class RepeatTypeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(RepeatType));
		L.RegVar("Once", get_Once, null);
		L.RegVar("Loop", get_Loop, null);
		L.RegVar("PingPang", get_PingPang, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Once(IntPtr L)
	{
		ToLua.Push(L, RepeatType.Once);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Loop(IntPtr L)
	{
		ToLua.Push(L, RepeatType.Loop);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PingPang(IntPtr L)
	{
		ToLua.Push(L, RepeatType.PingPang);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		RepeatType o = (RepeatType)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

