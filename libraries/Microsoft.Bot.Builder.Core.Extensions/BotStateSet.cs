// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Core.Extensions
{
    /// <summary>
    /// Middleware that will call `read()` and `write()` in parallel 
    /// on multiple `BotState` instances.
    /// </summary>
    public class BotStateSet : IMiddleware
    {
        private IList<IReadWriteBotState> _stateManagers = new List<IReadWriteBotState>();

        /// <summary>
        /// Creates a new instance of the BotStateSet without any registered state plugins
        /// </summary>
        public BotStateSet()
        {
        }

        /// <summary>
        /// Creates a new instance of the BotStateSet
        /// </summary>
        /// <param name="middleware">One or more BotState plugins to register</param>
        public BotStateSet(IReadWriteBotState [] middleware)
        {
            this.Use(middleware); 
        }

        /// <summary>
        /// Registers BotState middleware plugins with the set.
        /// </summary>
        /// <param name="middleware">One or more BotState plugins to register.</param>
        /// <returns></returns>
        public BotStateSet Use(IReadWriteBotState[] middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            if (middleware.Length == 0)
                throw new ArgumentException(nameof(middleware));

            foreach (var item in middleware)
            {
                _stateManagers.Add(item);
            }

            return this; 
        }

        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            await this.ReadAll(context);
            await next();
            await this.WriteAll(context);
        }

        /// <summary>
        /// Calls BotState.read() on all of the BotState plugins in the set. This will trigger 
        /// all of the plugins to read in their state in parallel.
        /// </summary>
        /// <param name="context">Context for current turn of conversation with the user.</param>        
        public async Task ReadAll(ITurnContext context)
        {
            List<Task> x = new List<Task>();
            foreach(var l in _stateManagers)
            {
                x.Add(l.ReadToContextService(context));
            }
            await Task.WhenAll(x);
        }

        /// <summary>
        /// Calls BotState.write() on all of the BotState plugins in the set. This will 
        /// trigger all of the plugins to write out their state in parallel.
        /// </summary>
        /// <param name="context">Context for current turn of conversation with the user.</param>        
        public async Task WriteAll(ITurnContext context)
        {
            List<Task> x = new List<Task>();
            foreach (var l in _stateManagers)
            {
                x.Add(l.WriteFromContextService(context));
            }
            await Task.WhenAll(x);
        }
    }
}
