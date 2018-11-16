using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Mediaresearch.Framework.Utilities
{
	// Author:     Nils Jonsson
	// Originated: 6/29/2004
	// http://www.codeproject.com/csharp/argumentexceptions.asp
	/// <summary>
	/// Provides static methods for throwing common exceptions.
	/// </summary>
	public class Exceptions
	{
		private static string PadWord(string text)
		{
			if (text == null)
				return string.Empty;
			else if (text == string.Empty)
				return text;
			else
				return text + " ";
		}

		#region ThrowIfDifferentRank overloads

		/// <summary>
		/// Throws <see cref="RankException"/> if an array argument
		/// <paramref name="value"/> named <paramref name="name"/> does not have
		/// the number of dimensions specified by <paramref name="rank"/>.
		/// </summary>
		/// <param name="value">The value of the array argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="rank">The expected number of dimensions.</param>
		/// <exception cref="RankException">Thrown if <paramref name="value"/>
		/// does not have the number of dimensions specified by
		/// <paramref name="rank"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see cref="ThrowIfDifferentRank(Array,string,int)"/> to implement argument validation
		/// in a static method.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyArrayUtils
		/// {
		///     public static int BinarySearch(Array array, object value)
		///     {
		///         Exceptions.ThrowIfNull(array, "array");
		///         Exceptions.ThrowIfDifferentRank(array, "array", 1);
		///         
		///         // Implement search logic here.
		///     }
		///     
		///     
		///     private MyArrayUtils()
		///     {
		///     }
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfDifferentRank(Array value, string name,
		                                        int rank)
		{
			Debug.Assert(value != null, "Argument value cannot be null.");

			if ((value != null) && (value.Rank != rank))
			{
				throw new RankException(string.Format("Argument {0}must be an "
				                                      + "array with {1} dimension{2}.", PadWord(name),
				                                      rank, rank != 1 ? "s" : string.Empty));
			}
		}

		/// <summary>
		/// Throws <see cref="RankException"/> if an array argument
		/// <paramref name="value"/> does not have the number of dimensions
		/// specified by <paramref name="rank"/>.
		/// </summary>
		/// <param name="value">The value of the array argument.</param>
		/// <param name="rank">The expected number of dimensions.</param>
		/// <exception cref="RankException">Thrown if <paramref name="value"/>
		/// does not have the number of dimensions specified by
		/// <paramref name="rank"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfDifferentRank(Array value, int rank)
		{
			ThrowIfDifferentRank(value, null, rank);
		}

		#endregion

		#region ThrowIfDifferentType() overloads

		/// <summary>
		/// Throws <see cref="ArgumentException"/> if an argument
		/// <paramref name="value"/> named <paramref name="name"/> is not an
		/// instance of <paramref name="type"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="type">The expected <see cref="Type"/>.</param>
		/// <exception cref="ArgumentException">Thrown if
		/// <paramref name="value"/> is not an instance of
		/// <paramref name="type"/>.</exception>
		/// <remarks>This method is useful in implementing
		/// <see cref="IComparable.CompareTo"/>.</remarks>
		/// <example>
		/// The following example shows how to use
		/// <see cref="ThrowIfDifferentType(object,string,Type)"/> to simplify an implementation of
		/// <see cref="IComparable.CompareTo"/>.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass : IComparable
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public int CompareTo(object obj)
		///     {
		///         if (obj == null)
		///             return 1;
		///         
		///         Exceptions.ThrowIfDifferentType(obj, "obj", this.GetType());
		///         
		///         // Implement CompareTo() logic here.
		///     }
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfDifferentType(object value, string name,
		                                        Type type)
		{
			Debug.Assert(value != null, "Argument value cannot be null.");
			Debug.Assert(type != null, "Argument type cannot be null.");

			if ((value != null) && (type != null) && (value.GetType() != type))
			{
				throw new ArgumentException(
					string.Format("Argument {0}must be of type {1}.",
					              PadWord(name), type.Name), name);
			}
		}

		/// <summary>
		/// Throws <see cref="ArgumentException"/> if an argument
		/// <paramref name="value"/> is not an instance of
		/// <paramref name="type"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="type">The expected <see cref="Type"/>.</param>
		/// <exception cref="ArgumentException">Thrown if
		/// <paramref name="value"/> is not an instance of
		/// <paramref name="type"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfDifferentType(object value, Type type)
		{
			ThrowIfDifferentType(value, null, type);
		}

		#endregion

		#region ThrowIfIncompatibleType() overloads

		/// <summary>
		/// Throws <see cref="ArgumentException"/> if an argument
		/// <paramref name="value"/> named <paramref name="name"/> cannot be
		/// cast to <paramref name="type"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="type">The expected <see cref="Type"/>.</param>
		/// <exception cref="ArgumentException">Thrown if
		/// <paramref name="value"/> cannot be cast to
		/// <paramref name="type"/>.</exception>
		/// <remarks>This method is useful in overloading the protected methods
		/// of <see cref="System.Collections.CollectionBase"/>.</remarks>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfIncompatibleType(object,string,Type)ThrowIfIncompatibleType</cref>
		/// </see> to simplify extending
		/// <see cref="System.Collections.CollectionBase"/>.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class TimeZoneCollection : CollectionBase
		/// {
		///     public TimeZoneCollection() : base()
		///     {
		///     }
		///     
		///     
		///     public TimeZone this[int index]
		///     {
		///         get { return (TimeZone)this.List[index]; }
		///         
		///         set { this.List[index] = value; }
		///     }
		///     
		///     
		///     public int Add(TimeZone value)
		///     {
		///         return this.List.Add(value);
		///     }
		///     
		///     public int IndexOf(TimeZone value)
		///     {
		///         return this.List.IndexOf(value);
		///     }
		///     
		///     protected override void OnValidate(object value)
		///     {
		///         base.OnValidate(value);
		///         
		///         Exceptions.ThrowIfIncompatibleType(value, "value",
		///          typeof(TimeZone));
		///     }
		///     
		///     public void Remove(TimeZone value)
		///     {
		///         this.List.Remove(value);
		///     }
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfIncompatibleType(object value, string name,
		                                           Type type)
		{
			Debug.Assert(value != null, "Argument value cannot be null.");
			Debug.Assert(type != null, "Argument type cannot be null.");

			if ((value != null) && (type != null)
			    && (!type.IsAssignableFrom(value.GetType())))
			{
				throw new ArgumentException(
					string.Format("Argument {0}must be compatible with type {1}.",
					              PadWord(name), type.Name), name);
			}
		}

		/// <summary>
		/// Throws <see cref="ArgumentException"/> if an argument
		/// <paramref name="value"/> cannot be cast to <paramref name="type"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="type">The expected <see cref="Type"/>.</param>
		/// <exception cref="ArgumentException">Thrown if
		/// <paramref name="value"/> cannot be cast to
		/// <paramref name="type"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfIncompatibleType(object value, Type type)
		{
			ThrowIfIncompatibleType(value, null, type);
		}

		#endregion

		#region ThrowIfInvalidEnumValue() overloads

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see cref="ThrowIfInvalidEnumValue(int,string,Type)"/> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value",
		///          typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(int value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see cref="ThrowIfInvalidEnumValue(int,Type)"/> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(int value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="string"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not the name of a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a name equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see cref="ThrowIfInvalidEnumValue(string,string,Type)"/> to implement argument
		/// validation in a method.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public static MyEnum ValueForString(string value)
		///     {
		///         Exceptions.ThrowIfNull(value, "value");
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value",
		///          typeof(MyEnum));
		///         
		///         return (MyEnum)Enum.Parse(typeof(MyEnum), value);
		///     }
		///     
		///     
		///     private MyClass()
		///     {
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(string value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="string"/> argument <paramref name="value"/> is not the
		/// name of a constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a name equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public string MyPropertyName
		///     {
		///         get { return Enum.Format(typeof(MyEnum), this.myProperty, "G"); }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfNull(value);
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = (MyEnum)Enum.Parse(typeof(MyEnum), value);
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(string value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : byte
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(byte value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : byte
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(byte value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : short
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(short value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : short
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(short value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : long
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(long value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : long
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(long value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : uint
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(uint value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : uint
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(uint value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : sbyte
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(sbyte value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : sbyte
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(sbyte value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : ushort
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(ushort value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : ushort
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(ushort value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is not a constant in
		/// <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(MyEnum value)
		///     {
		///         Exceptions.ThrowIfInvalidEnumValue(value, "value", typeof(MyEnum));
		///     }
		/// }
		/// 
		/// 
		/// public enum MyEnum : ulong
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(ulong value, string name,
		                                           Type enumType)
		{
			ThrowIfInvalidEnumValueCore(value, name, enumType);
		}

		/// <summary>
		/// Throws <see cref="InvalidEnumArgumentException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> is not a
		/// constant in <paramref name="enumType"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="enumType">An enumeration type.</param>
		/// <exception cref="InvalidEnumArgumentException">Thrown if no constant
		/// in <paramref name="enumType"/> has a value equal to
		/// <paramref name="value"/>.</exception>
		/// <example>
		/// The following example shows how to use
		/// <see>
		/// 	<cref>ThrowIfInvalidEnumValue</cref>
		/// </see> to implement argument
		/// validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public MyEnum MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfInvalidEnumValue(value, typeof(MyEnum));
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private MyEnum myProperty = MyEnum.MyValue1;
		/// }
		/// 
		/// 
		/// public enum MyEnum : ulong
		/// {
		///     MyValue1,
		///     MyValue2
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfInvalidEnumValue(ulong value, Type enumType)
		{
			ThrowIfInvalidEnumValue(value, null, enumType);
		}

		#endregion

		[DebuggerHidden]
		private static void ThrowIfInvalidEnumValueCore(object value,
		                                                string name, Type enumType)
		{
			Debug.Assert(value.GetType().IsPrimitive
			             || (value.GetType() == typeof (string)),
			             "Expected a primitive type or a String!");

			Debug.Assert(value != null, "Argument value cannot be null.");
			Debug.Assert(enumType != null, "Argument enumType cannot be null.");
			Debug.Assert(enumType.IsEnum,
			             "Argument enumType is not an enumeration type.");
			bool flagsAttributeSet
				= enumType.GetCustomAttributes(typeof (FlagsAttribute),
				                               true).Length > 0;
			Debug.Assert(!flagsAttributeSet, string.Format("Cannot validate "
			                                               + "against the type {0} because it has the custom attribute "
			                                               + "FlagsAttribute.", enumType));

			if ((value != null) && (enumType != null) && enumType.IsEnum
			    && !flagsAttributeSet && !Enum.IsDefined(enumType, value))
			{
				if (value.GetType() == typeof (string))
				{
					// Let the value argument be 0 if a string was passed in.
					throw new InvalidEnumArgumentException(name, 0, enumType);
				}
				else
				{
					int valueInt32 = 0;
					TypeConverter typeConverter
						= TypeDescriptor.GetConverter(
							Enum.GetUnderlyingType(enumType));
					try
					{
						valueInt32
							= (int) typeConverter.ConvertTo(value, typeof (int));
					}
					catch (OverflowException)
					{
						// Let the value argument be 0 if it overflows Int32.
					}
					throw new InvalidEnumArgumentException(name, valueInt32,
					                                       enumType);
				}
			}
		}

		#region ThrowIfNull() overloads

		/// <summary>
		/// Throws <see cref="ArgumentNullException"/> if an argument
		/// <paramref name="value"/> named <paramref name="name"/> is a null
		/// reference (<c>Nothing</c> in Visual Basic).
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <exception cref="ArgumentNullException">Thrown if
		/// <paramref name="value"/> is a null reference (<c>Nothing</c> in
		/// Visual Basic).</exception>
		/// <example>
		/// The following example shows how to use <see>
		///                                        	<cref>ThrowIfNull</cref>
		///                                        </see> to
		/// implement argument validation in a constructor.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass(string value)
		///     {
		///         Exceptions.ThrowIfNull(value, "value");
		///     }
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfNull(object value, string name)
		{
			if (value == null)
				throw new ArgumentNullException(name);
		}

		/// <summary>
		/// Throws <see cref="ArgumentNullException"/> if an argument
		/// <paramref name="value"/> is a null reference (<c>Nothing</c> in
		/// Visual Basic).
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <exception cref="ArgumentNullException">Thrown if
		/// <paramref name="value"/> is a null reference (<c>Nothing</c> in
		/// Visual Basic).</exception>
		/// <example>
		/// The following example shows how to use <see>
		///                                        	<cref>ThrowIfNull</cref>
		///                                        </see> to
		/// implement argument validation in a property.
		/// <code>
		/// using System;
		/// 
		/// 
		/// public class MyClass
		/// {
		///     public MyClass()
		///     {
		///     }
		///     
		///     
		///     public string MyProperty
		///     {
		///         get { return this.myProperty; }
		///         
		///         set
		///         {
		///             Exceptions.ThrowIfNull(value);
		///             
		///             this.myProperty = value;
		///         }
		///     }
		///     
		///     
		///     private string myProperty = string.Empty;
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void ThrowIfNull(object value)
		{
			ThrowIfNull(value, null);
		}

		#endregion

		#region ThrowIfOutOfRange() overloads

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(int value, string name,
		                                     int minValue, int maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(int value, int minValue,
		                                     int maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(byte value, string name,
		                                     byte minValue, byte maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(byte value, byte minValue,
		                                     byte maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(char value, string name,
		                                     char minValue, char maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(char value, char minValue,
		                                     char maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(decimal value, string name,
		                                     decimal minValue, decimal maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(decimal value, decimal minValue,
		                                     decimal maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(double value, string name,
		                                     double minValue, double maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(double value, double minValue,
		                                     double maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(float value, string name,
		                                     float minValue, float maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(float value, float minValue,
		                                     float maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(short value, string name,
		                                     short minValue, short maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(short value, short minValue,
		                                     short maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(long value, string name,
		                                     long minValue, long maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(long value, long minValue,
		                                     long maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(uint value, string name,
		                                     uint minValue, uint maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(uint value, uint minValue,
		                                     uint maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(sbyte value, string name,
		                                     sbyte minValue, sbyte maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(sbyte value, sbyte minValue,
		                                     sbyte maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(ushort value, string name,
		                                     ushort minValue, ushort maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(ushort value, ushort minValue,
		                                     ushort maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(ulong value, string name,
		                                     ulong minValue, ulong maxValue)
		{
			ThrowIfOutOfRangeCore(value, name, minValue, maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRange(ulong value, ulong minValue,
		                                     ulong maxValue)
		{
			ThrowIfOutOfRange(value, null, minValue, maxValue);
		}

		#endregion

		[DebuggerHidden]
		private static void ThrowIfOutOfRangeCore(IComparable value,
		                                          string name, ValueType minValue, ValueType maxValue)
		{
			Debug.Assert(value != null,
			             "Expected argument value not to be null!");
			Debug.Assert(value.GetType() == minValue.GetType(), "Expected "
			                                                    + "argument value to be the same type as argument minValue!");
			Debug.Assert(value.GetType() == maxValue.GetType(), "Expected "
			                                                    + "argument value to be the same type as argument maxValue!");

			if ((value.CompareTo(minValue) < 0)
			    || (0 < value.CompareTo(maxValue)))
			{
				throw new ArgumentOutOfRangeException(name, value,
				                                      string.Format("Argument {0}must be greater than or equal to "
				                                                    + "{1} and less than or equal to {2}.",
				                                                    PadWord(name), minValue, maxValue));
			}
		}

		#region ThrowIfOutOfRangeExclusive() overloads

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(int value, string name,
		                                              int lowerBound, int upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(int value, int lowerBound,
		                                              int upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(byte value, string name,
		                                              byte lowerBound, byte upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(byte value,
		                                              byte lowerBound, byte upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(char value, string name,
		                                              char lowerBound, char upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(char value,
		                                              char lowerBound, char upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(decimal value,
		                                              string name, decimal lowerBound, decimal upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(decimal value,
		                                              decimal lowerBound, decimal upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(double value,
		                                              string name, double lowerBound, double upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(double value,
		                                              double lowerBound, double upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(float value, string name,
		                                              float lowerBound, float upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(float value,
		                                              float lowerBound, float upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(short value, string name,
		                                              short lowerBound, short upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(short value,
		                                              short lowerBound, short upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(long value, string name,
		                                              long lowerBound, long upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(long value,
		                                              long lowerBound, long upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(uint value, string name,
		                                              uint lowerBound, uint upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(uint value,
		                                              uint lowerBound, uint upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(sbyte value, string name,
		                                              sbyte lowerBound, sbyte upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(sbyte value,
		                                              sbyte lowerBound, sbyte upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(ushort value, string name,
		                                              ushort lowerBound, ushort upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(ushort value,
		                                              ushort lowerBound, ushort upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(ulong value, string name,
		                                              ulong lowerBound, ulong upperBound)
		{
			ThrowIfOutOfRangeExclusiveCore(value, name, lowerBound,
			                               upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than or equal
		/// to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than or equal to
		/// <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeExclusive(ulong value,
		                                              ulong lowerBound, ulong upperBound)
		{
			ThrowIfOutOfRangeExclusive(value, null, lowerBound,
			                           upperBound);
		}

		#endregion

		[DebuggerHidden]
		private static void ThrowIfOutOfRangeExclusiveCore(IComparable value,
		                                                   string name, ValueType lowerBound, ValueType upperBound)
		{
			Debug.Assert(lowerBound != upperBound,
			             "Arguments lowerBound and upperBound cannot be equal.");

			Debug.Assert(value != null,
			             "Expected argument value not to be null!");
			Debug.Assert(value.GetType() == lowerBound.GetType(), "Expected "
			                                                      + "argument value to be the same type as argument lowerBound!");
			Debug.Assert(value.GetType() == upperBound.GetType(), "Expected "
			                                                      +
			                                                      "argument value must be the same type as argument upperBound!");

			if ((value.CompareTo(lowerBound) <= 0)
			    || (0 <= value.CompareTo(upperBound)))
			{
				throw new ArgumentOutOfRangeException(name, value,
				                                      string.Format(
				                                      	"Argument {0}must be greater than {1} and less than {2}.",
				                                      	PadWord(name), lowerBound, upperBound));
			}
		}

		#region ThrowIfOutOfRangeIncludeMax() overloads

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(int value, string name,
		                                               int lowerBound, int maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(int value,
		                                               int lowerBound, int maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(byte value, string name,
		                                               byte lowerBound, byte maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(byte value,
		                                               byte lowerBound, byte maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(char value, string name,
		                                               char lowerBound, char maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(char value,
		                                               char lowerBound, char maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(decimal value,
		                                               string name, decimal lowerBound, decimal maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(decimal value,
		                                               decimal lowerBound, decimal maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(double value,
		                                               string name, double lowerBound, double maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(double value,
		                                               double lowerBound, double maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(float value, string name,
		                                               float lowerBound, float maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(float value,
		                                               float lowerBound, float maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(short value, string name,
		                                               short lowerBound, short maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(short value,
		                                               short lowerBound, short maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(long value, string name,
		                                               long lowerBound, long maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(long value,
		                                               long lowerBound, long maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(uint value, string name,
		                                               uint lowerBound, uint maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> is less than or
		/// equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(uint value,
		                                               uint lowerBound, uint maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(sbyte value, string name,
		                                               sbyte lowerBound, sbyte maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(sbyte value,
		                                               sbyte lowerBound, sbyte maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(ushort value,
		                                               string name, ushort lowerBound, ushort maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(ushort value,
		                                               ushort lowerBound, ushort maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(ulong value, string name,
		                                               ulong lowerBound, ulong maxValue)
		{
			ThrowIfOutOfRangeIncludeMaxCore(value, name, lowerBound,
			                                maxValue);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> is less than
		/// or equal to <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="lowerBound">The lower bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="maxValue">The maximum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than or equal to
		/// <paramref name="lowerBound"/> or greater than
		/// <paramref name="maxValue"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMax(ulong value,
		                                               ulong lowerBound, ulong maxValue)
		{
			ThrowIfOutOfRangeIncludeMax(value, null, lowerBound,
			                            maxValue);
		}

		#endregion

		[DebuggerHidden]
		private static void ThrowIfOutOfRangeIncludeMaxCore(IComparable value,
		                                                    string name, ValueType lowerBound, ValueType maxValue)
		{
			Debug.Assert(value != null,
			             "Expected argument value not to be null!");
			Debug.Assert(value.GetType() == lowerBound.GetType(), "Expected "
			                                                      + "argument value to be the same type as argument lowerBound!");
			Debug.Assert(value.GetType() == maxValue.GetType(), "Expected "
			                                                    + "argument value must be the same type as argument maxValue!");

			if ((value.CompareTo(lowerBound) <= 0)
			    || (0 < value.CompareTo(maxValue)))
			{
				throw new ArgumentOutOfRangeException(name, value,
				                                      string.Format("Argument {0}must be greater than {1} and less "
				                                                    + "than or equal to {2}.", PadWord(name),
				                                                    lowerBound, maxValue));
			}
		}

		#region ThrowIfOutOfRangeIncludeMin() overloads

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(int value, string name,
		                                               int minValue, int upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="int"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(int value, int minValue,
		                                               int upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(byte value, string name,
		                                               byte minValue, byte upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="byte"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(byte value,
		                                               byte minValue, byte upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(char value, string name,
		                                               char minValue, char upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="char"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(char value,
		                                               char minValue, char upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(decimal value,
		                                               string name, decimal minValue, decimal upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="decimal"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(decimal value,
		                                               decimal minValue, decimal upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(double value,
		                                               string name, double minValue, double upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="double"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(double value,
		                                               double minValue, double upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(float value, string name,
		                                               float minValue, float upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="float"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(float value,
		                                               float minValue, float upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(short value, string name,
		                                               short minValue, short upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="short"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(short value,
		                                               short minValue, short upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(long value, string name,
		                                               long minValue, long upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="long"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(long value,
		                                               long minValue, long upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(uint value, string name,
		                                               uint minValue, uint upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="uint"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(uint value,
		                                               uint minValue, uint upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(sbyte value, string name,
		                                               sbyte minValue, sbyte upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if an
		/// <see cref="sbyte"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(sbyte value,
		                                               sbyte minValue, sbyte upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(ushort value,
		                                               string name, ushort minValue, ushort upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ushort"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(ushort value,
		                                               ushort minValue, ushort upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> named
		/// <paramref name="name"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(ulong value, string name,
		                                               ulong minValue, ulong upperBound)
		{
			ThrowIfOutOfRangeIncludeMinCore(value, name, minValue,
			                                upperBound);
		}

		/// <summary>
		/// Throws <see cref="ArgumentOutOfRangeException"/> if a
		/// <see cref="ulong"/> argument <paramref name="value"/> is less than
		/// <paramref name="minValue"/> or greater than or equal to
		/// <paramref name="upperBound"/>.
		/// </summary>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minValue">The minimum value (inclusive) of
		/// <paramref name="value"/>.</param>
		/// <param name="upperBound">The upper bound (exclusive) of
		/// <paramref name="value"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if
		/// <paramref name="value"/> is less than <paramref name="minValue"/> or
		/// greater than or equal to <paramref name="upperBound"/>.</exception>
		[DebuggerHidden]
		public static void ThrowIfOutOfRangeIncludeMin(ulong value,
		                                               ulong minValue, ulong upperBound)
		{
			ThrowIfOutOfRangeIncludeMin(value, null, minValue,
			                            upperBound);
		}

		#endregion

		[DebuggerHidden]
		private static void ThrowIfOutOfRangeIncludeMinCore(IComparable value,
		                                                    string name, ValueType minValue, ValueType upperBound)
		{
			Debug.Assert(value != null,
			             "Expected argument value not to be null!");
			Debug.Assert(value.GetType() == minValue.GetType(), "Expected "
			                                                    + "argument value to be the same type as argument minValue!");
			Debug.Assert(value.GetType() == upperBound.GetType(), "Expected "
			                                                      +
			                                                      "argument value must be the same type as argument upperBound!");

			if ((value.CompareTo(minValue) < 0)
			    || (0 <= value.CompareTo(upperBound)))
			{
				throw new ArgumentOutOfRangeException(name, value,
				                                      string.Format("Argument {0}must be greater than or equal to "
				                                                    + "{1} and less than {2}.", PadWord(name), minValue,
				                                                    upperBound));
			}
		}


		private Exceptions()
		{
		}
	}
}

//                                 ~ S. D. G. ~