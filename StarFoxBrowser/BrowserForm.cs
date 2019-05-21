using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
		Effect Effect;
		VertexDeclaration VertexDeclaration;
		Model SelectedModel = null;
		Timer Timer;

		public BrowserForm()
		{
			InitializeComponent();

			Direct3D = new Direct3D();

			Device = new Device(Direct3D, 0, DeviceType.Hardware, panel.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters(panel.ClientSize.Width, panel.ClientSize.Height));

			Effect = Effect.FromString(Device, @"
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

			VertexDeclaration = new VertexDeclaration(Device, vertexElements);

			treeView.Nodes.Add(new Usa10
			{
				Text = "Star Fox 1.0 (USA)",
				Resource = "StarFoxBrowser.Resources.StarFoxUsa10.bin"
			});

			Timer = new Timer
			{
				Interval = 10
			};

			Timer.Tick += Timer_Tick;

			Timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (treeView.SelectedNode is StarFoxModel)
			{
				SelectedModel = (Model)((StarFoxModel)treeView.SelectedNode).GetProperties();

				DrawModel();
			}
		}

		private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DataNode)
				((DataNode)e.Node).Reload();
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
					DrawModel();
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
			Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, SharpDX.Color.Black, 1.0f, 0);
			Device.BeginScene();

			//effect.Technique = effect.GetTechnique(0);
			Effect.Begin(FX.DoNotSaveState);

			Effect.BeginPass(0);

			// Update Camera
			var ratio = panel.ClientSize.Width / (float)panel.ClientSize.Height;

			var projection = Matrix.PerspectiveFovLH(3.14f / 3.0f, ratio, 1, 1000);
			var view = Matrix.LookAtLH(new Vector3(0, 20, -100), Vector3.Zero, Vector3.UnitY);

			// Draw Model
			Device.VertexDeclaration = VertexDeclaration;

			//var world = Matrix.Identity;
			var world = Matrix.RotationY((float)DateTime.Now.TimeOfDay.TotalSeconds);

			var worldViewProjection = world * view * projection;

			Effect.SetValue("worldViewProjection", worldViewProjection);

			SelectedModel.Draw(Device);

			Effect.EndPass();
			Effect.End();

			Device.EndScene();
			Device.Present();
		}
	}
}
