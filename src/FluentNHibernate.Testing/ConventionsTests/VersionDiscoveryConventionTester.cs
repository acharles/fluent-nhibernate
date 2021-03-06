using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class VersionDiscoveryConventionTester
    {
        private VersionDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new VersionDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIVersions()
        {
            convention.Accept(MockRepository.GenerateStub<IVersion>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<IProperty>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IVersionConvention>())
                .Return(new IVersionConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IVersion>());
            conventionFinder.AssertWasCalled(x => x.Find<IVersionConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IVersionConvention>(),
                MockRepository.GenerateMock<IVersionConvention>()
            };
            var id = MockRepository.GenerateStub<IVersion>();

            conventionFinder.Stub(x => x.Find<IVersionConvention>())
                .Return(conventions);

            convention.Apply(id);

            conventions[0].AssertWasCalled(x => x.Accept(id));
            conventions[1].AssertWasCalled(x => x.Accept(id));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IVersionConvention>(),
                MockRepository.GenerateMock<IVersionConvention>()
            };
            var id = MockRepository.GenerateStub<IVersion>();

            conventionFinder.Stub(x => x.Find<IVersionConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(id)).Return(true);
            conventions[1].Stub(x => x.Accept(id)).Return(false);

            convention.Apply(id);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(id));
            conventions[1].AssertWasNotCalled(x => x.Apply(id));
        }
    }
}