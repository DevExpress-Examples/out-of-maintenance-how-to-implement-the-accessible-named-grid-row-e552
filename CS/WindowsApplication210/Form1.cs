using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace WindowsApplication210
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {
                dataTable1.Rows.Add(new object[] {i, i*i, "abcde".Substring(i,1) });
            }

            GridControl grid = new CGrid.CGridControl();
            grid.DataSource = dataTable1;
            CGrid.CGridView view = new CGrid.CGridView(grid);
            view.RowNameField = "Column3";
            grid.MainView = view;
            view.OptionsView.ShowAutoFilterRow = true;
            view.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            view.IndicatorWidth = 40;
            view.OptionsBehavior.Editable = false;
            grid.Dock = DockStyle.Fill;
            this.Controls.Add(grid);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

    }
}