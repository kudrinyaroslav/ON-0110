using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Definitions.Interfaces
{
    public interface IProfileDefinition
    {
        /// <summary>
        /// List of functionalities included
        /// </summary>
        IEnumerable<FunctionalityItem> Functionalities { get; }

        bool IsFunctionalityOptional(Functionality functionality);
        /// <summary>
        /// List of mandatory scopes
        /// </summary>
        IEnumerable<String> MandatoryScopes { get; }
        /// <summary>
        /// Profile name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Scope used to "claim" profile support.
        /// </summary>
        string Scope { get; }
        /// <summary>
        /// Performs profile conformance testing.
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="features"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes);

    }
}
