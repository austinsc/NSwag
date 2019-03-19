//-----------------------------------------------------------------------
// <copyright file="TypeScriptAsyncType.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

namespace NSwag.CodeGeneration.JavaScript
{
    /// <summary>The TypeScript output templates.</summary>
    public enum JavaScriptTemplate
    {
        /// <summary>Uses JQuery with callbacks.</summary>
        JQueryCallbacks,

        /// <summary>Uses JQuery with Promises/A+.</summary>
        JQueryPromises,

        /// <summary>Uses $http from AngularJS 1.x.</summary>
        AngularJS,

        /// <summary>Uses the http service from AngularJS 2.x.</summary>
        Angular,

        /// <summary>Uses the window.fetch API.</summary>
        Fetch,

        /// <summary>Uses the Aurelia fetch service.</summary>
        Aurelia,

        /// <summary>Uses the Axios service.</summary>
        Axios,
    }
}