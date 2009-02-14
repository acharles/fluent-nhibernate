using System;

namespace FluentNHibernate.Mapping
{
    public class EnumerationTypeConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return type.IsEnum;
        }

        public void AlterMap(IProperty propertyMapping)
        {
            Type mapperType = typeof(GenericEnumMapper<>).MakeGenericType(propertyMapping.PropertyType);
            
            propertyMapping.CustomTypeIs(mapperType);
            propertyMapping.CustomSqlTypeIs("string");
        }
    }
}