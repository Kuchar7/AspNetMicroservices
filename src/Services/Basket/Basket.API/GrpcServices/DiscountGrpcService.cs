using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _clent;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient clent)
        {
            _clent = clent;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };

            return await _clent.GetDiscountAsync(discountRequest);
        }
    }
}
