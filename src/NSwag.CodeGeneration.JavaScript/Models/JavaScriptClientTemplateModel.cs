//-----------------------------------------------------------------------
// <copyright file="TypeScriptClientTemplateModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration.TypeScript;

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>The TypeScript client template model.</summary>
    public class JavaScriptClientTemplateModel
    {
        private readonly TypeScriptExtensionCode _extensionCode;
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;
        private readonly SwaggerDocument _document;

        /// <summary>Initializes a new instance of the <see cref="JavaScriptClientTemplateModel" /> class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller.</param>
        /// <param name="operations">The operations.</param>
        /// <param name="extensionCode">The extension code.</param>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        public JavaScriptClientTemplateModel(
            string controllerName,
            string controllerClassName,
            IEnumerable<JavaScriptOperationModel> operations,
            TypeScriptExtensionCode extensionCode,
            SwaggerDocument document,
            SwaggerToJavaScriptClientGeneratorSettings settings)
        {
            this._extensionCode = extensionCode;
            this._settings = settings;
            this._document = document;

            this.Class = controllerClassName;
            this.Operations = operations;

            this.BaseClass = this._settings.ClientBaseClass?.Replace("{controller}", controllerName);
            this.Framework = new JavaScriptFrameworkModel(settings);
        }

        /// <summary>Gets framework specific information.</summary>
        public JavaScriptFrameworkModel Framework { get; set; }

        /// <summary>Gets the class name.</summary>
        public string Class { get; }

        /// <summary>Gets a value indicating whether the client class has a base class.</summary>
        public bool HasBaseClass => !string.IsNullOrEmpty(this.BaseClass);

        /// <summary>Gets the client base class.</summary>
        public string BaseClass { get; }

        /// <summary>Gets or sets a value indicating whether to use the getBaseUrl(defaultUrl: string) from the base class.</summary>
        public bool UseGetBaseUrlMethod => this._settings.UseGetBaseUrlMethod;

        /// <summary>Gets the configuration class name.</summary>
        public string ConfigurationClass => this._settings.ConfigurationClass;

        /// <summary>Gets a value indicating whether the client class has a base class.</summary>
        public bool HasConfigurationClass => this.HasBaseClass && !string.IsNullOrEmpty(this.ConfigurationClass);

        /// <summary>Gets or sets a value indicating whether to call 'transformOptions' on the base class or extension class.</summary>
        public bool UseTransformOptionsMethod => this._settings.UseTransformOptionsMethod;

        /// <summary>Gets or sets a value indicating whether to call 'transformResult' on the base class or extension class.</summary>
        public bool UseTransformResultMethod => this._settings.UseTransformResultMethod;

        /// <summary>Gets a value indicating whether to generate optional parameters.</summary>
        public bool GenerateOptionalParameters => this._settings.GenerateOptionalParameters;

        /// <summary>Gets a value indicating whether the client is extended with an extension class.</summary>
        public bool HasExtensionCode => this._settings.TypeScriptGeneratorSettings.ExtendedClasses?.Any(c => c == this.Class) == true;

        /// <summary>Gets the extension body code.</summary>
        public string ExtensionCode => this._extensionCode.GetExtensionClassBody(this.Class);

        /// <summary>Gets or sets a value indicating whether the extension code has a constructor and no constructor has to be generated.</summary>
        public bool HasExtendedConstructor => this.HasExtensionCode && this.ExtensionCode.Contains("constructor(");

        /// <summary>Gets a value indicating whether the client has operations.</summary>
        public bool HasOperations => this.Operations.Any();

        /// <summary>Gets the operations.</summary>
        public IEnumerable<JavaScriptOperationModel> Operations { get; }

        /// <summary>Gets the service base URL.</summary>
        public string BaseUrl => this._document.BaseUrl;

        /// <summary>Gets a value indicating whether to generate client interfaces.</summary>
        public bool GenerateClientInterfaces => this._settings.GenerateClientInterfaces;

        /// <summary>Gets the promise type.</summary>
        public string PromiseType => this._settings.PromiseType == JavaScript.PromiseType.Promise ? "Promise" : "Q.Promise";

        /// <summary>Gets the promise constructor code.</summary>
        public string PromiseConstructor => this._settings.PromiseType == JavaScript.PromiseType.Promise ? "new Promise" : "Q.Promise";

        /// <summary>Gets or sets a value indicating whether to use Aurelia HTTP injection.</summary>
        public bool UseAureliaHttpInjection => this._settings.Template == JavaScriptTemplate.Aurelia;

        /// <summary>Gets a value indicating whether the target TypeScript version supports strict null checks.</summary>
        public bool SupportsStrictNullChecks => this._settings.TypeScriptGeneratorSettings.TypeScriptVersion >= 2.0m;

        /// <summary>Gets or sets a value indicating whether DTO exceptions are wrapped in a SwaggerException instance.</summary>
        public bool WrapDtoExceptions => this._settings.WrapDtoExceptions;

        /// <summary>Gets or sets the null value used for query parameters which are null.</summary>
        public string QueryNullValue => this._settings.QueryNullValue;

        /// <summary>Gets whether the export keyword should be added to all classes and enums.</summary>
        public bool ExportTypes => this._settings.TypeScriptGeneratorSettings.ExportTypes;
    }
}