using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Npgsql;

namespace Obs_Otomasyon
{
    public partial class rapor : DevExpress.XtraEditors.XtraForm
    {
        public rapor()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void notlistesi()
        {
            komut = @"Select * from notortalama";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void dekandeğişimlistesi()
        {
            komut = @"Select * from eskiyenidekan";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl2.DataSource = dt;
        }
        private void rapor_Load(object sender, EventArgs e)
        {
            notlistesi();
            dekandeğişimlistesi();



            komut = @"select ogretimuyesisayisi()";
            dt = new DataTable();
            comm = new NpgsqlCommand(komut, sql.baglanti());
            NpgsqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                txtogretmen.Text = dr[0].ToString();
            }

            komut = @"select personelsayisi()";
            dt = new DataTable();
            comm = new NpgsqlCommand(komut, sql.baglanti());
            NpgsqlDataReader dr1 = comm.ExecuteReader();
            while (dr1.Read())
            {
                txtpersonel.Text = dr1[0].ToString();
            }

            komut = @"select acilanderssayisi()";
            dt = new DataTable();
            comm = new NpgsqlCommand(komut, sql.baglanti());
            NpgsqlDataReader dr2 = comm.ExecuteReader();
            while (dr2.Read())
            {
                txtders.Text = dr2[0].ToString();
            }

            komut = @"select ogrencisayisi()";
            dt = new DataTable();
            comm = new NpgsqlCommand(komut, sql.baglanti());
            NpgsqlDataReader dr3 = comm.ExecuteReader();
            while (dr3.Read())
            {
                txtogr.Text = dr3[0].ToString();
            }
        }
    }
}