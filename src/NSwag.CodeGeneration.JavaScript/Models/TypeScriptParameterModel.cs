//-----------------------------------------------------------------------
// <copyright file="TypeScriptParameterModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.Models;

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>The TypeScript parameter model.</summary>
    public class TypeScriptParameterModel : ParameterModelBase
    {
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="TypeScriptParameterModel" /> class.</summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="typeName">The type name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="allParameters">All parameters.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The client generator base.</param>
        /// <param name="typeResolver">The type resolver.</param>
        public TypeScriptParameterModel(
            string parameterName,
            string variableName,
            string typeName,
            SwaggerParameter parameter,
            IList<SwaggerParameter> allParameters,
            SwaggerToJavaScriptClientGeneratorSettings settings,
            SwaggerToJavaScriptClientGenerator generator,
            TypeResolverBase typeResolver)
            : base(parameterName, variableName, typeName, parameter, allParameters, settings.TypeScriptGeneratorSettings, generator, typeResolver)
        {
            this._settings = settings;
        }

        /// <summary>Gets the type postfix (e.g. ' | null | undefined')</summary>
        public string TypePostfix
        {
            get
            {
                if (this._settings.TypeScriptGeneratorSettings.SupportsStrictNullChecks)
                {
                    return (this.IsNullable == true ? " | null" : "") + (this.IsRequired == false ? " | undefined" : "");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}