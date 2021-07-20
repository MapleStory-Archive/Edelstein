﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Scripting;
using NLua;

namespace Edelstein.Common.Scripting.NLua
{
    public class LuaScript : IScript
    {
        private readonly string _source;

        public LuaScript(string source)
            => _source = source;

        public Task<object> Evaluate(IDictionary<string, object> globals = null)
        {
            var state = new Lua();

            if (globals != null)
                foreach (var entry in globals) state[entry.Key] = entry.Value;
            return Task.FromResult(state.DoString(_source)[0]);
        }

        public Task<IScriptState> Run(IDictionary<string, object> globals = null)
        {
            var state = new Lua();

            if (globals != null)
                foreach (var entry in globals) state[entry.Key] = entry.Value;
            state.DoString(_source);

            return Task.FromResult<IScriptState>(new LuaScriptState(state));
        }
    }
}