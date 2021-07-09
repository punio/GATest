using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace WpfApp8
{
	public class TspFitness : IFitness
	{
		private readonly City[] _inputPoints;
		private readonly double _initialDistance;

		public TspFitness(City[] inputPoints)
		{
			_inputPoints = inputPoints;

			var cycle = _inputPoints.Append(_inputPoints[0]).ToArray();
			_initialDistance = GetTotalDistance(cycle);

		}

		double GetTotalDistance(City[] points) => points.Pairwise((x, y) => x.DistanceTo(y)).Sum();

		public double GetTotalDistance(IChromosome chromosome)
		{
			return GetTotalDistance(GetPoints(chromosome));
		}

		public City[] GetPoints(IChromosome chromosome) => chromosome.GetGenes().Select(x => (int)x.Value).Prepend(0).Append(0).Select(x => _inputPoints[x]).ToArray();

		public double Evaluate(IChromosome chromosome)
		{
			return Math.Max(0, 1.0 - (GetTotalDistance(chromosome) / _initialDistance));
		}


	}
}
