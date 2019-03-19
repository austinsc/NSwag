//-----------------------------------------------------------------------
// <copyright file="TypeScriptTypeNameGenerator.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NJsonSchema;

namespace NSwag.CodeGeneration.JavaScript
{
    /// <summary>Generates a TypeScript type name (Error is renamed to ErrorDto, otherwise the defaults are used).</summary>
    public class JavaScriptTypeNameGenerator : DefaultTypeNameGenerator
    {
        /// <summary>Generates the type name for the given schema.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="typeNameHint">The type name hint.</param>
        /// <returns>The type name.</returns>
        protected override string Generate(JsonSchema4 schema, string typeNameHint)
        {
            if (typeNameHint == "Error")
                typeNameHint = "ErrorDto";

            return base.Generate(schema, typeNameHint);
        }
    }
}