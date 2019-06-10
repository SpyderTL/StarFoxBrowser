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
		public ColorFace[] ColorFaces;
		public TextureFace[] Texture1Faces;
		public TextureFace[] Texture2Faces;

		public void DrawColor(Device device)
		{
			foreach (var face in ColorFaces)
				face.Draw(device);
		}

		public void DrawTexture1(Device device)
		{
			foreach (var face in Texture1Faces)
				face.Draw(device);
		}

		public void DrawTexture2(Device device)
		{
			foreach (var face in Texture2Faces)
				face.Draw(device);
		}
	}

	public class ColorFace
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

	public class TextureFace
	{
		public TextureVertex[] Vertices;
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

	public struct TextureVertex
	{
		public Vector4 Position;
		public Vector2 TexturePosition;
	}
}
