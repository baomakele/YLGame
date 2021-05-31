// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: role_client.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Protocol {

  /// <summary>Holder for reflection information generated from role_client.proto</summary>
  public static partial class RoleClientReflection {

    #region Descriptor
    /// <summary>File descriptor for role_client.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RoleClientReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFyb2xlX2NsaWVudC5wcm90bxIIcHJvdG9jb2wiKAoWQzJTX1JvbGVfSGVh",
            "cnRiZWF0X1JlcRIOCgZyb2xlSWQYASABKAQiOQoWUzJDX1JvbGVfSGVhcnRi",
            "ZWF0X1JlcxIfCgZyZXN1bHQYASABKA4yDy5wcm90b2NvbC5FUm9sZSovCgVF",
            "Um9sZRIMCghSb2xlTm9uZRAAEhgKFFJvbGVIZWFydGJlYXRTdWNjZXNzEAFi",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Protocol.ERole), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Protocol.C2S_Role_Heartbeat_Req), global::Protocol.C2S_Role_Heartbeat_Req.Parser, new[]{ "RoleId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Protocol.S2C_Role_Heartbeat_Res), global::Protocol.S2C_Role_Heartbeat_Res.Parser, new[]{ "Result" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  ///Role状态信息汇总
  /// </summary>
  public enum ERole {
    [pbr::OriginalName("RoleNone")] RoleNone = 0,
    /// <summary>
    ///心跳成功
    /// </summary>
    [pbr::OriginalName("RoleHeartbeatSuccess")] RoleHeartbeatSuccess = 1,
  }

  #endregion

  #region Messages
  /// <summary>
  ///心跳
  /// </summary>
  public sealed partial class C2S_Role_Heartbeat_Req : pb::IMessage<C2S_Role_Heartbeat_Req> {
    private static readonly pb::MessageParser<C2S_Role_Heartbeat_Req> _parser = new pb::MessageParser<C2S_Role_Heartbeat_Req>(() => new C2S_Role_Heartbeat_Req());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<C2S_Role_Heartbeat_Req> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Protocol.RoleClientReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_Role_Heartbeat_Req() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_Role_Heartbeat_Req(C2S_Role_Heartbeat_Req other) : this() {
      roleId_ = other.roleId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_Role_Heartbeat_Req Clone() {
      return new C2S_Role_Heartbeat_Req(this);
    }

    /// <summary>Field number for the "roleId" field.</summary>
    public const int RoleIdFieldNumber = 1;
    private ulong roleId_;
    /// <summary>
    ///角色ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong RoleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as C2S_Role_Heartbeat_Req);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(C2S_Role_Heartbeat_Req other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoleId != other.RoleId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RoleId != 0UL) hash ^= RoleId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (RoleId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(RoleId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (RoleId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(RoleId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(C2S_Role_Heartbeat_Req other) {
      if (other == null) {
        return;
      }
      if (other.RoleId != 0UL) {
        RoleId = other.RoleId;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            RoleId = input.ReadUInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///心跳返回
  /// </summary>
  public sealed partial class S2C_Role_Heartbeat_Res : pb::IMessage<S2C_Role_Heartbeat_Res> {
    private static readonly pb::MessageParser<S2C_Role_Heartbeat_Res> _parser = new pb::MessageParser<S2C_Role_Heartbeat_Res>(() => new S2C_Role_Heartbeat_Res());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<S2C_Role_Heartbeat_Res> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Protocol.RoleClientReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_Role_Heartbeat_Res() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_Role_Heartbeat_Res(S2C_Role_Heartbeat_Res other) : this() {
      result_ = other.result_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_Role_Heartbeat_Res Clone() {
      return new S2C_Role_Heartbeat_Res(this);
    }

    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 1;
    private global::Protocol.ERole result_ = 0;
    /// <summary>
    ///返回状态
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Protocol.ERole Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as S2C_Role_Heartbeat_Res);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(S2C_Role_Heartbeat_Res other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Result != other.Result) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Result != 0) hash ^= Result.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Result != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Result);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Result != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Result);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(S2C_Role_Heartbeat_Res other) {
      if (other == null) {
        return;
      }
      if (other.Result != 0) {
        Result = other.Result;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            result_ = (global::Protocol.ERole) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
