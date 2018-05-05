using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace Examples
{
    public class CustomRenderExample : IGraphicItem
    {
        private CustomRenderExample(){}

        /// <summary>
        /// 静态构造器
        /// </summary>
        public static CustomRenderExample Create()
        {
            return new CustomRenderExample();
        }

        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            //实现IGraphicItem接口中的方法,该方法用于几何体绘制.
            //形参中的IRenderPackage对象可以用来填充渲染数据,

            package.RequiresPerVertexColoration = true;//逐节点着色模式

            AddColoredQuadToPackage(package);//画两个三角形
            AddColoredLineToPackage(package);//划线
        }
        /// <summary>
        /// 填充顶点信息:两个三角形
        /// </summary>
        /// <param name="package"></param>
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
        /// <summary>
        /// 填充顶点信息:一条线段
        /// </summary>
        /// <param name="package"></param>
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
    }
}
