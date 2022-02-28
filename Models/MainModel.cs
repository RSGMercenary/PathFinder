using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PathFinder.Models
{
	[DataContract]
	public class MainModel : Model
	{
		private PageModel page;
		[DataMember]
		public bool WrapInput { get; set; } = false;
		[DataMember]
		public bool WrapOutput { get; set; } = false;
		[DataMember]
		public bool SavePages { get; set; } = true;

		public ObservableCollection<PageModel> Pages { get; private set; } = new ObservableCollection<PageModel>();

		public MainModel() { }

		[DataMember(Name = "Pages", Order = 0)]
		[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
		private ObservableCollection<PageModel> SerializePages
		{
			get => SavePages ? Pages : new ObservableCollection<PageModel>();
			set => Pages = value;
		}

		[DataMember(Name = "Page", Order = 1)]
		[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
		private int SerializePage
		{
			get => Pages.IndexOf(page);
			set
			{
				if(value > -1 && value < Pages.Count)
					Page = Pages[value];
			}
		}

		public PageModel Page
		{
			get => page;
			set
			{
				page = value;
				OnPropertyChanged();
			}
		}

		public void AddPage() => AddPage(new PageModel());

		public void AddPage(PageModel page) => AddPage(page, Pages.Count);

		public void AddPage(PageModel page, int index)
		{
			if(Pages.Count == 10)
				return;
			Pages.Insert(index, page);
			Page = Pages[index];
		}

		public void ClonePage(PageModel page) => AddPage(page.Clone(), Pages.IndexOf(page) + 1);

		public void RemovePage(PageModel page)
		{
			int index = Pages.IndexOf(page);
			Pages.RemoveAt(index);
			if(Pages.Count == 0)
				AddPage();
			else if(Pages.Count == index)
				Page = Pages[Pages.Count - 1];
		}
	}
}