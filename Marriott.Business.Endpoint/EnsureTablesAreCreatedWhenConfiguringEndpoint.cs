using System.Linq;
using Marriott.Business.Marketing.Data;
using Marriott.Business.Pricing.Data;
using Marriott.Business.RoomInventory.Data;
using NServiceBus.Features;

namespace Marriott.Business.Endpoint
{
    //https://docs.particular.net/nservicebus/pipeline/features
    public class EnsureTablesAreCreatedWhenConfiguringEndpoint : Feature
    {
        //always called
        public EnsureTablesAreCreatedWhenConfiguringEndpoint()
        {
            bool siteSeeded;
            using (var context = new SeedingContext())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Initialize(false); 
                }

                siteSeeded = context.SiteSeeded.SingleOrDefault() != null;
            }

            if (!siteSeeded)
            {
                using (var context = new RoomInventoryContext())
                {
                    RoomInventoryContext.Seed(context);
                }

                using (var context = new MarketingContext())
                {
                    MarketingContext.Seed(context);
                }

                using (var context = new PricingContext())
                {
                    PricingContext.Seed(context);
                }

                using (var context = new SeedingContext())
                {
                    context.SiteSeeded.Add(new SiteSeeded { SiteSeededId = 1 });
                    context.SaveChanges();
                }
            }
        }

        //called if all defined conditions are met and the feature is marked as enabled
        //Use this method to configure and initialize all required components for the feature.
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}
