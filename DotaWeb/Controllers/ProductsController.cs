﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DotaApi.Helpers;
using DotaWeb.Models;

namespace DotaWeb.Controllers
{
	public class ProductsController : ApiController
	{
		ProductModel[] products = new ProductModel[]
		{
			new ProductModel { ProductId = 1, ProductName = "Tomato Soup", ProductCategory = "Groceries", ProductPrice = 1 },

			new ProductModel { ProductId = 2, ProductName = "Yo-yo",     ProductCategory = "Toys", ProductPrice = 3.75M },

			new ProductModel { ProductId = 3, ProductName = "Hammer",     ProductCategory = "Hardware", ProductPrice = 16.99M },

			new ProductModel { ProductId = 4, ProductName = "Sugar", ProductCategory = "Groceries", ProductPrice = 10 },

			new ProductModel { ProductId = 5, ProductName = "Chhota-Bheem", ProductCategory = "Toys", ProductPrice = 15 },

			new ProductModel { ProductId = 6, ProductName = "Printer", ProductCategory = "Hardware", ProductPrice = 120 }
		};

		// GET: api/Products
		public IEnumerable<ProductModel> Get()
		{
			return products.ToList();
		}

		// GET: api/Products/5
		public IHttpActionResult Get(long id)
		{
			var MatchDetailsModel = CommonExtensions.GetMatchDetail(id);
			return Ok(MatchDetailsModel);

			//var product = products.Where(x => x.ProductId == id).ToList();
			//if (product == null)
			//{
			//	return NotFound();
			//}
			//return Ok(product);

		}


		// POST: api/Products
		public void Post([FromBody]string value)
		{
		}

		// PUT: api/Products/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/Products/5
		public void Delete(int id)
		{
		}
	}
}
