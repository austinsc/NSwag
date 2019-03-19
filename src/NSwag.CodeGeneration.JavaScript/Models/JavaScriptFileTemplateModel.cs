//-----------------------------------------------------------------------
// <copyright file="ClientTemplateModel.cs" company="NSwag">
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
    /// <summary>The TypeScript file template model.</summary>
    public class JavaScriptFileTemplateModel
    {
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;
        private readonly TypeScriptTypeResolver _resolver;
        private readonly string _clientCode;
        private readonly SwaggerDocument _document;
        private readonly TypeScriptExtensionCode _extensionCode;

        /// <summary>Initializes a new instance of the <see cref="JavaScriptFileTemplateModel" /> class.</summary>
        /// <param name="clientCode">The client code.</param>
        /// <param name="clientClasses">The client classes.</param>
        /// <param name="document">The Swagger document.</param>
        /// <param name="extensionCode">The extension code.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        public JavaScriptFileTemplateModel(
            string clientCode,
            IEnumerable<string> clientClasses,
            SwaggerDocument document,
            TypeScriptExtensionCode extensionCode,
            SwaggerToJavaScriptClientGeneratorSettings settings,
            TypeScriptTypeResolver resolver)
        {
            this._document = document;
            this._extensionCode = extensionCode;
            this._settings = settings;
            this._resolver = resolver;
            this._clientCode = clientCode;
            this.ClientClasses = clientClasses.ToArray();

            this.Types = this.GenerateDtoTypes();
            this.ExtensionCodeBottom = this.GenerateExtensionCodeAfter();
            this.Framework = new JavaScriptFrameworkModel(settings);
        }

        /// <summary>Gets framework specific information.</summary>
        public JavaScriptFrameworkModel Framework { get; set; }

        /// <summary>Gets a value indicating whether to generate client classes.</summary>
        public bool GenerateClientClasses => this._settings.GenerateClientClasses;

        /// <summary>Gets or sets a value indicating whether DTO exceptions are wrapped in a SwaggerException instance.</summary>
        public bool WrapDtoExceptions => this._settings.WrapDtoExceptions;

        /// <summary>Gets or sets a value indicating whether to wrap success responses to allow full response access.</summary>
        public bool WrapResponses => this._settings.WrapResponses;

        /// <summary>Gets or sets a value indicating whether to generate the response class (only applied when WrapResponses == true, default: true).</summary>
        public bool GenerateResponseClasses => this._settings.GenerateResponseClasses;

        /// <summary>Gets the response class names.</summary>
        public IEnumerable<string> ResponseClassNames
        {
            get
            {
                // TODO: Merge with ResponseClassNames of C#
                if (this._settings.OperationNameGenerator.SupportsMultipleClients)
                {
                    return this._document.Operations
                        .GroupBy(o => this._settings.OperationNameGenerator.GetClientName(this._document, o.Path, o.Method, o.Operation))
                        .Select(g => this._settings.ResponseClass.Replace("{controller}", g.Key))
                        .Where(a => this._settings.TypeScriptGeneratorSettings.ExcludedTypeNames?.Contains(a) != true)
                        .Distinct();
                }

                return new[] { this._settings.ResponseClass.Replace("{controller}", string.Empty) };
            }
        }

        /// <summary>Gets a value indicating whether required types should be imported.</summary>
        public bool ImportRequiredTypes => this._settings.ImportRequiredTypes;

        /// <summary>Gets a value indicating whether to call 'transformOptions' on the base class or extension class.</summary>
        public bool UseTransformOptionsMethod => this._settings.UseTransformOptionsMethod;

        /// <summary>Gets the clients code.</summary>
        public string Clients => this._settings.GenerateClientClasses ? this._clientCode : string.Empty;

        /// <summary>Gets the types code.</summary>
        public string Types { get; }

        /// <summary>Gets or sets the extension code imports.</summary>
        public string ExtensionCodeImport => this._extensionCode.ImportCode;

        /// <summary>Gets or sets the extension code to insert at the beginning.</summary>
        public string ExtensionCodeTop => this._settings.ConfigurationClass != null && this._extensionCode.ExtensionClasses.ContainsKey(this._settings.ConfigurationClass) ?
            this._extensionCode.ExtensionClasses[this._settings.ConfigurationClass] + "\n\n" + this._extensionCode.TopCode :
            this._extensionCode.TopCode;

        /// <summary>Gets or sets the extension code to insert at the end.</summary>
        public string ExtensionCodeBottom { get; }

        /// <summary>Gets a value indicating whether the file has module name.</summary>
        public bool HasModuleName => !string.IsNullOrEmpty(this._settings.TypeScriptGeneratorSettings.ModuleName);

        /// <summary>Gets the name of the module.</summary>
        public string ModuleName => this._settings.TypeScriptGeneratorSettings.ModuleName;

        /// <summary>Gets a value indicating whether the file has a namespace.</summary>
        public bool HasNamespace => !string.IsNullOrEmpty(this._settings.TypeScriptGeneratorSettings.Namespace);

        /// <summary>Gets the namespace.</summary>
        public string Namespace => this._settings.TypeScriptGeneratorSettings.Namespace;

        /// <summary>Gets whether the export keyword should be added to all classes and enums.</summary>
        public bool ExportTypes => this._settings.TypeScriptGeneratorSettings.ExportTypes;

        /// <summary>Gets a value indicating whether the FileParameter interface should be rendered.</summary>
        public bool RequiresFileParameterInterface =>
            !this._settings.TypeScriptGeneratorSettings.ExcludedTypeNames.Contains("FileParameter") &&
            this._document.Operations.Any(o => o.Operation.ActualParameters.Any(p => p.ActualTypeSchema.IsBinary));

        /// <summary>Gets a value indicating whether the FileResponse interface should be rendered.</summary>
        public bool RequiresFileResponseInterface =>
            !this.Framework.IsJQuery &&
            !this._settings.TypeScriptGeneratorSettings.ExcludedTypeNames.Contains("FileResponse") &&
            this._document.Operations.Any(o => o.Operation.ActualResponses.Any(r => r.Value.IsBinary(o.Operation)));

        /// <summary>Gets a value indicating whether the client functions are required.</summary>
        public bool RequiresClientFunctions =>
            this._settings.GenerateClientClasses &&
            !string.IsNullOrEmpty(this.Clients);

        /// <summary>Gets a value indicating whether the SwaggerException class is required. Note that if RequiresClientFunctions returns true this returns true since the client functions require it. </summary>
        public bool RequiresSwaggerExceptionClass =>
            this.RequiresClientFunctions &&
            !this._settings.TypeScriptGeneratorSettings.ExcludedTypeNames.Contains("SwaggerException");

        /// <summary>Table containing list of the generated classes.</summary>
        public string[] ClientClasses { get; }

        /// <summary>Gets a value indicating whether to handle references.</summary>
        public bool HandleReferences => this._settings.TypeScriptGeneratorSettings.HandleReferences;

        /// <summary>Gets a value indicating whether MomentJS duration format is needed (moment-duration-format package).</summary>
        public bool RequiresMomentJSDuration => this.Types?.Contains("moment.duration(") == true;

        private string GenerateDtoTypes()
        {
            var generator = new TypeScriptGenerator(this._document, this._settings.TypeScriptGeneratorSettings, this._resolver);
            return this._settings.GenerateDtoTypes ? generator.GenerateTypes(this._extensionCode).Concatenate() : string.Empty;
        }

        private string GenerateExtensionCodeAfter()
        {
            var clientClassesVariable = "{" + string.Join(", ", this.ClientClasses.Select(c => "'" + c + "': " + c)) + "}";
            return this._extensionCode.BottomCode.Replace("{clientClasses}", clientClassesVariable);
        }
    }
}