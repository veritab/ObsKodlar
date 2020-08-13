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
    public partial class dersekle : DevExpress.XtraEditors.XtraForm
    {
        public dersekle()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from derskayit";
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
        private void dersekle_Load(object sender, EventArgs e)
        {
            list();
            dersler();
            ogrenciler();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {

            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtkayitno.Text = dr["kayitno"].ToString();
            lookders.Text = dr["derskodu"].ToString();
            lookogrenci.Text = dr["ogrencino"].ToString();
            lblders.Text = dr["derskodu"].ToString();
            lblogrenci.Text = dr["ogrencino"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
              try
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


                komut = @"insert into derskayit(derskodu,ogrencino) values (@p1,@p2)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblders.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblogrenci.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Kaydı Yapıldı.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
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

                komut = @"update derskayit set derskodu=@p1 ,ogrencino=@p2 where kayitno=@p3";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblders.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblogrenci.Text));
                comm.Parameters.AddWithValue("@p3", int.Parse(txtkayitno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Ders Kaydına Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from derskayit where dersno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtkayitno.Text));
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

                komut = @"Select * from derskayit where derskodu ='" + lblders.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Ders Adı Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}