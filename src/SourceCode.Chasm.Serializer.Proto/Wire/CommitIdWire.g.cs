// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: CommitIdWire.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SourceCode.Chasm.IO.Proto.Wire {

  /// <summary>Holder for reflection information generated from CommitIdWire.proto</summary>
  public static partial class CommitIdWireReflection {

    #region Descriptor
    /// <summary>File descriptor for CommitIdWire.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommitIdWireReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJDb21taXRJZFdpcmUucHJvdG8aDlNoYTFXaXJlLnByb3RvIiUKDENvbW1p",
            "dElkV2lyZRIVCgJJZBgBIAEoCzIJLlNoYTFXaXJlQiGqAh5Tb3VyY2VDb2Rl",
            "LkNoYXNtLklPLlByb3RvLldpcmViBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::SourceCode.Chasm.IO.Proto.Wire.Sha1WireReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SourceCode.Chasm.IO.Proto.Wire.CommitIdWire), global::SourceCode.Chasm.IO.Proto.Wire.CommitIdWire.Parser, new[]{ "Id" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CommitIdWire : pb::IMessage<CommitIdWire> {
    private static readonly pb::MessageParser<CommitIdWire> _parser = new pb::MessageParser<CommitIdWire>(() => new CommitIdWire());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CommitIdWire> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SourceCode.Chasm.IO.Proto.Wire.CommitIdWireReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommitIdWire() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommitIdWire(CommitIdWire other) : this() {
      Id = other.id_ != null ? other.Id.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommitIdWire Clone() {
      return new CommitIdWire(this);
    }

    /// <summary>Field number for the "Id" field.</summary>
    public const int IdFieldNumber = 1;
    private global::SourceCode.Chasm.IO.Proto.Wire.Sha1Wire id_;
    /// <summary>
    /// Naming follows convention in ProtoSerializer
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SourceCode.Chasm.IO.Proto.Wire.Sha1Wire Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CommitIdWire);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CommitIdWire other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Id, other.Id)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (id_ != null) hash ^= Id.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (id_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Id);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (id_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Id);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CommitIdWire other) {
      if (other == null) {
        return;
      }
      if (other.id_ != null) {
        if (id_ == null) {
          id_ = new global::SourceCode.Chasm.IO.Proto.Wire.Sha1Wire();
        }
        Id.MergeFrom(other.Id);
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
          case 10: {
            if (id_ == null) {
              id_ = new global::SourceCode.Chasm.IO.Proto.Wire.Sha1Wire();
            }
            input.ReadMessage(id_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
