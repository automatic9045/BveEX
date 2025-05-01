using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnembeddedResources;

namespace FastMember
{
    internal static class MethodInfoExtensions
    {
        public static DynamicMethod ToDeclaredOnly(this MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            Type type = method.DeclaringType;
            Type returnType = method.ReturnType == typeof(void) ? null : method.ReturnType;
            DynamicMethod result = new DynamicMethod(string.Empty, returnType, new Type[] { type, typeof(object) }, type);

            ILGenerator iLGenerator = result.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];

                iLGenerator.Emit(OpCodes.Ldarg_1);
                iLGenerator.Emit(OpCodes.Ldc_I4_S, i);
                iLGenerator.Emit(OpCodes.Ldelem_Ref);

                if (parameter.ParameterType.IsPrimitive)
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, parameter.ParameterType);
                }
                else if (parameter.ParameterType == typeof(object))
                {
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Castclass, parameter.ParameterType);
                }
            }

            iLGenerator.Emit(OpCodes.Call, method);
            iLGenerator.Emit(OpCodes.Ret);

            return result;
        }
    }
}
