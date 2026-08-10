using CurrenciesLib;
using CurrenciesLib.ConversionProviders;
using log4net;
using Microsoft.Extensions.Hosting;
using PayGram.Public.Client;

namespace PayGram.Public
{
	/// <summary>
	/// Hosted service that periodically pulls exchange rates from the PayGram bot and feeds them into
	/// the conversion cache. Register it with <c>services.AddHostedService&lt;CurrenciesManager&gt;()</c>:
	/// the host owns its lifetime and passes the application-shutdown token to <see cref="ExecuteAsync"/>.
	/// </summary>
	public class CurrenciesManager : BackgroundService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(CurrenciesManager));

		readonly ulong updateRatesEveryMillis;
		readonly PayGramBotClient pClient;

		public CurrenciesManager(PayGramBotClient pClient, ulong updateRatesEveryMillis = 0)
		{
			this.pClient = pClient;

			if (updateRatesEveryMillis != 0)
			{
				this.updateRatesEveryMillis = updateRatesEveryMillis;
				ConversionProviderFactory.QuotesValidForMillis = updateRatesEveryMillis * 2; // we have some time to update
			}
			else
			{
				this.updateRatesEveryMillis = ConversionProviderFactory.QuotesValidForMillis;
				ConversionProviderFactory.QuotesValidForMillis *= 2; // we don't know how often paygram updates the rates, let's choose a large value
			}
			// RateGraph handles multi-hop conversion via BFS — no need for ToDefaultCurrencyConversionProvider
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			log.Info($"CurrenciesManager started; refreshing rates every {updateRatesEveryMillis} ms");
			try
			{
				while (!stoppingToken.IsCancellationRequested)
				{
					try
					{
						await doJob(pClient);
					}
					catch (Exception ex)
					{
						// never let a single failed refresh stop the loop: log it and retry next interval.
						log.Error("CurrenciesManager rate-update cycle failed; retrying next interval", ex);
					}

					// honour the shutdown token so the delay is interrupted immediately on stop,
					// instead of blocking the host for up to a full interval.
					await Task.Delay(TimeSpan.FromMilliseconds(updateRatesEveryMillis), stoppingToken);
				}
			}
			catch (OperationCanceledException)
			{
				// expected: the host requested shutdown while we were delaying.
			}
			log.Info("CurrenciesManager stopped");
		}

		private static async Task doJob(PayGramBotClient pClient)
		{
			var res = await pClient.GetExchangeRatesAsync();

			if (res.Success)
			{
				foreach (var bc in res.Rates.Keys)
				{
					foreach (var qc in res.Rates[bc].Values)
					{
						var nq = new Quote()
						{
							BaseCurrency = bc,
							QuoteCurrency = qc.QuoteCurrency,
							Midpoint = qc.Price,
							IsInferred = false,
							SpreadBuy = qc.SpreadBuy,
							SpreadSell = qc.SpreadSell
						};
						if (Currency.GetBySymbol(bc) == null || Currency.GetBySymbol(nq.QuoteCurrency) == null)
						{
							//log.Debug($"Basecurrency or quote currency could not be parsed {bc}:{nq.BaseCurrency}");
						}
						else
						{
							//log.Debug($"{nq}/{qc}");
							ConversionProviderFactory.CacheConversionProvider.UpdateCache(nq, qc.UpdatedUTC);
						}
					}
				}
			}
		}
	}
}
