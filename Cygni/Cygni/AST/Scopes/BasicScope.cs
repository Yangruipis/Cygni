﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
namespace Cygni.AST.Scopes
{
	/// <summary>
	/// Description of BasicScope.
	/// </summary>
	public sealed class BasicScope:IScope
	{
		EnvTable envTable;
		public BasicScope()
		{
			this.envTable = new EnvTable ();
		}
		public int Count { get { return envTable.Count; } }
		public DynValue Get(string name){
			DynValue _value;
			if (envTable.TryGetValue (name, out _value))
				return _value;
			throw RuntimeException.NotDefined (name);
		}
		public DynValue Put (string name, DynValue value){
			return envTable[name] = value;
		}
		public bool HasName(string name) {
			return envTable.ContainsKey(name);
		}
		public bool TryGetValue(string name,out DynValue value){
			return envTable.TryGetValue (name, out value);
		}
		public DynValue Get(int index){
			throw new NotSupportedException ();
		}
		public DynValue Put(int index,DynValue value){
			throw new NotSupportedException ();
		}
		public bool Delete(string name){
			return envTable.Remove (name);
		}
	}
}