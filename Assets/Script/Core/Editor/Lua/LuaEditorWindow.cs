using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;


public class LuaEditorWindow : EditorWindow
{
    const string c_LuaFilePath = "Lua";
    const string c_LuaLibFilePath = "LuaLib";
    const string c_LuaProtobufFilePath = "Lua/protobuf";

    List<string> m_LuaFileList;
    List<string> m_LuaLibFileList;
    List<string> m_LuaProtoBufFileList;
    Dictionary<string, SingleField> m_luaConfig;

    [MenuItem("Window/Lua设置编辑器 &7")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LuaEditorWindow));
    }

    void OnEnable()
    {
        EditorGUIStyleData.Init();
#if USE_LUA
        EditorCoroutineRunner.StartEditorCoroutine(LoadLuaConfig());
#endif
    }



    #region GUI


    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }


    void OnGUI()
    {
        titleContent.text = "Lua设置编辑器";
        if (GetIsInit())
        {
            if (GUILayout.Button("关闭LUA"))
            {
                EditorExpand.ChangeDefine(new string[] { }, new string[] { "USE_LUA" });
                UnityEditor.AssetDatabase.Refresh();
                //Close();
            }

            //if (GUILayout.Button("LUA重新初始化(会清除所有Lua文件,做好保存,慎重！！！)"))
            //{
            //    #if USE_LUA
            //            //InitLua();
            //    #endif
            //}

 



            EditorGUILayout.Space();
#if USE_LUA
            LuaFileGUI();
            LuaWarpFileGUI();
            AutoLuaConfigGUI();
#endif

        }
        else
        {

            if (GUILayout.Button("打开LUA并初始化"))
            {
                EditorExpand.ChangeDefine(new string[] { "USE_LUA" }, new string[] { });
      
                UnityEditor.AssetDatabase.Refresh();
                //Close();
            }


        }



    }

#endregion

#region 项目Lua初始化

    bool GetIsInit()
    {
#if USE_LUA
        return true;
#else
        return false;
#endif
    }

    void InitLuaGUI()
    {
        if (GUILayout.Button("Lua 项目初始化"))
        {
            //InitLua();
        }
    }

#if !USE_LUA
}
#endif

#if USE_LUA

    void InitLua()
    {                        

        FileTool.CreatFilePath(CustomSettings.saveDir);
        FileTool.DeleteDirectory(CustomSettings.saveDir);

        //创建导出列表文件
        CreateLuaExportFile();

        //CreateLuaBinder();
        //创建空LuaBinder文件
        CreateEmptyLuaBinder();

        ToLuaMenu.ClearLuaFiles();
        //创建Lua目录
        Directory.CreateDirectory(PathTool.GetAbsolutePath(ResLoadLocation.Resource, c_LuaLibFilePath));
        Directory.CreateDirectory(PathTool.GetAbsolutePath(ResLoadLocation.Resource, c_LuaFilePath));

        string resPath = Application.dataPath + "/Script/Core/Editor/res/LuaLib";
        string aimPath = Application.dataPath + "/Resources/LuaLib";

        FileTool.DeleteDirectory(aimPath);
        //复制lua初始库文件
        FileTool.CopyDirectory(resPath, aimPath);


        string pluginsResPath = Application.dataPath + "/Script/Core/Lua/ToLua/Plugins";
        string pluginsPath = Application.dataPath + "/Lua/Plugins";
        //拷贝LuaPlugins文件
        //  FileTool.CopyDirectory(pluginsResPath, pluginsPath);

        //修改文件名 dllx -> dll
        //FileTool.ChangeFileName(pluginsPath + "/x86/tolua.dllx", pluginsPath + "/x86/tolua.dll");
        //FileTool.ChangeFileName(pluginsPath + "/x86_64/tolua.dllx", pluginsPath + "/x86_64/tolua.dll");

        //FileTool.ChangeFileName(pluginsPath + "/CString.dllx", pluginsPath + "/x86_64/CString.dll");
        //FileTool.ChangeFileName(pluginsPath + "/Debugger.dllx", pluginsPath + "/x86_64/Debugger.dll");

        //初始Warp
        ToLuaMenu.GenLuaAll();

        //创建启动文件
        string luaMainPath = Application.dataPath + "/Resources/Lua/luaMain.txt";
        string content = "print(\"lua is launch!\");";

        ResourceIOTool.WriteStringByFile(luaMainPath, content);

        AssetDatabase.Refresh();

        //自动生成Lua配置文件
        GetLuaFileList();
        SaveLuaConfig();

        string info = "Lua初始化完成，\n";
        info += "请先生成luaWarp文件 （Window -> Lua设置管理器 -> 重新生成Lua Warp脚本）\n";
        info += "再重新生成打包设置（Window -> 打包设置管理器 -> 自动添加Resources目录下的资源并保存）\n";

      //  Debug.Log(info);
    }

    void CreateLuaExportFile()
    {
        string LoadPath = Application.dataPath + "/Script/Core/Editor/res/LuaExportListTemplate.txt";
        string SavePath = Application.dataPath + "/Script/UI/Editor/Lua/LuaExportList.cs";

        string ExportListContent = ResourceIOTool.ReadStringByFile(LoadPath);

        EditorUtil.WriteStringByFile(SavePath, ExportListContent);
    }

    static void CreateEmptyLuaBinder()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using LuaInterface;");
        sb.AppendLine();
        sb.AppendLine("public static class LuaBinder");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic static void Bind(LuaState L)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tthrow new LuaException(\"Please generate LuaBinder files first!\");");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        string filePath = CustomSettings.saveDir + "/LuaBinder.cs";

        EditorUtil.WriteStringByFile(filePath, sb.ToString());

    }
