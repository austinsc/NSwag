//-----------------------------------------------------------------------
// <copyright file="TypeScriptFrameworkRxJsModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

namespace NSwag.CodeGeneration.JavaScript.Models
{
    /// <summary>RxJs specific information.</summary>
    public class JavaScriptFrameworkRxJsModel
    {
        private readonly JavaScriptFrameworkModel _model;

        internal JavaScriptFrameworkRxJsModel(JavaScriptFrameworkModel model)
        {
            this._model = model;
        }

        /// <summary>Gets the RxJs observable mergeMap method name.</summary>
        public string ObservableMergeMapMethod => this._model.UseRxJs5 ? "flatMap" : "_observableMergeMap";

        /// <summary>Gets the RxJs observable catch method name.</summary>
        public string ObservableCatchMethod => this._model.UseRxJs5 ? "catch" : "_observableCatch";

        /// <summary>Gets the RxJs observable of method name.</summary>
        public string ObservableOfMethod => this._model.UseRxJs5 ? "Observable.of" : "_observableOf";

        /// <summary>Gets the RxJs observable from method name.</summary>
        public string ObservableFromMethod => this._model.UseRxJs5 ? "Observable.fromPromise" : "_observableFrom";

        /// <summary>Gets the RxJs observable throw method name.</summary>
        public string ObservableThrowMethod => this._model.UseRxJs5 ? "Observable.throw" : "_observableThrow";
    }
}