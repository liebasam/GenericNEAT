using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Crossovers;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericNEAT.Populations;
using GeneticSharp.Domain.Populations;

namespace GenericNEAT
{
    public static class TestHelpers
    {
        public static FlipBitMutation UniformMutation = new FlipBitMutation();
        public static UniformCrossover UniformCrossover = new UniformCrossover();
        public static IntegerChromosome IntegerChromosome() => new IntegerChromosome(0, 1);

        public static Vertex<IChromosome> Vertex(uint id) => new Vertex<IChromosome>(id, IntegerChromosome());
        public static IEnumerable<Vertex<IChromosome>> VertexCollection(params uint[] ids) => ids.Select(id => Vertex(id));

        public static Edge<IChromosome> Edge(uint idFrom, uint idTo) => new Edge<IChromosome>(idFrom, idTo, IntegerChromosome());
        public static IEnumerable<Edge<IChromosome>> EdgeCollection(params (uint, uint)[] ids) => ids.Select(a => Edge(a.Item1, a.Item2));

        public static bool AnyIsTrue(IChromosome c) => c.GetGenes().Any(g => g.Value is true);

        public static double GetValue(IChromosome floatingPointChomosome) =>
            (floatingPointChomosome as FloatingPointChromosome).ToFloatingPoints()[0];

        public static void SetFitnessesAndOrder(Population population, double fitness)
        {
            var g = population.CurrentGeneration;
            for (int i = 0; i < g.Chromosomes.Count; i++)
                g.Chromosomes[i].Fitness = fitness;
            population.EndCurrentGeneration();
        }
    }
}
