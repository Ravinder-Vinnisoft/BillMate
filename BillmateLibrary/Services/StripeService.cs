using System;
using System.Collections.Generic;
using Stripe;

namespace BillMate.Services
{
	public static class StripeService
	{
		private static readonly string StripeAPIKey = "sk_test_51Lz1FrInaraDE4oEknHv9x2ExXBzgokC6SkxRZDU0ctg0lbFErKQ8SXfyO8sCgftsRk4BoykR7pJc28s8laWldEr00ABqiJMrc";

		public static PaymentLink GenerateStripePaymentLink(decimal amount)
		{
			try
			{
				var productService = new ProductService();

				var productCreateOptions = new ProductCreateOptions()
				{
					Name = "Test Product"
				};

				var product = productService.Create(productCreateOptions, new RequestOptions() { ApiKey = StripeAPIKey });

				if(product != null)
				{
                    var priceService = new PriceService();

                    var priceCreateOptions = new PriceCreateOptions()
                    {
                        Currency = "usd",
						Product = product.Id,
						UnitAmount = (long?)(amount * 100)
                    };

                    var price = priceService.Create(priceCreateOptions, new RequestOptions() { ApiKey = StripeAPIKey });

                    var options = new PaymentLinkCreateOptions
                    {
                        LineItems = new List<PaymentLinkLineItemOptions>
						{
							new PaymentLinkLineItemOptions { Price = price.Id, Quantity = 1 },
						},
					};

                    var service = new PaymentLinkService();
                    var paymentLink = service.Create(options, new RequestOptions() { ApiKey = StripeAPIKey });

					return paymentLink;

                }
                else
				{
					throw new Exception("There was an issue creating the procuct");
				}

			} catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}

