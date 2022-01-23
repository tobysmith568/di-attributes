using System.Reflection;

namespace DiAttributes.Managers;

internal interface IManager
{
    void Register(Type @class, CustomAttributeData customAttributeData);
}
