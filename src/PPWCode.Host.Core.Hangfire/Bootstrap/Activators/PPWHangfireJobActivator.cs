// Copyright 2020 by PeopleWare n.v..
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

using Castle.MicroKernel;

using Hangfire;
using Hangfire.Server;

using JetBrains.Annotations;

using PPWCode.API.Core.Exceptions;

namespace PPWCode.Server.Core.Hangfire.Bootstrap.Activators
{
    public class PPWHangfireJobActivator : JobActivator
    {
        public PPWHangfireJobActivator(
            [NotNull] IKernel kernel)
        {
            Kernel = kernel;
        }

        [NotNull]
        public IKernel Kernel { get; }

        /// <inheritdoc />
        public override object ActivateJob([NotNull] Type jobType)
            => throw new ApiUsageError($"Asked to resolve a type, {jobType.FullName}. Did not expect this call, are you using obsolete code?");

        /// <inheritdoc />
        public override JobActivatorScope BeginScope([NotNull] JobActivatorContext context)
            => throw new ApiUsageError($"Asked to begin a scope, for job-type {context.BackgroundJob?.Job?.Type?.FullName}. Did not expect this call, are you using obsolete code?");

        /// <inheritdoc />
        public override JobActivatorScope BeginScope([NotNull] PerformContext context)
            => new PPWHangfireJobActivatorScope(Kernel, context).BeginScope();
    }
}
