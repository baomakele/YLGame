
using Google.Protobuf;
using Protocol;
using System;
using System.Collections.Generic;

namespace Proto
{
   public class ProtoDic
   {
       private static Dictionary<Type,int> _protoTypeIdDic = new Dictionary<Type,int>()
      {
            {typeof(C2S_Login_Req),1001},
            {typeof(S2C_Login_Res),1002},
            {typeof(C2S_Create_Role_Req),2001},
            {typeof(S2C_Create_Role_Res),2002},
            {typeof(C2S_Role_Heartbeat_Req),2003},
            {typeof(S2C_Role_Heartbeat_Res),2004},
        };

      private static Dictionary<int,Type>_protoIdTypeDic = new Dictionary<int,Type>()
      {
            {1001,typeof(C2S_Login_Req)},
            {1002,typeof(S2C_Login_Res)},
            {2001,typeof(C2S_Create_Role_Req)},
            {2002,typeof(S2C_Create_Role_Res)},
            {2003,typeof(C2S_Role_Heartbeat_Req)},
            {2004,typeof(S2C_Role_Heartbeat_Res)},
       };

       private static Dictionary<string,Type>_protoNameTypeDic = new Dictionary<string,Type>()
      {
            {"C2S_Login_Req_MSG",typeof(C2S_Login_Req)},
            {"S2C_Login_Res_MSG",typeof(S2C_Login_Res)},
            {"C2S_Create_Role_Req_MSG",typeof(C2S_Create_Role_Req)},
            {"S2C_Create_Role_Res_MSG",typeof(S2C_Create_Role_Res)},
            {"C2S_Role_Heartbeat_Req_MSG",typeof(C2S_Role_Heartbeat_Req)},
            {"S2C_Role_Heartbeat_Res_MSG",typeof(S2C_Role_Heartbeat_Res)},
       };

       private static Dictionary<int,string>_protoNameDic = new Dictionary<int,string>()
      {
            {1001,"C2S_Login_Req_MSG"},
            {1002,"S2C_Login_Res_MSG"},
            {2001,"C2S_Create_Role_Req_MSG"},
            {2002,"S2C_Create_Role_Res_MSG"},
            {2003,"C2S_Role_Heartbeat_Req_MSG"},
            {2004,"S2C_Role_Heartbeat_Res_MSG"},
       };

       private static readonly Dictionary<RuntimeTypeHandle, MessageParser> Parsers = new Dictionary<RuntimeTypeHandle, MessageParser>()
       {
            {typeof(C2S_Login_Req).TypeHandle,C2S_Login_Req.Parser },
            {typeof(S2C_Login_Res).TypeHandle,S2C_Login_Res.Parser },
            {typeof(C2S_Create_Role_Req).TypeHandle,C2S_Create_Role_Req.Parser },
            {typeof(S2C_Create_Role_Res).TypeHandle,S2C_Create_Role_Res.Parser },
            {typeof(C2S_Role_Heartbeat_Req).TypeHandle,C2S_Role_Heartbeat_Req.Parser },
            {typeof(S2C_Role_Heartbeat_Res).TypeHandle,S2C_Role_Heartbeat_Res.Parser },
       };

        public static MessageParser GetMessageParser(int protoID)
        {
            MessageParser messageParser;
             Type protoType = GetProtoTypeByProtoId(protoID);
            Parsers.TryGetValue(protoType.TypeHandle, out messageParser);
            return messageParser;
        }

        public static Type GetProtoTypeByProtoId(int protoId)
        {
            return _protoIdTypeDic[protoId];
        }

        public static string GetProtoNameByProtoId(int protoId)
        {
            return _protoNameDic[protoId];
        }

        public static Type GetProtoTypeByName(string Name)
        {
            return _protoNameTypeDic[Name];
        }
        public static bool ContainProtoType(Type t)
        {
            return _protoTypeIdDic.ContainsKey(t);
        }
        public static int GetProtoIDByType(Type t)
        {
            return _protoTypeIdDic[t];
        }
    }
}