#endif
#endregion
#if USE_LUA

#region 读取Lua配置
IEnumerator LoadLuaConfig()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (ConfigManager.GetIsExistConfig(LuaManager.c_LuaConfigName))
        {
            m_luaConfig = ConfigManager.GetData(LuaManager.c_LuaConfigName);
        }
        else
        {
            m_luaConfig = new Dictionary<string, SingleField>();
        }

        LoadLuaList();
    }

    void LoadLuaList()
    {
        if (m_luaConfig.ContainsKey(LuaManager.c_LuaListKey))
        {
            m_LuaFileList = new List<string>();
            m_LuaLibFileList = new List<string>();

            m_LuaFileList.AddRange(m_luaConfig[LuaManager.c_LuaListKey].GetStringArray());
            m_LuaLibFileList.AddRange(m_luaConfig[LuaManager.c_LuaLibraryListKey].GetStringArray());
        }
        else
        {
            m_LuaFileList = new List<string>();
            m_LuaLibFileList = new List<string>();
            m_LuaProtoBufFileList = new List<string>();
        }
    }
#endregion

#region Lua信息检视

    bool m_isFold = false;
    bool m_isFoldLib = false;
    bool m_isFoldprotobuf = false;
    Vector2 m_pos = Vector2.zero;
    Vector2 m_posLib = Vector2.zero;
    Vector2 m_posProbuf = Vector2.zero;
    void LuaFileGUI()
    {
        m_isFoldLib = EditorGUILayout.Foldout(m_isFoldLib, "Lua库列表");

        if (m_isFoldLib)
        {
            m_posLib = EditorGUILayout.BeginScrollView(m_posLib,GUILayout.ExpandHeight(false));
            EditorGUI.indentLevel = 1;
            if (m_LuaLibFileList != null)
            {
                for (int i = 0; i < m_LuaLibFileList.Count; i++)
                {
                    EditorGUILayout.LabelField(m_LuaLibFileList[i]);
                }
            }
            EditorGUILayout.EndScrollView();
        }


        m_isFoldprotobuf = EditorGUILayout.Foldout(m_isFoldprotobuf, "LuaProtobuf列表");

        if (m_isFoldprotobuf)
        {
            m_posProbuf = EditorGUILayout.BeginScrollView(m_posProbuf, GUILayout.ExpandHeight(false));
            EditorGUI.indentLevel = 1;
            if (m_LuaProtoBufFileList != null)
            {
                for (int i = 0; i < m_LuaProtoBufFileList.Count; i++)
                {
                    EditorGUILayout.LabelField(m_LuaProtoBufFileList[i]);
                }
            }
            EditorGUILayout.EndScrollView();
        }


        EditorGUI.indentLevel = 0;
        m_isFold = EditorGUILayout.Foldout(m_isFold, "Lua列表");
        m_pos = EditorGUILayout.BeginScrollView(m_pos);
        if (m_isFold)
        {
            EditorGUI.indentLevel = 1;

            
            if (m_LuaFileList != null)
            {
                for (int i = 0; i < m_LuaFileList.Count; i++)
                {
                    EditorGUILayout.LabelField(m_LuaFileList[i]);
                }
            }
            
        }
                

        EditorGUILayout.EndScrollView();
        EditorGUI.indentLevel--;
    }

