Can't add explicit references to a shared project.

Dependencies:

**Nuget Packages**:

        <?xml version="1.0" encoding="utf-8"?>
        <packages>
        <package id="Newtonsoft.Json" version="7.0.1" targetFramework="monoandroid60" />
        <package id="NLog" version="4.4.2-rc1" targetFramework="monoandroid60" />
        <package id="NLog.Schema" version="4.4.2-rc1" targetFramework="monoandroid60" />
        <package id="sqlite-net-pcl" version="1.0.11" targetFramework="monoandroid60" />
        <package id="SQLitePCL.raw_basic" version="0.7.1" targetFramework="monoandroid60" />
        <package id="Square.OkHttp3" version="3.2.0.0" targetFramework="monoandroid60" />
        <package id="Square.OkIO" version="1.6.0.0" targetFramework="monoandroid60" />
        <package id="Xamarin.Android.Support.Animated.Vector.Drawable" version="23.4.0.1" targetFramework="monoandroid60" />
        <package id="Xamarin.Android.Support.Design" version="23.4.0.1" targetFramework="monoandroid60" />
        <package id="Xamarin.Android.Support.v4" version="23.4.0.1" targetFramework="monoandroid60" />
        <package id="Xamarin.Android.Support.v7.AppCompat" version="23.4.0.1" targetFramework="monoandroid60" />
        <package id="Xamarin.Android.Support.v7.RecyclerView" version="23.4.0.1" targetFramework="monoandroid60" />
        <package id="Xamarin.Android.Support.Vector.Drawable" version="23.4.0.1" targetFramework="monoandroid60" />
        </packages>

**Xamarin Components**:

        <ItemGroup>
            <XamarinComponentReference Include="json.net">
            <Visible>False</Visible>
            <Version>7.0.1</Version>
            </XamarinComponentReference>
            <XamarinComponentReference Include="rxforxamarin">
            <Visible>False</Visible>
            <Version>2.2.1</Version>
            </XamarinComponentReference>
            <XamarinComponentReference Include="sqlite-net">
            <Visible>False</Visible>
            <Version>1.0.11</Version>
            </XamarinComponentReference>
            <XamarinComponentReference Include="square.okhttp3">
            <Visible>False</Visible>
            <Version>3.2.0.0</Version>
            </XamarinComponentReference>
        </ItemGroup>