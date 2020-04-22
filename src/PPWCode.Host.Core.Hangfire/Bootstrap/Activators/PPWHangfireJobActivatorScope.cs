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
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;

using Hangfire;
using Hangfire.Server;

using JetBrains.Annotations;

using PPWCode.API.Core.Exceptions;

namespace PPWCode.Server.Core.Hangfire.Bootstrap.Activators
{
    public class PPWHangfireJobActivatorScope : JobActivatorScope
    {
        private IDisposable _scope;

        public PPWHangfireJobActivatorScope(
            [NotNull] IKernel kernel,
            [NotNull] PerformContext performContext)
        {
            Kernel = kernel;
            PerformContext = performContext;
        }

        [NotNull]
        public IKernel Kernel { get; }

        [NotNull]
        public PerformContext PerformContext { get; }

        public PPWHangfireJobActivatorScope BeginScope()
        {
            if (_scope != null)
            {
                throw new ApiUsageError($"Did not expect more than one call, {nameof(BeginScope)}() on the same JobActivatorScope instance.");
            }

            _scope = Kernel.BeginScope();
            return this;
        }

        /// <inheritdoc />
        public override object Resolve(Type type)
        {
            Arguments arguments =
                Arguments
                    .FromProperties(
                        new
                        {
                            performContext = PerformContext,
                            callContextLifetimeScope = _scope as CallContextLifetimeScope
                        });
            return Kernel.Resolve(type, arguments);
        }

        /// <inheritdoc />
        public override void DisposeScope()
        {
            if (_scope != null)
            {
                _scope.Dispose();
                _scope = null;
            }
        }
    }
}
