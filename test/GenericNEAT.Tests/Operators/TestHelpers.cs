using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericNEAT.Operators.Tests
{
    public static class TestHelpers
    {
        public static FlipBitMutation UniformMutation = new FlipBitMutation();
        public static IntegerChromosome IntegerChromosome() => new IntegerChromosome(0, 1);

        public static Vertex<IChromosome> Vertex(uint id) => new Vertex<IChromosome>(id, IntegerChromosome());
        public static IEnumerable<Vertex<IChromosome>> VertexCollection(params uint[] ids) => ids.Select(id => Vertex(id));

        public static Edge<IChromosome> Edge(uint idFrom, uint idTo) => new Edge<IChromosome>(idFrom, idTo, IntegerChromosome());
        public static IEnumerable<Edge<IChromosome>> EdgeCollection(params (uint, uint)[] ids) => ids.Select(a => Edge(a.Item1, a.Item2));

        public static bool AnyIsTrue(IChromosome c) => c.GetGenes().Any(g => g.Value is true);
    }
}
