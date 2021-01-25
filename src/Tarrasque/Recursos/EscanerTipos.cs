using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nebularium.Tarrasque.Recursos
{
    public class EscanerTipos
    {
        public static Dictionary<Type, Type> EscanearImplementacoesInterfacesNoAssembly(Type tipo, Type tipoNoAssemblyAlvo = null)
        {
            var dic = new Dictionary<Type, Type>();
            var assembly = tipoNoAssemblyAlvo == null ? Assembly.GetExecutingAssembly() : Assembly.GetAssembly(tipoNoAssemblyAlvo);

            if (tipo.IsGenericType)
                assembly
                .GetTypes()
                .Where(item => item.GetInterfaces()
                .Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == tipo) && !item.IsAbstract && !item.IsInterface)
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().Where(i => i.IsGenericType).First(i => i.GetGenericTypeDefinition() == tipo);
                    dic.Add(assignedTypes, serviceType);
                });
            else
                assembly.GetTypes().Where(item => item.GetInterfaces()
                .Any(i => i == tipo) && !item.IsAbstract && !item.IsInterface)
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().First(i => i == tipo);
                    dic.Add(assignedTypes, serviceType);
                });
            return dic;
        }
    }
}
