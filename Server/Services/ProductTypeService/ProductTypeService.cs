﻿namespace BlazorMiamiPizza.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductTypes(ProductType productType)
        {
            productType.isEditing = productType.isNew = false;
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>>
            {
                Data = productTypes
            };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductTypes(ProductType productType)
        {
            var dbProductType = await _context.ProductTypes.FindAsync(productType.Id);
            if(dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Типы продуктов не найдены."
                };
            }

            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypes();
        }
    }
}
