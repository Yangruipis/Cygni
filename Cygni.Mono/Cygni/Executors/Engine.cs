﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.Settings;
using Cygni.AST.Scopes;
using Cygni.Libraries;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

namespace Cygni.Executors
{
	/// <summary>
	/// Description of Engine.
	/// </summary>
	public class Engine
	{
		ResizableArrayScope globalScope;

		public Engine ()
		{
			globalScope = new ResizableArrayScope ();
		}

		public void Initialize ()
		{
			globalScope.BuiltIn ();
		}

		public static Engine CreateInstance ()
		{
			var engine = new Engine ();
			engine.Initialize ();
			return engine;
		}

		public DynValue Evaluate (string code)
		{
			var executor = new CodeStringExecutor (globalScope, code);
			executor.Run ();
			return executor.Result;
		}

		public T Evaluate<T> (string code)
		{
			var executor = new CodeStringExecutor (globalScope, code);
			executor.Run ();
			var result = executor.Result;
			return (T)Convert.ChangeType (result.Value, typeof(T));
		}

		public DynValue ExecuteFile (string filepath, Encoding encoding = null)
		{
			var executor = new CodeFileExecutor (globalScope, filepath, encoding ?? Encoding.Default);
			executor.Run ();
			return executor.Result;
		}

		public T ExecuteFile<T> (string filepath, Encoding encoding = null)
		{
			var executor = new CodeFileExecutor (globalScope, filepath, encoding ?? Encoding.Default);
			executor.Run ();
			var result = executor.Result;
			return (T)Convert.ChangeType (result.Value, typeof(T));
		}

		public DynValue ExecuteFromEntryPoint (params DynValue[] args)
		{
			const string MainFuncName = "main";
			DynValue funcValue;
			if (globalScope.TryGetValue (MainFuncName, out funcValue)) {
				return funcValue.As<IFunction> ().DynInvoke (args);
			}
			throw new RuntimeException ("Missing Entry Point, expecting function '{0}'", MainFuncName);
		}

		public void ExecuteInConsole ()
		{
			var executor = new InteractiveExecutor (globalScope);
			executor.Run ();
		}

		public Engine SetSymbol (string name, DynValue value)
		{
			globalScope.Put (name, value);
			return this;
		}

		public DynValue GetSymbol (string name)
		{
			return globalScope.Get (name);
		}

		public bool TryGetValue (string name, out DynValue value)
		{
			return globalScope.TryGetValue (name, out value);
		}

	}
}
