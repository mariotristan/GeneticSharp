using System;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using NUnit.Framework;
using Rhino.Mocks;
using TestSharp;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    public class OnePointCrossoverTest
    {
        [Test]
        public void Cross_LessGenesThenSwapPoint_Exception()
        {
            var target = new OnePointCrossover(1);
            var chromosome1 = MockRepository.GenerateStub<ChromosomeBase>();

            chromosome1.AddGenes(new List<Gene>() { new Gene() });
            ExceptionAssert.IsThrowing(new ArgumentOutOfRangeException("parents", "The swap point index is 1, but there is only 1 genes. The swap should result at least one gene to each side."), () =>
            {
                target.Cross(new List<IChromosome>() {
                    chromosome1,
                    chromosome1
                });
            });

			var chromosome2 = MockRepository.GenerateStub<ChromosomeBase>();
			chromosome2.AddGenes(new List<Gene>() { new Gene(), new Gene() });
            ExceptionAssert.IsThrowing(new ArgumentOutOfRangeException("parents", "The swap point index is 1, but there is only 2 genes. The swap should result at least one gene to each side."), () =>
            {
                target.Cross(new List<IChromosome>() {
                    chromosome2,
                    chromosome2
                });
            });
        }

        [Test]
        public void Cross_ParentsWithTwoGenes_Cross()
        {
            var target = new OnePointCrossover(0);
			var chromosome1 = MockRepository.GenerateStub<ChromosomeBase>();
            chromosome1.AddGenes(new List<Gene>() 
            { 
                new Gene() { Value = 1 },
                new Gene() { Value = 2 }
            });
			chromosome1.Expect(c => c.CreateNew()).Return(MockRepository.GenerateStub<ChromosomeBase>());

            var chromosome2 = MockRepository.GenerateStub<ChromosomeBase>();            
			chromosome2.AddGenes(new List<Gene>() 
            { 
                new Gene() { Value = 3 },
                new Gene() { Value = 4 }
            });
			chromosome2.Expect(c => c.CreateNew()).Return(MockRepository.GenerateStub<ChromosomeBase>());

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual[0].Length);
            Assert.AreEqual(2, actual[1].Length);

            Assert.AreEqual(1, actual[0].GetGene(0).Value);
            Assert.AreEqual(4, actual[0].GetGene(1).Value);

            Assert.AreEqual(3, actual[1].GetGene(0).Value);
            Assert.AreEqual(2, actual[1].GetGene(1).Value);
        }
    }
}