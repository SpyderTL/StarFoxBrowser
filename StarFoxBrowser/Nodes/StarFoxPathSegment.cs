namespace StarFoxBrowser.Nodes
{
	internal class StarFoxPathSegment : DataNode
	{
		public string Resource { get; set; }
		public ushort Offset { get; set; }

		public override object GetProperties()
		{
			return new { Offset };
		}

		public override void Reload()
		{
			Nodes.Clear();
		}
	}
}