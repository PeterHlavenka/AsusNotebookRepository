using System;

namespace Mediaresearch.Framework.DataAccess.BLToolkit
{
    public interface IEntityWithId
    {
        Type PrimaryKeyType { get; }
        object PrimaryKeyId
        {
            //BLT ma v sobe chybu - vola i neexistujici getter pri Clone, proto zde musi byt tento public getter
            get; 
            set;
        }

        bool IdIsZeroValue();
    }

    public interface IEntityWithId<TId> : IEntityWithId
    {
        TId Id { get; set; }
    }
}