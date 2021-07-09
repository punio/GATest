using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Framework.Threading;
using Reactive.Bindings;

namespace WpfApp8
{
	public class MainViewModel
	{
		public ReactiveCollection<City> Points { get; } = new();
		public ReactiveCollection<CityPath> Paths { get; } = new();

		public ReactiveProperty<int> PointCount { get; } = new(20);
		public ReactiveProperty<int> Population { get; } = new(50);
		public ReactiveProperty<int> Generation { get; } = new(0);
		public ReactiveProperty<int> GenerationSkip { get; } = new(10);
		public ReactiveProperty<double> BestDistance { get; } = new(0);
		public ReactiveProperty<double> BestFitness { get; } = new(0);

		public ReactiveCommand InitializeCommand { get; } = new();
		public ReactiveCommand StartCommand { get; } = new();

		private City[] _currentArray;
		private TspChromosome _currentTspChromosome;
		private TspFitness _currentTspFitness;
		private GeneticAlgorithm _currentGa;

		public MainViewModel()
		{
			Initialize();

			InitializeCommand.Subscribe(_ => Initialize());
			StartCommand.Subscribe(_ => Start());
		}

		private void Initialize()
		{
			Paths.Clear();
			Points.Clear();
			for (var i = 0; i < PointCount.Value; i++)
			{
				Points.Add(new City
				{
					X = RandomizationProvider.Current.GetInt(0, 500),
					Y = RandomizationProvider.Current.GetInt(0, 500),
				});
			}

			_currentArray = Points.ToArray();
			_currentTspFitness = new TspFitness(_currentArray);
			_currentTspChromosome = new TspChromosome(1.To(_currentArray.Length - 1).ToArray());
			var population = new Population(Population.Value, Population.Value, _currentTspChromosome);

			var taskExecutor = new ParallelTaskExecutor();
			taskExecutor.MinThreads = 8;
			taskExecutor.MaxThreads = 16;

			_currentGa = new GeneticAlgorithm(
				population,
				_currentTspFitness,
				new TournamentSelection(),
				new OrderedCrossover(),
				new ReverseSequenceMutation());
			_currentGa.TaskExecutor = taskExecutor;

			Generation.Value = 0;
			_currentGa.Termination = new GenerationNumberTermination(Generation.Value);
			_currentGa.Start();
		}

		private void Start()
		{
			Generation.Value += GenerationSkip.Value;
			_currentGa.Termination = new GenerationNumberTermination(Generation.Value);
			_currentGa.Resume();


			Paths.Clear();

			var best = _currentGa.BestChromosome;
			BestFitness.Value = best.Fitness ?? 0;

			var points = _currentTspFitness.GetPoints(best);

			Paths.AddRangeOnScheduler(points.Pairwise((x, y) => new CityPath { From = x, To = y }));
			BestDistance.Value = _currentTspFitness.GetTotalDistance(best);
		}
	}
}
