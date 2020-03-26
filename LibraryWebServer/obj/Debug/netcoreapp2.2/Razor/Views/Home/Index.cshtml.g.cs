#pragma checksum "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f24edce76c48d640fe4cf8a80bd465bb6ba1b15c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 1 "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\_ViewImports.cshtml"
using LibraryWebServer;

#line default
#line hidden
#line 2 "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\_ViewImports.cshtml"
using LibraryWebServer.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f24edce76c48d640fe4cf8a80bd465bb6ba1b15c", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88e09ce4f4eee0301695f0685d4d51ac3aed3bbb", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\Home\Index.cshtml"
  
  ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(43, 639, true);
            WriteLiteral(@"
<script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js""></script>




<div class=""col-md-12"">
  <table id=""tblBooks"" class=""table table-striped"">
    <thead>
      <tr>
        <th align=""left"" class=""productth"">ISBN</th>
        <th align=""left"" class=""productth"">Title</th>
        <th align=""left"" class=""productth"">Author</th>
        <th align=""left"" class=""productth"">Serial</th>
        <th align=""left"" class=""productth"">Holder</th>
      </tr>
    </thead>
  </table>
</div>

<a href=""/Home/MyBooks"">Go to my books</a>
<br />
<br />

<button onClick='LogOut()'>Logout</button>


");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(703, 1629, true);
                WriteLiteral(@"
  <script type=""text/javascript"">



    function PopulateTable(tbl, books) {
      var newBody = document.createElement(""tbody"");
      

      $.each(books, function (i, item) {
        var tr = document.createElement(""tr"");

        var td = document.createElement(""td"");
        td.appendChild(document.createTextNode(item.isbn));
        tr.appendChild(td);
        
        var td = document.createElement(""td"");
        td.appendChild(document.createTextNode(item.title));
        tr.appendChild(td);

        var td = document.createElement(""td"");
        td.appendChild(document.createTextNode(item.author));
        tr.appendChild(td);

        var td = document.createElement(""td"");
        if (item.serial) {
          td.appendChild(document.createTextNode(item.serial));
        }
        else {
          td.appendChild(document.createTextNode(""no copies""));
        }
        tr.appendChild(td);

        
        var td = document.createElement(""td"");
        if (item.se");
                WriteLiteral(@"rial && item.name == """") {
          var button = document.createElement(""button"");
          button.appendChild(document.createTextNode(""checkout""));
          button.setAttribute(""onClick"", ""javascript:CheckOut('"" + item.serial + ""')"");
          td.appendChild(button);          
        }
        else {
          td.appendChild(document.createTextNode(item.name));
        }

        tr.appendChild(td);
        

        newBody.appendChild(tr);
      });

      tbl.appendChild(newBody);

    }


    function LogOut() {

      $.ajax({
        type: 'POST',
        url: '");
                EndContext();
                BeginContext(2333, 28, false);
#line 92 "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\Home\Index.cshtml"
         Write(Url.Action("LogOut", "Home"));

#line default
#line hidden
                EndContext();
                BeginContext(2361, 521, true);
                WriteLiteral(@"',
        dataType: 'json',
        success: function () {
          location.reload();
        },
        error: function (ex) {
          var r = jQuery.parseJSON(response.responseText);
          alert(""Message: "" + r.Message);
          alert(""StackTrace: "" + r.StackTrace);
          alert(""ExceptionType: "" + r.ExceptionType);
          location.reload();
        }
      });
      return false;
    }
    
    function CheckOut(serialInput) {
      $.ajax({
        type: 'POST',
        url: '");
                EndContext();
                BeginContext(2883, 34, false);
#line 111 "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\Home\Index.cshtml"
         Write(Url.Action("CheckOutBook", "Home"));

#line default
#line hidden
                EndContext();
                BeginContext(2917, 685, true);
                WriteLiteral(@"',
        dataType: 'json',
        data: { serial: serialInput },
        success: function (data, status) {
          location.reload();
        },
        error: function (ex) {
          var r = jQuery.parseJSON(response.responseText);
          alert(""Message: "" + r.Message);
          alert(""StackTrace: "" + r.StackTrace);
          alert(""ExceptionType: "" + r.ExceptionType);
          location.reload();
        }
      });
      return false;
    }
    
    
    $(function () {
      LoadData();
    });
    
    function LoadData() {
      var tbl = document.getElementById(""tblBooks"");
      
      $.ajax({
        type: 'POST',
        url: '");
                EndContext();
                BeginContext(3603, 31, false);
#line 138 "C:\Users\Andrew\Documents\Skool\5530 - Database Systems\Homework 6\LibraryWebServerHandout\LibraryWebServer\Views\Home\Index.cshtml"
         Write(Url.Action("AllTitles", "Home"));

#line default
#line hidden
                EndContext();
                BeginContext(3634, 471, true);
                WriteLiteral(@"',
        dataType: 'json',
        success: function (data, status) {
          //alert(JSON.stringify(data));
          PopulateTable(tbl, data);
        },
        error: function (ex) {
          var r = jQuery.parseJSON(response.responseText);
          alert(""Message: "" + r.Message);
          alert(""StackTrace: "" + r.StackTrace);
          alert(""ExceptionType: "" + r.ExceptionType);
        }
        });
      return false;
    } 
  </script>
");
                EndContext();
            }
            );
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
