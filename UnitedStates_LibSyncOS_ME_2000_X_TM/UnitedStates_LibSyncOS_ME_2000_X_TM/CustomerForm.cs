﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitedStates_LibSyncOS_ME_2000_X_TM
{
    public partial class Customer_Home : Form
    {
        LibraryController libraryController;
        CustomerAccountForm customerAccountForm;
        CustomerLoginForm customerLoginForm;
        CustomerItemSearchWindow customerItemSearchWindow;

        public Customer_Home()
        {
            InitializeComponent();
        }

        public Customer_Home(LibraryController controller) : this() {
            this.customerAccountForm = new CustomerAccountForm();
            this.customerLoginForm = new CustomerLoginForm();
            this.customerItemSearchWindow = new CustomerItemSearchWindow();
            this.libraryController = controller;
        }

        //////////////////////////// CLICK FUNCTIONS //////////////////////////

        private void customerFindItemButton_Click(object sender, EventArgs e)
        {
            try
            {
                while (true)
                {
                    var success = false;
                    var dialogReturn = customerItemSearchWindow.Display();
                    switch (dialogReturn)
                    {
                        case DialogReturn.CheckOut:
                            var checkoutItem = customerItemSearchWindow.SelectedItem;
                            if (checkoutItem is Book) {
                                var bookToCheckout = (Book)checkoutItem;
                                success = libraryController.DeleteItem(ItemTypes.Book, bookToCheckout.ID);
                                if (success)
                                {
                                    MessageBox.Show("Item Checked out");
                                    customerItemSearchWindow.ClearDisplayItems();
                                }
                                else {
                                    MessageBox.Show("Item could not be checked out");
                                }
                            }
                            break;
                        case DialogReturn.Cancel:
                            return;
                        case DialogReturn.Search:
                            SearchItemsButtonPressedInCustomerSearchItemWindow();
                            break;
                        default:
                            return;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void customerAccountInformationButton_Click(object sender, EventArgs e)
        {
            try
            {
                customerAccountForm.ClearDisplayItems();
                var loggedInUser = libraryController.GetUserIfLoggedIn();
                if (!loggedInUser.isLoggedIn) {
                    MessageBox.Show("User is not logged in");
                    return;
                }
                customerAccountForm.SetDisplayItems(loggedInUser.loggedInCustomer);

                while (true)
                {
                    var dialogResult = customerAccountForm.Display();
                    switch (dialogResult)
                    {
                        case DialogReturn.Return:
                            var itemToReturn = customerAccountForm.SelectedItem;
                            if (itemToReturn is Book)
                            {
                                var bookItemToReturn = (Book)itemToReturn;
                                var result = libraryController.ReturnItem(ItemTypes.Book, bookItemToReturn.ID);
                                if (result)
                                {
                                    MessageBox.Show("Item returned");
                                    customerAccountForm.RemoveItem(bookItemToReturn);
                                }
                                else
                                {
                                    MessageBox.Show("Item could not be returned");
                                }
                            }
                            else if (itemToReturn is Movie)
                            {
                                var movieItemToReturn = (Movie)itemToReturn;
                                var result = libraryController.ReturnItem(ItemTypes.Movie, movieItemToReturn.ID);
                                if (result)
                                {
                                    MessageBox.Show("Item Returned");
                                    customerAccountForm.RemoveItem(movieItemToReturn);
                                }
                                else
                                {
                                    MessageBox.Show("Item could not be returned");
                                }
                            }
                            else
                            {
                                throw new Exception("Item type could not be located");
                            }
                            break;
                        case DialogReturn.PayFine:
                            var userLoggedIn = libraryController.GetUserIfLoggedIn();
                            if (userLoggedIn.isLoggedIn)
                            {
                                var result = libraryController.PayFine(userLoggedIn.loggedInCustomer.Username);
                                if (result)
                                {
                                    MessageBox.Show("Fine has been payed");
                                    customerAccountForm.uxCustomerFine = "0";
                                }
                                else
                                {
                                    MessageBox.Show("User fine could not be payed");
                                }
                                break;
                            }
                            else
                            {
                                MessageBox.Show("User is not logged in");
                                break;
                            }
                        case DialogReturn.Cancel:
                            return;
                        default:
                            return;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }          
        }

        private void customerLoginButton_Click(object sender, EventArgs e)
        {
            try {
                customerLoginForm.ClearDisplayItems();
                while (true)
                {
                    var dialogResult = customerLoginForm.Display();
                    switch (dialogResult)
                    {
                        case DialogReturn.Login:
                            var username = customerLoginForm.UXCustomerUsername;
                            var password = customerLoginForm.UXCustomerPassword;
                            var isLoginASuccess = libraryController.CheckUserLoginCredentials(username, password);
                            if (isLoginASuccess)
                            {
                                MessageBox.Show("User logged in");
                                return;
                            }
                            else
                            {
                                MessageBox.Show("User could not be logged in");
                                break;
                            }
                        case DialogReturn.Cancel:
                            return;
                        default:
                            return;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void customerExitButton_Click(object sender, EventArgs e) // LOGOUT
        {
            try
            {
                var logoutResult = libraryController.LogoutUser();
                if (logoutResult)
                {
                    MessageBox.Show("User logged out");
                }
                else {
                    MessageBox.Show("User could not be logged out");
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        //////////////////////////// HELPER FUNCTIONS //////////////////////////
        public void SearchItemsButtonPressedInCustomerSearchItemWindow()
        {
            var searchString = customerItemSearchWindow.UXCustomerSearchText;

            var isBookCheckBoxChecked = customerItemSearchWindow.UXCustomerIsSearchBookCheckBoxSelected;
            var isMovieCheckBoxChecked = customerItemSearchWindow.UXCustomerIsSearchMovieCheckBoxSelected;
            var bookAndMovieDisplayObjects = new List<object>();

            if (isBookCheckBoxChecked && isMovieCheckBoxChecked)
            {
                bookAndMovieDisplayObjects = libraryController.searchItems(searchString, ItemSearchOptions.BookAndMovie);
            }
            else if (isBookCheckBoxChecked)
            {
                bookAndMovieDisplayObjects = libraryController.searchItems(searchString, ItemSearchOptions.Book);
            }
            else if (isMovieCheckBoxChecked)
            {
                bookAndMovieDisplayObjects = libraryController.searchItems(searchString, ItemSearchOptions.Movie);
            }
            else
            {
                MessageBox.Show("Check one or both of the following checkboxes: Movies, Books");
                return;
            }
            if (bookAndMovieDisplayObjects.Count == 0 || bookAndMovieDisplayObjects == null) {
                MessageBox.Show("No objects were found");
                return;
            }
            customerItemSearchWindow.ClearDisplayItems();
            customerItemSearchWindow.AddDisplayItems(bookAndMovieDisplayObjects.ToArray());
        }
    }
}
