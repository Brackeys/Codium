// Written by Michael 'Searge' Stoyke in 03/2015
// Released as public domain, do whatever you want with it!

using System;
using System.IO;
using System.Reflection;
using System.Text;
using Mono.CSharp;

using UnityEngine;

public class MCC
{
	private Evaluator _evaluator;

	private StringBuilder _stringBuilder;
	private ModuleContainer _module;
	private ReflectionImporter _importer;
	private MethodInfo _importTypes;
	private string _error = string.Empty;

	public MCC()
	{
		_stringBuilder = new StringBuilder();
		TextWriter _writer = new StringWriter(_stringBuilder);
		CompilerSettings settings = new CompilerSettings();
		settings.LoadDefaultReferences = false;
		settings.StdLib = false;
		ReportPrinter printer = new StreamReportPrinter(_writer);
		CompilerContext ctx = new CompilerContext(settings, printer);
		_evaluator = new Evaluator(ctx);

		InitEvaluator();
		RegisterTypes(BuiltInTypes);
		RegisterTypes(AdditionalTypes);
	}

	public bool Run(string code)
	{
		_stringBuilder.Length = 0;
		var result = _evaluator.Run(code);
		_error = _stringBuilder.ToString();
		return result;
	}

	public bool Evaluate<T>(string code, out T result)
	{
		object resultVal;
		bool resultSet;

		result = default(T);
		_stringBuilder.Length = 0;
		string res = _evaluator.Evaluate(code, out resultVal, out resultSet);
		_error = _stringBuilder.ToString();

		if (res != null)
		{
			return false;
		}

		if (resultSet)
		{
			result = (T)resultVal;
		}
		return true;
	}

	public string GetLastError()
	{
		return _error;
	}

	public void RegisterAssemblies(params Assembly[] apiAssembly)
	{
		foreach (var assembly in apiAssembly)
		{
			_evaluator.ReferenceAssembly(assembly);
		}
	}

	public void RegisterTypes(params Type[] types)
	{
		_importTypes.Invoke(_importer, new object[] { types, _module.GlobalRootNamespace, false });
	}

	private void InitEvaluator()
	{
		var fieldInfo1 = _evaluator.GetType().GetField("importer", BindingFlags.Instance | BindingFlags.NonPublic);
		_importer = (ReflectionImporter)fieldInfo1.GetValue(_evaluator);

		var fieldInfo2 = _evaluator.GetType().GetField("module", BindingFlags.Instance | BindingFlags.NonPublic);
		_module = (ModuleContainer)fieldInfo2.GetValue(_evaluator);

		_importTypes = _importer.GetType().GetMethod("ImportTypes", BindingFlags.NonPublic | BindingFlags.Instance, null,
			CallingConventions.Any, new Type[] { typeof(Type[]), typeof(Namespace), typeof(bool) }, null);
	}

	private static Type[] BuiltInTypes
	{
		get
		{
			var types = new Type[] {
				typeof (System.Object), typeof (ValueType), typeof (System.Attribute), typeof (Int32),
				typeof (UInt32), typeof (Int64), typeof (UInt64), typeof (Single),
				typeof (Double), typeof (Char), typeof (Int16), typeof (Decimal),
				typeof (Boolean), typeof (SByte), typeof (Byte), typeof (UInt16),
				typeof (String), typeof (System.Enum), typeof (System.Delegate), typeof (MulticastDelegate),
				typeof (void), typeof (Array), typeof (Type), typeof (System.Collections.IEnumerator),
				typeof (System.Collections.IEnumerable), typeof (IDisposable), typeof (IntPtr), typeof (UIntPtr),
				typeof (RuntimeFieldHandle), typeof (RuntimeTypeHandle), typeof (Exception), typeof (ParamArrayAttribute),
				typeof (System.Runtime.InteropServices.OutAttribute),
			};
			return types;
		}
	}

