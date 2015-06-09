using NUnit.Framework;
using System;
using MustacheCat.Core.Pooling;

namespace PoolingTest {

	public class MyObject {
		public int Value {get; set;}
	}

	public class MyObjectPool : AbstractObjectPool<MyObject> {
		public MyObjectPool(int size) : base(size) {
		}

		protected override MyObject Instantiate() {
			return new MyObject();
		}

		protected override void OnAcquire(MyObject instance) {
			instance.Value = 1;
		}

		protected override void OnRelease(MyObject instance) {
			instance.Value = 2;
		}
	}

	[TestFixture()]
	public class ObjectPoolTest {
		[Test()]
		public void TestAcquireRelease() {
			var pool = new MyObjectPool(2);

			var o1 = pool.Acquire();
			var o2 = pool.Acquire();
			var o3 = pool.Acquire();

			Assert.IsInstanceOf<MyObject>(o1);
			Assert.AreEqual(o1.Value, 1);
			Assert.IsInstanceOf<MyObject>(o2);
			Assert.AreEqual(o2.Value, 1);
			Assert.IsNull(o3);

			pool.Release(o2);
			Assert.AreEqual(o2.Value, 2);
			var o4 = pool.Acquire();
			Assert.IsInstanceOf<MyObject>(o4);
			Assert.AreEqual(o4.Value, 1);
		}
	}
}
