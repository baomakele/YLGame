﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class AudioManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(AudioManager), typeof(UnityEngine.Behaviour));
		L.RegFunction("Init", Init);
		L.RegFunction("Update", Update);
		L.RegFunction("PlayMusic2D", PlayMusic2D);
		L.RegFunction("StopMusic2D", StopMusic2D);
		L.RegFunction("PlaySound2D", PlaySound2D);
		L.RegFunction("PlaySound3D", PlaySound3D);
		L.RegFunction("GetAudioSource2D", GetAudioSource2D);
		L.RegFunction("GetAudioSource3D", GetAudioSource3D);
		L.RegFunction("GetAudioClip", GetAudioClip);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("s_2Dmusic", get_s_2Dmusic, set_s_2Dmusic);
		L.RegVar("s_MusicIsPlaying", get_s_MusicIsPlaying, set_s_MusicIsPlaying);
		L.RegVar("s_OnMusicComplete", get_s_OnMusicComplete, set_s_OnMusicComplete);
		L.RegVar("s_OnMusicVolumeChange", get_s_OnMusicVolumeChange, set_s_OnMusicVolumeChange);
		L.RegVar("s_OnSoundVolumeChange", get_s_OnSoundVolumeChange, set_s_OnSoundVolumeChange);
		L.RegVar("Instance", get_Instance, null);
		L.RegVar("s_GlobalVolume", get_s_GlobalVolume, set_s_GlobalVolume);
		L.RegVar("s_MusicVolume", get_s_MusicVolume, set_s_MusicVolume);
		L.RegVar("s_SoundVolume", get_s_SoundVolume, set_s_SoundVolume);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			AudioManager.Init();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			AudioManager obj = (AudioManager)ToLua.CheckObject(L, 1, typeof(AudioManager));
			obj.Update();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayMusic2D(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			bool arg1 = LuaDLL.luaL_checkboolean(L, 2);
			UnityEngine.AudioSource o = AudioManager.PlayMusic2D(arg0, arg1);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopMusic2D(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UnityEngine.AudioSource o = AudioManager.StopMusic2D();
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlaySound2D(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(string)))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.AudioSource o = AudioManager.PlaySound2D(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(float)))
			{
				string arg0 = ToLua.ToString(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				AudioManager.PlaySound2D(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(float), typeof(bool)))
			{
				string arg0 = ToLua.ToString(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				bool arg2 = LuaDLL.lua_toboolean(L, 3);
				UnityEngine.AudioSource o = AudioManager.PlaySound2D(arg0, arg1, arg2);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: AudioManager.PlaySound2D");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlaySound3D(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.GameObject arg1 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 2, typeof(UnityEngine.GameObject));
			UnityEngine.AudioSource o = AudioManager.PlaySound3D(arg0, arg1);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAudioSource2D(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			SoundType arg0 = (SoundType)ToLua.CheckObject(L, 1, typeof(SoundType));
			UnityEngine.AudioSource o = AudioManager.GetAudioSource2D(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAudioSource3D(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 1, typeof(UnityEngine.GameObject));
			UnityEngine.AudioSource o = AudioManager.GetAudioSource3D(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAudioClip(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.AudioClip o = AudioManager.GetAudioClip(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_2Dmusic(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AudioManager.s_2Dmusic);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_MusicIsPlaying(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AudioManager.s_MusicIsPlaying);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_OnMusicComplete(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AudioManager.s_OnMusicComplete);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_OnMusicVolumeChange(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AudioManager.s_OnMusicVolumeChange);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_OnSoundVolumeChange(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AudioManager.s_OnSoundVolumeChange);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AudioManager.Instance);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_GlobalVolume(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushnumber(L, AudioManager.s_GlobalVolume);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_MusicVolume(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushnumber(L, AudioManager.s_MusicVolume);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_s_SoundVolume(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushnumber(L, AudioManager.s_SoundVolume);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_2Dmusic(IntPtr L)
	{
		try
		{
			UnityEngine.AudioSource arg0 = (UnityEngine.AudioSource)ToLua.CheckUnityObject(L, 2, typeof(UnityEngine.AudioSource));
			AudioManager.s_2Dmusic = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_MusicIsPlaying(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AudioManager.s_MusicIsPlaying = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_OnMusicComplete(IntPtr L)
	{
		try
		{
			AudioCallBack arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (AudioCallBack)ToLua.CheckObject(L, 2, typeof(AudioCallBack));
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(typeof(AudioCallBack), func) as AudioCallBack;
			}

			AudioManager.s_OnMusicComplete = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_OnMusicVolumeChange(IntPtr L)
	{
		try
		{
			AudioCallBack arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (AudioCallBack)ToLua.CheckObject(L, 2, typeof(AudioCallBack));
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(typeof(AudioCallBack), func) as AudioCallBack;
			}

			AudioManager.s_OnMusicVolumeChange = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_OnSoundVolumeChange(IntPtr L)
	{
		try
		{
			AudioCallBack arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (AudioCallBack)ToLua.CheckObject(L, 2, typeof(AudioCallBack));
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(typeof(AudioCallBack), func) as AudioCallBack;
			}

			AudioManager.s_OnSoundVolumeChange = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_GlobalVolume(IntPtr L)
	{
		try
		{
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			AudioManager.s_GlobalVolume = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_MusicVolume(IntPtr L)
	{
		try
		{
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			AudioManager.s_MusicVolume = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_s_SoundVolume(IntPtr L)
	{
		try
		{
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			AudioManager.s_SoundVolume = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
