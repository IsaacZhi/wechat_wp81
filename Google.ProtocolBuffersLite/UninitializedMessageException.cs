// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.UninitializedMessageException
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Google.ProtocolBuffers
{
  public sealed class UninitializedMessageException : Exception
  {
    private readonly IList<string> missingFields;

    public IList<string> MissingFields
    {
      get
      {
        return this.missingFields;
      }
    }

    private UninitializedMessageException(IList<string> missingFields)
      : base(UninitializedMessageException.BuildDescription((IEnumerable<string>) missingFields))
    {
      this.missingFields = (IList<string>) new List<string>((IEnumerable<string>) missingFields);
    }

    public UninitializedMessageException(IMessageLite message)
      : base(string.Format("Message {0} is missing required fields", (object) message.GetType()))
    {
      this.missingFields = (IList<string>) new List<string>();
    }

    public InvalidProtocolBufferException AsInvalidProtocolBufferException()
    {
      return new InvalidProtocolBufferException(this.Message);
    }

    private static string BuildDescription(IEnumerable<string> missingFields)
    {
      StringBuilder stringBuilder = new StringBuilder("Message missing required fields: ");
      bool flag = true;
      foreach (string str in missingFields)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(", ");
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }
  }
}
