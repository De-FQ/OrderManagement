The Antiforgery token is enabled in Admin Controller at Login Action
for each submit form 


1. Admin project is created with Antiforgery token enabled by default 
   inside "_ViewImports.cshtml" file with Tag below. 
   @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

   a. every *.cshtml file which contain <form></from> tag
      a <input type="hidden" name="__RequestVerificationToken" value="any_random_char_number_value"/> 
      will be created automatically

   b. if you post a request to admin Accountcontroller for login method
      you can add [ValidateForgeryToken] attribute above the method, it will 
      validate antiforgery token seemless

   c. you can remove AntiforgeryToken from <from></form> tag, 
      with one attribute setting "asp-antifogery='false'" 
     <form method="post" id="dataForm" name="dataForm" 
        enctype="multipart/form-data" asp-antiforgery="false">
 

2. If you want to disable AntiforgeryToken generation from Admin Project
   you need to remove  "@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers"
   from "_ViewImports.cshtml" file


3. Add AntiforgeryToken in API Project Program.cs file
   add after AddControllers Service, this setup is not working
   "X-XSRF-TOKEN" will be received in each API request, 
    
 builder.Services.AddAntiforgery(
    options => {  
        options.Cookie.Name = "RequestVerificationToken"; 
        options.HeaderName = "X-XSRF-TOKEN";
        options.SuppressXFrameOptionsHeader = true; 
    });

   builder.Services.AddMvc(config => { config.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); });

 
    
  app.UseAuthentication();
  app.Use(next => context =>
  {
    // return current accessed path  
    string path = context.Request.Path.Value;
    if(path.Contains("/api/Common/") || path.Contains("/api/UserRole/GetViewAccessPermission")) 
    { //ignore it
    }else  if (path.IndexOf("/api/", StringComparison.OrdinalIgnoreCase) != -1)
    { 
        var tokens = context.RequestServices.GetRequiredService<IAntiforgery>();
        var token = tokens.GetAndStoreTokens(context);
        
        //add X-XSRF-TOKEN for each API request that is made from admin project
        context.Response.Headers.Append("X-XSRF-TOKEN", token.RequestToken);

     }
     return next(context);
  });


_Layout.cshml file "environment" variable will work if below tag is added in _Imports.cshtml file
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers 
