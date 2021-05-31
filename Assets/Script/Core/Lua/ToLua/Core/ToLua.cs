﻿/*
Copyright (c) 2015-2016 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

#if UNITY_EDITOR  
using UnityEditor;
using UnityEditor.Callbacks;
using System.Reflection;
#endif

namespace LuaInterface
{
    public static class ToLua
    {
        public static bool useRelativePath = true;             //loader使用长路径还是相对路径
        static Type monoType = typeof(Type).GetType();

#if UNITY_EDITOR
        //static int _instanceID = -1;
        //static int _line = 182;
        private static object consoleWindow;
        private static object logListView;
        private static FieldInfo logListViewCurrentRow;
        private static MethodInfo LogEntriesGetEntry;
        private static object logEntry;
        private static FieldInfo logEntryCondition;
#endif

        public static void OpenLibs(IntPtr L)
        {
            //AddLuaLoader(L);
            LuaDLL.tolua_atpanic(L, Panic);
            LuaDLL.tolua_pushcfunction(L, Print);
            LuaDLL.lua_setglobal(L, "print");
            //LuaDLL.tolua_pushcfunction(L, DoFile);
            //LuaDLL.lua_setglobal(L, "dofile");
            //LuaDLL.tolua_pushcfunction(L, LoadFile);
            //LuaDLL.lua_setglobal(L, "loadfile");
        



            LuaDLL.lua_getglobal(L, "tolua");

            LuaDLL.lua_pushstring(L, "isnull");
            LuaDLL.lua_pushcfunction(L, IsNull);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "typeof");
            LuaDLL.lua_pushcfunction(L, GetClassType);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "tolstring");
            LuaDLL.tolua_pushcfunction(L, BufferToString);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "toarray");
            LuaDLL.tolua_pushcfunction(L, TableToArray);
            LuaDLL.lua_rawset(L, -3);

            //手动模拟gc
            //LuaDLL.lua_pushstring(L, "collect");
            //LuaDLL.lua_pushcfunction(L, Collect);
            //LuaDLL.lua_rawset(L, -3);            

            int meta = LuaStatic.GetMetaReference(L, typeof(NullObject));
            LuaDLL.lua_pushstring(L, "null");
            LuaDLL.tolua_pushnewudata(L, meta, 1);
            LuaDLL.lua_rawset(L, -3);
            LuaDLL.lua_pop(L, 1);

            LuaDLL.tolua_pushudata(L, 1);
            LuaDLL.lua_setfield(L, LuaIndexes.LUA_GLOBALSINDEX, "null");

//#if UNITY_EDITOR
//            GetToLuaInstanceID();
//            GetConsoleWindowListView();
//#endif
        }

        /*--------------------------------对于tolua扩展函数------------------------------------------*/
        #region TOLUA_EXTEND_FUNCTIONS
        //static void AddLuaLoader(IntPtr L)
        //{
        //    LuaDLL.lua_getglobal(L, "package");
        //    LuaDLL.lua_getfield(L, -1, "loaders");
        //    LuaDLL.tolua_pushcfunction(L, Loader);

        //    for (int i = LuaDLL.lua_objlen(L, -2) + 1; i > 2; i--)
        //    {
        //        LuaDLL.lua_rawgeti(L, -2, i - 1);
        //        LuaDLL.lua_rawseti(L, -3, i);
        //    }

        //    LuaDLL.lua_rawseti(L, -2, 2);
        //    LuaDLL.lua_pop(L, 2);
        //}

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Panic(IntPtr L)
        {
            string reason = String.Format("PANIC: unprotected error in call to Lua API ({0})", LuaDLL.lua_tostring(L, -1));
            throw new LuaException(reason);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Print(IntPtr L)
        {
            try
            {
                int n = LuaDLL.lua_gettop(L);
                StringBuilder sb = StringBuilderCache.Acquire();
#if UNITY_EDITOR
                int line = LuaDLL.tolua_where(L, 1);
                string filename = LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_settop(L, n);

                if (!filename.Contains("."))
                {
                    sb.AppendFormat("[{0}.lua:{1}]:", filename, line);
                }
                else
                {
                    sb.AppendFormat("[{0}:{1}]:", filename, line);
                }
#endif

                for (int i = 1; i <= n; i++)
                {
                    if (i > 1) sb.Append("    ");

                    if (LuaDLL.lua_isstring(L, i) == 1)
                    {
                        sb.Append(LuaDLL.lua_tostring(L, i));
                    }
                    else if (LuaDLL.lua_isnil(L, i))
                    {
                        sb.Append("nil");
                    }
                    else if (LuaDLL.lua_isboolean(L, i))
                    {
                        sb.Append(LuaDLL.lua_toboolean(L, i) ? "true" : "false");
                    }
                    else
                    {
                        IntPtr p = LuaDLL.lua_topointer(L, i);

                        if (p == IntPtr.Zero)
                        {
                            sb.Append("nil");
                        }
                        else
                        {
                            sb.AppendFormat("{0}:0x{1}", LuaDLL.luaL_typename(L, i), p.ToString("X"));
                        }
                    }
                }

                //Debugger.Log(StringBuilderCache.GetStringAndRelease(sb));
                return 0;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }
        

        //[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        //static int Loader(IntPtr L)
        //{
        //    try
        //    {
        //        string fileName = LuaDLL.lua_tostring(L, 1);
        //        fileName = fileName.Replace(".", "/");
        //        byte[] buffer = LuaFileUtils.Instance.ReadFile(fileName);

        //        if (buffer == null)
        //        {
        //            string error = LuaFileUtils.Instance.FindFileError(fileName);
        //            LuaDLL.lua_pushstring(L, error);
        //            return 1;
        //        }

        //        //if (LuaConst.openZbsDebugger)
        //        //{
        //        //    fileName = LuaFileUtils.Instance.FindFile(fileName);
        //        //}

        //        if (LuaDLL.luaL_loadbuffer(L, buffer, buffer.Length, fileName) != 0)
        //        {
        //            string err = LuaDLL.lua_tostring(L, -1);
        //            throw new LuaException(err, LuaException.GetLastError());
        //        }

        //        return 1;
        //    }
        //    catch (Exception e)
        //    {
        //        return LuaDLL.toluaL_exception(L, e);
        //    }
        //}

        //[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        //public static int DoFile(IntPtr L)
        //{
        //    try
        //    {
        //        string fileName = LuaDLL.lua_tostring(L, 1);
        //        int n = LuaDLL.lua_gettop(L);
        //        byte[] buffer = LuaFileUtils.Instance.ReadFile(fileName);

        //        if (buffer == null)
        //        {
        //            string error = string.Format("cannot open {0}: No such file or directory", fileName);
        //            error += LuaFileUtils.Instance.FindFileError(fileName);
        //            throw new LuaException(error);
        //        }

        //        if (LuaDLL.luaL_loadbuffer(L, buffer, buffer.Length, fileName) == 0)
        //        {
        //            if (LuaDLL.lua_pcall(L, 0, LuaDLL.LUA_MULTRET, 0) != 0)
        //            {
        //                string error = LuaDLL.lua_tostring(L, -1);
        //                throw new LuaException(error, LuaException.GetLastError());
        //            }
        //        }

        //        return LuaDLL.lua_gettop(L) - n;
        //    }
        //    catch (Exception e)
        //    {
        //        return LuaDLL.toluaL_exception(L, e);
        //    }
        //}

        //[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        //public static int LoadFile(IntPtr L)
        //{
        //    try
        //    {
        //        string fileName = LuaDLL.lua_tostring(L, 1);
        //        byte[] buffer = LuaFileUtils.Instance.ReadFile(fileName);

        //        if (buffer == null)
        //        {
        //            string error = string.Format("cannot open {0}: No such file or directory", fileName);
        //            error += LuaFileUtils.Instance.FindFileError(fileName);
        //            throw new LuaException(error);
        //        }

        //        if (LuaDLL.luaL_loadbuffer(L, buffer, buffer.Length, fileName) == 0)
        //        {
        //            return 1;
        //        }

        //        LuaDLL.lua_pushnil(L);
        //        LuaDLL.lua_insert(L, -2);  /* put before error message */
        //        return 2;
        //    }
        //    catch (Exception e)
        //    {
        //        return LuaDLL.toluaL_exception(L, e);
        //    }
        //}


        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int IsNull(IntPtr L)
        {
            LuaTypes t = LuaDLL.lua_type(L, 1);

            if (t == LuaTypes.LUA_TNIL)
            {
                LuaDLL.lua_pushboolean(L, true);
            }
            else
            {
                object o = ToLua.ToObject(L, -1);

                if (o == null || o.Equals(null))
                {
                    LuaDLL.lua_pushboolean(L, true);
                }
                else
                {
                    LuaDLL.lua_pushboolean(L, false);
                }
            }

            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int BufferToString(IntPtr L)
        {
            try
            {
                object o = CheckObject(L, 1);

                if (o is byte[])
                {
                    byte[] buff = (byte[])o;
                    LuaDLL.lua_pushlstring(L, buff, buff.Length);
                }
                else if (o is char[])
                {
                    byte[] buff = System.Text.Encoding.UTF8.GetBytes((char[])o);
                    LuaDLL.lua_pushlstring(L, buff, buff.Length);
                }
                else if (o is string)
                {
                    LuaDLL.lua_pushstring(L, (string)o);
                }
                else
                {
                    LuaDLL.luaL_typerror(L, 1, "byte[] or char[]");
                }

                return 1;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }            
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetClassType(IntPtr L)
        {
            int reference = LuaDLL.tolua_getmetatableref(L, 1);

            if (reference > 0)
            {
                Type t = LuaStatic.GetClassType(L, reference);
                Push(L, t);
            }
            else
            {
                LuaValueType ret = LuaDLL.tolua_getvaluetype(L, -1);

                switch (ret)
                {
                    case LuaValueType.Ray:
                        Push(L, typeof(Ray));
                        break;
                    case LuaValueType.Bounds:
                        Push(L, typeof(Bounds));
                        break;
                    case LuaValueType.LayerMask:
                        Push(L, typeof(LayerMask));
                        break;
                    case LuaValueType.RaycastHit:
                        Push(L, typeof(RaycastHit));
                        break;
                    default:
                        Debugger.LogError("type not register to lua");
                        LuaDLL.lua_pushnil(L);
                        break;
                }
            }

            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int TableToArray(IntPtr L)
        {
            try
            {
                object[] objs = ToLua.CheckObjectArray(L, 1);
                Type t = (Type)ToLua.CheckObject(L, 2, typeof(Type));
                Array ret = System.Array.CreateInstance(t, objs.Length);

                for (int i = 0; i < objs.Length; i++)
                {
                    ret.SetValue(objs[i], i);
                }

                ToLua.Push(L, ret);
                return 1;
            }
            catch(LuaException e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }            
        }


        /*[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int Collect(IntPtr L)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, 1);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                translator.RemoveObject(udata);
            }

            return 0;
        }*/

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int op_ToString(IntPtr L)
        {
            object obj = ToLua.ToObject(L, 1);

            if (obj != null)
            {
                LuaDLL.lua_pushstring(L, obj.ToString());
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            return 1;
        }

#if UNITY_EDITOR
        private static bool GetConsoleWindowListView()
        {
            if (logListView == null)
            {
                Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
                Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
                FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
                consoleWindow = fieldInfo.GetValue(null);

                if (consoleWindow == null)
                {
                    logListView = null;
                    return false;
                }

                FieldInfo listViewFieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
                logListView = listViewFieldInfo.GetValue(consoleWindow);
                logListViewCurrentRow = listViewFieldInfo.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);

                Type logEntriesType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntries");
                LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
                Type logEntryType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntry");
                logEntry = Activator.CreateInstance(logEntryType);
                logEntryCondition = logEntryType.GetField("condition", BindingFlags.Instance | BindingFlags.Public);                
            }

            return true;
        }


        private static string GetListViewRowCount(ref int line)
        {
            int row = (int)logListViewCurrentRow.GetValue(logListView);
            LogEntriesGetEntry.Invoke(null, new object[] { row, logEntry });
            string condition = logEntryCondition.GetValue(logEntry) as string;
            condition = condition.Substring(0, condition.IndexOf('\n'));
            int index = condition.IndexOf(".lua:");

            if (index >= 0)
            {
                int start = condition.IndexOf("[");
                int end = condition.IndexOf("]:");
                string _line = condition.Substring(index + 5, end - index - 4);
                Int32.TryParse(_line, out line);
                return condition.Substring(start + 1, index + 3 - start);
            }

            index = condition.IndexOf(".cs:");

            if (index >= 0)
            {
                int start = condition.IndexOf("[");
                int end = condition.IndexOf("]:");
                string _line = condition.Substring(index + 5, end - index - 4);
                Int32.TryParse(_line, out line);
                return condition.Substring(start + 1, index + 2 - start);
            }

            return null;
        }

        //static void GetToLuaInstanceID()
        //{
        //    if (_instanceID == -1)
        //    {
        //        int start = LuaConst.toluaDir.IndexOf("Assets");
        //        int end = LuaConst.toluaDir.LastIndexOf("/Lua");
        //        string dir = LuaConst.toluaDir.Substring(start, end - start);
        //        dir += "/Core/ToLua.cs";
        //        _instanceID = AssetDatabase.LoadAssetAtPath(dir, typeof(MonoScript)).GetInstanceID();//"Assets/ToLua/Core/ToLua.cs"
        //    }
        //}

        //[OnOpenAssetAttribute(0)]
        //public static bool OnOpenAsset(int instanceID, int line)
        //{
        //    GetToLuaInstanceID();

        //    if (!GetConsoleWindowListView() || (object)EditorWindow.focusedWindow != consoleWindow)
        //    {
        //        return false;
        //    }

        //    if (instanceID == _instanceID && line == _line)
        //    {
        //        string fileName = GetListViewRowCount(ref line);

        //        if (fileName == null)
        //        {
        //            return false;
        //        }

        //        if (fileName.EndsWith(".cs"))
        //        {
        //            string filter = fileName.Substring(0, fileName.Length - 3);
        //            filter += " t:MonoScript";
        //            string[] searchPaths = AssetDatabase.FindAssets(filter);

        //            for (int i = 0; i < searchPaths.Length; i++)
        //            {
        //                string path = AssetDatabase.GUIDToAssetPath(searchPaths[i]);

        //                if (path.EndsWith(fileName))
        //                {
        //                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
        //                    AssetDatabase.OpenAsset(obj, line);
        //                    return true;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            string filter = fileName.Substring(0, fileName.Length - 4);
        //            string[] searchPaths = AssetDatabase.FindAssets(filter);

        //            for (int i = 0; i < searchPaths.Length; i++)
        //            {
        //                string path = AssetDatabase.GUIDToAssetPath(searchPaths[i]);

        //                if (path.EndsWith(fileName) || path.EndsWith(fileName + ".bytes"))
        //                {
        //                    UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);
        //                    AssetDatabase.OpenAsset(obj, line);
        //                    return true;
        //                }
        //            }

        //        }
        //    }

        //    return false;
        //}
#endif
#endregion
    /*-------------------------------------------------------------------------------------------*/

        public static string ToString(IntPtr L, int stackPos)
        {
            if (LuaDLL.lua_isstring(L, stackPos) == 1)
            {
                return LuaDLL.lua_tostring(L, stackPos);
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) == 1)
            {
                return (string)ToObject(L, stackPos);
            }

            return null;
        }

        public static object ToObject(IntPtr L, int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                return translator.GetObject(udata);
            }

            return null;
        }

        public static LuaFunction ToLuaFunction(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return LuaStatic.GetFunction(L, reference);
        }

        public static LuaTable ToLuaTable(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return LuaStatic.GetTable(L, reference);
        }

        public static LuaThread ToLuaThread(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return LuaStatic.GetLuaThread(L, reference);
        }

        public static Vector3 ToVector3(IntPtr L, int stackPos)
        {
            float x = 0, y = 0, z = 0;
            LuaDLL.tolua_getvec3(L, stackPos, out x, out y, out z);
            return new Vector3(x, y, z);
        }

        public static Vector4 ToVector4(IntPtr L, int stackPos)
        {
            float x, y, z, w;
            LuaDLL.tolua_getvec4(L, stackPos, out x, out y, out z, out w);
            return new Vector4(x, y, z, w);
        }

        public static Vector2 ToVector2(IntPtr L, int stackPos)
        {
            float x, y;
            LuaDLL.tolua_getvec2(L, stackPos, out x, out y);
            return new Vector2(x, y);
        }

        public static Quaternion ToQuaternion(IntPtr L, int stackPos)
        {
            float x, y, z, w;
            LuaDLL.tolua_getquat(L, stackPos, out x, out y, out z, out w);
            return new Quaternion(x, y, z, w);
        }

        public static Color ToColor(IntPtr L, int stackPos)
        {
            float r, g, b, a;
            LuaDLL.tolua_getclr(L, stackPos, out r, out g, out b, out a);
            return new Color(r, g, b, a);
        }

        public static Ray ToRay(IntPtr L, int stackPos)
        {
            int top = LuaDLL.lua_gettop(L);
            LuaStatic.GetUnpackRayRef(L);
            LuaDLL.lua_pushvalue(L, stackPos);

            if (LuaDLL.lua_pcall(L, 1, 2, 0) == 0)
            {
                Vector3 origin = ToVector3(L, top + 1);
                Vector3 dir = ToVector3(L, top + 2);
                return new Ray(origin, dir);
            }
            else
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static Bounds ToBounds(IntPtr L, int stackPos)
        {
            int top = LuaDLL.lua_gettop(L);
            LuaStatic.GetUnpackBounds(L);
            LuaDLL.lua_pushvalue(L, stackPos);

            if (LuaDLL.lua_pcall(L, 1, 2, 0) == 0)
            {
                Vector3 center = ToVector3(L, top + 1);
                Vector3 size = ToVector3(L, top + 2);
                return new Bounds(center, size);
            }
            else
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static LayerMask ToLayerMask(IntPtr L, int stackPos)
        {
            return LuaDLL.tolua_getlayermask(L, stackPos);
        }

        public static object ToVarObject(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            switch (type)
            {
                case LuaTypes.LUA_TNUMBER:                    
                    return LuaDLL.lua_tonumber(L, stackPos);
                case LuaTypes.LUA_TSTRING:
                    return LuaDLL.lua_tostring(L, stackPos);
                case LuaTypes.LUA_TUSERDATA:
                    if (LuaDLL.tolua_isint64(L, stackPos))
                    {
                        return LuaDLL.tolua_toint64(L, stackPos);
                    }
                    else
                    {
                        return ToObject(L, stackPos);
                    }
                case LuaTypes.LUA_TBOOLEAN:
                    return LuaDLL.lua_toboolean(L, stackPos);
                case LuaTypes.LUA_TFUNCTION:
                    return ToLuaFunction(L, stackPos);
                case LuaTypes.LUA_TTABLE:
                    return ToVarTable(L, stackPos);
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TLIGHTUSERDATA:
                    return LuaDLL.lua_touserdata(L, stackPos);
                case LuaTypes.LUA_TTHREAD:
                    return ToLuaThread(L, stackPos);
                default:
                    return null;
            }
        }

        public static object ToVarObject(IntPtr L, int stackPos, Type t)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNUMBER)
            {
                object o = LuaDLL.lua_tonumber(L, stackPos);
                o = Convert.ChangeType(o, t);
                return o;
            }

            return ToVarObject(L, stackPos);
        }

        public static object ToVarTable(IntPtr L, int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaValueType ret = LuaDLL.tolua_getvaluetype(L, stackPos);

            switch (ret)
            {
                case LuaValueType.Vector3:
                    return ToVector3(L, stackPos);
                case LuaValueType.Quaternion:
                    return ToQuaternion(L, stackPos);
                case LuaValueType.Vector2:
                    return ToVector2(L, stackPos);
                case LuaValueType.Color:
                    return ToColor(L, stackPos);
                case LuaValueType.Vector4:
                    return ToVector4(L, stackPos);
                case LuaValueType.Ray:
                    return ToRay(L, stackPos);
                case LuaValueType.Bounds:
                    return ToBounds(L, stackPos);
                //case LuaValueType.Touch:
                //    break;
                case LuaValueType.LayerMask:
                    return ToLayerMask(L, stackPos);
                case LuaValueType.None:
                    LuaDLL.lua_pushvalue(L, stackPos);
                    int reference = LuaDLL.toluaL_ref(L);
                    return LuaStatic.GetTable(L, reference);
                default:                    
                    return null;
            }
        }

        public static LuaFunction CheckLuaFunction(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TFUNCTION)
            {
                LuaDLL.luaL_typerror(L, stackPos, "function");
                return null;
            }

            return ToLuaFunction(L, stackPos);
        }

        public static LuaTable CheckLuaTable(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TTABLE)
            {
                LuaDLL.luaL_typerror(L, stackPos, "table");
                return null;
            }

            return ToLuaTable(L, stackPos);
        }

        public static LuaThread CheckLuaThread(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TTHREAD)
            {
                LuaDLL.luaL_typerror(L, stackPos, "thread");
                return null;
            }

            return ToLuaThread(L, stackPos);
        }

        public static string CheckString(IntPtr L, int stackPos)
        {
            if (LuaDLL.lua_isstring(L, stackPos) == 1)
            {
                return LuaDLL.lua_tostring(L, stackPos);
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) == 1)
            {
                return (string)CheckObject(L, stackPos, typeof(string));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string");
            return null;
        }

        public static object CheckObject(IntPtr L, int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                return translator.GetObject(udata);
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "object");
            return null;
        }

        public static object CheckObject(IntPtr L, int stackPos, Type type)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    Type objType = obj.GetType();

                    if (type == objType || type.IsAssignableFrom(objType))
                    {
                        return obj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", type.FullName, objType.FullName));
                }

                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos) && !TypeChecker.IsValueType(type))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        public static object CheckObject<T>(IntPtr L, int stackPos) where T : class
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    Type objType = obj.GetType();

                    if (obj is T)
                    {
                        return obj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", TypeTraits<T>.GetTypeName(), objType.FullName));
                }

                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, TypeTraits<T>.GetTypeName());
            return null;
        }

        static public Vector3 CheckVector3(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector3)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Vector3", type.ToString());
                return Vector3.zero;
            }

            float x, y, z;
            LuaDLL.tolua_getvec3(L, stackPos, out x, out y, out z);
            return new Vector3(x, y, z);
        }

        static public Quaternion CheckQuaternion(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Quaternion)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Quaternion", type.ToString());
                return Quaternion.identity;
            }

            float x, y, z, w;
            LuaDLL.tolua_getquat(L, stackPos, out x, out y, out z, out w);
            return new Quaternion(x, y, z, w);
        }

        public static T[] CheckStructArray<T>(IntPtr L, int stackPos) where T : struct
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TTABLE:
                    int len = LuaDLL.lua_objlen(L, stackPos);
                    T[] list = new T[len];
                    int pos = LuaDLL.lua_gettop(L) + 1;

                    for (int i = 1; i <= len; i++)
                    {
                        LuaDLL.lua_rawgeti(L, stackPos, i);

                        if (!TypeTraits<T>.Check(L, pos))
                        {
                            LuaDLL.lua_pop(L, 1);
                            LuaDLL.luaL_typerror(L, stackPos, typeof(T[]).FullName);
                            return list;
                        }

                        list[i - 1] = StackTraits<T>.To(L, pos);
                        LuaDLL.lua_pop(L, 1);
                    }
                    return list;
                case LuaTypes.LUA_TUSERDATA:
                    return (T[])CheckObject(L, stackPos, typeof(T[]));
                default:
                    LuaDLL.luaL_typerror(L, stackPos, TypeTraits<T[]>.GetTypeName());
                    return null;
            }
        }

        public static bool CheckParamsType<T>(IntPtr L, int begin, int count)
        {
            if (typeof(T) == typeof(object))
            {
                return true;
            }

            for (int i = 0; i < count; i++)
            {
                if (!TypeTraits<T>.Check(L, i + begin))
                {
                    return false;
                }
            }

            return true;
        }

        public static string[] ToStringArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TTABLE:
                    int len = LuaDLL.lua_objlen(L, stackPos);
                    string[] list = new string[len];
                    int pos = LuaDLL.lua_gettop(L) + 1;

                    for (int i = 1; i <= len; i++)
                    {
                        LuaDLL.lua_rawgeti(L, stackPos, i);

                        if (!TypeTraits<string>.Check(L, pos))
                        {
                            LuaDLL.lua_pop(L, 1);
                            LuaDLL.luaL_typerror(L, stackPos, "string[]");
                            return list;
                        }

                        list[i - 1] = StackTraits<string>.To(L, pos);
                        LuaDLL.lua_pop(L, 1);
                    }
                    return list;
                case LuaTypes.LUA_TUSERDATA:
                    return (string[])ToObject(L, stackPos);
                default:
                    return null;
            }
        }


        public static T[] ToStructArray<T>(IntPtr L, int stackPos) where T : struct
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TTABLE:
                    int len = LuaDLL.lua_objlen(L, stackPos);
                    T[] list = new T[len];
                    int pos = LuaDLL.lua_gettop(L) + 1;

                    for (int i = 1; i <= len; i++)
                    {
                        LuaDLL.lua_rawgeti(L, stackPos, i);

                        if (!TypeTraits<T>.Check(L, pos))
                        {
                            LuaDLL.lua_pop(L, 1);
                            LuaDLL.luaL_typerror(L, stackPos, typeof(T[]).FullName);
                            return list;
                        }

                        list[i - 1] = StackTraits<T>.To(L, pos);
                        LuaDLL.lua_pop(L, 1);
                    }
                    return list;
                case LuaTypes.LUA_TUSERDATA:
                    return (T[])ToObject(L, stackPos);
                default:
                    return null;
            }
        }


        public static void PushSealed<T>(IntPtr L, T o)
        {
            if (o == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                int reference = TypeTraits<T>.GetLuaReference(L);

                if (reference <= 0)
                {
                    reference = LoadPreType(L, o.GetType());
                }

                ToLua.PushUserData(L, o, reference);
            }
        }

        static public Type CheckMonoType(IntPtr L, int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    if (obj is Type)
                    {
                        return (Type)obj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("Type expected, got {0}", obj.GetType().FullName));
                }

                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "Type");
            return null;
        }

        public static object[] ToObjectArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TTABLE:
                    int len = LuaDLL.lua_objlen(L, stackPos);
                    object[] list = new object[len];
                    int pos = LuaDLL.lua_gettop(L) + 1;

                    for (int i = 1; i <= len; i++)
                    {
                        LuaDLL.lua_rawgeti(L, stackPos, i);
                        list[i - 1] = ToVarObject(L, pos);
                        LuaDLL.lua_pop(L, 1);
                    }
                    return list;
                case LuaTypes.LUA_TUSERDATA:
                    return (object[])ToObject(L, stackPos);
                default:
                    return null;
            }
        }


        public static T[] ToObjectArray<T>(IntPtr L, int stackPos) where T : class
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TTABLE:
                    int len = LuaDLL.lua_objlen(L, stackPos);
                    T[] list = new T[len];
                    int pos = LuaDLL.lua_gettop(L) + 1;

                    for (int i = 1; i <= len; i++)
                    {
                        LuaDLL.lua_rawgeti(L, stackPos, i);

                        if (!TypeTraits<T>.Check(L, pos))
                        {
                            LuaDLL.lua_pop(L, 1);
                            LuaDLL.luaL_typerror(L, stackPos, typeof(T[]).FullName);
                            return list;
                        }

                        list[i - 1] = StackTraits<T>.To(L, pos);
                        LuaDLL.lua_pop(L, 1);
                    }
                    return list;
                case LuaTypes.LUA_TUSERDATA:
                    return (T[])ToObject(L, stackPos);
                default:
                    return null;
            }
        }

        static Vector2 CheckVector2(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector2)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Vector2", type.ToString());
                return Vector2.zero;
            }

            float x, y;
            LuaDLL.tolua_getvec2(L, stackPos, out x, out y);
            return new Vector2(x, y);
        }

        static Vector4 CheckVector4(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector4)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Vector4", type.ToString());
                return Vector4.zero;
            }

            float x, y, z, w;
            LuaDLL.tolua_getvec4(L, stackPos, out x, out y, out z, out w);
            return new Vector4(x, y, z, w);
        }

        static Color CheckColor(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Color)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Color", type.ToString());
                return Color.black;
            }

            float r, g, b, a;
            LuaDLL.tolua_getclr(L, stackPos, out r, out g, out b, out a);
            return new Color(r, g, b, a);
        }

        static Ray CheckRay(IntPtr L, int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Ray)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Ray", type.ToString());
                return new Ray();
            }

            return ToRay(L, stackPos);
        }

        static Bounds CheckBounds(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Bounds)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Bounds", type.ToString());
                return new Bounds();
            }

            return ToBounds(L, stackPos);
        }

        static LayerMask CheckLayerMask(IntPtr L, int stackPos)
        {
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.LayerMask)
            {
                LuaDLL.luaL_typerror(L, stackPos, "LayerMask", type.ToString());
                return 0;
            }

            return LuaDLL.tolua_getlayermask(L, stackPos);
        }


        public static object CheckVarObject(IntPtr L, int stackPos, Type t)
        {
            bool beValue = TypeChecker.IsValueType(t);            
            LuaTypes luaType = LuaDLL.lua_type(L, stackPos);

            if (!beValue && luaType == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            if (beValue)
            {
                if (TypeChecker.IsNullable(t))
                {
                    if (luaType == LuaTypes.LUA_TNIL)
                    {
                        return null;
                    }

                    Type[] ts = t.GetGenericArguments();
                    t = ts[0];
                }

                if (t == typeof(bool))
                {
                    return LuaDLL.luaL_checkboolean(L, stackPos);
                }
                else if (t == typeof(long))
                {
                    return LuaDLL.tolua_checkint64(L, stackPos);
                }
                else if (t == typeof(ulong))
                {
                    return LuaDLL.tolua_checkuint64(L, stackPos);
                }
                else if (t.IsPrimitive)
                {
                    double d = LuaDLL.luaL_checknumber(L, stackPos);
                    return Convert.ChangeType(d, t);
                }
                else if (t == typeof(Ray))
                {
                    return CheckRay(L, stackPos);
                }
                else if (t == typeof(Bounds))
                {
                    return CheckBounds(L, stackPos);
                }
                else if (t == typeof(LayerMask))
                {
                    return CheckLayerMask(L, stackPos);
                }
                else
                {
                    return CheckObject(L, stackPos, t);
                }
            }
            else
            {
                if (t.IsEnum)
                {
                    return ToLua.CheckObject(L, stackPos, typeof(System.Enum));
                }
                else if ( t == typeof(string))
                {
                    return CheckString(L, stackPos);
                }
                else if (t == typeof(LuaByteBuffer))
                {
                    int len = 0;
                    IntPtr source = LuaDLL.tolua_tolstring(L, stackPos, out len);
                    return new LuaByteBuffer(source, len);
                }
                else
                {
                    return CheckObject(L, stackPos, t);
                }
            }            
        }

        public static UnityEngine.Object CheckUnityObject(IntPtr L, int stackPos, Type type)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);
            object obj = null;

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                obj = translator.GetObject(udata);

                if (obj != null)
                {
                    UnityEngine.Object uObj = (UnityEngine.Object)obj;

                    if (uObj == null)
                    {
                        LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got nil", type.FullName));
                        return null;
                    }

                    Type objType = uObj.GetType();

                    if (type == objType || objType.IsSubclassOf(type))
                    {
                        return uObj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", type.FullName, objType.FullName));
                }

                //传递了tolua.null过来
                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        public static UnityEngine.TrackedReference CheckTrackedReference(IntPtr L, int stackPos, Type type)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);
            object obj = null;

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                obj = translator.GetObject(udata);

                if (obj != null)
                {
                    UnityEngine.TrackedReference uObj = (UnityEngine.TrackedReference)obj;

                    if (uObj == null)
                    {
                        LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got nil", type.FullName));
                        return null;
                    }

                    Type objType = uObj.GetType();

                    if (type == objType || objType.IsSubclassOf(type))
                    {
                        return uObj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", type.FullName, objType.FullName));
                }

                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        //必须检测类型
        public static object[] CheckObjectArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                object val = null;                
                List<object> list = new List<object>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray(); ;
                    }

                    val = ToVarObject(L, -1);
                    list.Add(val);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (object[])CheckObject(L, stackPos, typeof(object[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "object[] or table");
            return null;
        }

        public static T[] CheckObjectArray<T>(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;                
                Type t = typeof(T);
                List<T> list = new List<T>();
                LuaDLL.lua_pushvalue(L, stackPos);                

                while(true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray(); ;
                    }
                    else if (!TypeChecker.CheckType(L, t, -1))
                    {
                        LuaDLL.lua_pop(L, 1);
                        break;
                    }

                    list.Add((T)ToVarObject(L, -1));                    
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (T[])CheckObject(L, stackPos, typeof(T[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, typeof(T[]).FullName);            
            return null;
        }

        public static char[] CheckCharBuffer(IntPtr L, int stackPos)
        {
            if (LuaDLL.lua_isstring(L, stackPos) != 0)
            {                
                string str = LuaDLL.lua_tostring(L, stackPos);
                return str.ToCharArray(); ;
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) != 0)
            {
                return (char[])CheckObject(L, stackPos, typeof(char[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string or char[]");
            return null;
        }

        public static byte[] CheckByteBuffer(IntPtr L, int stackPos)
        {            
            if (LuaDLL.lua_isstring(L, stackPos) != 0)
            {
                int len;
                IntPtr source = LuaDLL.lua_tolstring(L, stackPos, out len);
                byte[] buffer = new byte[len];
                Marshal.Copy(source, buffer, 0, len);                
                return buffer;
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) != 0)
            {
                return (byte[])CheckObject(L, stackPos, typeof(byte[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string or byte[]");
            return null;
        }

        public static T[] CheckNumberArray<T>(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                T ret = default(T);
                List<T> list = new List<T>();
                LuaDLL.lua_pushvalue(L, stackPos);
                Type type = typeof(T);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray();
                    }
                    else if (luatype != LuaTypes.LUA_TNUMBER)
                    {
                        break;
                    }

                    ret = (T)Convert.ChangeType(LuaDLL.lua_tonumber(L, -1), type);                    
                    list.Add(ret);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (T[])CheckObject(L, stackPos, typeof(T[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, LuaMisc.GetTypeName(typeof(T[])));
            return null;
        }
        

        public static bool[] CheckBoolArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;                
                Type t = typeof(bool);
                List<bool> list = new List<bool>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray();
                    }
                    else if (!TypeChecker.CheckType(L, t, -1))
                    {
                        LuaDLL.lua_pop(L, 1);
                        break;
                    }

                    list.Add(LuaDLL.lua_toboolean(L, -1));                    
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (bool[])CheckObject(L, stackPos, typeof(bool[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "bool[]");
            return null;                    
        }

        public static string[] CheckStringArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                string retVal = null;
                Type t = typeof(string);
                List<string> list = new List<string>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray();
                    }
                    else if (!TypeChecker.CheckType(L, t, -1))
                    {
                        LuaDLL.lua_pop(L, 1);
                        break;
                    }

                    retVal = ToString(L, -1);
                    list.Add(retVal);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (string[])CheckObject(L, stackPos, typeof(string[]));                                
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string[]");            
            return null;
        }

        public static object CheckGenericObject(IntPtr L, int stackPos, Type type, out Type ArgType)
        {
            object obj = ToLua.ToObject(L, 1);
            Type t = obj.GetType();
            ArgType = null;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == type)
            {
                Type[] ts = t.GetGenericArguments();
                ArgType = ts[0];
                return obj;
            }

            LuaDLL.luaL_argerror(L, stackPos, LuaMisc.GetTypeName(type));
            return null;
        }

        public static object CheckGenericObject(IntPtr L, int stackPos, Type type, out Type t1, out Type t2)
        {
            object obj = ToLua.ToObject(L, 1);
            Type t = obj.GetType();
            t1 = null;
            t2 = null;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == type)
            {
                Type[] ts = t.GetGenericArguments();
                t1 = ts[0];
                t2 = ts[1];
                return obj;
            }

            LuaDLL.luaL_argerror(L, stackPos, LuaMisc.GetTypeName(type));
            return null;
        }

        public static object CheckGenericObject(IntPtr L, int stackPos, Type type)
        {
            object obj = ToLua.ToObject(L, 1);
            Type t = obj.GetType();

            if (t.IsGenericType && t.GetGenericTypeDefinition() == type)
            {
                return obj;
            }

            LuaDLL.luaL_argerror(L, stackPos, LuaMisc.GetTypeName(type));
            return null;
        }

        public static object[] ToParamsObject(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            object[] list = new object[count];
            int pos = 0;

            while (pos < count)
            {
                list[pos++] = ToVarObject(L, stackPos++);                                                
            }

            return list;
        }

        public static T[] ToParamsObject<T>(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            T[] list = new T[count];
            Type type = typeof(T);
            int pos = 0; 

            while (pos < count)
            {
                object temp = ToVarObject(L, stackPos++);                
                list[pos++] = TypeChecker.ChangeType<T>(temp, type);
            }

            return list;
        }

        public static string[] ToParamsString(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            string[] list = new string[count];
            int pos = 0;   

            while (pos < count)
            {
                list[pos++] = ToString(L, stackPos++);                       
            }

            return list;
        }

        public static T[] ToParamsNumber<T>(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            T[] list = new T[count];
            Type type = typeof(T);
            int pos = 0;       

            while (pos < count)
            {
                double d = LuaDLL.lua_tonumber(L, stackPos++);
                list[pos++] = (T)Convert.ChangeType(d, type);                
            }

            return list;
        }

        public static char[] ToParamsChar(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            char[] list = new char[count];
            int pos = 0;

            while (pos < count)
            {
                list[pos++] = (char)LuaDLL.lua_tointeger(L, stackPos++);                                 
            }

            return list;
        }

        public static bool[] CheckParamsBool(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            bool[] list = new bool[count];
            int pos = 0;

            while (pos < count)
            {
                list[pos++] = LuaDLL.luaL_checkboolean(L, stackPos++);                
            }

            return list;
        }

        public static T[] CheckParamsNumber<T>(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            T[] list = new T[count];                        
            Type type = typeof(T);
            int pos = 0;

            while (pos < count)
            {
                double d = LuaDLL.luaL_checknumber(L, stackPos++);
                list[pos++] = (T)Convert.ChangeType(d, type);                
            }

            return list;
        }

        public static char[] CheckParamsChar(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            char[] list = new char[count];
            int pos = 0;

            while (pos < count)
            {
                list[pos++] = (char)LuaDLL.luaL_checkinteger(L, stackPos++);                
            }

            return list;
        }

        public static string[] CheckParamsString(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            string[] list = new string[count];                        
            int pos = 0;

            while (pos < count)
            {
                list[pos++] = CheckString(L, stackPos++);                                                
            }

            return list;
        }

        public static T[] CheckParamsObject<T>(IntPtr L, int stackPos, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            T[] list = new T[count];
            Type type = typeof(T);
            int pos = 0;

            while (pos < count)
            {
                object temp = CheckVarObject(L, stackPos++, type);
                list[pos++] = TypeChecker.ChangeType<T>(temp, type);
            }

            return list;
        }

        public static void Push(IntPtr L, Ray ray)
        {
            LuaStatic.GetPackRay(L);
            Push(L, ray.direction);
            Push(L, ray.origin);

            if (LuaDLL.lua_pcall(L, 2, 1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void Push(IntPtr L, Bounds bound)
        {                        
            LuaStatic.GetPackBounds(L);
            Push(L, bound.center);
            Push(L, bound.size);

            if (LuaDLL.lua_pcall(L, 2, 1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void Push(IntPtr L, RaycastHit hit)
        {
            Push(L, hit, RaycastBits.ALL);
        }

        public static void Push(IntPtr L, RaycastHit hit, int flag)
        {                        
            LuaStatic.GetPackRaycastHit(L);

            if ((flag & RaycastBits.Collider) != 0)
            {
                Push(L, hit.collider);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            LuaDLL.lua_pushnumber(L, hit.distance);

            if ((flag & RaycastBits.Normal) != 0)
            {
                Push(L, hit.normal);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & RaycastBits.Point) != 0)
            {
                Push(L, hit.point);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & RaycastBits.Rigidbody) != 0)
            {            
                Push(L, hit.rigidbody);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & RaycastBits.Transform) != 0)
            {
                Push(L, hit.transform);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if (LuaDLL.lua_pcall(L, 6, 1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void Push(IntPtr L, Touch t)
        {
            Push(L, t, TouchBits.ALL);
        }

        public static void Push(IntPtr L, Touch t, int flag)
        {                                    
            LuaStatic.GetPackTouch(L);
            LuaDLL.lua_pushinteger(L, t.fingerId);

            if ((flag & TouchBits.Position) != 0)
            {
                Push(L, t.position);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & TouchBits.RawPosition) != 0)
            {
                Push(L, t.rawPosition);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & TouchBits.DeltaPosition) != 0)
            {
                Push(L, t.deltaPosition);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            LuaDLL.lua_pushnumber(L, t.deltaTime);
            LuaDLL.lua_pushinteger(L, t.tapCount);
            LuaDLL.lua_pushinteger(L, (int)t.phase);

            if (LuaDLL.lua_pcall(L, 7, -1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void PushLayerMask(IntPtr L, LayerMask l)
        {
            LuaDLL.tolua_pushlayermask(L, l.value);
        }

        public static void Push(IntPtr L, LuaByteBuffer bb)
        {            
            LuaDLL.lua_pushlstring(L, bb.buffer, bb.buffer.Length);
        }

        public static void PushByteBuffer(IntPtr L, byte[] buffer)
        {
            LuaDLL.tolua_pushlstring(L, buffer, buffer.Length);
        }

        public static void Push(IntPtr L, Array array)
        {
            if (array == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                int arrayMetaTable = LuaStatic.GetArrayMetatable(L);
                PushUserData(L, array, arrayMetaTable);
            }
        }

        public static void Push(IntPtr L, LuaBaseRef lbr)
        {
            if (lbr == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                LuaDLL.lua_getref(L, lbr.GetReference());
            }
        }

        public static void Push(IntPtr L, Type t)
        {
            if (t == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                int typeMetatable = LuaStatic.GetTypeMetatable(L);
                PushUserData(L, t, typeMetatable);
            }
        }

        public static void Push(IntPtr L, Delegate ev)
        {            
            if (ev == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                int delegateMetatable = LuaStatic.GetDelegateMetatable(L);
                PushUserData(L, ev, delegateMetatable);
            }
        }

        public static void Push(IntPtr L, EventObject ev)
        {
            if (ev == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                int eventMetatable = LuaStatic.GetEventMetatable(L);                
                PushUserData(L, ev, eventMetatable);
            }
        }

        public static void Push(IntPtr L, IEnumerator iter)
        {
            if (iter == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                int reference = LuaStatic.GetMetaReference(L, iter.GetType());

                if (reference > 0)
                {
                    PushUserData(L, iter, reference);
                }
                else
                {
                    int iterMetatable = LuaStatic.GetIterMetatable(L);
                    PushUserData(L, iter, iterMetatable);
                }            
            }
        }

        public static IEnumerator CheckIter(IntPtr L, int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    if (obj is IEnumerator)
                    {
                        return (IEnumerator)obj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("Type expected, got {0}", obj.GetType().FullName));
                }

                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "Type");
            return null;
        }

        public static void Push(IntPtr L, System.Enum e)
        {
            if (e == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                object obj = null;
                int enumMetatable = LuaStatic.GetEnumObject(L, e, out obj);
                PushUserData(L, obj, enumMetatable);
            }
        }

        //基础类型获取需要一个函数
        public static void PushOut<T>(IntPtr L, LuaOut<T> lo)
        {
            ObjectTranslator translator = ObjectTranslator.Get(L);
            int index = translator.AddObject(lo);
            int outMetatable = LuaStatic.GetOutMetatable(L);
            LuaDLL.tolua_pushnewudata(L, outMetatable, index);
        }

        public static void PushStruct(IntPtr L, object o)
        {
            if (o == null || o.Equals(null))
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            if (o is Enum)
            {
                ToLua.Push(L, (Enum)o);
                return;
            }

            Type type = o.GetType();
            int reference = LuaStatic.GetMetaReference(L, type);

            if (reference <= 0)
            {
                reference = LoadPreType(L, type);
            }

            ObjectTranslator translator = ObjectTranslator.Get(L);
            int index = translator.AddObject(o);
            LuaDLL.tolua_pushnewudata(L, reference, index);
        }

        public static int LoadPreType(IntPtr L, Type type)
        {
            LuaCSFunction LuaOpenLib = LuaStatic.GetPreModule(L, type);
            int reference = -1;

            if (LuaOpenLib != null)
            {
#if UNITY_EDITOR
                Debugger.LogWarning("register PreLoad type {0} to lua", LuaMisc.GetTypeName(type));
#endif
                reference = LuaPCall(L, LuaOpenLib);
            }
            else
            {
                //类型未Wrap                            
                reference = LuaStatic.GetMissMetaReference(L, type);
            }

            return reference;
        }

        

        public static void PushValue(IntPtr L, ValueType v)
        {
            if (v == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            Type type = v.GetType();
            int reference = LuaStatic.GetMetaReference(L, type);            

            if (reference <= 0)
            {
                LuaCSFunction LuaOpenLib = LuaStatic.GetPreModule(L, type);

                if (LuaOpenLib != null)
                {
#if UNITY_EDITOR
                    Debugger.LogWarning("register PreLoad type {0} to lua", LuaMisc.GetTypeName(type));
#endif
                    reference = LuaPCall(L, LuaOpenLib);                    
                }
                else
                {
                    //类型未Wrap                            
                    reference = LuaStatic.GetMissMetaReference(L, type);
                }
            }

            ObjectTranslator translator = ObjectTranslator.Get(L);
            int index = translator.AddObject(v);
            LuaDLL.tolua_pushnewudata(L, reference, index);
        }


        public  static void PushUserData(IntPtr L, object o, int reference)
        {
            int index;
            ObjectTranslator translator = ObjectTranslator.Get(L);

            if (translator.Getudata(o, out index))
            {
                if (LuaDLL.tolua_pushudata(L, index))
                {
                    return;
                }
            }

            index = translator.AddObject(o);
            LuaDLL.tolua_pushnewudata(L, reference, index);
        }

        static int LuaPCall(IntPtr L, LuaCSFunction func)
        {
            int top = LuaDLL.lua_gettop(L);
            LuaDLL.tolua_pushcfunction(L, func);

            if (LuaDLL.lua_pcall(L, 0, -1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_settop(L, top);
                throw new LuaException(error, LuaException.GetLastError());
            }

            int reference = LuaDLL.tolua_getclassref(L, -1);              
            LuaDLL.lua_settop(L, top);
            return reference;
        }

        static void PushPreLoadType(IntPtr L, object o, Type type)
        {
            LuaCSFunction LuaOpenLib = LuaStatic.GetPreModule(L, type);
            int reference = -1;

            if (LuaOpenLib != null)
            {
#if UNITY_EDITOR
                Debugger.LogWarning("register PreLoad type {0} to lua", LuaMisc.GetTypeName(type));
#endif
                reference = LuaPCall(L, LuaOpenLib);                
            }
            else
            {
                //类型未Wrap                            
                reference = LuaStatic.GetMissMetaReference(L, type);
            }

            if (reference > 0)
            {
                PushUserData(L, o, reference);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }
        }
        
        //o 不为 null
        static void PushUserObject(IntPtr L, object o)
        {
            Type type = o.GetType();
            int reference = LuaStatic.GetMetaReference(L, type);

            if (reference > 0)
            {
                PushUserData(L, o, reference);
            }
            else
            {
                PushPreLoadType(L, o, type);
            }
        }

        public static void Push(IntPtr L, UnityEngine.Object obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserObject(L, obj);
            }
        }

        public static void Push(IntPtr L, UnityEngine.TrackedReference obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserObject(L, obj);
            }
        }

        public static void PushObject(IntPtr L, object o)
        {
            if (o == null || o.Equals(null))
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserObject(L, o);
            }
        }

        /*static void PushNull(IntPtr L)
        {
            LuaDLL.tolua_pushudata(L, 1);
        }*/

        //PushVarObject
        public static void Push(IntPtr L, object obj)
        {
            if (obj == null || obj.Equals(null))
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            Type t = obj.GetType();

            if (t.IsValueType)
            {
                if (t == typeof(bool))
                {
                    bool b = (bool)obj;
                    LuaDLL.lua_pushboolean(L, b);
                }
                else if (t.IsEnum)
                {
                    Push(L, (System.Enum)obj);
                }
                else if (t == typeof(long))
                {
                    LuaDLL.tolua_pushint64(L, (long)obj);
                }
                else if (t == typeof(ulong))
                {
                    LuaDLL.tolua_pushuint64(L, (ulong)obj);
                }
                else if (t.IsPrimitive)
                {
                    double d = LuaMisc.ToDouble(obj);
                    LuaDLL.lua_pushnumber(L, d);
                }
                else if (t == typeof(Quaternion))
                {
                    Push(L, (Quaternion)obj);
                }
                else if (t == typeof(RaycastHit))
                {
                    Push(L, (RaycastHit)obj);
                }
                else if (t == typeof(Touch))
                {
                    Push(L, (Touch)obj);
                }
                else if (t == typeof(Ray))
                {
                    Push(L, (Ray)obj);
                }
                else if (t == typeof(Bounds))
                {
                    Push(L, (Bounds)obj);
                }
                else if (t == typeof(LayerMask))
                {
                    PushLayerMask(L, (LayerMask)obj);
                }
                else
                {
                    PushValue(L, (ValueType)obj);                    
                }
            }
            else
            {
                if (t.IsArray)
                {
                    Push(L, (Array)obj);
                }
                else if(t == typeof(string))
                {
                    LuaDLL.lua_pushstring(L, (string)obj);
                }
                else if (t.IsSubclassOf(typeof(LuaBaseRef)))
                {
                    Push(L, (LuaBaseRef)obj);
                }
                else if (t.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    Push(L, (UnityEngine.Object)obj);
                }
                else if (t.IsSubclassOf(typeof(UnityEngine.TrackedReference)))
                {
                    Push(L, (UnityEngine.TrackedReference)obj);
                }
                else if (t == typeof(LuaByteBuffer))
                {
                    LuaByteBuffer lbb = (LuaByteBuffer)obj;
                    LuaDLL.lua_pushlstring(L, lbb.buffer, lbb.buffer.Length);
                }
                else if (t.IsSubclassOf(typeof(Delegate)))
                {
                    Push(L, (Delegate)obj);
                }
                else if (obj is System.Collections.IEnumerator)
                {
                    Push(L, (IEnumerator)obj);
                }
                else if (t == typeof(EventObject))
                {
                    Push(L, (EventObject)obj);
                }
                else if (t == monoType)
                {
                    Push(L, (Type)obj);
                }
                else
                {
                    PushObject(L, obj);                    
                }
            }
        }

        public static void SetBack(IntPtr L, int stackPos, object o)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);
            ObjectTranslator translator = ObjectTranslator.Get(L);

            if (udata != -1)
            {
                translator.SetBack(udata, o);
            }
        }

        public static int Destroy(IntPtr L)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, 1);
            ObjectTranslator translator = ObjectTranslator.Get(L);
            translator.Destroy(udata);
            return 0;
        }

        public static void CheckArgsCount(IntPtr L, string method, int count)
        {
            int c = LuaDLL.lua_gettop(L);

            if (c != count)
            {
                throw new LuaException(string.Format("no overload for method '{0}' takes '{1}' arguments", method, c));
            }
        }

        public static void CheckArgsCount(IntPtr L, int count)
        {
            int c = LuaDLL.lua_gettop(L);

            if (c != count)
            {
                throw new LuaException(string.Format("no overload for method takes '{0}' arguments", c));
            }
        }

        public static Delegate CheckDelegate(Type t, IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TFUNCTION:
                    LuaFunction func = ToLua.ToLuaFunction(L, stackPos);
                    return DelegateFactory.CreateDelegate(t, func);
                case LuaTypes.LUA_TUSERDATA:
                    return (Delegate)ToLua.CheckObject(L, stackPos, t);
                default:
                    LuaDLL.luaL_typerror(L, stackPos, LuaMisc.GetTypeName(t));
                    return null;
            }
        }

        public static Delegate CheckDelegate<T>(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            switch (luatype)
            {
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TFUNCTION:
                    LuaFunction func = ToLua.ToLuaFunction(L, stackPos);
                    return DelegateTraits<T>.Create(func);
                case LuaTypes.LUA_TUSERDATA:
                    return (Delegate)ToLua.CheckObject(L, stackPos, typeof(T));
                default:
                    LuaDLL.luaL_typerror(L, stackPos, TypeTraits<T>.GetTypeName());
                    return null;
            }
        }
    }
}