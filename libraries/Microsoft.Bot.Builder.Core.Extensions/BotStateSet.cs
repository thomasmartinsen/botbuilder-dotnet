// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Core.Extensions
{
    /// <summary>
    /// Middleware that will call `read()` and `write()` in parallel 
    /// on multiple `BotState` instances.
    /// </summary>
    public class BotStateSet : IMiddleware
    {
        private IList<BotState> 
        public Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
