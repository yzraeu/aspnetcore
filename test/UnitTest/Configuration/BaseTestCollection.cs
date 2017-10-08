using Xunit;

namespace UnitTest
{
	[CollectionDefinition("Base Collection")]
	public abstract class Name : ICollectionFixture<BaseTestFixture>
	{
		
	}
}