using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace Examples
{
    /// <summary>
    /// 注意:构建的dll在被加载到程序中后,会在控件箱中形成若干层目录和一些节点.
    /// 其中工程名是一层目录,命名空间是一层目录,类名是最后一层目录,类中的可见元素是节点
    /// publiuc的Properties和函数会显示成节点,函数的参数和返回值分别对应节点的输入和输出端口
    /// 
    /// </summary>
    public class BasicExample : IGraphicItem
    {

        /*
         * IGraphicItem接口是用来向Watch3D绘图的,如果自定义节点不需要绘图则无需实现这个接口
         */
        private Point point;


        ///-------------------------------------------------------------
        ///关于XML注释:
        /// 如果你对类和方法进行了xml注释(即方法头自动注释),
        /// 注释的内容将会以TOP Tips的形式显示在界面上.
        ///
        /// 为了实现这个效果,你需要这样操作:
        /// 1.在解决方案管理器中右击项目节点
        /// 2.选择属性
        /// 3.选择生成
        /// 4.勾选XML文档的checkbox
        ///
        /// 生成的xml必须在dll旁边并且叫相同的名字
        /// -------------------------------------------------------------


        /// <summary>
        /// Public 的 Properties 会显示为Node
        /// </summary>
        public double Awesome { get { return 42.0; } }

        /// <summary>
        /// The Point stored on the object.
        /// </summary>
        public Point Point { get { return point; } }

        /// <summary>
        /// internal的Properties和函数不会出现在UI上
        /// </summary>
        internal double InvisibleProperty { get { return 42.0; } }

        /// <summary>
        /// Private 函数不显示
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        private BasicExample(double x, double y, double z)
        {
            point = Point.ByCoordinates(x, y, z);
        }

        /// <summary>
        /// 使用静态构造器
        /// 如果使用了默认函数,就不必给节点的输入端口连接其他节点.
        /// 如果填充了xmlcomments,节点会具备Top Tips.
        /// </summary>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <param name="z">The z coordinate of the point.</param>
        /// <returns>A HelloDynamoZeroTouch object.</returns>
        public static BasicExample Create(double x=42.0, double y=42.0, double z=42.0)
        {
            /*
             * 抛出ArgumentException异常将会阻止图的运行和实例化,并且节点会变成黄色
             * 上边会出现话泡,并显示出异常信息
             */
            if (x < 0)
            {
                throw new ArgumentException("x");
            }

            if (y < 0)
            {
                throw new ArgumentException("y");
            }

            if (z < 0)
            {
                throw new ArgumentException("z");
            }

            return new BasicExample(x, y, z);
        }

        /// <summary>
        /// 如果参数不是基本类型,要给出默认参数的话,可以使用这种写法
        /// </summary>
        /// <param name="point">A point.</param>
        /// <returns>A BasicExample object.</returns>
        public static BasicExample Create([DefaultArgumentAttribute("Point.ByCoordinates(5,5,5);")]Point point)
        {
            return new BasicExample(point.X, point.Y, point.Z);
        }

        public int Calculator(int a, int b)
        {
            return a + b;
        }
        public static int StaticCalculator(int a, int b)
        {
            return a + b;
        }
        /// <summary>
        /// 如果一个节点要返回多个值,就把返回类型设为Dictionary
        /// 并在前面标记MultiReturn Attribute,Attribute参数是两个返回端口的名称
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "thing 1", "thing 2" })]
        public static Dictionary<string, List<string>> MultiReturnExample()
        {
            return new Dictionary<string, List<string>>()
            {
                { "thing 1", new List<string>{"apple", "banana", "cat"} },
                { "thing 2", new List<string>{"Tywin", "Cersei", "Hodor"} }
            };
        }

        /// <summary>
        /// 当节点被连接到Watch节点上时,有可能会调用ToString,重写这个函数可以指定这种情况下输出的是什么
        /// </summary>
        public override string ToString()
        {
            return string.Format("HelloDynamoZeroTouch:{0},{1},{2}", point.X, point.Y, point.Z);
        }

        #region IGraphicItem interface

        /// <summary>
        /// 填充package可以绘图.兼容IGraphicItem的类对象可以被解释成几何体从而在Watch 3D 中绘制
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            // This example contains information to draw a point
            package.AddPointVertex(point.X, point.Y, point.Z);
            package.AddPointVertexColor(255, 0, 0, 255);
        }

        #endregion
    }

    /// <summary>
    /// 使用IsVisibleInDynamoLibrary attribute标注一个类并设置为false,
    /// 意味着这个类可以被VM中的其他类所依赖,但是不会显示为一个节点目录.
    ///
    /// 使用SupressImportIntoVM attribute标注一个类,
    /// 意味着这个类不会被导入到VM中,对于其他依赖与这个类的方法和类,这个类将不可见.
    /// 例如测试类,就可以这样写
    /// </summary>
    [SupressImportIntoVM]
    [IsVisibleInDynamoLibrary(false)]
    public class DoesNotImportClass
    {
        public DoesNotImportClass(){}
    }
}
