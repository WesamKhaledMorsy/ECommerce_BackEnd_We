using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketServices.DTOs;
using Store.Service.Services.PaymentServices;
using Stripe;

namespace Store.Web.Controllers
{   
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        //this is my stripe CLI Webhook secret for testing my end Point  locally
        const string endPointSecret = "whsec_421746668a44491e873ccc871296c1a4db3789bd68d7f0133e967b7ea9f92afc";
        public PaymentController(IPaymentService paymentService,ILogger<PaymentController> logger)
        {
            _paymentService=paymentService;
            _logger=logger;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymen(CustomerBasketDto input)
            => Ok(await _paymentService.CreateOrUpdatePaymentIntent(input));


        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            // that request body coming from stripe
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                //var stripeEvent = EventUtility.ParseEvent(json);
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe:Signutre"], endPointSecret);

                PaymentIntent paymentIntent;    
                // Handle the event
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                    var order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Succeeded : ", order.Id);
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                   var order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Failed : ", order.Id);
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentCreated)
                {
                    //var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    _logger.LogInformation("Payment Created!");

                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }



    }
}
