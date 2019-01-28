﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Matika.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="EnumeratedWordsDb - Copy")]
	public partial class EnumeratedWordsDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertB_Word(B_Word instance);
    partial void UpdateB_Word(B_Word instance);
    partial void DeleteB_Word(B_Word instance);
    partial void InsertL_Word(L_Word instance);
    partial void UpdateL_Word(L_Word instance);
    partial void DeleteL_Word(L_Word instance);
    partial void InsertM_Word(M_Word instance);
    partial void UpdateM_Word(M_Word instance);
    partial void DeleteM_Word(M_Word instance);
    partial void InsertP_Word(P_Word instance);
    partial void UpdateP_Word(P_Word instance);
    partial void DeleteP_Word(P_Word instance);
    #endregion
		
		public EnumeratedWordsDBDataContext() : 
				base(global::Matika.Properties.Settings.Default.EnumeratedWordsDb___CopyConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public EnumeratedWordsDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EnumeratedWordsDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EnumeratedWordsDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EnumeratedWordsDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<B_Word> B_Words
		{
			get
			{
				return this.GetTable<B_Word>();
			}
		}
		
		public System.Data.Linq.Table<L_Word> L_Words
		{
			get
			{
				return this.GetTable<L_Word>();
			}
		}
		
		public System.Data.Linq.Table<M_Word> M_Words
		{
			get
			{
				return this.GetTable<M_Word>();
			}
		}
		
		public System.Data.Linq.Table<P_Word> P_Words
		{
			get
			{
				return this.GetTable<P_Word>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.B_Words")]
	public partial class B_Word : INotifyPropertyChanging, INotifyPropertyChanged, IWord
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private bool _IsEnumerated;
		
		private string _CoveredName;
		
		private string _Help;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnIsEnumeratedChanging(bool value);
    partial void OnIsEnumeratedChanged();
    partial void OnCoveredNameChanging(string value);
    partial void OnCoveredNameChanged();
    partial void OnHelpChanging(string value);
    partial void OnHelpChanged();
    #endregion
		
		public B_Word()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsEnumerated", DbType="Bit NOT NULL")]
		public bool IsEnumerated
		{
			get
			{
				return this._IsEnumerated;
			}
			set
			{
				if ((this._IsEnumerated != value))
				{
					this.OnIsEnumeratedChanging(value);
					this.SendPropertyChanging();
					this._IsEnumerated = value;
					this.SendPropertyChanged("IsEnumerated");
					this.OnIsEnumeratedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CoveredName", DbType="NChar(100)")]
		public string CoveredName
		{
			get
			{
				return this._CoveredName;
			}
			set
			{
				if ((this._CoveredName != value))
				{
					this.OnCoveredNameChanging(value);
					this.SendPropertyChanging();
					this._CoveredName = value;
					this.SendPropertyChanged("CoveredName");
					this.OnCoveredNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Help", DbType="NVarChar(100)")]
		public string Help
		{
			get
			{
				return this._Help;
			}
			set
			{
				if ((this._Help != value))
				{
					this.OnHelpChanging(value);
					this.SendPropertyChanging();
					this._Help = value;
					this.SendPropertyChanged("Help");
					this.OnHelpChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.L_Words")]
	public partial class L_Word : INotifyPropertyChanging, INotifyPropertyChanged, IWord
    {
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private bool _IsEnumerated;
		
		private string _CoveredName;
		
		private string _Help;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnIsEnumeratedChanging(bool value);
    partial void OnIsEnumeratedChanged();
    partial void OnCoveredNameChanging(string value);
    partial void OnCoveredNameChanged();
    partial void OnHelpChanging(string value);
    partial void OnHelpChanged();
    #endregion
		
		public L_Word()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsEnumerated", DbType="Bit NOT NULL")]
		public bool IsEnumerated
		{
			get
			{
				return this._IsEnumerated;
			}
			set
			{
				if ((this._IsEnumerated != value))
				{
					this.OnIsEnumeratedChanging(value);
					this.SendPropertyChanging();
					this._IsEnumerated = value;
					this.SendPropertyChanged("IsEnumerated");
					this.OnIsEnumeratedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CoveredName", DbType="NChar(100)")]
		public string CoveredName
		{
			get
			{
				return this._CoveredName;
			}
			set
			{
				if ((this._CoveredName != value))
				{
					this.OnCoveredNameChanging(value);
					this.SendPropertyChanging();
					this._CoveredName = value;
					this.SendPropertyChanged("CoveredName");
					this.OnCoveredNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Help", DbType="NVarChar(100)")]
		public string Help
		{
			get
			{
				return this._Help;
			}
			set
			{
				if ((this._Help != value))
				{
					this.OnHelpChanging(value);
					this.SendPropertyChanging();
					this._Help = value;
					this.SendPropertyChanged("Help");
					this.OnHelpChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.M_Words")]
	public partial class M_Word : INotifyPropertyChanging, INotifyPropertyChanged, IWord
    {
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private bool _IsEnumerated;
		
		private string _CoveredName;
		
		private string _Help;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnIsEnumeratedChanging(bool value);
    partial void OnIsEnumeratedChanged();
    partial void OnCoveredNameChanging(string value);
    partial void OnCoveredNameChanged();
    partial void OnHelpChanging(string value);
    partial void OnHelpChanged();
    #endregion
		
		public M_Word()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsEnumerated", DbType="Bit NOT NULL")]
		public bool IsEnumerated
		{
			get
			{
				return this._IsEnumerated;
			}
			set
			{
				if ((this._IsEnumerated != value))
				{
					this.OnIsEnumeratedChanging(value);
					this.SendPropertyChanging();
					this._IsEnumerated = value;
					this.SendPropertyChanged("IsEnumerated");
					this.OnIsEnumeratedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CoveredName", DbType="NChar(100)")]
		public string CoveredName
		{
			get
			{
				return this._CoveredName;
			}
			set
			{
				if ((this._CoveredName != value))
				{
					this.OnCoveredNameChanging(value);
					this.SendPropertyChanging();
					this._CoveredName = value;
					this.SendPropertyChanged("CoveredName");
					this.OnCoveredNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Help", DbType="NVarChar(100)")]
		public string Help
		{
			get
			{
				return this._Help;
			}
			set
			{
				if ((this._Help != value))
				{
					this.OnHelpChanging(value);
					this.SendPropertyChanging();
					this._Help = value;
					this.SendPropertyChanged("Help");
					this.OnHelpChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.P_Words")]
	public partial class P_Word : INotifyPropertyChanging, INotifyPropertyChanged, IWord
    {
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private bool _IsEnumerated;
		
		private string _CoveredName;
		
		private string _Help;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnIsEnumeratedChanging(bool value);
    partial void OnIsEnumeratedChanged();
    partial void OnCoveredNameChanging(string value);
    partial void OnCoveredNameChanged();
    partial void OnHelpChanging(string value);
    partial void OnHelpChanged();
    #endregion
		
		public P_Word()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsEnumerated", DbType="Bit NOT NULL")]
		public bool IsEnumerated
		{
			get
			{
				return this._IsEnumerated;
			}
			set
			{
				if ((this._IsEnumerated != value))
				{
					this.OnIsEnumeratedChanging(value);
					this.SendPropertyChanging();
					this._IsEnumerated = value;
					this.SendPropertyChanged("IsEnumerated");
					this.OnIsEnumeratedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CoveredName", DbType="NChar(100)")]
		public string CoveredName
		{
			get
			{
				return this._CoveredName;
			}
			set
			{
				if ((this._CoveredName != value))
				{
					this.OnCoveredNameChanging(value);
					this.SendPropertyChanging();
					this._CoveredName = value;
					this.SendPropertyChanged("CoveredName");
					this.OnCoveredNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Help", DbType="NChar(100)")]
		public string Help
		{
			get
			{
				return this._Help;
			}
			set
			{
				if ((this._Help != value))
				{
					this.OnHelpChanging(value);
					this.SendPropertyChanging();
					this._Help = value;
					this.SendPropertyChanged("Help");
					this.OnHelpChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