#endregion

#region ULua原生功能

    void LuaWarpFileGUI()
    {
        if (!File.Exists(CustomSettings.saveDir + "/LuaBinderCatch.cs"))
        {
            if (GUILayout.Button("清除Lua Warp脚本(如果有增删class最好先清除脚本)"))
            {
                FileTool.CreatFilePath(CustomSettings.saveDir);
                FileTool.DeleteDirectory(CustomSettings.saveDir);
                //    CreateLuaBinder();
                ToLuaMenu.ClearLuaWraps();
                CreateLuaExportFile();
                CreateEmptyLuaBinder();
                AssetDatabase.Refresh();

            }
        }

        if (GUILayout.Button("重新生成Lua Warp脚本"))
        {
            FileTool.CreatPath(CustomSettings.saveDir);
            FileTool.DeleteDirectory(CustomSettings.saveDir);                                        
            //CreateEmptyLuaBinder();
          //  ToLuaMenu.ClearLuaFiles();
            EditorCoroutineRunner.StartEditorCoroutine(GenLuaEvent()) ;

        }
    }

    IEnumerator GenLuaEvent()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        ToLuaMenu.GenLuaAll();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成LuaBinder文件
    /// </summary>
    void CreateLuaBinder()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using LuaInterface;");
        sb.AppendLine();
        sb.AppendLine("public static class LuaBinder");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic static void Bind(LuaState L)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tthrow new LuaException(\"Please generate LuaBinder files first!\");");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        EditorUtil.WriteStringByFile(CustomSettings.saveDir + "/LuaBinderCatch.cs", sb.ToString());
    }

#endregion

