using System;

namespace MustacheCat.Core.Pooling {
	public interface IObjectPool<TType> where TType : class {
		TType Acquire();
		void Release(TType reference);
	}
}
