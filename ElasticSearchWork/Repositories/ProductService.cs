using ElasticSearchWork.API.Dtos;
using ElasticSearchWork.API.Models;
using Elastic.Clients.Elasticsearch;
using System.Net;

namespace ElasticSearchWork.API.Repositories
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ResponseDto<ProductDto?>> SaveAsync(ProductCreateDto newProduct)
        {

            Product? response = await _repository.SaveAsync(newProduct.CreateProduct());

            if (response == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "bir hata meydana geldi" }, HttpStatusCode.InternalServerError);
            }
            return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.Created);

        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var repsonse = await _repository.GetAllAsync();
            List<ProductDto> products = new List<ProductDto>();
            foreach (var rep in repsonse)
            {
                if (rep.ProductFeature is null)
                {
                    products.Add(new ProductDto(rep.ID, rep.Name, rep.Price, rep.Stock, null));
                }

                else
                {
                    products.Add(new ProductDto(rep.ID, rep.Name, rep.Price, rep.Stock, new ProductFeatureDto(rep.ProductFeature.Width, rep.ProductFeature.Height, rep.ProductFeature.Color.ToString())));
                }

            }

            //var dtoResponse= repsonse.Select(x => new ProductDto(x.ID, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.ProductFeature.Width, x.ProductFeature.Height, x.ProductFeature.Color))).ToList();
            return ResponseDto<List<ProductDto>>.Success(products, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            Product? response = await _repository.GetByIdAsync(id);

            if (response == null)
            {
                return ResponseDto<ProductDto>.Fail("Ürün bulunamadı", HttpStatusCode.BadRequest);
            }

            return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
        {
            bool response = await _repository.UpdateAsync(updateProduct);

            if (!response)
                return ResponseDto<bool>.Fail("Ürün güncellenemedi", HttpStatusCode.InternalServerError);

            return ResponseDto<bool>.Success(response, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var response = await _repository.DeleteAsync(id);

            if (!response)
                return ResponseDto<bool>.Fail("Ürün silinemedi", HttpStatusCode.InternalServerError);

            return ResponseDto<bool>.Success(response, HttpStatusCode.OK);
        }
    }

}
