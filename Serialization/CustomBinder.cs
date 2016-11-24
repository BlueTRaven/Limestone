using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Limestone.Serialization
{
    public sealed class CustomizedBinder : SerializationBinder
    {
        static private Dictionary<string, Type> typeBindings = new Dictionary<string, Type>();

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type type;
            var assemblyQualifiedTypeName = String.Format("{0}, {1}", typeName, assemblyName);

            // use cached result if it exists
            if (typeBindings.TryGetValue(assemblyQualifiedTypeName, out type))
            {
                return type;
            }

            // try the fully qualified name
            try { type = Type.GetType(assemblyQualifiedTypeName); }
            catch { type = null; }

            if (type == null)
            {
                // allow any assembly version
                var assemblyNameWithoutVersion = assemblyName.Remove(assemblyName.IndexOf(','));
                var assemblyQualifiedTypeNameWithoutVersion = String.Format("{0}, {1}", typeName, assemblyNameWithoutVersion);
                try { type = Type.GetType(assemblyQualifiedTypeNameWithoutVersion); }
                catch { type = null; }
            }

            if (type == null)
            {
                // check all assemblies for type full name
                try
                {
                    type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.ExportedTypes)
                        .Where(a => a.FullName == typeName)
                        .FirstOrDefault();
                }
                catch { type = null; }
            }

            if (type == null)
            {
                // check all assemblies for type name
                var name = typeName.Split('.').Last();
                try
                {
                    type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.ExportedTypes)
                        .Where(a => a.Name == name)
                        .FirstOrDefault();
                }
                catch { type = null; }
            }

            if (type == null)
            {   
                //One final check to see if it's not contained in my assembly - eg. System.Collections.Generic.List<T>
                //type = Type.GetType(String.Format("{0}, {1}", typeName, temp));
            }

            typeBindings[assemblyQualifiedTypeName] = type; //cache it
            return type;
        }

        //public override Type BindToType(string assemblyName, string typeName)
        //{
        //    string temp = assemblyName;
        //    Type typeToDeserialize = null;

        //    String currentAssembly = Assembly.GetExecutingAssembly().FullName;

        //    // In this case we are always using the current assembly
        //    assemblyName = currentAssembly;

        //    // Get the type using the typeName and assemblyName
        //    typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

        //    if (typeToDeserialize == null)
        //    {
        //        typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, temp));
        //    }
        //    return typeToDeserialize;
        //}


        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);
            assemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }
    }
}
