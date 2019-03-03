using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace GameCursachProject
{
    static class Serialization
    {
        static public string Serialize(object ObjToSerialize)
        {
            var JsonFormatter = new DataContractJsonSerializer(ObjToSerialize.GetType());
            var stream = new MemoryStream();
            JsonFormatter.WriteObject(stream, ObjToSerialize);
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        static public void DeSerialize(string SerializedObj, object Result)
        {
            var jsonFormatter = new DataContractJsonSerializer(Result.GetType());
            Result = jsonFormatter.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(SerializedObj)));
        }

        static public void DeSerialize(string SerializedObj, ref object Result, Type Objtype)
        {
            var jsonFormatter = new DataContractJsonSerializer(Objtype);
            Result = jsonFormatter.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(SerializedObj)));
        }
    }
}
