[![Build status](https://ci.appveyor.com/api/projects/status/iclidwunix8aulj4?svg=true)](https://ci.appveyor.com/project/AngelaViVi/expressior-addin)

![Image](https://github.com/AngelaViVi/Expressior/blob/master/src/DynamoCoreWpf/UI/Images/StartPage/dynamo-logo.png)
===================================
# Addin API 
### A. zero touch
1. 需要引用的包:<br>

|id|version|targetFramework|
|:-----------:|--------------------|----------------|
|CommonServiceLocator| 1.0|net45|
|DynamoVisualProgramming.Core| 2.0.0|net45|
|DynamoVisualProgramming.DynamoServices| 2.0.0|net45|
|DynamoVisualProgramming.ZeroTouchLibrary| 2.0.0|net45|
|NUnit|2.6.3|net45|
|Prism|4.1.0.0|net45|

2. 使用方法:生成dll,生成xml,启动Expressior,文件,导入库,选择对应的dll.<br>
3. 层次说明:构建的dll在被加载到程序中后,会在控件箱中形成若干层目录和一些节点.
   其中工程名是一层目录,命名空间是一层目录,类名是最后一层目录,类中的可见元素是节点.<br>
4. 可见性:<br>
   4.1. publiuc的Properties和函数会显示成节点,函数的参数和返回值分别对应节点的输入和输出端口;<br>
   4.2. static的函数的输入端口和函数的参数数量严格对应.(调用静态方法不需要类的实例);<br>
   4.3. 非static的函数多一个输入端口,类型为函数所在的类.(非静态方法必须通过实例调用);<br>
   4.3. 类中必须包含一个static的Create方法作为构造器.<br>
5. Attribute:<br>
   5.1. 多重返回值<br>

   ```c#
        [MultiReturn(new[] { "thing 1", "thing 2" })]
        public static Dictionary<string, List<string>> MultiReturnExample()
        {
            return new Dictionary<string, List<string>>()
            {
                { "thing 1", new List<string>{"apple", "banana", "cat"} },
                { "thing 2", new List<string>{"Tywin", "Cersei", "Hodor"} }
            };
        }
   ```

   5.2. 可见性<br>
    可以对方法,属性,类进行标注.例如某个变量需要是public的,但是我们不希望在界面上看到他,就可以这样标记:<br>

    ```C#
        [IsVisibleInDynamoLibrary(false)]
    ```
   5.3. 不导入VM<br>
    不导入VM的类无法被引用,有些类,例如测试类,和具体功能无关,也没有其他业务逻辑依赖于该类,就可以这样标记:
   ```C#
       [SupressImportIntoVM]
   ```

6. 绘图API:IGraphicItem接口:<br>
   ```C#
    public interface IGraphicItem
    {
        void Tessellate(IRenderPackage package, TessellationParameters parameters);
    }
   ```

   实现这个接口的类可以被解释成3D对象,从而在Watch 3D节点中绘图.接口方法中的IRenderPackage参数可以用来填充图元.
   ```C#
   [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            //实现IGraphicItem接口中的方法,该方法用于几何体绘制.
            //形参中的IRenderPackage对象可以用来填充渲染数据,

            package.RequiresPerVertexColoration = true;//逐节点着色模式

            AddColoredQuadToPackage(package);//画两个三角形
            AddColoredLineToPackage(package);//划线
        }

        private static void AddColoredQuadToPackage(IRenderPackage package)
        {
            // Triangle 1
            package.AddTriangleVertex(5, 0, 0);
            package.AddTriangleVertex(1, 0, 0);
            package.AddTriangleVertex(1, 1, 0);

            // For each vertex, add a color.
            package.AddTriangleVertexColor(255, 0, 0, 255);
            package.AddTriangleVertexColor(0, 255, 0, 255);
            package.AddTriangleVertexColor(0, 0, 255, 255);

            //Triangle 2
            package.AddTriangleVertex(0, 0, 0);
            package.AddTriangleVertex(1, 1, 0);
            package.AddTriangleVertex(0, 1, 0);
            package.AddTriangleVertexColor(255, 0, 0, 255);
            package.AddTriangleVertexColor(0, 255, 0, 255);
            package.AddTriangleVertexColor(0, 0, 255, 255);

            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);

            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
        }

        private static void AddColoredLineToPackage(IRenderPackage package)
        {
            package.AddLineStripVertex(0,0,0);
            package.AddLineStripVertex(5,5,5);

            package.AddLineStripVertexColor(255,0,0,255);
            package.AddLineStripVertexColor(255,0,0,255);

            // Specify line segments by adding a line vertex count.
            // Ex. The above line has two vertices, so we add a line
            // vertex count of 2. If we had tessellated a curve with n
            // vertices, we would add a line vertex count of n.
            package.AddLineStripVertexCount(2);
        }
   ```
B. 



# Dynamo Samples
A collection of samples demonstrating how to develop libraries for Dynamo.

These samples make use of the [Dynamo NuGet packages](https://www.nuget.org/packages?q=DynamoVisualProgramming). NuGet should take care of restoring these packages if they are not available on your system at build time. 

# Building the Samples

- Clone the repository.
- Choose a branch:
  - The master branch of Dynamo Samples corresponds to the master branch of Dynamo. To build against a specific version, choose that version's branch. I.e. 0.8.0, 0.9.0, etc.
- In VisualStudio 2013 or greater, open DynamoSamples.2013.sln.
- Build the Debug/Any CPU configuration.
- The `dynamo_package` folder at the root of the repository will now have the built libraries. The `Dynamo Samples` folder in that directory can be copied directly to your Dynamo packages directory:`C:\Users\<you>\AppData\Roaming\Dynamo\0.8\packages`.
- To install the sample view extension the `SampleViewExtension\bin\debug` folder (or release) should contain
  - `SampleViewExtension.dll` which should be copied to your root Dynamo build location
  - `SampleViewExtension_ViewExtensionDefinition` which should be copied to the `viewExtensions` folder inside your root Dynamo build location
- Run Dynamo. You should find `SampleLibraryUI` and `SampleLibraryZeroTouch` categories in your library and the `view` tab inside of Dynamo should now contain `Show View Extension Sample Window`.

Assembly Reference
Path to assembly for binaries are defined in CS.props and user_local.props which can be found at $(SolutionDirectory)\Config
user_local.props defines path to binaries found in the bin folder of the local Dynamo repository
If the specified binary is not found, the path to the nuget packages binaries will be used instead which is defined in the CS.props file
