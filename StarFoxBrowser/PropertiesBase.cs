using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public abstract class PropertiesBase : IMenuCommandService, ISite, IComponent
	{
		[Browsable(false)]
		public abstract IEnumerable<DesignerVerb> Commands { get; }

		[Browsable(false)]
		public DesignerVerbCollection Verbs => new DesignerVerbCollection(Commands.ToArray());

		[Browsable(false)]
		public IComponent Component => this;

		[Browsable(false)]
		public IContainer Container => null;

		[Browsable(false)]
		public bool DesignMode => throw new NotImplementedException();

		[Browsable(false)]
		public string Name { get => "Properties"; set => throw new NotImplementedException(); }

		[Browsable(false)]
		public ISite Site { get => this; set => throw new NotImplementedException(); }

		public event EventHandler Disposed;

		public void AddCommand(MenuCommand command)
		{
		}

		public void AddVerb(DesignerVerb verb)
		{
		}

		public void Dispose()
		{
			Disposed?.Invoke(this, new EventArgs());
		}

		public MenuCommand FindCommand(CommandID commandID)
		{
			throw new NotImplementedException();
		}

		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IMenuCommandService))
				return this;

			return null;
		}

		public bool GlobalInvoke(CommandID commandID)
		{
			throw new NotImplementedException();
		}

		public void RemoveCommand(MenuCommand command)
		{
			throw new NotImplementedException();
		}

		public void RemoveVerb(DesignerVerb verb)
		{
			throw new NotImplementedException();
		}

		public void ShowContextMenu(CommandID menuID, int x, int y)
		{
			throw new NotImplementedException();
		}
	}
}
