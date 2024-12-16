using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace _045_Ramos_Piñera_F2
{
    public partial class frmPirates : Form
    {
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Kyle\\Downloads\\dpPirates (1).accdb ";
        OleDbConnection conn;
        bool isnewrec;
        bool isFirstButtonClick = false;
        public frmPirates()
        {
            InitializeComponent();
        }
        private void refresh()
        {

            DataTable dt = new DataTable();
            string query = "Select ID as ID, piratename as ALIAS, givenname as NAME, age as AGE, bounty as BOUNTY, pirategroup as PIRATEGROUP from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();



            grdView.DataSource = dt;
            grdView.Columns["age"].Visible = false;
            grdView.Columns["ID"].Visible = false;
        }
        private void distinct()
        {
            DataTable dt = new DataTable();
            string query = "select distinct pirategroup from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;

            cboPirate.DataSource = dt;
            cboPirate.DisplayMember = "pirategroup";
            cboGroup.DataSource = dt;
            cboGroup.DisplayMember = "pirategroup";
        }

        private void frmPirates_Load(object sender, EventArgs e)
        {
            refresh();

            distinct();

            DataTable dt = new DataTable();
            string query = "Select ID as ID, piratename as ALIAS, givenname as NAME, age as AGE, bounty as BOUNTY, pirategroup as PIRATEGROUP from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;

            grdView.Columns["age"].Visible = false;
            grdView.Columns["ID"].Visible = false;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKeyword.Text) || (string.IsNullOrEmpty(cboPirate.Text)))
            {
                MessageBox.Show("Please input correct data", "ERROR");
            }
            else
            {
                DataTable dt = new DataTable();
                string query = "Select piratename as ALIAS, givenname as NAME, age as AGE, bounty as BOUNTY, pirategroup as PIRATEGROUP from pirates where piratename = '" + txtKeyword.Text + "' or givenname = '" + txtKeyword.Text + "'";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                adapter.Fill(dt);
                conn.Close();

                grdView.DataSource = dt;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            isnewrec = false;
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            txtAlias.Enabled = true;
            txtName.Enabled = true;
            txtAge.Enabled = true;
            cboGroup.Enabled = true;
            txtBounty.Enabled = true;
            DataTable dt = new DataTable();
            string query = "Select piratename as ALIAS, givenname as NAME, age as AGE, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates where piratename = '" + txtAlias.Text + "'";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isnewrec == true)
            {
                string query = "INSERT into [pirates] (piratename, givenname, age, bounty, pirategroup) values(@alias, @name, @age, @bounty, @pirategroup)";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@bounty", txtBounty.Text);
                cmd.Parameters.AddWithValue("@pirategroup", cboGroup.Text);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Input added");

                refresh();
            }

            else
            {

                string query = "Update [pirates] set piratename = @alias, givenname = @name, age = @age, bounty = @bounty, pirategroup = @pirategroup where ID = @id";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@bounty", txtBounty.Text);
                cmd.Parameters.AddWithValue("@pirategroup", cboGroup.Text);
                cmd.Parameters.AddWithValue("@id", grdView.SelectedCells[0].Value.ToString());
                cmd.ExecuteNonQuery();
                conn.Close();

                if (isFirstButtonClick)
                {
                    MessageBox.Show("Saved");
                }
                else
                {
                    isFirstButtonClick = true;
                    MessageBox.Show("Click the save button again to save!");
                }



                refresh();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isnewrec = true;
            txtAlias.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            cboGroup.Text = "";
            txtBounty.Text = "";
            btnSave.Enabled = true;


            txtAlias.Enabled = true;
            txtName.Enabled = true;
            txtAge.Enabled = true;
            cboGroup.Enabled = true;
            txtBounty.Enabled = true;
            btnNew.Enabled = true;
        }

        private void grdView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAlias.Text = grdView.SelectedCells[1].Value.ToString();
            txtName.Text = grdView.SelectedCells[2].Value.ToString();
            txtAge.Text = grdView.SelectedCells[3].Value.ToString();
            txtBounty.Text = grdView.SelectedCells[4].Value.ToString();
            cboGroup.Text = grdView.SelectedCells[5].Value.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAlias.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            cboGroup.Text = "";
            txtBounty.Text = "";


            txtAlias.Enabled = false;
            txtName.Enabled = false;
            txtAge.Enabled = false;
            cboGroup.Enabled = false;
            txtBounty.Enabled = false;
            btnSave.Enabled = false;
            btnNew.Enabled = true;

            refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete from pirates where piratename = @alias";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Data Deleted");

            txtAlias.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            cboGroup.Text = "";
            txtBounty.Text = "";

            refresh();
        }
    }
}
