﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class NetWorkMessageWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(NetWorkMessage), null);
		L.RegFunction("New", _CreateNetWorkMessage);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("m_MessageType", get_m_MessageType, set_m_MessageType);
		L.RegVar("m_MsgCode", get_m_MsgCode, set_m_MsgCode);
		L.RegVar("m_LuaByte", get_m_LuaByte, set_m_LuaByte);
		L.RegVar("m_data", get_m_data, set_m_data);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNetWorkMessage(IntPtr L)
	{
		NetWorkMessage obj = new NetWorkMessage();
		ToLua.PushValue(L, obj);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_MessageType(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			string ret = obj.m_MessageType;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_MessageType on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_MsgCode(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			int ret = obj.m_MsgCode;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_MsgCode on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_LuaByte(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			byte[] ret = obj.m_LuaByte;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_LuaByte on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_data(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			System.Collections.Generic.Dictionary<string,object> ret = obj.m_data;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_data on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_MessageType(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.m_MessageType = arg0;
			ToLua.SetBack(L, 1, obj);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_MessageType on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_MsgCode(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.m_MsgCode = arg0;
			ToLua.SetBack(L, 1, obj);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_MsgCode on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_LuaByte(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			byte[] arg0 = ToLua.CheckByteBuffer(L, 2);
			obj.m_LuaByte = arg0;
			ToLua.SetBack(L, 1, obj);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_LuaByte on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_data(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NetWorkMessage obj = (NetWorkMessage)o;
			System.Collections.Generic.Dictionary<string,object> arg0 = (System.Collections.Generic.Dictionary<string,object>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.Dictionary<string,object>));
			obj.m_data = arg0;
			ToLua.SetBack(L, 1, obj);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_data on a nil value" : e.Message);
		}
	}
}

