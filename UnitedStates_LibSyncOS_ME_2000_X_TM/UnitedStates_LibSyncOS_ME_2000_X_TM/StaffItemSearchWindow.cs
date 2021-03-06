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
    public partial class StaffItemSearchWindow : Form, ILibraryForm
    {

        public string staffSearchString {
            get {
                return staffSearchItemsTextBox.Text.ToString();
            }
            set {
                staffSearchItemsTextBox.Text = value;
            }
        }

        public bool staffIsSearchBookCheckBoxSelected
        {
            get {
                return staffSearchBooksCheckBox.Checked;
            }
            set {
                staffSearchBooksCheckBox.Checked = value;
            }          
        }

        public bool staffIsSearchMovieCheckBoxSelected
        {
            get
            {
                return staffSearchMovieCheckBox.Checked;
            }
            set
            {
                staffSearchMovieCheckBox.Checked = value;
            }
        }

        public ItemSearchOptions staffSearchCriteria
        {
            get
            {
                string attribute = Staff_Search_Attribute_ComboBox.Text;
                switch (attribute)
                {
                    case "Person":
                        return ItemSearchOptions.Person;
                    case "Genre":
                        return ItemSearchOptions.Genre;
                    default:
                        return ItemSearchOptions.Title;
                }
            }
        }

        public StaffItemSearchWindow()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        // Inspiration: 501 Bookshop program written by Masaaki Mizuno
        public int SelectedIndex
        {
            get
            {
                if (genericItemsList.SelectedIndex == -1) throw new Exception("Select a Line");
                return genericItemsList.SelectedIndex;
            }

            set
            {
                genericItemsList.SelectedIndex = value;
            }
        }

        // Inspiration: 501 Bookshop program written by Masaaki Mizuno
        public object SelectedItem
        {
            get
            {
                if (genericItemsList.SelectedItem == null) throw new Exception("Select a line");
                return genericItemsList.SelectedItem;
            }
        }

        // Inspiration: 501 Bookshop program written by Masaaki Mizuno
        public void AddDisplayItems(params object [] displayObjects)
        {
            genericItemsList.Items.AddRange(displayObjects.ToArray());
        }

        // Inspiration: 501 Bookshop program written by Masaaki Mizuno
        public void ClearDisplayItems()
        {
            staffSearchBooksCheckBox.Checked = false;
            staffSearchMovieCheckBox.Checked = false;
            staffSearchItemsTextBox.Text = "";
            genericItemsList.Items.Clear();
        }

        // Inspiration: 501 Bookshop program written by Masaaki Mizuno
        public DialogReturn Display()
        {
            while (true) {
                switch (this.ShowDialog())
                {
                    case DialogResult.OK: // ADD BOOK
                        return DialogReturn.AddBook;
                    case DialogResult.Yes: // ADD MOVIE
                        return DialogReturn.AddMovie;
                    case DialogResult.No: // DELETE GENERIC ITEM
                        if (CheckDataValidity())
                            return DialogReturn.Delete;
                        break;
                    case DialogResult.Cancel:
                        return DialogReturn.Cancel;
                    case DialogResult.Retry:
                        return DialogReturn.Search;
                    case DialogResult.Abort:
                        if (CheckDataValidity())
                            return DialogReturn.Select;
                        break;
                    default:
                        return DialogReturn.Undefined;
                }
            }         
        }

        public void AddItem(object displayItem)
        {
        }

        public bool CheckDataValidity() {
            if (genericItemsList.SelectedItem == null) {
                MessageBox.Show("Please select an item");
                return false;
            }
            return true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
