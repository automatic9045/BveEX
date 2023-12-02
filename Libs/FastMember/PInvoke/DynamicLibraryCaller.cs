using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastMember.PInvoke
{
    /// <summary>
    /// 動的にロードされたアンマネージ ライブラリ (C や C++ などで記述された DLL) のラッパーを表します。
    /// </summary>
    public class DynamicLibraryCaller
    {
        private readonly TypeBuilder TypeBuilder;
        private readonly List<DynamicFastMethod> DefinedFunctions = new List<DynamicFastMethod>();

        public string FilePath { get; }
        public bool IsCompiled { get; private set; } = false;

        public DynamicLibraryCaller(string filePath) : this(filePath, GetDefaultTempAssemblyName(filePath))
        {
        }

        public DynamicLibraryCaller(string filePath, AssemblyName tempAssemblyName)
        {
            FilePath = filePath;

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);
            TypeBuilder = moduleBuilder.DefineType("__DynamicCallerTempType");
        }

        private static AssemblyName GetDefaultTempAssemblyName(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            AssemblyName assemblyName = new AssemblyName($"{fileName}.DynamicCaller.{Guid.NewGuid()}");
            return assemblyName;
        }

        public DynamicFastMethod RegisterFunction(string functionName, Type returnType, params Type[] parameterTypes)
        {
            MethodBuilder methodBuilder = TypeBuilder.DefinePInvokeMethod(functionName, FilePath,
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl | MethodAttributes.HideBySig, CallingConventions.Standard,
                returnType, parameterTypes,
                CallingConvention.StdCall, CharSet.Unicode);
            methodBuilder.SetImplementationFlags(MethodImplAttributes.PreserveSig);

            DynamicFastMethod result = new DynamicFastMethod(functionName);
            DefinedFunctions.Add(result);
            return result;
        }

        public void Compile()
        {
            Type type = TypeBuilder.CreateType();

            Parallel.ForEach(DefinedFunctions, function =>
            {
                MethodInfo source = type.GetMethod(function.Name);
                FastMethod method = FastMethod.Create(source);
                function.Compile(method);
            });
        }
    }
}
