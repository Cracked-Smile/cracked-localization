using System;

namespace CrackedSmile.Localization.Services.Utility
{
	internal sealed class ActionDisposable : IDisposable
	{
		private Action _dispose;
			
		public ActionDisposable(Action dispose)
		{
			_dispose = dispose;
		}

		public void Dispose()
		{
			_dispose?.Invoke();
			_dispose = null;
		}
	}
}