using System;
using System.Collections.Generic;

namespace MustacheCat.Core.Pooling {
	public abstract class AbstractObjectPool<TType> : IObjectPool<TType> where TType : class {
		List<TType> pool;
		Dictionary<TType, int> index;
		Stack<int> available;


		// Make a new pool of a fixed size
		public AbstractObjectPool(int size) {
			pool = new List<TType>(size);
			index = new Dictionary<TType, int>(size);
			available = new Stack<int>(size);

			for (var i = 0; i < size; i++) {
				var instance = Instantiate();
				pool.Add(instance);
				index[instance] = i;
				available.Push(i);
			}
		}

		// Acquire an instance from the pool
		// By default, this instance will have old state
		public TType Acquire() {
			if (available.Count > 0) {
				int pos = available.Pop();
				var instance = pool[pos];;
				OnAcquire(instance);
				return instance;
			} else {
				return AcquireOnLimitReached();
			}
		}

		// Release an instance to the pool
		// Does not free instance from returning end
		public void Release(TType reference) {
			int pos = index[reference];
			available.Push(pos);
			OnRelease(reference);
		}

		// return null by default
		// Override to grow the stack, kill the oldest object, etc.
		protected virtual TType AcquireOnLimitReached() {
			return default(TType);
		}

		// noop by default
		// Overwrite this to clear the object on acquire, do bookkeeping, etc
		protected virtual void OnAcquire(TType instance) {
		}

		// noop by default
		// Overwrite this to clear the object on release
		protected virtual void OnRelease(TType reference) {
		}

		// Implement to create an object of the Pool's type
		protected abstract TType Instantiate();
	}
}
