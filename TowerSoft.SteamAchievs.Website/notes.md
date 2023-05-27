
#### Running Gulp Before Publishing Automatically
* Edit the .pubxml for the publish profile
* Add the following `<target>` element inside the of `<Project>` element like this:

```xml
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        The publish settings will be list here already
    </PropertyGroup>
    <Target Name="GulpDefaultTask" BeforeTargets="BeforeBuild">
      <Exec Command="call node_modules\.bin\gulp" WorkingDirectory="$(ProjectDir)" />
    </Target>
</Project>
```

* Gulp will now run the default task automatically before the project is built