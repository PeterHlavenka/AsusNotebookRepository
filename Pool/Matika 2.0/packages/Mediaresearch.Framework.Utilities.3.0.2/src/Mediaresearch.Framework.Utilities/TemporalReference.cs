using System;
using System.Collections.Generic;
using Mediaresearch.Framework.Domain.History;

namespace Mediaresearch.Framework.Utilities
{
	[Serializable]
	public sealed class TemporalReference<T> : TemporalProperty<T>
		where T : class, IObjectWithValidity
	{
		public TemporalReference(IList<T> values) : base(values)
		{
		}

		public TemporalReference()
		{
		}

		public override void AddEntity(T entity, DateTime validFrom)
		{
			InternalAddEntity(entity, validFrom, false);
		}
	}
}