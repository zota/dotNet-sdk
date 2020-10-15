# Zotapay dotNET SDK
.NET SDK for Zotapay API

HTML Test Coverage Report Generation <br/>

```Shell 
dotnet add package coverlet.collector
dotnet test --collect:"XPlat Code Coverage"
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.cobertura.xml -targetdir:coverlet/reports -reporttypes:HtmlInline_AzurePipelines
```

