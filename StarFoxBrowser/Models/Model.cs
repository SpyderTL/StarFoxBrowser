using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;

namespace StarFoxBrowser.Models
{
	public class Model
	{
		public Face[] Faces;

		public void Draw(Device device)
		{
			foreach (var face in Faces)
				face.Draw(device);
		}
	}

	public class Face
	{
		public Vertex[] Vertices;
		public int[] Indices;
		public PrimitiveType PrimitiveType;
		public int PrimitiveCount;

		public void Draw(Device device)
		{
			device.DrawIndexedUserPrimitives(PrimitiveType, 0, Vertices.Length, PrimitiveCount, Indices, Format.Index32, Vertices);
		}
	}

	public struct Vertex
	{
		public Vector4 Position;
		public Vector4 Color;
	}
}
