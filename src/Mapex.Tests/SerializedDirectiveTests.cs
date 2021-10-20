using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_calling_equals_on_serialized_directive_equality_comparer
	{
		[Test]
		public void It_should_return_true_if_the_instances_have_the_same_reference()
		{
			var serializedDirective = new SerializedDirective();
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsTrue(comparer.Equals(serializedDirective, serializedDirective));
		}

		[Test]
		public void It_should_return_false_if_the_first_instance_is_null()
		{
			var serializedDirective = new SerializedDirective();
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsFalse(comparer.Equals(null, serializedDirective));
		}

		[Test]
		public void It_should_return_false_if_the_second_instance_is_null()
		{
			var serializedDirective = new SerializedDirective();
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsFalse(comparer.Equals(serializedDirective, null));
		}

		[Test]
		public void It_should_return_false_if_the_types_are_different()
		{
			var serializedDirective = new SerializedDirective();
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsFalse(comparer.Equals(serializedDirective, new SerializedDirectiveDerivative()));
		}

		[Test]
		public void It_should_return_false_if_the_ids_are_different()
		{
			var serializedDirective1 = new SerializedDirective { Id = 100, Data = "data-1" };
			var serializedDirective2 = new SerializedDirective { Id = 200, Data = "data-1" };
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsFalse(comparer.Equals(serializedDirective1, serializedDirective2));
		}

		[Test]
		public void It_should_return_false_if_the_data_are_different()
		{
			var serializedDirective1 = new SerializedDirective { Id = 100, Data = "data-1" };
			var serializedDirective2 = new SerializedDirective { Id = 100, Data = "data-2" };
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsFalse(comparer.Equals(serializedDirective1, serializedDirective2));
		}

		[Test]
		public void It_should_return_true_if_the_ids_and_data_are_the_same()
		{
			var serializedDirective1 = new SerializedDirective { Id = 100, Data = "data-1" };
			var serializedDirective2 = new SerializedDirective { Id = 100, Data = "data-1" };
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsTrue(comparer.Equals(serializedDirective1, serializedDirective2));
		}

		private class SerializedDirectiveDerivative : SerializedDirective
		{
		}
	}

	[TestFixture]
	public class When_calling_get_hashcode_on_serialized_directive_equality_comparer
	{
		[Test]
		public void It_should_return_the_same_hashcode_for_equal_values()
		{
			var serializedDirective1 = new SerializedDirective {Id = 100, Data = "data-1"};
			var serializedDirective2 = new SerializedDirective {Id = 100, Data = "data-1"};

			var comparer = new SerializedDirective.EqualityComparer();

			Assert.AreEqual(comparer.GetHashCode(serializedDirective1), comparer.GetHashCode(serializedDirective2));
		}

		[Test]
		public void It_should_return_a_different_hashcode_for_a_different_id()
		{
			var serializedDirective1 = new SerializedDirective { Id = 100, Data = "data-1" };
			var serializedDirective2 = new SerializedDirective { Id = 200, Data = "data-1" };
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.AreNotEqual(comparer.GetHashCode(serializedDirective1), comparer.GetHashCode(serializedDirective2));
		}

		[Test]
		public void It_should_return_a_different_hashcode_for_different_data()
		{
			var serializedDirective1 = new SerializedDirective { Id = 100, Data = "data-1" };
			var serializedDirective2 = new SerializedDirective { Id = 100, Data = "data-2" };
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.AreNotEqual(comparer.GetHashCode(serializedDirective1), comparer.GetHashCode(serializedDirective2));
		}

		[Test]
		public void It_should_return_a_hashcode_for_null_data()
		{
			var serializedDirective = new SerializedDirective { Id = 100, Data = null };
			
			var comparer = new SerializedDirective.EqualityComparer();
			
			Assert.IsNotNull(comparer.GetHashCode(serializedDirective));
		}
	}
}
