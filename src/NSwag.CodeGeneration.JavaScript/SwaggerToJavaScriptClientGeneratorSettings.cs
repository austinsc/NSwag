//-----------------------------------------------------------------------
// <copyright file="SwaggerToTypeScriptClientGeneratorSettings.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Reflection;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;

namespace NSwag.CodeGeneration.JavaScript
{
    /// <summary>Settings for the <see cref="SwaggerToJavaScriptClientGenerator"/>.</summary>
    public class SwaggerToJavaScriptClientGeneratorSettings : ClientGeneratorBaseSettings
    {
        /// <summary>Initializes a new instance of the <see cref="SwaggerToJavaScriptClientGeneratorSettings"/> class.</summary>
        public SwaggerToJavaScriptClientGeneratorSettings()
        {
            this.ClassName = "{controller}Client";
            this.Template = JavaScriptTemplate.Fetch;
            this.PromiseType = PromiseType.Promise;
            this.BaseUrlTokenName = "API_BASE_URL";
            this.ImportRequiredTypes = true;
            this.QueryNullValue = "";

            this.TypeScriptGeneratorSettings = new TypeScriptGeneratorSettings
            {
                SchemaType = SchemaType.Swagger2,
                MarkOptionalProperties = true,
                TypeNameGenerator = new JavaScriptTypeNameGenerator(),
                TypeScriptVersion = 2.7m
            };

            this.TypeScriptGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(this.TypeScriptGeneratorSettings, new Assembly[]
            {
                typeof(TypeScriptGeneratorSettings).GetTypeInfo().Assembly,
                typeof(SwaggerToJavaScriptClientGeneratorSettings).GetTypeInfo().Assembly,
            });

            this.ProtectedMethods = new string[0];
        }

        /// <summary>Gets the TypeScript generator settings.</summary>
        public TypeScriptGeneratorSettings TypeScriptGeneratorSettings { get; }

        /// <summary>Gets the code generator settings.</summary>
        [JsonIgnore]
        public override CodeGeneratorSettingsBase CodeGeneratorSettings => this.TypeScriptGeneratorSettings;

        /// <summary>Gets or sets the output template.</summary>
        public JavaScriptTemplate Template { get; set; }

        /// <summary>Gets or sets the promise type.</summary>
        public PromiseType PromiseType { get; set; }

        /// <summary>Gets or sets a value indicating whether DTO exceptions are wrapped in a SwaggerException instance (default: false).</summary>
        public bool WrapDtoExceptions { get; set; }

        /// <summary>Gets or sets the client base class.</summary>
        public string ClientBaseClass { get; set; }

        /// <summary>Gets or sets the full name of the configuration class (<see cref="ClientBaseClass"/> must be set).</summary>
        public string ConfigurationClass { get; set; }

        /// <summary>Gets or sets a value indicating whether to call 'transformOptions' on the base class or extension class.</summary>
        public bool UseTransformOptionsMethod { get; set; }

        /// <summary>Gets or sets a value indicating whether to call 'transformResult' on the base class or extension class.</summary>
        public bool UseTransformResultMethod { get; set; }

        /// <summary>Gets or sets the token name for injecting the API base URL string (used in the Angular2 template, default: '').</summary>
        public string BaseUrlTokenName { get; set; }

        /// <summary>Gets or sets the list of methods with a protected access modifier ("classname.methodname").</summary>
        public string[] ProtectedMethods { get; set; }

        /// <summary>Gets or sets a value indicating whether required types should be imported (default: true).</summary>
        public bool ImportRequiredTypes { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the 'getBaseUrl(defaultUrl: string)' from the base class (default: false).</summary>
        public bool UseGetBaseUrlMethod { get; set; }

        /// <summary>Gets or sets the null value used for query parameters which are null (default: '').</summary>
        public string QueryNullValue { get; set; }

        // TODO: Angular specific => move

        /// <summary>Gets or sets the HTTP service class (applies only for the Angular template, default: HttpClient).</summary>
        public HttpClass HttpClass { get; set; } = HttpClass.HttpClient;

        /// <summary>Gets the RxJs version (Angular template only, default: 6.0).</summary>
        public decimal RxJsVersion { get; set; } = 6.0m;

        /// <summary>Gets a value indicating whether to use the Angular 6 Singleton Provider (Angular template only, default: false).</summary>
        public bool UseSingletonProvider { get; set; } = false;

        /// <summary>Gets or sets the injection token type (applies only for the Angular template).</summary>
        public InjectionTokenType InjectionTokenType { get; set; } = InjectionTokenType.OpaqueToken;

        internal ITemplate CreateTemplate(object model)
        {
            if (this.Template == JavaScriptTemplate.Aurelia)
                return this.CodeGeneratorSettings.TemplateFactory.CreateTemplate("TypeScript", JavaScriptTemplate.Fetch + "Client", model);

            return this.CodeGeneratorSettings.TemplateFactory.CreateTemplate("TypeScript", this.Template + "Client", model);
        }
    }
}