﻿using System;
using System.Linq;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
namespace CygniLib.Collections
{
	public class stack: Stack<DynValue>,IDot
	{
		public stack():base(){
		}
		public DynValue GetByDot(string fieldName){
			switch(fieldName){
			case "push":
				return DynValue.FromDelegate (args =>{
					RuntimeException.FuncArgsCheck(args.Length == 1,"push");
					this.Push (args [0]);
					return DynValue.Null;
				});
			case "pop":
				return DynValue.FromDelegate (args => this.Pop ());
			case "peek":
				return DynValue.FromDelegate (args => this.Peek ());
			case "count":
				return (double)this.Count;
			default:
					throw RuntimeException.FieldNotExist ("stack", fieldName);
			}
		}
		public string[] FieldNames{get{ return new string[] {
					"push","pop","peek","count"
			};
			}
		}
		public DynValue SetByDot(string fieldName, DynValue value){
					throw RuntimeException.FieldNotExist ("stack", fieldName);
		}
		public override string ToString ()
		{
			return string.Concat ("stack([", string.Join (", ", this.Select (i => i.Value)), "])");
		}
	}
}