	private static Type[] AdditionalTypes
	{
		get
		{
			var types = new Type[] {
				//Extra
				typeof (Debug),
				typeof (MonoBehaviour),
				typeof (ICodiumBase),

				// mscorlib System
				typeof (Console),
				typeof (Action),
				typeof (Action<>),
				typeof (Action<,>),
				typeof (Action<,,>),
				typeof (Action<,,,>),
				typeof (ArgumentException),
				typeof (ArgumentNullException),
				typeof (ArgumentOutOfRangeException),
				typeof (ArithmeticException),
				typeof (ArraySegment<>),
				typeof (ArrayTypeMismatchException),
				typeof (AsyncCallback),
				typeof (BitConverter),
				typeof (Buffer),
				typeof (Comparison<>),
				typeof (Convert),
				typeof (Converter<,>),
				typeof (DateTime),
				typeof (DateTimeKind),
				typeof (DateTimeOffset),
				typeof (DayOfWeek),
				typeof (DivideByZeroException),
				typeof (EventArgs),
				typeof (EventHandler),
				typeof (EventHandler<>),
				typeof (FlagsAttribute),
				typeof (FormatException),
				typeof (Func<>),
				typeof (Func<,>),
				typeof (Func<,,>),
				typeof (Func<,,,>),
				typeof (Func<,,,,>),
				typeof (Guid),
				typeof (IAsyncResult),
				typeof (ICloneable),
				typeof (IComparable),
				typeof (IComparable<>),
				typeof (IConvertible),
				typeof (ICustomFormatter),
				typeof (IEquatable<>),
				typeof (IFormatProvider),
				typeof (IFormattable),
				typeof (IndexOutOfRangeException),
				typeof (InvalidCastException),
				typeof (InvalidOperationException),
				typeof (InvalidTimeZoneException),
				typeof (Math),
				typeof (MidpointRounding),
				typeof (NonSerializedAttribute),
				typeof (NotFiniteNumberException),
				typeof (NotImplementedException),
				typeof (NotSupportedException),
				typeof (Nullable),
				typeof (Nullable<>),
				typeof (NullReferenceException),
				typeof (ObjectDisposedException),
				typeof (ObsoleteAttribute),
				typeof (OverflowException),
				typeof (Predicate<>),
				typeof (System.Random),
				typeof (RankException),
				typeof (SerializableAttribute),
				typeof (StackOverflowException),
				typeof (StringComparer),
				typeof (StringComparison),
				typeof (StringSplitOptions),
				typeof (SystemException),
				typeof (TimeoutException),
				typeof (TimeSpan),
				typeof (TimeZone),
				typeof (TimeZoneInfo),
				typeof (TimeZoneNotFoundException),
				typeof (TypeCode),
				typeof (Version),
				typeof (WeakReference),
				// mscorlib System.Collections
				typeof (System.Collections.BitArray),
				typeof (System.Collections.ICollection),
				typeof (System.Collections.IComparer),
				typeof (System.Collections.IDictionary),
				typeof (System.Collections.IDictionaryEnumerator),
				typeof (System.Collections.IEqualityComparer),
				typeof (System.Collections.IList),
				// mscorlib System.Collections.Generic
				typeof (System.Collections.Generic.Comparer<>),
				typeof (System.Collections.Generic.Dictionary<,>),
				typeof (System.Collections.Generic.EqualityComparer<>),
				typeof (System.Collections.Generic.ICollection<>),
				typeof (System.Collections.Generic.IComparer<>),
				typeof (System.Collections.Generic.IDictionary<,>),
				typeof (System.Collections.Generic.IEnumerable<>),
				typeof (System.Collections.Generic.IEnumerator<>),
				typeof (System.Collections.Generic.IEqualityComparer<>),
				typeof (System.Collections.Generic.IList<>),
				typeof (System.Collections.Generic.KeyNotFoundException),
				typeof (System.Collections.Generic.KeyValuePair<,>),
				typeof (System.Collections.Generic.List<>),
				// mscorlib System.Collections.ObjectModel
				typeof (System.Collections.ObjectModel.Collection<>),
				typeof (System.Collections.ObjectModel.KeyedCollection<,>),
				typeof (System.Collections.ObjectModel.ReadOnlyCollection<>),
				// mscorlib System.Globalization
				typeof (System.Globalization.CharUnicodeInfo),
				typeof (System.Globalization.CultureInfo),
				typeof (System.Globalization.DateTimeFormatInfo),
				typeof (System.Globalization.DateTimeStyles),
				typeof (System.Globalization.NumberFormatInfo),
				typeof (System.Globalization.NumberStyles),
				typeof (System.Globalization.RegionInfo),
				typeof (System.Globalization.StringInfo),
				typeof (System.Globalization.TextElementEnumerator),
				typeof (System.Globalization.TextInfo),
				typeof (System.Globalization.UnicodeCategory),
				// mscorlib System.IO
				typeof (BinaryReader),
				typeof (BinaryWriter),
				typeof (BufferedStream),
				typeof (EndOfStreamException),
				typeof (FileAccess),
				typeof (FileMode),
				typeof (FileNotFoundException),
				typeof (IOException),
				typeof (MemoryStream),
				typeof (Path),
				typeof (PathTooLongException),
				typeof (SeekOrigin),
				typeof (Stream),
				typeof (StringReader),
				typeof (StringWriter),
				typeof (TextReader),
				typeof (TextWriter),
				// mscorlib System.Text
				typeof (ASCIIEncoding),
				typeof (Decoder),
				typeof (Encoder),
				typeof (Encoding),
				typeof (EncodingInfo),
				typeof (StringBuilder),
				typeof (UnicodeEncoding),
				typeof (UTF32Encoding),
				typeof (UTF7Encoding),
				typeof (UTF8Encoding),
				// System System.Collections.Generic
				typeof (System.Collections.Generic.LinkedList<>),
				typeof (System.Collections.Generic.LinkedListNode<>),
				typeof (System.Collections.Generic.Queue<>),
				typeof (System.Collections.Generic.SortedDictionary<,>),
				typeof (System.Collections.Generic.SortedList<,>),
				typeof (System.Collections.Generic.Stack<>),
				// System System.Collections.Specialized
				typeof (System.Collections.Specialized.BitVector32),
				// System System.IO.Compression
				typeof (System.IO.Compression.CompressionMode),
				typeof (System.IO.Compression.DeflateStream),
				typeof (System.IO.Compression.GZipStream),
				// System System.Text.RegularExpressions
				typeof (System.Text.RegularExpressions.Capture),
				typeof (System.Text.RegularExpressions.CaptureCollection),
				typeof (System.Text.RegularExpressions.Group),
				typeof (System.Text.RegularExpressions.GroupCollection),
				typeof (System.Text.RegularExpressions.Match),
				typeof (System.Text.RegularExpressions.MatchCollection),
				typeof (System.Text.RegularExpressions.MatchEvaluator),
				typeof (System.Text.RegularExpressions.Regex),
				typeof (System.Text.RegularExpressions.RegexCompilationInfo),
				typeof (System.Text.RegularExpressions.RegexOptions),
				// System.Core System.Collections.Generic
				typeof (System.Collections.Generic.HashSet<>),
				// System.Core System.Linq
				typeof (System.Linq.Enumerable),
				typeof (System.Linq.IGrouping<,>),
				typeof (System.Linq.ILookup<,>),
				typeof (System.Linq.IOrderedEnumerable<>),
				typeof (System.Linq.IOrderedQueryable),
				typeof (System.Linq.IOrderedQueryable<>),
				typeof (System.Linq.IQueryable),
				typeof (System.Linq.IQueryable<>),
				typeof (System.Linq.IQueryProvider),
				typeof (System.Linq.Lookup<,>),
				typeof (System.Linq.Queryable),
			};
			return types;
		}
	}
}
