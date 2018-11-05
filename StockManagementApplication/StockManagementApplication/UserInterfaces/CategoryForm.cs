﻿using StockManagementApplication.BLL;
using StockManagementApplication.Models;
using System;
using System.Windows.Forms;

namespace StockManagementApplication.UserInterfaces
{
    public partial class CategoryForm : Form
    {
        CategoryManager _categoryManager = new CategoryManager();
        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            SaveButton.Visible = true;
            UpdateButton.Visible = false;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (nameTextBox.Text == "")
                {
                    var validationMessage = "Please give category name";
                    RefreshFiled();
                    MessageBox.Show(validationMessage);
                    return;
                }
                var category = new Category();
                category.Name = nameTextBox.Text;
                category.CreateBy = "Admin";
                category.CreateDate = DateTime.Now;
                var isSave = _categoryManager.Save(category);
                if (isSave)
                {
                    var successMessage = "Category info Save Successfully";
                    MessageBox.Show(successMessage);
                    return;
                }
                var failMessage = "Category info Save Successfully";
                MessageBox.Show(failMessage);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public void RefreshFiled()
        {
            nameTextBox.Text = "";
        }
    }
}
