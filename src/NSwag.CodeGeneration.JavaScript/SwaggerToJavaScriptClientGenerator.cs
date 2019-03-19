//-----------------------------------------------------------------------
// <copyright file="SwaggerToTypeScriptClientGenerator.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.JavaScript.Models;

namespace NSwag.CodeGeneration.JavaScript
{
    /// <summary>Generates the CSharp service client code. </summary>
    public class SwaggerToJavaScriptClientGenerator : ClientGeneratorBase<JavaScriptOperationModel, JavaScriptParameterModel, JavaScriptResponseModel>
    {
        private readonly SwaggerDocument _document;
        private readonly TypeScriptTypeResolver _resolver;
        private readonly TypeScriptExtensionCode _extensionCode;

        /// <summary>Initializes a new instance of the <see cref="SwaggerToJavaScriptClientGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public SwaggerToJavaScriptClientGenerator(SwaggerDocument document, SwaggerToJavaScriptClientGeneratorSettings settings)
            : this(document, settings, new TypeScriptTypeResolver(settings.TypeScriptGeneratorSettings))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SwaggerToJavaScriptClientGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public SwaggerToJavaScriptClientGenerator(SwaggerDocument document, SwaggerToJavaScriptClientGeneratorSettings settings, TypeScriptTypeResolver resolver)
            : base(document, settings.CodeGeneratorSettings, resolver)
        {
            this.Settings = settings;

            this._document = document ?? throw new ArgumentNullException(nameof(document));
            this._resolver = resolver;
            this._resolver.RegisterSchemaDefinitions(this._document.Definitions);
            this._extensionCode = new TypeScriptExtensionCode(
                this.Settings.TypeScriptGeneratorSettings.ExtensionCode,
                (this.Settings.TypeScriptGeneratorSettings.ExtendedClasses ?? new string[] { }).Concat(new[] { this.Settings.ConfigurationClass }).ToArray(),
                new[] { this.Settings.ClientBaseClass });
        }

        /// <summary>Gets or sets the generator settings.</summary>
        public SwaggerToJavaScriptClientGeneratorSettings Settings { get; set; }

        /// <summary>Generates the file.</summary>
        /// <returns>The file contents.</returns>
        public override string GenerateFile()
        {
            return this.GenerateFile(this._document, ClientGeneratorOutputType.Full);
        }

        /// <summary>Gets the base settings.</summary>
        public override ClientGeneratorBaseSettings BaseSettings => this.Settings;

        /// <summary>Gets the type.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        /// <param name="typeNameHint">The type name hint.</param>
        /// <returns>The type name.</returns>
        public override string GetTypeName(JsonSchema4 schema, bool isNullable, string typeNameHint)
        {
            if (schema == null)
                return "void";

            if (schema.ActualTypeSchema.IsBinary)
                return this.GetBinaryResponseTypeName();

            if (schema.ActualTypeSchema.IsAnyType)
                return "any";

            return this._resolver.Resolve(schema.ActualSchema, isNullable, typeNameHint);
        }

        /// <summary>Gets the file response type name.</summary>
        /// <returns>The type name.</returns>
        public override string GetBinaryResponseTypeName()
        {
            return this.Settings.Template != JavaScriptTemplate.JQueryCallbacks &&
                   this.Settings.Template != JavaScriptTemplate.JQueryPromises ? "FileResponse" : "any";
        }

        /// <summary>Generates the file.</summary>
        /// <param name="clientCode">The client code.</param>
        /// <param name="clientClasses">The client classes.</param>
        /// <param name="outputType">Type of the output.</param>
        /// <returns>The code.</returns>
        protected override string GenerateFile(string clientCode, IEnumerable<string> clientClasses, ClientGeneratorOutputType outputType)
        {
            var model = new JavaScriptFileTemplateModel(clientCode, clientClasses, this._document, this._extensionCode, this.Settings, this._resolver);
            var template = this.BaseSettings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("JavaScript", "File", model);
            return template.Render();
        }

        /// <summary>Generates the client class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller class.</param>
        /// <param name="operations">The operations.</param>
        /// <param name="outputType">Type of the output.</param>
        /// <returns>The code.</returns>
        protected override string GenerateClientClass(string controllerName, string controllerClassName, IList<JavaScriptOperationModel> operations, ClientGeneratorOutputType outputType)
        {
            this.UpdateUseDtoClassAndDataConversionCodeProperties(operations);

            var model = new JavaScriptClientTemplateModel(controllerName, controllerClassName, operations, this._extensionCode, this._document, this.Settings);
            var template = this.Settings.CreateTemplate(model);
            return template.Render();
        }

        /// <summary>Creates an operation model.</summary>
        /// <param name="operation"></param>
        /// <param name="settings">The settings.</param>
        /// <returns>The operation model.</returns>
        protected override JavaScriptOperationModel CreateOperationModel(SwaggerOperation operation, ClientGeneratorBaseSettings settings)
        {
            return new JavaScriptOperationModel(operation, (SwaggerToJavaScriptClientGeneratorSettings)settings, this, this.Resolver);
        }

        private void UpdateUseDtoClassAndDataConversionCodeProperties(IEnumerable<JavaScriptOperationModel> operations)
        {
            // TODO: Remove this method => move to appropriate location

            foreach (var operation in operations)
            {
                foreach (var response in operation.Responses.Where(r => r.HasType))
                {
                    response.DataConversionCode = DataConversionGenerator.RenderConvertToClassCode(new DataConversionParameters
                    {
                        Variable = "result" + response.StatusCode,
                        Value = "resultData" + response.StatusCode,
                        Schema = response.ResolvableResponseSchema,
                        IsPropertyNullable = response.IsNullable,
                        TypeNameHint = string.Empty,
                        Settings = this.Settings.TypeScriptGeneratorSettings,
                        Resolver = this._resolver,
                        NullValue = TypeScriptNullValue.Null
                    });
                }

                if (operation.HasDefaultResponse && operation.DefaultResponse.HasType)
                {
                    operation.DefaultResponse.DataConversionCode = DataConversionGenerator.RenderConvertToClassCode(new DataConversionParameters
                    {
                        Variable = "result" + operation.DefaultResponse.StatusCode,
                        Value = "resultData" + operation.DefaultResponse.StatusCode,
                        Schema = operation.DefaultResponse.ResolvableResponseSchema,
                        IsPropertyNullable = operation.DefaultResponse.IsNullable,
                        TypeNameHint = string.Empty,
                        Settings = this.Settings.TypeScriptGeneratorSettings,
                        Resolver = this._resolver,
                        NullValue = TypeScriptNullValue.Null
                    });
                }
            }
        }
    }
}
