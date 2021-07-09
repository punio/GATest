using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace WpfApp8
{
	public class TspChromosome : ChromosomeBase
	{
		private readonly int[] _inputGenes;

		public TspChromosome(int[] inputGenes) : base(inputGenes.Length)
		{
			_inputGenes = inputGenes;

			// Shuffle
			var genes = inputGenes.Select(g => g).OrderBy(g => new Guid()).Select(x => new Gene(x)).ToArray();

			ReplaceGenes(0, genes);
		}

		public override Gene GenerateGene(int geneIndex)
		{
			return new Gene(RandomizationProvider.Current.GetInt(0, _inputGenes.Length));
		}

		public override IChromosome CreateNew()
		{
			return new TspChromosome(_inputGenes);
		}
	}
}
