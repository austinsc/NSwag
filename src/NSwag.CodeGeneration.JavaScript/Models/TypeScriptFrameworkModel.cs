//-----------------------------------------------------------------------
// <copyright file="TypeScriptFrameworkModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NJsonSchema.CodeGeneration.TypeScript;

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>Framework specific information.</summary>
    public class TypeScriptFrameworkModel
    {
        private readonly SwaggerToJavaScriptClientGeneratorSettings _settings;

        internal TypeScriptFrameworkModel(SwaggerToJavaScriptClientGeneratorSettings settings)
        {
            this._settings = settings;

            this.RxJs = new TypeScriptFrameworkRxJsModel(this);
            this.Angular = new TypeScriptFrameworkAngularModel(settings);
        }

        /// <summary>Gets a value indicating whether the generated code is for Angular 2.</summary>
        public bool IsAngular => this._settings.Template == JavaScriptTemplate.Angular;

        /// <summary>Gets a value indicating whether the generated code is for Aurelia.</summary>
        public bool IsAurelia => this._settings.Template == JavaScriptTemplate.Aurelia;

        /// <summary>Gets a value indicating whether the generated code is for Angular.</summary>
        public bool IsAngularJS => this._settings.Template == JavaScriptTemplate.AngularJS;

        /// <summary>Gets a value indicating whether the generated code is for Knockout.</summary>
        public bool IsKnockout => this._settings.TypeScriptGeneratorSettings.TypeStyle == TypeScriptTypeStyle.KnockoutClass;

        /// <summary>Gets a value indicating whether to render for JQuery.</summary>
        public bool IsJQuery => this._settings.Template == JavaScriptTemplate.JQueryCallbacks || this._settings.Template == JavaScriptTemplate.JQueryPromises;

        /// <summary>Gets a value indicating whether to render for Fetch or Aurelia</summary>
        public bool IsFetchOrAurelia => this._settings.Template == JavaScriptTemplate.Fetch ||
                                        this._settings.Template == JavaScriptTemplate.Aurelia;

        /// <summary>Gets a value indicating whether to render for Axios.</summary>
        public bool IsAxios => this._settings.Template == JavaScriptTemplate.Axios;

        /// <summary>Gets a value indicating whether MomentJS is required.</summary>
        public bool UseMomentJS => this._settings.TypeScriptGeneratorSettings.DateTimeType == TypeScriptDateTimeType.MomentJS ||
                                   this._settings.TypeScriptGeneratorSettings.DateTimeType == TypeScriptDateTimeType.OffsetMomentJS;

        /// <summary>Gets a value indicating whether to use RxJs 5.</summary>
        public bool UseRxJs5 => this._settings.RxJsVersion < 6.0m;

        /// <summary>Gets a value indicating whether to use RxJs 6.</summary>
        public bool UseRxJs6 => this._settings.RxJsVersion >= 6.0m;

        /// <summary>Gets Rxjs information.</summary>
        public TypeScriptFrameworkRxJsModel RxJs { get; }

        /// <summary>Gets Angular information.</summary>
        public TypeScriptFrameworkAngularModel Angular { get; set; }
    }
}
