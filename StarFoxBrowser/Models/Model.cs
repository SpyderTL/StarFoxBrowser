using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace StarFoxBrowser.Models
{
	public class Model
	{
		public VertexDeclaration VertexDeclaration;
		public VertexBuffer VertexBuffer;
		public IndexBuffer IndexBuffer;
		public Face[] Faces;
	}

	public class Face
	{
		public int StartIndex;
		public int PrimitiveCount;
		public PrimitiveType PrimitiveType;
	}
}
