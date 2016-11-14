﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using System.Collections;
namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of ClassInfo.
	/// </summary>
	public sealed class ClassInfo: IComputable,IEnumerable<DynValue>, IComparable<DynValue>, IDot,IFunction
	{
		readonly string name;
		BlockEx body;
		NestedScope classScope;
		ClassInfo[] parents;
		bool IsInstance;

		public ClassInfo (string name, NestedScope classScope, BlockEx body = null, ClassInfo[] parents = null, bool IsInstance = false)
		{
			this.name = name;
			this.classScope = classScope;
			this.parents = parents;
			this.IsInstance = IsInstance;
			this.body = body;
			body.Eval (classScope); // FIX ME: 'this' keyword optimaization
		}

		public ClassInfo Init (DynValue[] parameters)
		{
			var newScope = classScope.Clone();
			var newClass = new ClassInfo (name: name, classScope: newScope, body: body, parents: parents, IsInstance: true);
			newScope.Put ("this", DynValue.FromClass (newClass)); /* define 'this' */
			if (this.HasParents)
				newClass.InitParents ();/* initialize parents */
			if (newScope.HasName ("__INIT__")) /* initialize */
				newScope.Get ("__INIT__").As<Function> ().Update (parameters).Invoke ();
			return newClass;
		}

		void InitParents ()
		{
			for (int i = 0; i < parents.Length; i++) {
				classScope.Put (parents [i].name, DynValue.FromClass (parents [i]));
				/* add parent class pointers */
			}
		}
		public bool HasParents {
			get { return this.parents != null; }
		}
#region IDot implementation

		public DynValue GetByDot (string fieldname)
		{
			return Search (fieldname);
		}

		public DynValue SetByDot (string fieldname, DynValue value)
		{
			return classScope.Put (fieldname, value);
		}

#endregion

		DynValue Search (string fieldname)
		{
			DynValue value;
			if (classScope.TryGetValue (fieldname, out value))/* Find in self */
				return value;
			if (parents != null)
				foreach (var parent in parents) { /* Find in parents */
					if (parent.classScope.TryGetValue (fieldname, out value))
						return value;
				}
			throw RuntimeException.NotDefined (fieldname);
		}

#region IComparable implementation

		public int CompareTo (DynValue other)
		{
			return (int)classScope.Get ("__COMPARETO__").As<Function> ().Update (new []{ other }).Invoke ().AsNumber ();
		}

#endregion

#region IComputable implementation


		public DynValue Add (DynValue other)
		{
			return classScope.Get ("__ADD__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Subtract (DynValue other)
		{
			return classScope.Get ("__SUBTRACT__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Multiply (DynValue other)
		{
			return classScope.Get ("__MULTIPLY__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Divide (DynValue other)
		{
			return classScope.Get ("__DIVIDE__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Modulo (DynValue other)
		{
			return classScope.Get ("__MODULO__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Power (DynValue other)
		{
			return classScope.Get ("__POWER__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue UnaryPlus ()
		{
			return classScope.Get ("__UNARYPLUS__").As<Function> ().Update (new DynValue[0]).Invoke ();
		}


		public DynValue UnaryMinus ()
		{
			return classScope.Get ("__UNARYMINUS__").As<Function> ().Update (new DynValue[0]).Invoke ();
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			return DynValue.FromClass (this.Init (args));
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return (args) => DynValue.FromClass (this.Init (args));
		}

#endregion

		public override string ToString ()
		{
			if (IsInstance && classScope.HasName ("__TOSTRING__"))
				return classScope.Get ("__TOSTRING__").As<Function> ().Update (new DynValue[0]).Invoke ().AsString ();
			else
				return string.Concat ("(class: ", name, ")");
		}

		public IEnumerator<DynValue> GetEnumerator ()
		{
			if (classScope.HasName ("__COLLECTION__")) {
				var collection = classScope.Get ("__COLLECTION__").As<Function> ().Invoke ().As<IEnumerable<DynValue>> ();
				foreach (var item in collection) 
					yield return item;
			} else {
				classScope.Get ("__ITER__").As<Function> ().Invoke ();
				var next = classScope.Get ("__Next__").As<Function> ().AsDelegate ();
				var current = classScope.Get ("__CURRENT__").As<Function> ().AsDelegate ();
				while (next (new DynValue[0]).AsBoolean()) {
					yield return current (new DynValue[0]);
				}
			}
		}

		System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator ()
		{
			yield return this.AsEnumerable ();
		}
	}
}