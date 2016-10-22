﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.Settings;
namespace Cygni.Executors
{
	/// <summary>
	/// Description of Engine.
	/// </summary>
	public class Engine
	{
		BasicScope globalScope;
		public Engine()
		{
			globalScope = new BasicScope();
		}
		
		public void Initialize()
		{
			GlobalSettings.SetBuiltInFunctions(globalScope);
			GlobalSettings.SetBuiltInVariables(globalScope);
		}
		
		public static Engine CreateInstance()
		{
			var engine = new Engine();
			engine.Initialize();
			return engine;
		}
		
		public DynValue Evaluate(string code)
		{
			var executor = new CodeStringExecutor(globalScope, code);
			return executor.Run();
		}
		
		public DynValue DoFile(string filepath, Encoding encoding = null)
		{
			var executor = new CodeFileExecutor(globalScope, filepath, encoding ?? Encoding.Default);
			return executor.Run();
		}
		
		public DynValue ExecuteInConsole()
		{
			var executor = new InteractiveExecutor(globalScope);
			return executor.Run();
		}
		
		public Engine SetSymbol(string name, DynValue value)
		{
			globalScope[name] = value;
			return this;
		}
		
		public DynValue GetSymbol(string name)
		{
			return globalScope[name];
		}
		
		public bool TryGetValue(string name, out DynValue value)
		{
			return globalScope.TryGetValue(name, out value);
		}
	}
}