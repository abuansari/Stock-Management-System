﻿using StockManagementApplication.BLL;
using StockManagementApplication.Models;
using System;
using System.Windows.Forms;

namespace StockManagementApplication.UserInterfaces
{
    public partial class CategoryForm : Form
    {
        readonly CategoryManager _categoryManager = new CategoryManager();
        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            SaveButton.Visible = true;
            UpdateButton.Visible = false;
            GetAllCategory();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                messageLabel.Text = "";
                if (nameTextBox.Text == "")
                {
                    var validationMessage = "Please give category name";
                    messageLabel.Text = validationMessage;
                    return;
                }

                var isExist = _categoryManager.IsNameExist(nameTextBox.Text);
                if (isExist)
                {
                    var validationMessage = "Name already exist. Please give another name";
                    messageLabel.Text = validationMessage;
                    return;
                }

                var category = new Category
                {
                    Name = nameTextBox.Text,
                    CreateBy = LoggerInfo.UserName,
                    CreateDate = DateTime.Now
                };
                var isSave = _categoryManager.Save(category);
                if (isSave)
                {
                    RefreshFiled();
                    GetAllCategory();
                    return;
                }
                var failMessage = "Category info Save Fail";
                messageLabel.Text = failMessage;
            }
            catch (Exception exception)
            {
                messageLabel.Text = exception.Message;
            }
        }

        public void GetAllCategory()
        {
            var categoryList = _categoryManager.GetAll();
            CategoryDataGridView.DataSource = categoryList;
        }

        public void RefreshFiled()
        {
            nameTextBox.Text = hiddenLabel.Text = "";
            SaveButton.Visible = true;
            UpdateButton.Visible = false;
        }

        private void CategoryDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            hiddenLabel.Text = CategoryDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
            nameTextBox.Text = CategoryDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            SaveButton.Visible = false;
            UpdateButton.Visible = true;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                messageLabel.Text = "";
                if (nameTextBox.Text == "")
                {
                    var validationMessage = "Please give category name";
                    messageLabel.Text = validationMessage;
                    return;
                }

                var isExist = _categoryManager.IsNameExist(nameTextBox.Text);
                if (isExist)
                {
                    var validationMessage = "Name already exist. Please give another name";
                    messageLabel.Text = validationMessage;
                    return;
                }

                var category = new Category
                {
                    Id = Convert.ToInt32(hiddenLabel.Text),
                    Name = nameTextBox.Text,
                    UpdateBy = LoggerInfo.UserName,
                    UpdateDate = DateTime.Now
                };

                var isUpdate = _categoryManager.Update(category);
                if (isUpdate)
                {
                    RefreshFiled();
                    GetAllCategory();
                    return;
                }
                var failMessage = "Category info Update Fail";
                messageLabel.Text = failMessage;
            }
            catch (Exception exception)
            {
                messageLabel.Text = exception.Message;
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            RefreshFiled();
        }
    }
}
