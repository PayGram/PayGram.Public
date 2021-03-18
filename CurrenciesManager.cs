using CurrenciesLib;
using CurrenciesLib.ConversionProviders;
using PayGram.Public.Telegram;
using System.Threading;
using System.Threading.Tasks;

namespace PayGram.Public
{
	public class CurrenciesManager
	{
		public bool IsRunning { get; private set; }

		ulong updateRatesEveryMillis;
		PayGramBotClient pClient;
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
				ConversionProviderFactory.QuotesValidForMillis *= 2; // we don't know how ofter paygram updates the rates, let's choose a large value
			}
			ConversionProviderFactory.RegisterConversionProvider(new ToDefaultCurrencyConversionProvider(Currencies.BTC));
			ConversionProviderFactory.RegisterConversionProvider(new ToDefaultCurrencyConversionProvider(Currencies.USD));
		}

		public void Start(CancellationToken token = default(CancellationToken))
		{
			if (IsRunning) return;
			IsRunning = true;
			//ct = new CancellationTokenSource();
			Task.Run(mainLoop, token).ConfigureAwait(false);
		}

		public void Stop()
		{
			//ct.Cancel();
			IsRunning = false;
		}

		private async Task mainLoop()
		{
			//PayGramBotClient pClient = new PayGramBotClient(new System.Guid("64cf8a6f-2f50-44da-900e-e3a958841b3a"));

			while (IsRunning)
			{
				await doJob(pClient);

				await Task.Delay((int)updateRatesEveryMillis);
			}
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
							Midpoint = qc.Price
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
