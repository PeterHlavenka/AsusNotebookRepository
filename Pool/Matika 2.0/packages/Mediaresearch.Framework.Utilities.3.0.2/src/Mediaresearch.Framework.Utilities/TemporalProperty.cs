using System;
using System.Collections.Generic;
using Mediaresearch.Framework.Domain.History;

namespace Mediaresearch.Framework.Utilities
{
	/// <summary>
	/// Implementace temporal property (viz http://martinfowler.com/ap2/temporalProperty.html )
	/// Temporal property je vlastnost entity (v nasem pripade je to vzdy jen a jen reference na jinou tridu, nikdy jen hodnotovy typ) ktera
	/// se meni s casem a zaroven je potreba znat jeji minule hodnoty v kazdem casovem okamziku....
	/// Temporal property musi mit vzdy hodnotu. Musi tedy platit ze From hodnoty musi byt rovno To hodnoty predchozi
	/// </summary>
	/// <typeparam name="T">Datovy typ predstavujici hodnotu vlastnosti.</typeparam>
	[Serializable]
	public class TemporalProperty<T>
		where T : class, IObjectWithValidity
	{
		protected List<T> m_Values;

        public List<T> Values
		{
			get { return m_Values; }
		}

		public TemporalProperty()
		{
            m_Values = new List<T>();
		}

		public TemporalProperty(IList<T> values)
		{
			m_Values = new List<T>(values);
		}

		public virtual void AddEntity(T entity, DateTime validFrom)
		{
			InternalAddEntity(entity, validFrom, true);
		}

		public void AddEntity(T entity)
		{
			AddEntity(entity, DateTime.Now);
		}

		public T CurrentReferencedEntity
		{
			get { return GetReferencedEntityFor(DateTime.Now); }
		}

		public T GetReferencedEntityFor(DateTime date)
		{
			foreach (T version in m_Values)
			{
				if (version.Validity.Contains(date)) return version;
			}
			return null;
		}

		public void CloseReferencedEntityValidity(DateTime stamp)
		{
			T entity = CurrentReferencedEntity;
			if (entity == null)
				throw new InvalidOperationException("There is no active record in version");

			entity.Validity = new ValidityRange(entity.Validity.From, stamp);
		}

		protected void InternalAddEntity(T entity, DateTime validFrom, bool throwExceptionIfNoActiveEntity)
		{
			if (m_Values.Contains(entity))
				throw new ArgumentException(string.Format("Entities collection already contains this entity '{0}'", entity),
				                            "entity");

			validFrom = new DateTime(validFrom.Year, validFrom.Month, validFrom.Day, validFrom.Hour, validFrom.Minute, validFrom.Second);

			T newer = null;
			if (m_Values.Count >= 1)
			{
				foreach (T version in m_Values)
				{
					if (version.Validity.To == HistoryConstants.DefaultValidTo)
					{
						newer = version;
						break;
					}
				}
				if (newer == null && throwExceptionIfNoActiveEntity)
					throw new InvalidOperationException("There is no active record in version");
			}

			if (newer != null)
			{
				newer.Validity = new ValidityRange(newer.Validity.From, validFrom);
			}

			entity.Validity = new ValidityRange(validFrom, HistoryConstants.DefaultValidTo);
			
			m_Values.Add(entity);			
		}
	}
}