#region 自动生成Lua设置

    void AutoLuaConfigGUI()
    {
        if(GUILayout.Button("自动生成Lua配置文件"))
        {
            GetLuaFileList();
            SaveLuaConfig();
        }
    }

    void SaveLuaConfig()
    {
        //Lua库文件
        string luaLibConfig = "";
        for (int i = 0; i < m_LuaLibFileList.Count; i++)
        {
            luaLibConfig += m_LuaLibFileList[i];
            if (i != m_LuaLibFileList.Count - 1)
            {
                luaLibConfig += "|";
            }
        }

        //Lua文件
        string luaConfig = "";
        for (int i = 0; i < m_LuaFileList.Count; i++)
        {
            luaConfig += m_LuaFileList[i];
            if (i != m_LuaFileList.Count -1)
            {
                luaConfig += "|";
            }
        }

        //Luaprotorbuf文件
        string luaProtobufConfig = "";
        for (int i = 0; i < m_LuaProtoBufFileList.Count; i++)
        {
            luaProtobufConfig += m_LuaProtoBufFileList[i];
            if (i != m_LuaProtoBufFileList.Count - 1)
            {
                luaProtobufConfig += "|";
            }
        }

        if (!m_luaConfig.ContainsKey(LuaManager.c_LuaListKey))
        {
            m_luaConfig.Add(LuaManager.c_LuaListKey, new SingleField(luaConfig));
        }
        else
        {
            m_luaConfig[LuaManager.c_LuaListKey].m_content = luaConfig;
        }

        if (!m_luaConfig.ContainsKey(LuaManager.c_LuaLibraryListKey))
        {
            m_luaConfig.Add(LuaManager.c_LuaLibraryListKey, new SingleField(luaLibConfig));
        }
        else
        {
            m_luaConfig[LuaManager.c_LuaLibraryListKey].m_content = luaLibConfig;
        }

        if (!m_luaConfig.ContainsKey(LuaManager.c_LuaProtobufListKey))
        {
            m_luaConfig.Add(LuaManager.c_LuaProtobufListKey, new SingleField(luaProtobufConfig));
        }
        else
        {
            m_luaConfig[LuaManager.c_LuaProtobufListKey].m_content = luaProtobufConfig;
        }

        ConfigEditorWindow.SaveData(LuaManager.c_LuaConfigName, m_luaConfig);
    }

    void GetLuaFileList()
    {

        string luaConfigPath = PathTool.GetAbsolutePath(ResLoadLocation.Resource, PathTool.GetRelativelyPath(
                                      ConfigManager.c_directoryName,
                                      LuaManager.c_LuaConfigName,
                                      "json"));
        FileTool.DeleteFile(luaConfigPath);
        AssetDatabase.Refresh();

        m_LuaLibFileList = new List<string>();
        m_LuaFileList= new List<string>();
        m_LuaProtoBufFileList = new List<string>();

        FindLuaProtobufPBFile(PathTool.GetAbsolutePath(ResLoadLocation.Resource, c_LuaProtobufFilePath));
        FindLuaLibFile(PathTool.GetAbsolutePath(ResLoadLocation.Resource, c_LuaLibFilePath));
        FindLuaFile(PathTool.GetAbsolutePath(ResLoadLocation.Resource, c_LuaFilePath));


    }

    public void FindLuaFile(string path)
    {
        string[] allUIPrefabName = Directory.GetFiles(path);
        foreach (var item in allUIPrefabName)
        {
            if (item.EndsWith(".txt"))
            {
                string configName = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(item));
                m_LuaFileList.Add(configName);
            }
        }

        string[] dires = Directory.GetDirectories(path);
        for (int i = 0; i < dires.Length; i++)
        {
            FindLuaFile(dires[i]);
        }
    }

    public void FindLuaLibFile(string path)
    {
        string[] allUIPrefabName = Directory.GetFiles(path);
        foreach (var item in allUIPrefabName)
        {
            if (item.EndsWith(".txt"))
            {
                string configName = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(item));
                m_LuaLibFileList.Add(configName);
            }
        }

        string[] dires = Directory.GetDirectories(path);
        for (int i = 0; i < dires.Length; i++)
        {
            FindLuaLibFile(dires[i]);
        }
    }


    public void FindLuaProtobufPBFile(string path)
    {
        string[] allUIPrefabName = Directory.GetFiles(path);
        List<string> protoTxtList = new List<string>();
        foreach (var item in allUIPrefabName)
        {
            string configName = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(item));
            if (item.EndsWith(".lua"))
            {        
                m_LuaProtoBufFileList.Add(configName);
                //
            }
            else if (item.EndsWith(".txt"))
            {
                protoTxtList.Add(configName);
            }
        }

        for (int i = 0; i < protoTxtList.Count; i ++ )
        {
            string filePath = Application.dataPath + "/Resources/Lua/protobuf/" + protoTxtList[i] + ".txt";
            FileTool.DeleteFile(filePath);
        }

        for (int i = 0; i < m_LuaProtoBufFileList.Count; i++)
        {
            CreateProtobufTxt(m_LuaProtoBufFileList[i]);
        }


        for (int i = 0; i < m_LuaProtoBufFileList.Count; i++)
        {
            string filePath = Application.dataPath + "/Resources/Lua/protobuf/" + m_LuaProtoBufFileList[i] + ".lua";
            FileTool.DeleteFile(filePath);
        }

    }

    public static void CreateProtobufTxt(string protobufName)
    {
       
        string LoadPath = Application.dataPath + "/Resources/Lua/protobuf/" + protobufName + ".lua";
        protobufName += "t";
        string SavePath = Application.dataPath + "/Resources/Lua/protobuf/" + protobufName + ".txt";

        string UItemplate = ResourceIOTool.ReadStringByFile(LoadPath);
     //   string classContent = UItemplate.Replace("{0}", protobufName);

        EditorUtil.WriteStringByFile(SavePath, UItemplate);

        AssetDatabase.Refresh();
    }

    #endregion
}

#endif