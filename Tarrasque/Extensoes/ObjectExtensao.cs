using AutoMapper;
using Nebularium.Tarrasque.Funcoes;
using System;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class ObjectExtensao
    {
        public static T como<T>(this object obj, bool naoMapear = false, bool permitirExcecao = false, T valorPadrao = default)
        {
            try
            {
                if (obj == null)
                    return valorPadrao;

                if (obj.GetType() == typeof(T))
                    return (T)obj;

                if (obj is Enum)
                {
                    if (typeof(T) == typeof(int) || typeof(T) == typeof(byte) || typeof(T) == typeof(Enum) || typeof(T) == typeof(long))
                        return (T)obj;

                    if (valorPadrao.Equals(default(T)))
                        return ConverteUtils.sempreConverteEnum<T>(obj);
                    return ConverteUtils.sempreConverteEnum(obj, valorPadrao);
                }

                if (!naoMapear)
                {
                    var r = GestorDependencia.Instancia.ObterInstancia<IMapper>().Map<T>(obj);
                    return r;
                }

                return (T)obj;
            }
            catch (AutoMapperMappingException)
            {
                return (T)obj;
            }
            catch (Exception)
            {
                if (permitirExcecao)
                    throw;
                if (valorPadrao?.Equals(default(T)) ?? true)
                    return ConverteUtils.sempreConverte<T>(obj);
                return ConverteUtils.sempreConverte(obj, valorPadrao);
            }
        }
    }
}
