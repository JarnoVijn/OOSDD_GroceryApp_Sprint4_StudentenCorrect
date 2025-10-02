
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            int productIdInt = productId.GetValueOrDefault();
            List<BoughtProducts> boughtProductsList = new();
            List<GroceryListItem> groceryListItems = _groceryListItemsRepository.GetAll();
            List < GroceryListItem > temporaryGroceryList = new();
            foreach (var item in groceryListItems)
            {
                if (item.ProductId != productIdInt)
                {
                    temporaryGroceryList.Add(item);
                }
            }
            foreach (var item in temporaryGroceryList)
            {
                groceryListItems.Remove(item);
            }

            foreach (var item in groceryListItems)
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                BoughtProducts boughtProduct = new(_clientRepository.Get(groceryList.ClientId), _groceryListRepository.Get(item.GroceryListId), _productRepository.Get(productIdInt));
                boughtProductsList.Add(boughtProduct);
            }
            return boughtProductsList;

        }
    }
}
