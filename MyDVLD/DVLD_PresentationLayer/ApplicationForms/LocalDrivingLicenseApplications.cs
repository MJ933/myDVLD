using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.ApplicationForms
{
    public partial class LocalDrivingLicenseApplications : Form
    {
        public LocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void LocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            DataTable dt = clsManageApplicationsBL.GetAllData();
            DataView dataView1 = new DataView(dt);
            dataGridView1.DataSource = dataView1;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            foreach (DataColumn column in dt.Columns)
            {
                cbFilters.Items.Add(column.ColumnName);
            }
            if (cbFilters.Items.Count > 0)
            {
                cbFilters.SelectedIndex = 0;
            }
            lblResult.Text = dataGridView1.RowCount.ToString();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dataView = dataGridView1.DataSource as DataView;
            if (dataView != null && cbFilters.SelectedItem != null)
            {
                string selectedColumn = cbFilters.SelectedItem.ToString();
                string filterText = txtFilter.Text;
                try
                {
                    if (string.IsNullOrWhiteSpace(filterText))
                    {
                        dataView.RowFilter = string.Empty;
                    }
                    else
                    {
                        DataColumn column = dataView.Table.Columns[selectedColumn];
                        if (column.DataType == typeof(string))
                        {
                            dataView.RowFilter = $"[{selectedColumn}] LIKE '%{filterText}%'";
                        }
                        else if (column.DataType == typeof(int))
                        {
                            int.TryParse(filterText, out int value);
                            dataView.RowFilter = $"[{selectedColumn}]={value}";
                        }
                        else if (column.DataType == typeof(DateTime))
                        {
                            DateTime.TryParse(filterText, out DateTime value);
                            dataView.RowFilter = $"[{selectedColumn}]=#{value:yyyy/MM/dd}#";
                        }
                        else
                        {
                            dataView.RowFilter = "1=0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error applying the filter: {ex.Message}");
                }
                lblResult.Text = dataGridView1.RowCount.ToString();

            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedItem.ToString() == "L.D.L.AppID")
            {

                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                {
                    e.Handled = true;
                }
            }
            else if (cbFilters.SelectedItem.ToString() == "Passed Test")
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form frm = new NewLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    int rowIndex = e.RowIndex;
                    DataGridViewRow row = dataGridView1.Rows[rowIndex];
                    DataGridViewCell cell = dataGridView1.Rows[rowIndex].Cells[e.ColumnIndex];
                    Point collection = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;

                    contextMenuStrip1.Show(dataGridView1, collection);
                    contextMenuStrip1.Tag = row;
                }
            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)contextMenuStrip1.Tag;
            int PersonID = (int)row.Cells["ID"].Value;
            EditApplicationTypeForm.ID = PersonID;
            Form frm = new EditApplicationTypeForm();
            frm.ShowDialog();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)contextMenuStrip1.Tag;
            int LDLAppID = (int)(row.Cells["L.D.L.AppID"].Value);
            clsDrivingLicenseServicesBL Application1 = clsDrivingLicenseServicesBL.FindApplicationByLDLApplicationID(LDLAppID);
            if (Application1 != null)
            {
                if (MessageBox.Show("Do you want to cancel this application?") == DialogResult.OK)
                {
                    if (Application1.ApplicationStatus == 1)
                    {
                        if (Application1.CancelLocalApplicationStatus())
                        {
                            MessageBox.Show("The Application has Canceled");
                            ReloadDataGridView();
                        }
                        else MessageBox.Show("The Application has a problem and NOT Canceled!");
                    }
                    else if (Application1.ApplicationStatus == 2)
                        MessageBox.Show("The Application is Already  Canceled!");
                    else if (Application1.ApplicationStatus == 3)
                        MessageBox.Show("The Application is Already  Completed!");


                    else MessageBox.Show("The Application has a problem and NOT Canceled!");

                }
                else return;

            }
            else MessageBox.Show($"There IS Not LDLApplication With ID = {LDLAppID}");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReloadDataGridView()
        {
            DataTable dt = clsManageApplicationsBL.GetAllData();
            DataView dataView1 = new DataView(dt);
            dataGridView1.DataSource = dataView1;
            lblResult.Text = dataGridView1.RowCount.ToString();
        }
    }
}
