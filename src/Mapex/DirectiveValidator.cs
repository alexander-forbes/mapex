using System;
using System.Collections.Generic;
using Mapex.Logging;

namespace Mapex
{
	internal interface IDirectiveValidator
	{
		void Validate(IEnumerable<IDirective> directives);
	}

	internal class DirectiveValidator : IDirectiveValidator
	{
		private static readonly ILog Log = LogProvider.For<DirectiveValidator>();

		public void Validate(IEnumerable<IDirective> directives)
		{
			if (directives == null)
				throw new ArgumentNullException(nameof(directives));

			foreach (var directive in directives)
			{
				var notification = directive.Validate();

				if (notification.HasErrors)
				{
					var message = $"Directive {directive.Id} is invalid because of the following errors:" +
					              $"{Environment.NewLine}{string.Join($",{Environment.NewLine}", notification.Errors)}.";

					Log.Error(message);
					throw new Exception(message);
				}
			}
		}
	}
}
