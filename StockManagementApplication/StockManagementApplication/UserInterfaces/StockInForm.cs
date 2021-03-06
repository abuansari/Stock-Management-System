﻿using StockManagementApplication.BLL;
using StockManagementApplication.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StockManagementApplication.UserInterfaces
{
    public partial class StockInForm : Form
    {
        public StockInForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        private readonly CategoryManager _categoryManager = new CategoryManager();
        private readonly ItemManager _itemManager = new ItemManager();
        private readonly StockInManager _stockInManager = new StockInManager();
        public void LoadCategory()
        {
            try
            {
                var categories = _categoryManager.GetAll();
                categoryComboBox.DataSource = categories;
                categoryComboBox.DisplayMember = "Name";
                categoryComboBox.ValueMember = "Id";
                categoryComboBox.SelectedIndex = -1;

            }
            catch (Exception e)
            {
                messageLabel.Text = e.Message;
            }
        }
        private void categoryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                companyComboBox.DataSource = null;
                itemComboBox.DataSource = null;
                reorderLevelTextBox.Text = "";
                avialableQuantityTextBox.Text = "";
                messageLabel.Text = "";
                if (categoryComboBox.SelectedValue != null)
                {
                    var item = new Item();
                    item.CategoryId = Convert.ToInt32(categoryComboBox.SelectedValue);
                    var items = _itemManager.GetAllCompanyByCategoryId(item);
                    companyComboBox.DataSource = items;
                    companyComboBox.DisplayMember = "Name";
                    companyComboBox.ValueMember = "Id";
                    companyComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception exception)
            {
                messageLabel.Text = exception.Message;
            }
        }
        private void companyComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                itemComboBox.DataSource = null;
                reorderLevelTextBox.Text = "";
                avialableQuantityTextBox.Text = "";
                messageLabel.Text = "";
                if (companyComboBox.SelectedValue != null)
                {
                    Item item = new Item();
                    item.CompanyId = Convert.ToInt32(companyComboBox.SelectedValue);
                    var items = _itemManager.GetAllItemByCompanyId(item);
                    itemComboBox.DataSource = items;
                    itemComboBox.DisplayMember = "Name";
                    itemComboBox.ValueMember = "Id";
                    itemComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception exception)
            {
                messageLabel.Text = exception.Message;
            }
        }
        private void itemComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                messageLabel.Text = "";
                if (itemComboBox.SelectedValue != null)
                {
                    var item = new Item();
                    item.Id = Convert.ToInt32(itemComboBox.SelectedValue);
                    var reader = _itemManager.GetReorderLevel(item);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            reorderLevelTextBox.Text = reader["ReorderLevel"].ToString();
                        }

                    }
                    StockIn stock = new StockIn();
                    stock.ItemId = Convert.ToInt32(itemComboBox.SelectedValue);
                    var reader1 = _stockInManager.GetAvaialableQtyByItemId(stock);
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            var avialableQty = reader1["AvialableQty"].ToString();
                            if (String.IsNullOrEmpty(avialableQty))
                            {
                                avialableQuantityTextBox.Text = 0.ToString();
                                return;
                            }

                            avialableQuantityTextBox.Text = avialableQty;
                        }
                    }
                    return;
                }
                reorderLevelTextBox.Text = 0.ToString();
            }
            catch (Exception exception)
            {
                messageLabel.Text = exception.Message;
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (categoryComboBox.SelectedValue == null || companyComboBox.SelectedValue == null || itemComboBox.SelectedValue == null || quantityTextBox.Text == "")
                {
                    string validationMessage = "Please Fillup Required Field";
                    messageLabel.Text = validationMessage;
                    return;
                }

                var stock = new StockIn();
                stock.CategoryId = Convert.ToInt32(categoryComboBox.SelectedValue);
                stock.CompanyId = Convert.ToInt32(companyComboBox.SelectedValue);
                stock.ItemId = Convert.ToInt32(itemComboBox.SelectedValue);
                stock.Quantity = Convert.ToInt32(quantityTextBox.Text);
                stock.ReceiveDate = DateTime.Now;
                stock.CreateBy = LoggerInfo.UserName;
                stock.CreateDate = DateTime.Now;
                var isSave = _stockInManager.Save(stock);
                if (isSave)
                {
                    RefreshField();
                    string successMessage = "Stock in Successfuuly";
                    messageLabel.Text = successMessage;
                    messageLabel.ForeColor = Color.Green;
                    return;
                }
                string failMessage = "Stock in Fail";
                messageLabel.Text = failMessage;
            }
            catch (Exception exception)
            {
                messageLabel.Text = exception.Message;
            }
        }
        public void RefreshField()
        {
            itemComboBox.Text = reorderLevelTextBox.Text = avialableQuantityTextBox.Text = quantityTextBox.Text = "";
        }
    }
}
