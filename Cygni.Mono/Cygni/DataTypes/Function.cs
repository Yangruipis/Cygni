﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
    /// <summary>
    /// Description of Function.
    /// </summary>
    public sealed class Function:IFunction
    {
        readonly ASTNode body;
        readonly FunctionScope funcScope;
        readonly int nArgs;

        public string Name { get { return this.funcScope.ScopeName; } }

        public Function(int nArgs, ASTNode body, FunctionScope funcScope)
        {
            this.body = body;
            this.funcScope = funcScope;
            this.nArgs = nArgs;
        }

        public Function Update(IScope scope)
        {
            return new Function(nArgs, body, this.funcScope.Update(scope));
        }

        public DynValue Invoke()
        {
            DynValue result = body.Eval(funcScope);
            return result.type == DataType.Return
				? result.Value as DynValue
				: DynValue.Void;
        }

        public DynValue DynInvoke(DynValue[] args)
        {
            if (args.Length > nArgs)
            {
                throw RuntimeException.BadArgsNum(Name, nArgs);
            }
            DynValue[] values = new DynValue[funcScope.Count];
            int i = 0;
            while (i < args.Length)
            {
                values[i] = args[i];
                i++;
            }
            while (i < nArgs)
            {
                values[i] = DynValue.Nil;
                i++;
            }
            var newScope = new FunctionScope(this.Name, values, funcScope.Parent);
            return new Function(nArgs, body, newScope).Invoke();
        }

        public DynValue DynEval(ASTNode[] args, IScope scope)
        {
            if (args.Length > nArgs)
                throw RuntimeException.BadArgsNum(Name, nArgs);
            DynValue[] values = new DynValue[funcScope.Count];
            int i = 0;
            while (i < args.Length)
            {
                values[i] = args[i].Eval(scope);
                i++;
            }
            while (i < nArgs)
            {
                values[i] = DynValue.Nil;
                i++;
            }
            var newScope = new FunctionScope(this.Name, values, funcScope.Parent);
            return new Function(nArgs, body, newScope).Invoke();
        }

        public override string ToString()
        {
            return "(Function: " + this.Name + ")";
        }
    }
}
