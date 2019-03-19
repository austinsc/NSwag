//-----------------------------------------------------------------------
// <copyright file="TypeScriptResponseModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.Models;

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>The TypeScript response model.</summary>
    public class JavaScriptResponseModel : ResponseModelBase
    {
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="JavaScriptResponseModel" /> class.</summary>
        /// <param name="operationModel">The operation model.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="response">The response.</param>
        /// <param name="isPrimarySuccessResponse">if set to <c>true</c> [is success response].</param>
        /// <param name="exceptionSchema">The exception schema.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="settings">The settings.</param>
        public JavaScriptResponseModel(IOperationModel operationModel, SwaggerOperation operation, string statusCode, SwaggerResponse response, bool isPrimarySuccessResponse, 
            JsonSchema4 exceptionSchema, IClientGenerator generator, TypeResolverBase resolver, SwaggerToJavaScriptClientGeneratorSettings settings) 
            : base(operationModel, operation, statusCode, response, isPrimarySuccessResponse, exceptionSchema, resolver, settings.TypeScriptGeneratorSettings, generator)
        {
            this._settings = settings;
        }

        /// <summary>Gets or sets the data conversion code.</summary>
        public string DataConversionCode { get; set; }

        /// <summary>Gets or sets a value indicating whether to use a DTO class.</summary>
        public bool UseDtoClass => this._settings.TypeScriptGeneratorSettings.GetTypeStyle(this.Type) != TypeScriptTypeStyle.Interface;
    }
}