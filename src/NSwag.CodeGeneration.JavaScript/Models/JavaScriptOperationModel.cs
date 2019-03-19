//-----------------------------------------------------------------------
// <copyright file="TypeScriptOperationModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.Models;

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>The TypeScript operation model.</summary>
    public class JavaScriptOperationModel : OperationModelBase<JavaScriptParameterModel, JavaScriptResponseModel>
    {
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;
        private readonly SwaggerToJavaScriptClientGenerator _generator;
        private readonly SwaggerOperation _operation;

        /// <summary>Initializes a new instance of the <see cref="JavaScriptOperationModel" /> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        public JavaScriptOperationModel(
            SwaggerOperation operation,
            SwaggerToJavaScriptClientGeneratorSettings settings,
            SwaggerToJavaScriptClientGenerator generator,
            TypeResolverBase resolver)
            : base(null, operation, resolver, generator, settings)
        {
            this._operation = operation;
            this._settings = settings;
            this._generator = generator;

            var parameters = this._operation.ActualParameters.ToList();
            if (settings.GenerateOptionalParameters)
            {
                parameters = parameters
                    .OrderBy(p => p.Position ?? 0)
                    .OrderBy(p => !p.IsRequired)
                    .ToList();
            }

            this.Parameters = parameters.Select(parameter =>
                new JavaScriptParameterModel(parameter.Name,
                    this.GetParameterVariableName(parameter, this._operation.Parameters), this.ResolveParameterType(parameter),
                    parameter, parameters, this._settings, this._generator, resolver))
                .ToList();
        }

        /// <summary>Gets the actual name of the operation (language specific).</summary>
        public override string ActualOperationName => ConversionUtilities.ConvertToLowerCamelCase(this.OperationName, false)
            + (this.MethodAccessModifier == "protected " ? "Core" : string.Empty);

        /// <summary>Gets the actual name of the operation (language specific).</summary>
        public string ActualOperationNameUpper => ConversionUtilities.ConvertToUpperCamelCase(this.OperationName, false);

        /// <summary>Gets or sets the type of the result.</summary>
        public override string ResultType
        {
            get
            {
                var response = this.GetSuccessResponse();
                var isNullable = response.Value?.IsNullable(this._settings.CodeGeneratorSettings.SchemaType) == true;

                var resultType = isNullable && this.SupportsStrictNullChecks && this.UnwrappedResultType != "void" && this.UnwrappedResultType != "null" ?
                    this.UnwrappedResultType + " | null" :
                    this.UnwrappedResultType;

                if (this.WrapResponse)
                {
                    return this._settings.ResponseClass.Replace("{controller}", this.ControllerName) + "<" + resultType + ">";
                }
                else
                {
                    return resultType;
                }
            }
        }

        /// <summary>Gets a value indicating whether the operation requires mappings for DTO generation.</summary>
        public bool RequiresMappings => this.Responses.Any(r => r.HasType && r.ActualResponseSchema.UsesComplexObjectSchema());

        /// <summary>Gets a value indicating whether the target TypeScript version supports strict null checks.</summary>
        public bool SupportsStrictNullChecks => this._settings.TypeScriptGeneratorSettings.TypeScriptVersion >= 2.0m;

        /// <summary>Gets a value indicating whether to handle references.</summary>
        public bool HandleReferences => this._settings.TypeScriptGeneratorSettings.HandleReferences;

        /// <summary>Gets a value indicating whether the template can request blobs.</summary>
        public bool CanRequestBlobs => !this.IsJQuery;

        /// <summary>Gets a value indicating whether to use blobs with Angular.</summary>
        public bool RequestAngularBlobs => this.IsAngular && this.IsFile;

        /// <summary>Gets a value indicating whether to use blobs with AngularJS.</summary>
        public bool RequestAngularJSBlobs => this.IsAngularJS && this.IsFile;

        /// <summary>Gets a value indicating whether to render for AngularJS.</summary>
        public bool IsAngularJS => this._settings.Template == JavaScriptTemplate.AngularJS;

        /// <summary>Gets a value indicating whether to render for Angular2.</summary>
        public bool IsAngular => this._settings.Template == JavaScriptTemplate.Angular;

        /// <summary>Gets a value indicating whether to render for JQuery.</summary>
        public bool IsJQuery => this._settings.Template == JavaScriptTemplate.JQueryCallbacks ||
                                this._settings.Template == JavaScriptTemplate.JQueryPromises;

        /// <summary>Gets a value indicating whether to render for Fetch or Aurelia</summary>
        public bool IsFetchOrAurelia => this._settings.Template == JavaScriptTemplate.Fetch ||
                                        this._settings.Template == JavaScriptTemplate.Aurelia;

        /// <summary>Gets a value indicating whether to use HttpClient with the Angular template.</summary>
        public bool UseAngularHttpClient => this.IsAngular && this._settings.HttpClass == HttpClass.HttpClient;

        /// <summary>Gets or sets the type of the exception.</summary>
        public override string ExceptionType
        {
            get
            {
                if (this._operation.ActualResponses.Count(r => !HttpUtilities.IsSuccessStatusCode(r.Key)) == 0)
                    return "string";

                return string.Join(" | ", this._operation.ActualResponses
                    .Where(r => !HttpUtilities.IsSuccessStatusCode(r.Key) && r.Value.Schema != null)
                    .Select(r => this._generator.GetTypeName(r.Value.Schema, r.Value.IsNullable(this._settings.CodeGeneratorSettings.SchemaType), "Exception"))
                    .Concat(new[] { "string" }));
            }
        }

        /// <summary>Gets the method's access modifier.</summary>
        public string MethodAccessModifier
        {
            get
            {
                var controllerName = this._settings.GenerateControllerName(this.ControllerName);
                if (this._settings.ProtectedMethods?.Contains(controllerName + "." + ConversionUtilities.ConvertToLowerCamelCase(this.OperationName, false)) == true)
                    return "protected ";

                return "";
            }
        }

        /// <summary>Gets a value indicating whether to wrap success responses to allow full response access.</summary>
        public bool WrapResponses => this._settings.WrapResponses;

        /// <summary>Gets the response class name.</summary>
        public string ResponseClass => this._settings.ResponseClass.Replace("{controller}", this.ControllerName);

        /// <summary>Resolves the type of the parameter.</summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The parameter type name.</returns>
        protected override string ResolveParameterType(SwaggerParameter parameter)
        {
            var schema = parameter.ActualSchema;
            if (schema.IsBinary)
            {
                if (parameter.CollectionFormat == SwaggerParameterCollectionFormat.Multi && !schema.Type.HasFlag(JsonObjectType.Array))
                    return "FileParameter[]";

                return "FileParameter";
            }

            return base.ResolveParameterType(parameter);
        }

        /// <summary>Creates the response model.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="response">The response.</param>
        /// <param name="exceptionSchema">The exception schema.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        protected override JavaScriptResponseModel CreateResponseModel(SwaggerOperation operation, string statusCode, SwaggerResponse response,
            JsonSchema4 exceptionSchema, IClientGenerator generator, TypeResolverBase resolver, ClientGeneratorBaseSettings settings)
        {
            return new JavaScriptResponseModel(this, operation, statusCode, response, response == this.GetSuccessResponse().Value,
                exceptionSchema, generator, resolver, (SwaggerToJavaScriptClientGeneratorSettings)settings);
        }
    }
}
