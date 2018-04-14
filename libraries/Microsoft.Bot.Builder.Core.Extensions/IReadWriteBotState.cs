// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Core.Extensions
{
    /// <summary>
    /// Interface supported by State Managers that enables reading/writing
    /// to be done in parallel by the BotStateSet. This avoids latency
    /// penalties when multiple state services are installed. 
    /// </summary>
    public interface IReadWriteBotState
    {
        Task ReadToContextService(ITurnContext context);
        Task WriteFromContextService(ITurnContext context);
    }
}
