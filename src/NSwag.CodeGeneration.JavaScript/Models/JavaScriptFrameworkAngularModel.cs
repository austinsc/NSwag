//-----------------------------------------------------------------------
// <copyright file="TypeScriptFrameworkAngularModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>Angular specific information.</summary>
    public class JavaScriptFrameworkAngularModel
    {
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;

        internal JavaScriptFrameworkAngularModel(SwaggerToJavaScriptClientGeneratorSettings settings)
        {
            this._settings = settings;
        }

        /// <summary>Gets or sets the injection token type (used in the Angular template).</summary>
        public string InjectionTokenType => this._settings.InjectionTokenType.ToString();

        /// <summary>Gets or sets the token name for injecting the API base URL string (used in the Angular template).</summary>
        public string BaseUrlTokenName => this._settings.BaseUrlTokenName;

        /// <summary>Gets the HTTP client class name.</summary>
        public string HttpClass => this.UseHttpClient ? "HttpClient" : "Http";

        /// <summary>Gets a value indicating whether to use HttpClient with the Angular template.</summary>
        public bool UseHttpClient => this._settings.Template == JavaScriptTemplate.Angular && 
                                     this._settings.HttpClass == JavaScript.HttpClass.HttpClient;

        /// <summary>Gets a value indicating whether to use the Angular 6 Singleton Provider (Angular template only, default: false).</summary>
        public bool UseSingletonProvider => this._settings.Template == JavaScriptTemplate.Angular && 
                                            this._settings.UseSingletonProvider;

        /// <summary>Gets whether the export keyword should be added to all classes and enums.</summary>
        public bool ExportTypes => this._settings.TypeScriptGeneratorSettings.ExportTypes;
    }
}