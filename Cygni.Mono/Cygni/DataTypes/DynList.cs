﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynList.
	/// </summary>
	public sealed class DynList:List<DynValue>, IIndexable, IDot
	{
		public DynList (int capacity)
			: base (capacity)
		{
		}

		public DynList (IEnumerable<DynValue>collection)
			: base (collection)
		{
		}

		public DynList (IEnumerable<DynValue>collection, int capacity)
			: base (capacity)
		{
			AddRange (collection);
		}

		public DynValue GetByIndex (DynValue index)
		{
			if (index.type == DataType.Range) {
				return ListLib.Slice (this, index.Value as Range);
			} else {
				return base [index.AsInt32 ()];
			}
		}

		public DynValue SetByIndex (DynValue index, DynValue value)
		{
			if (index.type == DataType.Range) {
				return ListLib.SliceAssign (this, index.Value as Range, value);
			} else {
				return base [index.AsInt32 ()] = value;
			}
		}

		public DynValue GetByIndexes (DynValue[] indexes)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "list");
			return this [indexes [0].AsInt32 ()];
		}

		public DynValue SetByIndexes (DynValue[] indexes, DynValue value)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "list");
			return this [indexes [0].AsInt32 ()] = value; 
		}


		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "append":
				return DynValue.FromDelegate ("append", (args) => ListLib.append (this, args));
			case "count":
				return (long)this.Count;
			case "removeAt":
				return DynValue.FromDelegate ("removeAt", (args) => ListLib.removeAt (this, args));
			case "insert":
				return DynValue.FromDelegate ("insert", (args) => ListLib.insert (this, args));
			case "sort":
				return DynValue.FromDelegate ("sort", (args) => ListLib.sort (this, args));
			case "max":
				return DynValue.FromDelegate ("max", (args) => ListLib.max (this, args));
			case "min":
				return DynValue.FromDelegate ("min", (args) => ListLib.min (this, args));
			case "bSearch":
				return DynValue.FromDelegate ("bSearch", (args) => ListLib.bSearch (this, args));
			case "find":
				return DynValue.FromDelegate ("find", (args) => ListLib.find (this, args));
			case "concat":
				return DynValue.FromDelegate ("concat", (args) => ListLib.concat (this, args));
			case "copy":
				return DynValue.FromDelegate ("copy", (args) => ListLib.copy (this, args));
			case "pop":
				return DynValue.FromDelegate ("pop", (args) => ListLib.pop (this, args));
			case "clear":
				return DynValue.FromDelegate ("clear", (args) => ListLib.clear (this, args));
			default:
				throw RuntimeException.FieldNotExist ("list", fieldName);
			}
		}

		public string[] FieldNames {
			get {
				return new [] {
					"append", "count", "removeAt", "insert", 
					"sort", "max", "min", "bSearch", "find", 
					"concat", "copy", "pop", "clear"
				};
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("list", fieldName);
		}

		public override string ToString ()
		{
			return string.Concat ("[ ", string.Join (", ", this), " ]");
		}
	}
}
