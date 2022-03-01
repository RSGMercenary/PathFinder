using PathFinder.Commands;
using PathFinder.Extensions;
using PathFinder.Processors;
using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace PathFinder.Models
{
	[DataContract]
	public class PageModel : Model, IPageModel
	{
		#region Static
		private const int MaxHistory = 25;
		private const double TypingDelay = 0.5;
		#endregion

		#region Fields
		private string parameter = "";
		private string delimiter = ",";
		private string input = "";
		private bool formData = true;
		private bool convert = true;
		private bool caseSensitive = false;
		private int outputsTotal = 0;
		private int progress = 0;
		private ProcessCategory processCategory;
		private IProcessor processor;
		private Match scroll;
		private bool ignoreParameter = false;
		#endregion

		#region Properties
		private ProcessWorker Worker { get; }
		[DataMember(Order = 1)]
		public ObservableCollection<string> History { get; } = new ObservableCollection<string>();
		public ObservableCollection<ProcessResult> Outputs { get; } = new ObservableCollection<ProcessResult>();
		public ObservableCollection<ProcessCategory> ProcessCategories { get; } = new ObservableCollection<ProcessCategory>();
		public CollectionViewSource ProcessCategoriesSource { get; }
		private Dictionary<ProcessType, IProcessor> Processors { get; } = new Dictionary<ProcessType, IProcessor>();
		#endregion

		#region Commands
		public Command InfoCommand { get; }
		public Command HistoryCommand { get; }
		public Command FormatCommand { get; }
		public Command<int> ScrollCommand { get; }
		public Command<int> ReinputCommand { get; }
		#endregion

		public PageModel()
		{
			ProcessCategoriesSource = new CollectionViewSource() { Source = ProcessCategories };
			ProcessCategoriesSource.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

			InfoCommand = new Command(GetInfo);
			HistoryCommand = new Command(AddHistory);
			FormatCommand = new Command(Format);
			ScrollCommand = new Command<int>(ScrollInput);
			ReinputCommand = new Command<int>(Reinput);

			Worker = new ProcessWorker(RemoveOutputs, AddOutputs, AddOutput);

			foreach(var processType in Enum.GetValues(typeof(ProcessType)).Cast<ProcessType>().Except(ProcessType.None.Enumerate()))
				ProcessCategories.Add(new ProcessCategory(processType));

			foreach(var processor in ProcessorProvider.GetProcessors())
			{
				try
				{
					Processors.Add(processor.ProcessType, processor);
				}
				catch(Exception exception)
				{
					MessageBox.Show($"Failed loading {processor.ProcessType.GetDescriptionOrToString()} processor." +
						$" Tried to load {processor.GetType()}, but {Processors[processor.ProcessType].GetType()} already exists." +
						$"{Environment.NewLine}{exception}", "Processor Load Failure");
				}
			}

			ProcessCategory = ProcessCategories[0];
		}

		public PageModel Clone()
		{
			var page = new PageModel();
			page.Parameter = Parameter;
			page.Input = Input;
			page.Delimiter = Delimiter;
			page.CaseSensitive = CaseSensitive;
			page.FormData = FormData;
			page.Convert = Convert;
			page.ProcessType = ProcessType;
			History.ForEach(page.History.Add);
			return page;
		}

		private void GetInfo()
		{
			if(Processor is IInfoProcessor processor)
				Process.Start(processor.InfoUrl);
		}

		#region Parameters
		[DataMember(Order = 0)]
		public string Parameter
		{
			get => parameter;
			set
			{
				if(ignoreParameter)
					return;
				parameter = value;
				OnPropertyChanged();
				if(!Processor.Implements<IParameterProcessor>())
					return;
				Worker.Start(TypingDelay);
			}
		}

		public IEnumerable<string> Parameters => Delimiter.Length > 0 ? Parameter.Split(Delimiter[0]).Where(p => !string.IsNullOrWhiteSpace(p)) : Parameter.Enumerate();

		[DataMember(Order = 2)]
		public string Delimiter
		{
			get => delimiter;
			set
			{
				delimiter = value;
				OnPropertyChanged();
				Worker.Start();
			}
		}

		private void AddHistory()
		{
			if(string.IsNullOrWhiteSpace(Parameter))
				return;
			ignoreParameter = true;
			if(History.Contains(Parameter))
				History.Remove(Parameter);
			History.Insert(0, Parameter);
			if(History.Count > MaxHistory)
				History.RemoveAt(MaxHistory);
			ignoreParameter = false;
		}
		#endregion

		#region Input
		[DataMember(Order = 3)]
		public string Input
		{
			get => input;
			set
			{
				input = value;
				OnPropertyChanged();
				Worker.Start(TypingDelay);
			}
		}

		private void Format()
		{
			try
			{
				if(Processor is IFormatterProcessor processor)
					Input = processor.Format(Input);
			}
			catch { }
		}
		#endregion

		#region Process
		[DataMember(Order = 5)]
		public bool FormData
		{
			get => formData;
			set
			{
				formData = value;
				OnPropertyChanged();
				Worker.Start();
			}
		}

		[DataMember(Order = 6)]
		public bool CaseSensitive
		{
			get => caseSensitive;
			set
			{
				caseSensitive = value;
				OnPropertyChanged();
				Worker.Start();
			}
		}

		[DataMember(Order = 7)]
		public bool Convert
		{
			get => convert;
			set
			{
				convert = value;
				OnPropertyChanged();
				Worker.Start();
			}
		}

		[DataMember(Order = 4)]
		public ProcessType ProcessType
		{
			get => ProcessCategory.ProcessType;
			set => ProcessCategory = ProcessCategories.First(p => p.ProcessType == value);
		}

		public ProcessCategory ProcessCategory
		{
			get => processCategory;
			set
			{
				processCategory = value;
				OnPropertyChanged();
				Processor = Processors.ContainsKey(processCategory.ProcessType) ? Processors[processCategory.ProcessType] : null;
				Worker.Start();
			}
		}

		public IProcessor Processor
		{
			get => processor;
			private set
			{
				processor = value;
				OnPropertyChanged();
			}

		}

		public int Progress
		{
			get => progress;
			set
			{
				progress = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region Outputs
		public int OutputsTotal
		{
			get => outputsTotal;
			private set
			{
				outputsTotal = value;
				OnPropertyChanged();
			}
		}

		public Match Scroll
		{
			get => scroll;
			private set
			{
				scroll = value;
				OnPropertyChanged();
			}
		}

		private void ScrollInput(int index)
		{
			var result = Outputs[index];
			if(result.Scroll != null)
				Scroll = result.Scroll;
			else
			{
				var casing = Processor.Implements<ICaseSensitiveProcessor>(true) && CaseSensitive;
				Scroll = ProcessorUtility.GetMatch(Input, result.Output, index, casing);
			}
		}

		private void Reinput(int index)
		{
			if(OutputsTotal <= 0)
				return;

			IEnumerable<string> outputs;
			if(index > -1)
				outputs = Outputs[index].Output.Enumerate();
			else
				outputs = Outputs.Select(o => o.Output);
			Input = Processor.Reinput(outputs);
		}

		private void RemoveOutputs()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Progress = 0;
				OutputsTotal = 0;
				Outputs.Clear();
			});
		}

		private (int, IEnumerable<ProcessResult>) AddOutputs()
		{
			var results = Enumerable.Empty<ProcessResult>();
			var type = Processor?.ProcessType ?? ProcessCategory.ProcessType;
			var phase = ProcessPhase.Outputs;

			try
			{
				results = Processor?.Process(this) ?? throw new ProcessorException(ProcessPhase.Processor);
				Application.Current.Dispatcher.Invoke(() => OutputsTotal = results.Count());
			}
			catch(ProcessorException exception)
			{
				phase = exception.Phase;
			}

			var total = OutputsTotal > 0 ? OutputsTotal : 1;
			return (total, ProcessorUtility.GetOutputs(type, phase, results));
		}

		private void AddOutput(int progress, ProcessResult result)
		{
			Progress = progress;
			Outputs.Add(result);
		}
		#endregion
	}
}