using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;


namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // repository is database access layer
        IRepository<Product> context; 
        IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            // here productContext is not an ordinary type
            // productContext, a class which implements IRepository<Product> interface

            context = productContext;
            productCategories = productCategoryContext;

            // IRepository<Product> context = new InMemoryRepository<Product>();
            // the context is an interface variable.
            // we cannot access a method 
            // that is not defined by the interface.

            // InMemoryRepository<Product> context = new InMemoryRepository<Product>();
            // now we can access a method in InMemoryRepository<Product> class
            // that is not defined by the interface
        }

        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            // Product is the model from MyShop.core
            // Collection is the IQueryable method from InMemoryRepository
            // it returns a queryable, list of Product model

            return View(products);
        }
        
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            // ViewModel property 'Product' is getting an instance of model 'Product'
            // ViewModel property 'ProductCategories' is getting an instance of 'ProductCategoryRepository'
            // Collection() is the IQueryable method from InMemoryRepository model
            // it returns a queryable, list of ProductCategory model
            // here the ProductCategories property is IEnumerable

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }

            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);

            if(product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product,string Id)
        {
            Product productToEdit = context.Find(Id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(! ModelState.IsValid)
                {
                    return View(product);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}