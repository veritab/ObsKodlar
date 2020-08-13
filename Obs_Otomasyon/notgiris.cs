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
    public partial class notgiris : DevExpress.XtraEditors.XtraForm
    {
        public notgiris()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from notkayit";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void dersler()
        {
            komut = @" select derskodu,dersadi from kayitders";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookders.Properties.ValueMember = "derskodu";
            lookders.Properties.DisplayMember = "dersadi";
            lookders.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        void ogrenciler()
        {
            komut = @"Select ogrencino,ad,soyad from ogrenci";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookogrenci.Properties.ValueMember = "ogrencino";
            lookogrenci.Properties.DisplayMember = "ad";
            lookogrenci.Properties.DisplayMember = "soyad";
            lookogrenci.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void notgiris_Load(object sender, EventArgs e)
        {
            list();
            ogrenciler();
            dersler();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            lookders.Text = dr["derskodu"].ToString();
            lookogrenci.Text = dr["ogrencino"].ToString();
            lblders.Text = dr["derskodu"].ToString();
            lblogrenci.Text = dr["ogrencino"].ToString();
            txtvize.Text = dr["vize"].ToString();
            txtfinal.Text = dr["final"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //try
            {

                komut = @"Select ogrencino from ogrenci where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogrenci.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblogrenci.Text = dr["ogrencino"].ToString();
                }

                komut = @"Select derskodu from kayitders where dersadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookders.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lblders.Text = dr1["derskodu"].ToString();
                }


                komut = @"insert into notkayit(derskodu,ogrencino,vize,final) values (@p1,@p2,@p3,@p4)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblders.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblogrenci.Text));
                comm.Parameters.AddWithValue("@p3", decimal.Parse(txtvize.Text));
                comm.Parameters.AddWithValue("@p4", decimal.Parse(txtfinal.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Not Kaydı Yapıldı.");
            }
         /*   catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
         //   try
            {
                komut = @"Select ogrencino from ogrenci where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogrenci.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblogrenci.Text = dr["ogrencino"].ToString();
                }

                komut = @"Select derskodu from kayitders where dersadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookders.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblders.Text = dr1["derskodu"].ToString();
                }

                komut = @"update notkayit set derskodu=@p1 ,ogrencino=@p2 ,vize=@p3 ,final=@p4 where ogrencino=@p5";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblders.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblogrenci.Text));
                comm.Parameters.AddWithValue("@p3", decimal.Parse(txtvize.Text));
                comm.Parameters.AddWithValue("@p4", decimal.Parse(txtfinal.Text));
                comm.Parameters.AddWithValue("@p5", int.Parse(lblogrenci.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Not Kaydı Güncellendi.");
            }
     /*       catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Not Kaydına Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"Select ogrencino from ogrenci where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogrenci.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblogrenci.Text = dr["ogrencino"].ToString();
                }

                komut = @"delete from notkayit where ogrencino=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblogrenci.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Bilgi Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {

                komut = @"Select derskodu from kayitders where dersadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookders.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lblders.Text = dr1["derskodu"].ToString();
                }

                komut = @"Select * from notkayit where ogrencino ='" + lblogrenci.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Öğrenci Adı Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}