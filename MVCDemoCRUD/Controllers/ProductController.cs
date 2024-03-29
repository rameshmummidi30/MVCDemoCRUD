﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MVCDemoCRUD.Models;
using System.Configuration;

namespace MVCDemoCRUD.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        /// <summary>
        /// Method used to select all products from the product table and display in the index View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            // create new data table
            DataTable dataTblProduct = new DataTable();
            // declare the sql connection object and pass in the connection string
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                // open the sql connection and create the sql data adapter with the query
                sqlCon.Open();
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter("SELECT * FROM Product", sqlCon);
                // now fill the results from the quert into the data table
                sqlDataAdap.Fill(dataTblProduct);
            }
            // pass the data table into the View
            return View(dataTblProduct);
        }

        /// <summary>
        /// Using this action method we will return a form to enter the details of a new product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            // pass an object of the ProductModel class
            return View(new ProductModel());
        }

        /// <summary>
        /// Method to pass the paramter values from the form to an INSERT query to add the entered product
        /// </summary>
        /// <param name="productModel">data sent from the form will be binded to this productModel object</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                // here we do an insert into the product table that takes the data from the form and enters it into the appropriate columns
                sqlCon.Open();
                string query = "INSERT INTO Product VALUES(@ProductName,@Price,@Count)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Count", productModel.Count);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method used to get the product details to edit in Edit View form
        /// </summary>
        /// <param name="id">the passed ProductID to be edited</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dataTblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM PRODUCT Where ProductID = @ProductID";
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(query, sqlCon);
                sqlDataAdap.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlDataAdap.Fill(dataTblProduct);
            }
            if (dataTblProduct.Rows.Count == 1) // check if there is a row, if there is there is data associated with that product id
            {
                productModel.ProductID = Convert.ToInt32(dataTblProduct.Rows[0][0].ToString());
                productModel.ProductName = dataTblProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToDecimal(dataTblProduct.Rows[0][2].ToString());
                productModel.Count = Convert.ToInt32(dataTblProduct.Rows[0][3].ToString());
                return View(productModel);
            }
            return View();
        }

        /// <summary>
        /// Method to pass the paramter values from the form to an UPDATE query to modify the chosen product
        /// </summary>
        /// <param name="productModel">data sent from the form will be binded to this productModel object</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                // here we do an insert into the product table that takes the data from the form and enters it into the appropriate columns
                sqlCon.Open();
                string query = "UPDATE Product SET ProductName = @ProductName, Price = @Price, Count = @Count WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productModel.ProductID);
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Count", productModel.Count);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method to delete the chosen product with the ProductID
        /// </summary>
        /// <param name="id">the passed ProductID to be deleted</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Product WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
