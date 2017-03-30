using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Marketing;
using Marriott.Business.Marketing.Data;

namespace Marriott.Client.Web.Composers.Marketing
{
    public class MarketingComposer
    {
        public async Task<List<RoomType>> ComposeIndex()
        {
            using (var marketingContext = new MarketingContext())
            {
                return await marketingContext.RoomTypes.ToListAsync();
            }
        }

        public async Task<RoomType> ComposeEdit(int roomTypeId)
        {
            using (var marketingContext = new MarketingContext())
            {
                return await marketingContext.RoomTypes.SingleAsync(x => x.RoomTypeId == roomTypeId);
            }
        }
    }
}