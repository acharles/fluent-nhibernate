using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase
    {
        bool IsInverse { get; }
        bool IsLazy { get; }
        string Name { get; set; }
        KeyMapping Key { get; set; }
        ICollectionContentsMapping Contents { get; set; }
        CollectionAttributes Attributes { get; }
    }
}