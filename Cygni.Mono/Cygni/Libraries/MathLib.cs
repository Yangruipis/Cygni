﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Errors;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of MathLib.
	/// </summary>
	public static class MathLib
	{
		public static DynValue sqrt(DynValue[] args)
		{
			return (Math.Sqrt(args[0].AsNumber()));
		}
		public static DynValue abs(DynValue[] args)
		{
			return (Math.Abs(args[0].AsNumber()));
		}
		public static DynValue log(DynValue[] args)
		{
			if (args.Length == 1)
				return (Math.Log(args[0].AsNumber()));
			if (args.Length == 2)
				return (Math.Log(args[0].AsNumber(), args[1].AsNumber()));
			throw RuntimeException.BadArgsNum ("log", "1 or 2");
		}
		public static DynValue max(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "max");
			return (Math.Max(args[0].AsNumber(), args[1].AsNumber()));
		}
		public static DynValue min(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "min");
			return (Math.Min(args[0].AsNumber(), args[1].AsNumber()));
		}
		public static DynValue exp(DynValue[] args)
		{
			return (Math.Exp(args[0].AsNumber()));
		}
		public static DynValue sign(DynValue[] args){
			return (double)Math.Sign(args[0].AsNumber());
		}
		public static DynValue sin(DynValue[] args){
			return Math.Sin (args [0].AsNumber ());
		}
		public static DynValue cos(DynValue[] args){
			return Math.Cos (args [0].AsNumber ());
		}
		public static DynValue tan(DynValue[] args){
			return Math.Tan (args [0].AsNumber ());
		}
		public static DynValue asin(DynValue[] args){
			return Math.Asin (args [0].AsNumber ());
		}
		public static DynValue acos(DynValue[] args){
			return Math.Acos (args [0].AsNumber ());
		}
		public static DynValue atan(DynValue[] args){
			return Math.Atan (args [0].AsNumber ());
		}
		public static DynValue ceiling(DynValue[] args){
			return Math.Ceiling (args [0].AsNumber ());
		}
		public static DynValue floor(DynValue[] args){
			return Math.Floor (args [0].AsNumber ());
		}
		public static DynValue round(DynValue[] args){
			return Math.Round (args [0].AsNumber (), MidpointRounding.AwayFromZero);
		}
	}
}