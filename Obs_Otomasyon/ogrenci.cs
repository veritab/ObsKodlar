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
    public partial class ogrenci : DevExpress.XtraEditors.XtraForm
    {
        public ogrenci()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from ogrenci";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void bolumler()
        {
            komut = @"Select bolumno,bolumadi from bolum";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookbolum.Properties.ValueMember = "bolumno";
            lookbolum.Properties.DisplayMember = "bolumadi";
            lookbolum.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void ogrenci_Load(object sender, EventArgs e)
        {
            list();
            bolumler();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtogrencino.Text = dr["ogrencino"].ToString();
            lookbolum.Text = dr["bolumno"].ToString();
            lblbolum.Text = dr["bolumno"].ToString();
            txtad.Text = dr["ad"].ToString();
            txtsoyad.Text = dr["soyad"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {

                komut = @"Select bolumno from bolum where bolumadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookbolum.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblbolum.Text = dr["bolumno"].ToString();
                }

                komut = @"insert into ogrenci(ad,soyad,bolumno) values (@p1,@p2,@p3)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", txtad.Text);
                comm.Parameters.AddWithValue("@p2", txtsoyad.Text);
                comm.Parameters.AddWithValue("@p3", int.Parse(lblbolum.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğrenci Kaydı Yapıldı.");
            }
            catch (Exception)
            {

                MessageBox.Show("Lütfen Tüm Alanları Doldurduğunuzdan Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"update ogrenci set ad=@p1 ,soyad=@p2,bolumno=@p3  where ogrencino=@p4";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", txtad.Text);
                comm.Parameters.AddWithValue("@p2", txtsoyad.Text);
                comm.Parameters.AddWithValue("@p3", int.Parse(lblbolum.Text));
                comm.Parameters.AddWithValue("@p4", int.Parse(txtogrencino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğrenci Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Öğrenciye Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from ogrenci where ogrencino=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtogrencino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğrenci Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from ogrenci where ad like '" + txtad.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Ad Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}