using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using StarFoxBrowser.Models;
using StarFoxBrowser.Nodes;

namespace StarFoxBrowser
{
	public partial class BrowserForm : Form
	{
		Direct3D Direct3D;
		Device Device;
		Effect ColorEffect;
		Effect TextureEffect;
		VertexDeclaration ColorVertexDeclaration;
		VertexDeclaration TextureVertexDeclaration;
		Texture Texture1;
		Texture Texture2;

		Model SelectedModel = null;
		Timer Timer;

		public BrowserForm()
		{
			InitializeComponent();

			try
			{
				Direct3D = new Direct3D();

				Device = new Device(Direct3D, 0, DeviceType.Hardware, panel.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters(panel.ClientSize.Width, panel.ClientSize.Height));

				ColorEffect = Effect.FromString(Device, @"
float4x4 worldViewProjection;

struct VS_IN
{
	float4 position : POSITION;
	float4 color : COLOR0;
};

struct PS_IN
{
	float4 position : POSITION;
	float4 color : COLOR0;
};

PS_IN VS( VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.position = mul(input.position,worldViewProjection);
	output.color=input.color;

	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	return input.color;
}

technique Main { 
 	pass P0 { 
 		VertexShader = compile vs_2_0 VS(); 
         PixelShader  = compile ps_2_0 PS();
	}
}", ShaderFlags.None);

				var vertexElements = new[] {
				new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
				new VertexElement(0, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
				VertexElement.VertexDeclarationEnd
			};

				ColorVertexDeclaration = new VertexDeclaration(Device, vertexElements);

				TextureEffect = Effect.FromString(Device, @"
float4x4 worldViewProjection;
texture currentTexture;
sampler textureSampler = sampler_state
{
    Texture = currentTexture;
    MipFilter = POINT;
    MinFilter = POINT;
    MagFilter = POINT;
};

struct VS_IN
{
	float4 position : POSITION;
	float2 texturePosition : TEXCOORD;
};

struct PS_IN
{
	float4 position : POSITION;
	float2 texturePosition : TEXCOORD;
};

PS_IN VS( VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.position = mul(input.position,worldViewProjection);
	output.texturePosition=input.texturePosition;

	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	return tex2D(textureSampler, input.texturePosition);
}

technique Main { 
 	pass P0 { 
 		VertexShader = compile vs_2_0 VS(); 
         PixelShader  = compile ps_2_0 PS();
	}
}", ShaderFlags.None);

				vertexElements = new[] {
				new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
				new VertexElement(0, 16, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
				VertexElement.VertexDeclarationEnd
			};

				TextureVertexDeclaration = new VertexDeclaration(Device, vertexElements);

				Texture1 = new Texture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
				Texture2 = new Texture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);

				var rectangle1 = Texture1.LockRectangle(0, LockFlags.None, out DataStream stream1);
				var rectangle2 = Texture2.LockRectangle(0, LockFlags.None, out DataStream stream2);

				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StarFoxBrowser.Resources.StarFoxUsa10.bin"))
				using (var reader = new BinaryReader(stream))
				{
					// Load Palette
					stream.Position = 0x18b0a;

					var palette = Enumerable.Range(0, 16)
						.Select(n => reader.ReadUInt16())
						.Select(n => new SharpDX.Color((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3, 255))
						.ToArray();

					palette[0] = SharpDX.Color.Transparent;

					stream.Position = 0x90000;

					for (var y = 0; y < 256; y++)
					{
						for (var x = 0; x < 256; x++)
						{
							var value = reader.ReadByte();

							var texture1Value = value & 0x0f;
							var texture2Value = value >> 4;

							stream1.Write(palette[texture1Value].B);
							stream1.Write(palette[texture1Value].G);
							stream1.Write(palette[texture1Value].R);
							stream1.Write(palette[texture1Value].A);

							stream2.Write(palette[texture2Value].B);
							stream2.Write(palette[texture2Value].G);
							stream2.Write(palette[texture2Value].R);
							stream2.Write(palette[texture2Value].A);
						}
					}
				}

				Texture1.UnlockRectangle(0);
				Texture2.UnlockRectangle(0);

				Timer = new Timer
				{
					Interval = 10
				};

				Timer.Tick += Timer_Tick;

				Timer.Start();
			}
			catch (Exception)
			{
			}

			treeView.Nodes.Add(new Usa10
			{
				Text = "Star Fox 1.0 (USA)",
				Resource = "StarFoxBrowser.Resources.StarFoxUsa10.bin"
			});
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (treeView.SelectedNode is StarFoxModel &&
				tabControl.SelectedIndex == 1)
			{
				SelectedModel = (Model)((StarFoxModel)treeView.SelectedNode).GetProperties();

				DrawModel();
			}
		}

		private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DataNode)
			{
				treeView.BeginUpdate();
				((DataNode)e.Node).Reload();
				treeView.EndUpdate();
			}
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DataNode)
			{
				propertyGrid.SelectedObject = ((DataNode)e.Node).GetProperties();

				if (propertyGrid.SelectedObject is Bitmap)
				{
					pictureBox.Image = (Bitmap)propertyGrid.SelectedObject;
					panel.Hide();
					SelectedModel = null;
				}
				else if (propertyGrid.SelectedObject is Model)
				{
					pictureBox.Image = null;
					SelectedModel = (Model)propertyGrid.SelectedObject;
					//DrawModel();
					panel.Show();
				}
				else
				{
					pictureBox.Image = null;
					panel.Hide();
					SelectedModel = null;
				}
			}
			else
				propertyGrid.SelectedObject = null;
		}

		private void DrawModel()
		{
			Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new SharpDX.Color(64, 32, 128, 255), 1.0f, 0);

			if (SelectedModel != null)
			{
				Device.BeginScene();

				ColorEffect.Technique = ColorEffect.GetTechnique(0);
				ColorEffect.Begin(FX.DoNotSaveState);
				ColorEffect.BeginPass(0);

				Device.SetRenderState(RenderState.AlphaBlendEnable, false);
				//Device.SetRenderState(RenderState.CullMode, Cull.None);

				// Update Camera
				var ratio = panel.ClientSize.Width / (float)panel.ClientSize.Height;

				var projection = Matrix.PerspectiveFovLH(3.14f / 3.0f, ratio, 1, 10000);
				//var view = Matrix.LookAtLH(new Vector3(0, 80, -150), new Vector3(0, 50, 0), Vector3.UnitY);
				//var view = Matrix.LookAtLH(new Vector3(0, 0, -150), new Vector3(0, 50, 0), Vector3.UnitY);
				var view = Matrix.LookAtLH(new Vector3(0, 250, -1000), new Vector3(0, 50, 0), Vector3.UnitY);

				// Draw Model
				Device.VertexDeclaration = ColorVertexDeclaration;

				//var world = Matrix.Identity;
				//var world = Matrix.RotationY(MathUtil.Pi);
				var world = Matrix.RotationY((float)DateTime.Now.TimeOfDay.TotalSeconds);

				var worldViewProjection = world * view * projection;

				ColorEffect.SetValue("worldViewProjection", worldViewProjection);
				ColorEffect.CommitChanges();

				SelectedModel.DrawColor(Device);

				ColorEffect.EndPass();
				ColorEffect.End();

				TextureEffect.Technique = TextureEffect.GetTechnique(0);
				TextureEffect.Begin(FX.DoNotSaveState);
				TextureEffect.BeginPass(0);

				Device.SetRenderState(RenderState.AlphaBlendEnable, true);
				Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
				Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

				Device.VertexDeclaration = TextureVertexDeclaration;

				TextureEffect.SetValue("worldViewProjection", worldViewProjection);
				TextureEffect.SetTexture("currentTexture", Texture1);
				TextureEffect.CommitChanges();

				SelectedModel.DrawTexture1(Device);

				TextureEffect.SetTexture("currentTexture", Texture2);
				TextureEffect.CommitChanges();

				SelectedModel.DrawTexture2(Device);

				TextureEffect.EndPass();
				TextureEffect.End();

				Device.EndScene();
			}

			Device.Present();
		}
	}
}
