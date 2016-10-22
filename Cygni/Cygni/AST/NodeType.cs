﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.AST
{
	/// <summary>
	/// Description of NodeType.
	/// </summary>
	public enum NodeType
	{
		Binary,
		Block,
		Constant,
		DefClass,
		DefFunc,
		Dot,
		If,
		Invoke,
		Name,
		Unary,
		While,
		For,
		Return,
		Command,
		ListInit,
		Index,
	}
}