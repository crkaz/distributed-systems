#pragma checksum "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\Signin\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b8ff390cfc436dc38b093a0214331ebad07069a7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Signin_Index), @"mvc.1.0.view", @"/Views/Signin/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Signin/Index.cshtml", typeof(AspNetCore.Views_Signin_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\_ViewImports.cshtml"
using OAuthLab;

#line default
#line hidden
#line 2 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\_ViewImports.cshtml"
using OAuthLab.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b8ff390cfc436dc38b093a0214331ebad07069a7", @"/Views/Signin/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d608e047ebb13d93af98885862668199af97df04", @"/Views/_ViewImports.cshtml")]
    public class Views_Signin_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\Signin\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
            BeginContext(43, 27, true);
            WriteLiteral("\r\n<h1>Index</h1>\r\n\r\n<div>\r\n");
            EndContext();
#line 9 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\Signin\Index.cshtml"
     if (User.Identity.IsAuthenticated)
    {

#line default
#line hidden
            BeginContext(118, 20, true);
            WriteLiteral("        <h4>Welcome ");
            EndContext();
            BeginContext(139, 18, false);
#line 11 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\Signin\Index.cshtml"
               Write(User.Identity.Name);

#line default
#line hidden
            EndContext();
            BeginContext(157, 7, true);
            WriteLiteral("</h4>\r\n");
            EndContext();
#line 12 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\Signin\Index.cshtml"
    }
    else
    {

#line default
#line hidden
            BeginContext(188, 41, true);
            WriteLiteral("        <h4>You are not logged in.</h4>\r\n");
            EndContext();
#line 16 "D:\repos\Distributed Systems\Lab 6\Ex2 OAuth\OAuthLab\Views\Signin\Index.cshtml"
    }

#line default
#line hidden
            BeginContext(236, 6, true);
            WriteLiteral("</div>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591